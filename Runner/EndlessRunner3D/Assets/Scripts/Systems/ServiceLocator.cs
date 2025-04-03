using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    public static readonly Dictionary<Type, object> _serviceMap = new Dictionary<Type, object>();

    /// <summary>
    /// Clear static references that may be held between Playmode sessions
    /// </summary>
    public static void Clear()
    {
        _serviceMap.Clear();
    }

    /// <summary>
    /// Register an object in the service map
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    public static void Register<T>(object service)
    {
        if (_serviceMap.ContainsKey(typeof(T)))
        {
            Debug.LogError($"Service of type {service.GetType()} already registered");
            return;
        }

        _serviceMap.Add(typeof(T), service);

    }


    /// <summary>
    /// Register an object using a given Sysem.Type
    /// </summary>
    /// <param name="serviceType"></param>
    /// <param name="service"></param>
    public static void Register(Type serviceType, object service)
    {
        if (_serviceMap.ContainsKey(serviceType))
        {
            Debug.LogError($"Service of type {service.GetType()} already registered");
            return;
        }

        _serviceMap.Add(serviceType, service);
    }

    /// <summary>
    /// Retrieve an object by Type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Get<T>()
    {
        object ret = default(T);
        _serviceMap.TryGetValue(typeof(T), out ret);
        if (ret == null)
        {
            Debug.Log($"Could not find [{typeof(T)}] as a registered system");
        }
        return (T)ret;
    }
}
