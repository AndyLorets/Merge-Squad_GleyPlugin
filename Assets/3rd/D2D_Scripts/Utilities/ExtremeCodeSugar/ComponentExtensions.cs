
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ComponentExtensions
{
    private static class ComponentsCache<TComponent> where TComponent : Component
    {
        public static readonly Dictionary<GameObject, TComponent> components = new Dictionary<GameObject, TComponent>();
    }

#if UNITY_EDITOR
    /// <summary>
    /// We need to cleanup each generic cache when domain reload is disabled.
    /// </summary>
    private static readonly Dictionary<Type, bool> typesToCleanup = new Dictionary<Type, bool>();
    private static int reloadCounter;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        reloadCounter++;
        typesToCleanup.Clear();
    }
    
    private static void CheckCacheCleanup<TComponent>() where TComponent : Component
    {
        if (reloadCounter > 0)
        {
            var componentType = typeof(TComponent);
            if (!typesToCleanup.TryGetValue(componentType, out var needsCleanup))
            {
                typesToCleanup.Add(componentType, true);
            }

            if (needsCleanup)
            {
                ComponentsCache<TComponent>.components.Clear();
                typesToCleanup[componentType] = false;
            }
        }
    }
#endif
    
    public static bool TryGetComponentCached<TComponent>(this Component instance, out TComponent component)
        where TComponent : Component
    {
        return TryGetComponentCached(instance.gameObject, out component);
    }
    
    public static bool TryGetComponentCached<TComponent>(this GameObject gameObject, out TComponent component) 
        where TComponent : Component
    {
#if UNITY_EDITOR
        CheckCacheCleanup<TComponent>();
#endif
        if (ComponentsCache<TComponent>.components.TryGetValue(gameObject, out component)) 
            return component;
        
        if (gameObject.TryGetComponent(out component))
            ComponentsCache<TComponent>.components.Add(gameObject, component);

        return component;
    }

    public static TComponent GetComponentCached<TComponent>(this Component instance)
        where TComponent : Component
    {
        return GetComponentCached<TComponent>(instance);
    }
    
    public static TComponent GetComponentCached<TComponent>(this GameObject gameObject) 
        where TComponent : Component
    {
#if UNITY_EDITOR
        CheckCacheCleanup<TComponent>();
#endif
        if (ComponentsCache<TComponent>.components.TryGetValue(gameObject, out var component)) 
            return component;

        if (gameObject.TryGetComponent(out component))
            ComponentsCache<TComponent>.components.Add(gameObject, component);

        return component;
    }
}