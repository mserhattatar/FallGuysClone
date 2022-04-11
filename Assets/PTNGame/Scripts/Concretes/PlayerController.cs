using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Joystick joystick;

    private CharacterAnimations playerAnimations;
    private Rigidbody rb;

    private const float Speed = 4;
    private const float RotationSpeed = 100;
    
    private bool move;
    private Vector3 spawnPos;
    private bool isSpawning;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimations = new CharacterAnimations(GetComponent<Animator>());
    }

    private void OnEnable()
    {
        SetSpawnPos(transform.position);
    }

    private void FixedUpdate()
    {
        if (!move) return;

        transform.Translate(Vector3.forward * Time.deltaTime * Speed * joystick.Vertical);
        transform.Rotate(Vector3.up, Time.deltaTime * RotationSpeed * joystick.Horizontal);
    }

    private void LateUpdate()
    {
        playerAnimations.SetRun(joystick.Vertical != 0
            ? Math.Abs(joystick.Vertical)
            : Math.Abs(joystick.Horizontal));

        if (!isSpawning && transform.position.y < -2f)
            SpawnAgain();
    }

    private void SpawnAgain()
    {
        if (move)
        {
            move = false;
            playerAnimations.SetFalling(true);
        }

        if (!isSpawning && transform.position.y < -10f)
        {
            transform.position = spawnPos;
            Debug.Log("player spwan pos =  " + spawnPos);
            isSpawning = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.GetContact(0).otherCollider.gameObject.CompareTag("AddForceObstacle"))
        {
            
            StartCoroutine(StopMovements());
            rb.AddExplosionForce(2000, collision.transform.position.normalized, 360, 0.1f);
        }

        if (!isSpawning && collision.gameObject.CompareTag("SpawnObstacle"))
        {
            move = false;
            playerAnimations.SetFalling(true);
            transform.position = spawnPos;
            Debug.Log("player spwan pos =  " + spawnPos);
            isSpawning = true;
        }

        if (!move && collision.gameObject.CompareTag("Platform"))
        {
            isSpawning = false;
            playerAnimations.SetFalling(false);
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
        playerAnimations.SetFallingDown(true);
        move = false;
        yield return new WaitForSeconds(0.7f);
        playerAnimations.SetFallingDown(false);
    }

    private void SetSpawnPos(Vector3 pos)
    {
        spawnPos = new Vector3(pos.x, 15f, pos.z);
    }


    //for standing up animation event
    private void SetMoveActive()
    {
        playerAnimations.SetFallingDown(false);
        move = true;
    }
}