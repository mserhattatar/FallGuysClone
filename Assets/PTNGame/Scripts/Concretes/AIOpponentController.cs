using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIOpponentController : Character
{
    private NavMeshAgent agent;
    private readonly Vector3 finishPos = new Vector3(0, 0, 50f);

    private void Awake()
    {
        Init();
        agent = GetComponent<NavMeshAgent>();
        ObstacleForce = 3000f;
    }

    protected override float SetRunAniSpeed() => Math.Abs(agent.velocity.normalized.magnitude);


    protected override void SetAgent(bool active)
    {
        if (agent.enabled == active)
            return;

        if (active)
        {
            StartCoroutine(SetAgentDestination());
        }
        else
        {
            if (!agent.isOnNavMesh)
                return;
            agent.ResetPath();
            agent.isStopped = true;
            agent.enabled = false;
        }
    }

    private IEnumerator SetAgentDestination()
    {
        yield return new WaitForSeconds(0.3f);
        agent.enabled = true;

        if (!agent.isOnNavMesh)
        {
            StartCoroutine(SpawnCharacter());
            yield break;
        }

        agent.isStopped = false;
        agent.SetDestination(finishPos);
    }

    protected override void CollisionFinisLine()
    {
        SetAgent(false);
        gameObject.SetActive(false);
    }
}