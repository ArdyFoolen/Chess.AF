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

        private static readonly Object lockObj = new Object();
        private static Container instance = null;
        public static Container Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                lock (lockObj)
                {
                    if (instance == null)
                        instance = new Container();
                }
                return instance;
            }
        }

    #endregion

    #region private methods

    private void Add(string interfaceName, Func<object> valueFactory)
    {
        if (!dict.ContainsKey(interfaceName))
            dict.Add(interfaceName, new Lazy<object>(valueFactory));
    }

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

    private TInterface CreateInstance<TInterface, TInstance>(Func<TInstance, TInterface> factoryMethod)
        where TInstance : class
    {
        TInstance instance = GetInstanceOf<TInstance>();
        return factoryMethod(instance);
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

    /// <summary>
    /// Get Instance of TInterface
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <returns></returns>
    public TInterface GetInstanceOf<TInterface>()
        => dict.ContainsKey(typeof(TInterface).FullName) ? (TInterface)dict[typeof(TInterface).FullName].Value : default(TInterface);

    /// <summary>
    /// Register TInterface with a factory method
    /// TFactory is an interface to a factory, which has to be registered first
    /// Eq:
    /// Register<IGameFactory, GameFactory>();
    /// Register<IGame, IGameFactory>(f => f.MakeGame("Chess Game"));
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <typeparam name="TFactory"></typeparam>
    /// <param name="factoryMethod"></param>
    public void Register<TInterface, TFactory>(Func<TFactory, TInterface> factoryMethod)
        where TFactory : class
        => Add(typeof(TInterface).FullName, () => CreateInstance(factoryMethod));

    /// <summary>
    /// Register TInterface with a factory method
    /// Eq:
    /// Register(() => Board.CreateBuilder());
    /// Return type of factory determines generic type
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <param name="factoryMethod"></param>
    public void Register<TInterface>(Func<TInterface> factoryMethod)
        => Add(typeof(TInterface).FullName, () => CreateInstance(factoryMethod));

    /// <summary>
    /// Register interface type with an instance type
    /// Eq:
    /// Register(interfaceType, implementationType);
    /// This can be used from appsettings.json configuration files,
    /// where the types are written as strings and not known as generic type
    /// </summary>
    /// <param name="interfaceType"></param>
    /// <param name="instanceType"></param>
    public void Register(Type interfaceType, Type instanceType)
        => Add(interfaceType.FullName, () => CreateInstance(interfaceType, instanceType));


    /// <summary>
    /// Register TInterface with a generic TInstance type
    /// Eq:
    /// Register<IGameBuilder, GameBuilder>();
    /// Recommended use of Register, however not possible when there are multiple constructors
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <typeparam name="TInstance"></typeparam>
    public void Register<TInterface, TInstance>()
        where TInstance : class
        => Add(typeof(TInterface).FullName, () => CreateInstance<TInterface, TInstance>());

    #endregion
}
}
