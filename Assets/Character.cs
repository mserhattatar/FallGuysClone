using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterAnimations characterAnimations;
    private Rigidbody rb;

    private bool move;
    private Vector3 spawnPos;
    private bool isSpawning;
    private float obstacleForce;
    private float rotatingPlatformForce;
    public float RunAniSpeed { get; protected internal set; }

    protected internal void Init()
    {
        rb = GetComponent<Rigidbody>();
        characterAnimations = new CharacterAnimations(GetComponent<Animator>());
        SetSpawnPos(transform.position);
    }

    private void LateUpdate()
    {
        characterAnimations.SetRun(RunAniSpeed);

        if (!isSpawning && transform.position.y < -2f)
            SpawnCharacter();
    }

    #region Collision

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("RotatingPlatform"))
        {
            var turnPos = collisionInfo.gameObject.GetComponent<ObstacleMover>().PlayerRotationPos;
            transform.Translate(turnPos * Time.deltaTime, Space.World);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("AddForceObstacle") || collision.gameObject.CompareTag("Player"))
        {
            CollisionAddForce();
            StartCoroutine(StopMovements());
            rb.AddExplosionForce(2500, collision.transform.position, 360, 0.2f);
        }

        else if (!isSpawning && collision.gameObject.CompareTag("SpawnObstacle"))
        {
            CollisionSpawn();

            move = false;
            characterAnimations.SetFalling(true);
            transform.position = spawnPos;
            Debug.Log("enemy spwan pos =  " + spawnPos);
            isSpawning = true;
        }

        else if (!move && collision.gameObject.CompareTag("Platform"))
        {
            CollisionPlatform();
            isSpawning = false;
            characterAnimations.SetFalling(false);
        }

        else if (collision.gameObject.CompareTag("SpawnPoint"))
        {
            SetSpawnPos(collision.transform.position);
        }
    }

    protected internal virtual void CollisionAddForce()
    {
    }

    protected internal virtual void CollisionSpawn()
    {
    }

    protected internal virtual void CollisionPlatform()
    {
    }

    #endregion


    private void SpawnCharacter()
    {
        if (move)
        {
            move = false;
            characterAnimations.SetFalling(true);
        }
        else if (!isSpawning && transform.position.y < -10f)
        {
            transform.position = spawnPos;
            Debug.Log("player spwan pos =  " + spawnPos);
            isSpawning = true;
        }
    }

    private IEnumerator StopMovements()
    {
        //TODO: animasyonun birden fazla tekrar baslamasini durdur
        characterAnimations.SetFallingDown(true);
        move = false;
        yield return new WaitForSeconds(0.7f);
        characterAnimations.SetFallingDown(false);
    }

    private void SetSpawnPos(Vector3 pos)
    {
        spawnPos = new Vector3(pos.x, 15f, pos.z);
    }

    //for standing up animation event
    protected internal void SetMoveActive()
    {
        characterAnimations.SetFallingDown(false);
        move = true;
    }
}