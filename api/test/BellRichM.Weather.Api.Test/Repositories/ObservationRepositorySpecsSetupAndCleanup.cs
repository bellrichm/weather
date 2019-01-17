using Machine.Specifications;
using Microsoft.Data.Sqlite;

namespace BellRichM.Weather.Api.Repositories.Test
{
    public class ObservationRepositorySpecsSetupAndCleanup : IAssemblyContext
    {
        private const bool SaveDb = false; // set to true to write database to disk for debugging
        private const string SaveConnectionString = "Data Source=../../../TestObservationRepository.db";
        private const string TestDataConnectionString = "Data Source=../../../testData.db";
        private const string InMemoryConnectionString = "Data Source=TestObservationRepository;Mode=Memory;Cache=Shared";
        private static SqliteFactory dbProviderFactory;

        private SqliteConnection inMemoryConnection;

        public void OnAssemblyStart()
        {
            dbProviderFactory = SqliteFactory.Instance;

            var testDataConnection = dbProviderFactory.CreateConnection() as SqliteConnection;
            testDataConnection.ConnectionString = TestDataConnectionString;

            // The in-memory database only persists while a connection is open to it.
            inMemoryConnection = dbProviderFactory.CreateConnection() as SqliteConnection;
            inMemoryConnection.ConnectionString = InMemoryConnectionString;
            inMemoryConnection.Open();

            testDataConnection.Open();
            testDataConnection.BackupDatabase(inMemoryConnection);
            testDataConnection.Close();
        }

        public void OnAssemblyComplete()
        {
#pragma warning disable 162
            if (SaveDb)
            {
                var backupConnection = (SqliteConnection)dbProviderFactory.CreateConnection();
                backupConnection.ConnectionString = SaveConnectionString;

                // seems to overwrite existing databases
                ((SqliteConnection)inMemoryConnection).BackupDatabase(backupConnection);
            }

            inMemoryConnection.Close();
        }
#pragma warning disable 162
    }
}
