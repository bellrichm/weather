using BellRichM.Helpers.Test;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BellRichM.TestRunner
{
    public class EmbeddedRunner
    {
        private Type _testType;

        private Type[] testAssemblyTypes;
        private List<ContextAssembly> contextAssemblies;

        public EmbeddedRunner(Type testType)
        {
            _testType = testType;

            var assembly = Assembly.GetAssembly(_testType);
            testAssemblyTypes = assembly.GetTypes();
            contextAssemblies = new List<ContextAssembly>();
        }

        public static void RunTest(Type test)
        {
            if (test == null)
            {
                throw new ArgumentNullException(nameof(test));
            }

            if (System.Attribute.IsDefined(test, typeof(IgnoreAttribute)))
            {
                  return;
            }

            var testInstance = Activator.CreateInstance(test);
            var testCase = GetTestCase(test, testInstance);
            SetupTestCase(testCase);
            try
            {
                CheckTestCaseResults(testCase);
                CheckTestCaseBehaviors(testCase, testInstance);
            }
            catch (System.Reflection.TargetInvocationException ex) when (ex.InnerException is Machine.Specifications.SpecificationException)
            {
                Console.WriteLine(ex.InnerException.Message);
                Console.WriteLine(ex.InnerException.StackTrace);
            }
        }

        public void OnAssemblyStart()
        {
            var interfaceType = typeof(IAssemblyContext);

            var implementations = testAssemblyTypes.Where(t => t.GetInterfaces().Contains(interfaceType));

            foreach (Type type in implementations)
            {
                var method = type.GetMethod("OnAssemblyStart", BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

                var instance = Activator.CreateInstance(type);
                method.Invoke(instance, null);
                contextAssemblies.Add(new ContextAssembly { ImplementationType = type, ImplementationInstance = instance });
            }
        }

        public void OnAssemblyComplete()
        {
            foreach (var contextAssembly in contextAssemblies)
            {
                var method = contextAssembly.ImplementationType.GetMethod("OnAssemblyComplete", BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                method.Invoke(contextAssembly.ImplementationInstance, null);
            }
        }

        public void RunTests()
        {
            var subclasses = testAssemblyTypes.Where(t => t.IsSubclassOf(_testType));
            if (!subclasses.Any())
            {
                subclasses = testAssemblyTypes.Where(t => t == _testType);
            }

            var subclasses2 = testAssemblyTypes.Where(t => t.BaseType == _testType);

            foreach (var subclass in subclasses)
            {
                RunTest(subclass);
            }
        }

        private static void SetupTestCase(TestCase testCase)
        {
            testCase.EstablishDelegate.Method.Invoke(testCase.EstablishDelegate.Target, null);
            testCase.BecauseDelegate.Method.Invoke(testCase.BecauseDelegate.Target, null);
        }

        private static void CheckTestCaseResults(TestCase testCase)
        {
            foreach (var itDelegate in testCase.ItDelegates)
            {
                itDelegate.Method.Invoke(itDelegate.Target, null);
            }
        }

        private static void CheckTestCaseBehaviors(TestCase testCase, object testInstance)
        {
                foreach (var loggingBehavior in testCase.LoggingBehaviors)
                {
                    var loggingBehaviorType = loggingBehavior.GetType();
                    var testInstanceType = testInstance.GetType();

                    var loggerMockValue = testInstanceType
                        .GetField("loggerMock", BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                        .GetValue(testInstance);
                    loggingBehaviorType
                        .GetField("loggerMock", BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                        .SetValue(loggingBehavior, loggerMockValue);

                    var loggingDataValue = testInstanceType
                        .GetField("loggingData", BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                        .GetValue(testInstance);
                    loggingBehaviorType
                        .GetField("loggingData", BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                        .SetValue(loggingBehavior, loggingDataValue);

                    var loggingBehaviorFieldInfos = loggingBehaviorType.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                    foreach (var loggingBehaviorFieldInfo in loggingBehaviorFieldInfos)
                    {
                        if (loggingBehaviorFieldInfo.FieldType == typeof(Machine.Specifications.It))
                        {
                            var loggingBehaviorCheck = loggingBehaviorFieldInfo.GetValue(loggingBehavior) as Delegate;
                            loggingBehaviorCheck.Method.Invoke(loggingBehaviorCheck.Target, null);
                        }
                    }
                }
        }

        private static TestCase GetTestCase(Type test, object testInstance)
        {
            var testCase = new TestCase();
            var fieldInfos = test.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

            foreach (var fieldInfo in fieldInfos)
            {
                var fieldType = fieldInfo.FieldType;
                if (fieldType == typeof(Machine.Specifications.Establish))
                {
                    testCase.EstablishDelegate = fieldInfo.GetValue(testInstance) as Delegate;
                }

                if (fieldType == typeof(Machine.Specifications.Because))
                {
                    testCase.BecauseDelegate = fieldInfo.GetValue(testInstance) as Delegate;
                }

                if (fieldType == typeof(Machine.Specifications.It))
                {
                    testCase.ItDelegates.Add(fieldInfo.GetValue(testInstance) as Delegate);
                }

                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Machine.Specifications.Behaves_like<>))
                {
                    var behavior = fieldType.GenericTypeArguments[0];
                    if (behavior.IsGenericType && behavior.GetGenericTypeDefinition() == typeof(BellRichM.Logging.LoggingBehaviors<>))
                    {
                        var loggingBehavior = behavior.GetGenericTypeDefinition();
                        testCase.LoggingBehaviors.Add(Activator.CreateInstance(loggingBehavior.MakeGenericType(behavior.GenericTypeArguments)));
                    }
                }
            }

            return testCase;
        }
    }
}