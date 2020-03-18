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
            Console.WriteLine(test.Name);
            if (System.Attribute.IsDefined(test, typeof(IgnoreAttribute)))
            {
                  return;
            }
        }
    }
}