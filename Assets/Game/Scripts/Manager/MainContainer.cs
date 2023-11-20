using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Container;
using UnityEngine;

namespace Game.Scripts.Manager
{
    public class MainContainer : AbstractMonoBehaviour
    {
        [SerializeField] private ComponentContainerBehaviour[] containerBehavioursList;

        private ComponentsContainer _componentsContainer;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _componentsContainer = new ComponentsContainer();
            SetComponents(containerBehavioursList);
        }

        public ComponentContainerBehaviour GetContainerComponent(string componentKey)
        {
            return _componentsContainer.GetContainerComponent(componentKey).Component;
        }

        /*public new Component GetComponent(Type componentType) =>
            _componentsContainer.GetComponent(componentType).Component;

        public T GetComponent<T>(Type componentType) =>
            (T)Convert.ChangeType(_componentsContainer.GetComponent(componentType).Component, typeof(T));*/

        private void SetComponents(IEnumerable<ComponentContainerBehaviour> containerBehaviours)
        {
            IEnumerable<ComponentContainerBehaviour> componentContainerBehaviours =
                containerBehaviours as ComponentContainerBehaviour[] ?? containerBehaviours.ToArray();

            foreach (var container in componentContainerBehaviours) AddContainer(container);
            foreach (var container in componentContainerBehaviours) container.ContainerOnAwake();
            foreach (var container in componentContainerBehaviours) container.ContainerDoAfterAwake();
        }

        private void RemoveComponent(IEnumerable<ComponentContainerBehaviour> containerBehaviours)
        {
            foreach (var containerBehaviour in containerBehaviours)
                if (containerBehaviour)
                    _componentsContainer.RemoveComponent(containerBehaviour.GetType().Name);
        }


        private void AddContainer(ComponentContainerBehaviour containerBehaviour)
        {
            var key = containerBehaviour.GetType().Name;
            _componentsContainer.AddComponent(key, containerBehaviour);
            containerBehaviour.SetContainer(key, this);
        }

        public void RemoveSceneComponentContainer(ComponentContainerBehaviour[] sceneComponentContainers)
        {
            RemoveComponent(sceneComponentContainers);
        }

        public void AddSceneComponentContainer(ComponentContainerBehaviour[] sceneComponentContainers)
        {
            SetComponents(sceneComponentContainers);
        }

        protected override void DoKill()
        {
        }
    }
}