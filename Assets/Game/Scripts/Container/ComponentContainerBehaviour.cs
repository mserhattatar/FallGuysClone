using Game.Scripts.Manager;

namespace Game.Scripts.Container
{
    public abstract class ComponentContainerBehaviour : AbstractMonoBehaviour
    {
        private ContainerRef _containerRef;
        protected MainContainer MainContainer { get; private set; }


        public ContainerRef GetContainer()
        {
            return _containerRef;
        }

        public ContainerRef SetContainer(string key, MainContainer mainContainer)
        {
            MainContainer = mainContainer;
            _containerRef = new ContainerRef(key, this);
            return _containerRef;
        }

        public virtual void ContainerOnAwake()
        {
        }

        public virtual void ContainerDoAfterAwake()
        {
        }

        public virtual void ContainerRemoving()
        {
        }
    }
}