using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpponentAIController : MonoBehaviour
{
    private CharacterAnimations opponentAnimation;
    private Rigidbody rb;
    private NavMeshAgent agent;

    private Vector3 spawnPos;
    private bool move;
    private bool isSpawning;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        opponentAnimation = new CharacterAnimations(GetComponent<Animator>());
    }

    private void OnEnable()
    {
        SetSpawnPos(transform.position);
    }


    private void LateUpdate()
    {
        opponentAnimation.SetRun(Math.Abs(agent.velocity.normalized.magnitude));

        if (!isSpawning && transform.position.y < -2f)
            SpawnAgain();
    }

    private void SpawnAgain()
    {
        if (move)
        {
            move = false;
            opponentAnimation.SetFalling(true);
        }

        if (!isSpawning && transform.position.y < -10f)
        {
            transform.position = spawnPos;
            Debug.Log("obstacle spwan pos =  " + spawnPos);
            isSpawning = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("AddForceObstacle") || collision.gameObject.CompareTag("Player"))
        {
            
      /**/      agent.agentTypeID = 1; //TODO:
            StartCoroutine(StopMovements());
            rb.AddExplosionForce(2500, collision.transform.position, 360, 0.2f);
        }

        if (!isSpawning && collision.gameObject.CompareTag("SpawnObstacle"))
        {
    /**/        agent.agentTypeID = 1; //TODO:

            move = false;
            opponentAnimation.SetFalling(true);
            transform.position = spawnPos;
            Debug.Log("enemy spwan pos =  " + spawnPos);
            isSpawning = true;
        }

        if (!move && collision.gameObject.CompareTag("Platform"))
        {
            agent.agentTypeID = 0; //TODO:
            isSpawning = false;
            opponentAnimation.SetFalling(false);
        }

        if (collision.gameObject.CompareTag("SpawnPoint"))
        {
            SetSpawnPos(collision.transform.position);
        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("RotatingPlatform"))
        {
            var turnPos = collisionInfo.gameObject.GetComponent<ObstacleMover>().PlayerRotationPos;
            transform.Translate(turnPos * Time.deltaTime, Space.World);
        }
    }

    private IEnumerator StopMovements()
    {
        //TODO: animasyonun birden fazla tekrar baslamasini durdur
        opponentAnimation.SetFallingDown(true);
        move = false;
        yield return new WaitForSeconds(0.7f);
        opponentAnimation.SetFallingDown(false);
    }

    private void SetSpawnPos(Vector3 pos)
    {
        spawnPos = new Vector3(pos.x, 15f, pos.z);
    }

    //for standing up animation event
    private void SetMoveActive()
    {
        agent.agentTypeID = 0;
        opponentAnimation.SetFallingDown(false);
        agent.SetDestination(new Vector3(0, 0, 48f));
        move = true;
    }
}