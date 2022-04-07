using System;
using System.Collections.Generic;
using System.Reflection;

namespace AF.Bootstrapper
{
    public class Container
    {
        #region fields
        
        private Dictionary<string, Lazy<object>> dict = new Dictionary<string, Lazy<object>>();

        #endregion

        #region Singleton

        private static Container instance = null;
        public static Container Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                instance = new Container();
                return instance;
            }
        }

        #endregion

        #region private methods

        private ConstructorInfo GetFirstConstructor(Type type)
            => type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)[0];

        private object[] GetInstancesOfParameters(ConstructorInfo contr)
        {
            var parms = contr.GetParameters();
            object[] pObjs = new object[parms.Length];
            int i = 0;
            foreach (var parm in parms)
                pObjs[i++] = GetInstanceOf(parm);
            return pObjs;
        }

        private object[] GetConstructorParameterInstances(Type instanceType)
        {
            var contr = GetFirstConstructor(instanceType);
            object[] pObjs = GetInstancesOfParameters(contr);
            return pObjs;
        }

        private object CreateInstance(Type interfaceType, Type instanceType)
        {
            object[] pObjs = GetConstructorParameterInstances(instanceType);

            return instanceType.InvokeMember(instanceType.Name, BindingFlags.CreateInstance,
                new ContainerBinder(), null, pObjs);
        }

        private TInterface CreateInstance<TInterface, TFactory>(Func<TFactory, TInterface> factoryMethod)
            where TFactory : class
        {
            TFactory factory = GetInstanceOf<TFactory>();
            return factoryMethod(factory);
        }

        private TInterface CreateInstance<TInterface>(Func<TInterface> factoryMethod)
            => factoryMethod();

        private TInterface CreateInstance<TInterface, TInstance>()
            where TInstance : class
        {
            object[] pObjs = GetConstructorParameterInstances(typeof(TInstance));

            return (TInterface)typeof(TInstance).InvokeMember(typeof(TInstance).Name, BindingFlags.CreateInstance,
                new ContainerBinder(), null, pObjs);
        }

        private object GetInstanceOf(ParameterInfo parameter)
        {
            var methodInfo = GetType().GetMethod(nameof(GetInstanceOf), BindingFlags.Instance | BindingFlags.Public,
                new ContainerBinder(), Type.EmptyTypes, null);
            var genericMethod = methodInfo.MakeGenericMethod(parameter.ParameterType);
            return genericMethod.Invoke(this, null);
        }

        #endregion

        #region public methods

        public T GetInstanceOf<T>()
            => dict.ContainsKey(typeof(T).FullName) ? (T)dict[typeof(T).FullName].Value : default(T);

        public void Register(Type interfaceType, Type factoryType)
            => dict.Add(interfaceType.FullName, new Lazy<object>(() => CreateInstance(interfaceType, factoryType)));

        public void Register<TInterface, TFactory>(Func<TFactory, TInterface> factoryMethod)
            where TFactory : class
            => dict.Add(typeof(TInterface).FullName, new Lazy<object>(() => CreateInstance(factoryMethod)));

        public void Register<TInterface>(Func<TInterface> factoryMethod)
            => dict.Add(typeof(TInterface).FullName, new Lazy<object>(() => CreateInstance(factoryMethod)));

        public void Register<TInterface, TInstance>()
            where TInstance : class
            => dict.Add(typeof(TInterface).FullName, new Lazy<object>(() => (TInterface)CreateInstance<TInterface, TInstance>()));

        #endregion
    }
}
