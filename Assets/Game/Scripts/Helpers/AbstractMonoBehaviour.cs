using UnityEngine;

namespace Game.Scripts.Manager
{
    public abstract class AbstractMonoBehaviour : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            RegisterEvents();
        }

        protected virtual void OnDisable()
        {
            UnRegisterEvents();
        }

        protected virtual void OnDestroy()
        {
            DoKill();
        }

        protected virtual void RegisterEvents()
        {
        }

        protected virtual void UnRegisterEvents()
        {
        }

        protected virtual void DoKill()
        {
        }
    }
}