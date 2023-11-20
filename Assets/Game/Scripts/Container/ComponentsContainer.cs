using System.Collections.Generic;
using Game.Scripts.Container;
using UnityEngine;

namespace Game.Scripts.Manager
{
    public class ComponentsContainer
    {
        private readonly Dictionary<string, ComponentContainerBehaviour> _components;

        public ComponentsContainer()
        {
            _components = new Dictionary<string, ComponentContainerBehaviour>();
        }

        public void AddComponent(string componentKey, ComponentContainerBehaviour component)
        {
            if (_components.ContainsKey(componentKey))
            {
                Debug.LogWarning("Container already has this key = " + componentKey);
                return;
            }

            _components.Add(componentKey, component);
        }

        public void RemoveComponent(string componentKey)
        {
            if (!_components.ContainsKey(componentKey))
            {
                Debug.LogWarning("Container not contains this key = " + componentKey);
                return;
            }

            GetContainerComponent(componentKey).Component.ContainerRemoving();
            _components.Remove(componentKey);
        }

        public ContainerRef GetContainerComponent(string componentKey)
        {
            if (!_components.ContainsKey(componentKey))
                return new ContainerRef("null", null);
            if (_components[componentKey] is null)
            {
                _components.Remove(componentKey);
                return new ContainerRef("null", null);
            }

            return _components[componentKey].GetContainer();
        }

        /*
        public ContainerRef GetContainerComponent(Type componentType)
        {
            if (componentType != nameof(ComponentContainerBehaviour))
            {
                Debug.Log(componentType + " is not ComponentContainerBehaviour");
            }

            return GetComponent(nameof(componentType));
        }*/

        /*
        public ContainerRef GetComponent(ComponentContainerBehaviour component)
        {
            if (component == null) throw new ArgumentNullException(nameof(component));
            string componentKey = nameof(component);
            return GetContainerComponent(componentKey);
        }

        public Dictionary<string, ComponentContainerBehaviour> GetAllComponentContainer()
        {
            return _components;
        }*/
    }
}