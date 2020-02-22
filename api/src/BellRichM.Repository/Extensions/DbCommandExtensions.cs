namespace System.Data.Common
{
    /// <summary>
    /// DbCommand extensions.
    /// </summary>
    #pragma warning disable CA1062
    public static class DbCommandExtensions
    {
        /// <summary>
        /// do it.
        /// </summary>
        /// <param name="dbCommand">The.</param>
        /// <param name="dbParameterName">The parmar.</param>
        /// <param name="dbParameterValue">The dbcommand.</param>
        public static void AddParamWithValue(this DbCommand dbCommand, string dbParameterName, object dbParameterValue)
        {
            var parm = dbCommand.CreateParameter();
            parm.ParameterName = dbParameterName;
            parm.Value = dbParameterValue ?? System.DBNull.Value;
            dbCommand.Parameters.Add(parm);
        }

        /// <summary>
        /// Get value.
        /// </summary>
        /// <param name="rdr"> The see DbDataReader.</param>
        /// <param name="columnName">The column to retrieve.</param>
        /// <typeparam name="T">The data type.</typeparam>
        /// <returns>The data.</returns>
        public static T? GetValue<T>(this DbDataReader rdr, string columnName)
            where T : struct
        {
            return rdr[columnName] == System.DBNull.Value ? null : rdr[columnName] as T?;
        }

        /// <summary>
        /// Get value.
        /// </summary>
        /// <param name="rdr"> The see DbDataReader.</param>
        /// <param name="columnName">The column to retrieve.</param>
        /// <returns>The data.</returns>
        public static string GetStringValue(this DbDataReader rdr, string columnName)
        {
            return rdr[columnName] == System.DBNull.Value ? null : rdr[columnName] as string;
        }
    }
    #pragma warning restore CA1062
}