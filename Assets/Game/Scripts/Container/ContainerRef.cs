using Game.Scripts.Container;

namespace Game.Scripts.Manager
{
    public class ContainerRef
    {
        public ComponentContainerBehaviour Component;

        public string Key;

        public ContainerRef(string key, ComponentContainerBehaviour component)
        {
            Key = key;
            Component = component;
        }
    }
}