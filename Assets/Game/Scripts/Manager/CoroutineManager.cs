using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Container;
using Game.Scripts.Helpers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Manager
{
    public class CoroutineManager : ComponentContainerBehaviour
    {
        [ShowInInspector] private readonly Dictionary<string, IEnumerator> _coroutines = new();

        protected override void OnDisable()
        {
            HStopAllCoroutine();
        }

        public string HStartCoroutine(string key, IEnumerator coroutine)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                key = GenerateKey(coroutine.ToString());
            }
            else if (_coroutines.ContainsKey(key))
            {
                Debug.LogWarning("key is currently active = " + key);
                return key;
            }

            _coroutines.Add(key, coroutine);
            StartCoroutine(coroutine);
            return key;
        }

        private string GenerateKey(string keyName)
        {
            var key = keyName + Time.unscaledTime;
            for (var i = 0; i < 5; i++)
                key += ConstantsVariables.Glyphs[Random.Range(0, ConstantsVariables.Glyphs.Length)];

            return key;
        }

        public void HStopCoroutine(string key)
        {
            if (string.IsNullOrWhiteSpace(key) || !_coroutines.ContainsKey(key)) return;

            if (_coroutines[key] is not null)
                StopCoroutine(_coroutines[key]);
            _coroutines.Remove(key);
        }

        private void HStopAllCoroutine()
        {
            foreach (var coroutine in _coroutines) StopCoroutine(coroutine.Value);

            _coroutines.Clear();
            StopAllCoroutines();
        }

        protected override void RegisterEvents()
        {
        }

        protected override void UnRegisterEvents()
        {
        }

        protected override void DoKill()
        {
        }

        public override void ContainerOnAwake()
        {
        }

        public override void ContainerDoAfterAwake()
        {
        }
    }
}