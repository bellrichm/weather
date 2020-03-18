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

        private List<ContextAssembly> contextAssemblies;

        public EmbeddedRunner(Type testType)
        {
            _testType = testType;

            contextAssemblies = new List<ContextAssembly>();
        }

        public void OnAssemblyStart()
        {
            Type interfaceType = typeof(IAssemblyContext);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();

            IEnumerable<Type> impl = types.Where(t => t.GetInterfaces().Contains(interfaceType));

            foreach (Type type in impl)
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
    }
}