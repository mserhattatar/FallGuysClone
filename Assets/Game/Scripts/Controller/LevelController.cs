using System;
using Game.Scripts.Container;

namespace Game.Scripts.Controller
{
    public class LevelController : ComponentContainerBehaviour
    {
        public Action GameStarted;

        public void StartGame()
        {
            GameStarted?.Invoke();
        }
    }
}