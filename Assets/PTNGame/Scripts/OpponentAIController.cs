using System;
using UnityEngine;
using UnityEngine.AI;

public class OpponentAIController : Character
{
    private NavMeshAgent agent;

    private void Awake()
    {
        Init();
        agent = GetComponent<NavMeshAgent>();
        ObstacleForce = 2500f;
    }

    private void SetAgentType(int id) => agent.agentTypeID = id;


    protected override void CollisionAddForce() => SetAgentType(1);

    protected override void CollisionSpawn() => SetAgentType(1);

    protected override void CollisionPlatform() => SetAgentType(0);

    protected override float SetRunAniSpeed() => Math.Abs(agent.velocity.normalized.magnitude);

    protected override void SetMoveActiveOverride()
    {
        agent.agentTypeID = 0;
        agent.SetDestination(new Vector3(0, 0, 48f));
    }
}