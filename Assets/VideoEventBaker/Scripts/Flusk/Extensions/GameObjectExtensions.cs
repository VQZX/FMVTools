using UnityEngine;

namespace Flusk.Extensions
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// If a component does not already exist on an object, add the component
        /// Otherwise, return the existing component
        /// </summary>
        public static T AddSingleComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
            return component;
        }

        public static bool AddSingleComponent<T>(this GameObject gameObject, out T component) where T : Component
        {
            component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
                return true;
            }
            return false;
        }
    }
}