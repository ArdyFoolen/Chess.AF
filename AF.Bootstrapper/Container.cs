using System;
using System.Collections.Generic;
using System.Reflection;

namespace AF.Bootstrapper
{
    public class Container
    {
        #region fields
        
        private Dictionary<Type, Lazy<object>> dict = new Dictionary<Type, Lazy<object>>();

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

        private T CreateInstanceOf<T>(Type factoryType)
            where T : class
            => Activator.CreateInstance(factoryType) as T;

        private object CreateInstance(Type interfaceType, Type factoryType)
        {
            var methodInfo = GetType().GetMethod(nameof(CreateInstanceOf), BindingFlags.Instance | BindingFlags.NonPublic,
                new ContainerBinder(), new Type[] { typeof(Type) }, null);
            var genericMethod = methodInfo.MakeGenericMethod(interfaceType);
            return genericMethod.Invoke(this, new[] { factoryType });
        }

        private TInterface CreateInstance<TInterface, TFactory>(Func<TFactory, TInterface> factoryMethod)
            where TFactory : class
        {
            TFactory factory = GetInstanceOf<TFactory>();
            return factoryMethod(factory);
        }

        private TInterface CreateInstance<TInterface>(Func<TInterface> factoryMethod)
        {
            return factoryMethod();
        }

        private TInstance CreateInstance<TInterface, TInstance>()
            where TInstance : class
        {
            var contr = typeof(TInstance).GetConstructors(BindingFlags.Public | BindingFlags.Instance)[0];
            var parms = contr.GetParameters();
            object[] pObjs = new object[parms.Length];
            int i = 0;
            foreach (var parm in parms)
                pObjs[i++] = GetInstanceOf(parm);

            return typeof(TInstance).InvokeMember(typeof(TInstance).Name, BindingFlags.CreateInstance,
                new ContainerBinder(), null, pObjs) as TInstance;
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
            where T : class
            => dict.ContainsKey(typeof(T)) ? dict[typeof(T)].Value as T : null;

        public void Register(Type interfaceType, Type factoryType)
            => dict.Add(interfaceType, new Lazy<object>(() => CreateInstance(interfaceType, factoryType)));

        public void Register<TInterface, TFactory>(Func<TFactory, TInterface> factoryMethod)
            where TFactory : class
            => dict.Add(typeof(TInterface), new Lazy<object>(() => CreateInstance(factoryMethod)));

        public void Register<TInterface>(Func<TInterface> factoryMethod)
            => dict.Add(typeof(TInterface), new Lazy<object>(() => CreateInstance(factoryMethod)));

        public void Register<TInterface, TInstance>()
            where TInstance : class
            => dict.Add(typeof(TInterface), new Lazy<object>(() => CreateInstance<TInterface, TInstance>()));

        #endregion
    }
}
