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
        private Assembly _assembly;
        private Type _test;

        private Type[] testAssemblyTypes;
        private List<ContextAssembly> contextAssemblies;
        private int testcount;

        public EmbeddedRunner(Type testType, string testName)
        {
            if (testType == null)
            {
                throw new ArgumentNullException(nameof(testType));
            }

            _testType = testType;
            var assembly = Assembly.GetAssembly(_testType);
            _test = assembly.GetType(_testType.FullName + "+" + testName);
            testAssemblyTypes = assembly.GetTypes();
            contextAssemblies = new List<ContextAssembly>();
            testcount = 0;
        }

        public EmbeddedRunner(Type testType)
        {
            _testType = testType;

            var assembly = Assembly.GetAssembly(_testType);
            testAssemblyTypes = assembly.GetTypes();
            contextAssemblies = new List<ContextAssembly>();
            testcount = 0;
        }

        public EmbeddedRunner(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            _assembly = assembly;
            _testType = null;
            testAssemblyTypes = assembly.GetTypes();
            contextAssemblies = new List<ContextAssembly>();
            testcount = 0;
        }

        public void RunTests()
        {
            OnAssemblyStart();

            if (_test != null)
            {
                RunTestCase(_test);
            }
            else if (_testType != null)
            {
                RunTest(_testType);
            }
            else
            {
                var interfaceType = typeof(IAssemblyContext);

                var testTypes = _assembly.GetTypes()
                                        .Where(t => t.FullName.StartsWith("BellRichM", StringComparison.InvariantCulture)
                                                    && t.BaseType == Type.GetType("System.Object")
                                                    && !t.IsNested
                                                    && t.Name != "Program"
                                                    && !t.GetInterfaces().Contains(interfaceType));

                foreach (var testType in testTypes)
                {
                    _testType = testType; // ToDo - fix hack
                    RunTest(testType);
                }
            }

            OnAssemblyComplete();
            Console.WriteLine(testcount);
        }

        public void RunTest(Type testType)
        {
            if (testType == null)
            {
                throw new ArgumentNullException(nameof(testType));
            }

            // var subclasses2 = testAssemblyTypes.Where(t => t.BaseType == _testType);
            var tests = testAssemblyTypes.Where(t => t.IsSubclassOf(testType));

            if (!tests.Any())
            {
                tests = testType.GetNestedTypes(BindingFlags.Static |
                                                 BindingFlags.Instance |
                                                 BindingFlags.Public |
                                                BindingFlags.NonPublic)
                                 .Where(t => t.GetCustomAttribute<System.Runtime.CompilerServices.CompilerGeneratedAttribute>() == null);
            }

            if (!tests.Any())
            {
                tests = testAssemblyTypes.Where(t => t == testType);
            }

            Console.WriteLine("Running " + testType.Name);
            foreach (var test in tests)
            {
                RunTestCase(test);
            }
        }

        public void RunTestCase(Type test)
        {
            if (test == null)
            {
                throw new ArgumentNullException(nameof(test));
            }

            if (System.Attribute.IsDefined(test, typeof(IgnoreAttribute)))
            {
                  return;
            }

            Console.WriteLine("  " + test.Name);

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

        private static void SetupTestCase(TestCase testCase)
        {
            foreach (var establishDelegate in testCase.EstablishDelegates)
            {
                establishDelegate.Method.Invoke(establishDelegate.Target, null);
            }

            testCase.BecauseDelegate.Method.Invoke(testCase.BecauseDelegate.Target, null);
        }

        private void CheckTestCaseResults(TestCase testCase)
        {
            foreach (var itDelegateDetail in testCase.ItDelegatesDetail)
            {
                Console.WriteLine("    " + itDelegateDetail.Name);
                itDelegateDetail.DelegateField.Method.Invoke(itDelegateDetail.DelegateField.Target, null);
                testcount++;
            }
        }

        private void CheckTestCaseBehaviors(TestCase testCase, object testInstance)
        {
                foreach (var loggingBehavior in testCase.LoggingBehaviors)
                {
                    var loggingBehaviorType = loggingBehavior.GetType();
                    var testInstanceType = testInstance.GetType();

                    Console.WriteLine("    checking " + loggingBehaviorType.Name);

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
                            testcount++;
                        }
                    }
                }
        }

        private TestCase GetTestCase(Type test, object testInstance)
        {
            var testCase = new TestCase();

            var establishFieldInfo = _testType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                             .Where(t => t.FieldType == typeof(Machine.Specifications.Establish)).FirstOrDefault();
            if (establishFieldInfo != null)
            {
                // another hack
                if (_testType.IsAbstract)
                {
                    testCase.EstablishDelegates.Add(establishFieldInfo.GetValue(testInstance) as Delegate);
                }
                else
                {
                    var instance = Activator.CreateInstance(_testType);
                    testCase.EstablishDelegates.Add(establishFieldInfo.GetValue(instance) as Delegate);
                }
            }

            var fieldInfos = test.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

            foreach (var fieldInfo in fieldInfos)
            {
                var fieldType = fieldInfo.FieldType;
                if (fieldType == typeof(Machine.Specifications.Establish))
                {
                    testCase.EstablishDelegates.Add(fieldInfo.GetValue(testInstance) as Delegate);
                }

                if (fieldType == typeof(Machine.Specifications.Because))
                {
                    testCase.BecauseDelegate = fieldInfo.GetValue(testInstance) as Delegate;
                }

                if (fieldType == typeof(Machine.Specifications.It))
                {
                    testCase.ItDelegatesDetail.Add(
                        new DelegateDetail
                        {
                            Name = fieldInfo.Name,
                            DelegateField = fieldInfo.GetValue(testInstance) as Delegate
                        });
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