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
            var methodInfo = GetType().GetMethod("CreateInstanceOf", BindingFlags.Instance | BindingFlags.NonPublic);
            var genericMethod = methodInfo.MakeGenericMethod(interfaceType);
            return genericMethod.Invoke(this, new[] { factoryType });
        }

        #endregion

        #region public methods

        public T GetInstanceOf<T>()
            where T : class
            => dict.ContainsKey(typeof(T)) ? dict[typeof(T)].Value as T : null;

        public void Register(Type interfaceType, Type factoryType)
            => dict.Add(interfaceType, new Lazy<object>(() => CreateInstance(interfaceType, factoryType)));

        #endregion
    }
}
