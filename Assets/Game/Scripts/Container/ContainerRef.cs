namespace Game.Scripts.Container
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