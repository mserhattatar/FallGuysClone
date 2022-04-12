using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
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


    private void SetAgent(bool active)
    {
        //if (agent.enabled == active)
        // return;

        Debug.Log(gameObject.name + "  =  SetAgent = " + active);

        if (active)
            StartCoroutine(SetAgentDestination());
        else
            agent.enabled = false;
    }

    private IEnumerator SetAgentDestination()
    {
        yield return new WaitUntil(() => canMove);

        Debug.Log(gameObject.name + "  =  SetAgentDestination =   canMove" + canMove);

        yield return new WaitForSeconds(0.5f);
        agent.enabled = true;
        Debug.Log(gameObject.name + "  =  SetDestination ");
        agent.SetDestination(finishPos);
    }

    //Test

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }


    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Warning)
            EditorApplication.isPaused = true;
    }

    //test


    protected override void CharacterFalling() => SetAgent(false);

    protected override void OnSpawnObstacle() => SetAgent(false);

    protected override void OnAddForceObstacle() => SetAgent(false);

    protected override void StandingUpAniFinished() => SetAgent(true);

    protected override void OnFinishLine()
    {
        SetAgent(false);
        gameObject.SetActive(false);
    }
}