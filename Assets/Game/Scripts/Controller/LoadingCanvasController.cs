using Game.Scripts.Container;

namespace Game.Scripts.Controller
{
    public class LoadingCanvasController : ComponentContainerBehaviour
    {
        public void Set(bool setActive)
        {
            gameObject.SetActive(setActive);
        }
    }
}