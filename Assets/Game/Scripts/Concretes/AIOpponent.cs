using System;
using System.Collections;
using Game.Scripts.Base;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Concretes
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIOpponent : CharacterBase
    {
        private NavMeshAgent _agent;

        private Vector3 _finishDestinationPos;
        public string OpponentName { get; private set; }

        protected override void Init()
        {
            base.Init();
            _agent = GetComponent<NavMeshAgent>();
            ObstacleForce = 3000f;
        }

        public void CreateAgent(string agentName, Vector3 startPos, float destinationPos)
        {
            OpponentName = agentName;
            transform.position = startPos;
            _finishDestinationPos = new Vector3(0, 0, destinationPos);
        }

        private void SetAgent(bool active)
        {
            //TODO: add routine controller
            StopAllCoroutines();

            if (active)
                StartCoroutine(SetAgentDestination());
            else
                _agent.enabled = false;
        }

        private IEnumerator SetAgentDestination()
        {
            yield return new WaitWhile(() => CanMove);
            yield return new WaitForSeconds(1f);

            if (IsFalling)
            {
                CanMove = false;
                yield break;
            }

            CanMove = true;
            _agent.enabled = true;
            yield return new WaitUntil(() => _agent.hasPath);
            _agent.SetDestination(_finishDestinationPos);
        }

        protected override float SetRunAniSpeed()
        {
            return Math.Abs(_agent.velocity.normalized.magnitude);
        }

        protected override void OnCharacterSpawning()
        {
            SetAgent(false);
        }

        protected override void OnSpawnObstacle()
        {
            SetAgent(false);
        }

        protected override void OnAddForceObstacle()
        {
            SetAgent(false);
        }

        protected override void StandingUpAniFinished()
        {
            SetAgent(true);
        }

        protected override void OnFinishLine()
        {
            SetAgent(false);
            gameObject.SetActive(false);
        }
    }
}