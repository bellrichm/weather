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

            var subclasses2 = testAssemblyTypes.Where(t => t.BaseType == _testType);

            foreach (var subclass in subclasses)
            {
                RunTest(subclass);
            }
        }

        public void RunTest(Type test)
        {
            if (System.Attribute.IsDefined(test, typeof(IgnoreAttribute)))
            {
                  return;
            }

            var testInstance = Activator.CreateInstance(test);
            var testCase = GetTestCase(test, testInstance);
            SetupTestCase(testCase);
            CheckTestCaseResults(testCase);
            Console.WriteLine(test.Name);
        }

        private void SetupTestCase(TestCase testCase)
        {
            testCase.EstablishDelegate.Method.Invoke(testCase.EstablishDelegate.Target, null);
            testCase.BecauseDelegate.Method.Invoke(testCase.BecauseDelegate.Target, null);
        }

        private void CheckTestCaseResults(TestCase testCase)
        {
            foreach (var itDelegate in testCase.ItDelegates)
            {
                itDelegate.Method.Invoke(itDelegate.Target, null);
            }
        }

        private TestCase GetTestCase(Type test, object testInstance)
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