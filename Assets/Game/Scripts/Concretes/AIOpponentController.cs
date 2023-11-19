using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Concretes
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIOpponentController : Character
    {
        private readonly Vector3 finishDestinationPos = new(0, 0, 50f);
        private NavMeshAgent agent;

        protected override void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            ObstacleForce = 3000f;
            base.Awake();
        }

        private void SetAgent(bool active)
        {
            if (active)
                StartCoroutine(SetAgentDestination());
            else
                agent.enabled = false;
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
            agent.enabled = true;
            agent.SetDestination(finishDestinationPos);
        }

        protected override float SetRunAniSpeed()
        {
            return Math.Abs(agent.velocity.normalized.magnitude);
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