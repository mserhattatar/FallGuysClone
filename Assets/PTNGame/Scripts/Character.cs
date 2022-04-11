using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterAnimations characterAnimations;
    private Rigidbody rb;

    private Vector3 spawnPos;
    private bool isSpawning;
    protected bool Move;
    protected float ObstacleForce = 2000f;


    private float RunAniSpeed => SetRunAniSpeed();

    protected virtual float SetRunAniSpeed()
    {
        return 0f;
    }

    protected void Init()
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
            rb.AddExplosionForce(ObstacleForce, collision.transform.position, 360, 0.2f);
        }

        else if (!isSpawning && collision.gameObject.CompareTag("SpawnObstacle"))
        {
            CollisionSpawn();

            Move = false;
            characterAnimations.SetFalling(true);
            transform.position = spawnPos;
            isSpawning = true;
        }

        else if (!Move && collision.gameObject.CompareTag("Platform"))
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

    protected virtual void CollisionAddForce()
    {
    }

    protected virtual void CollisionSpawn()
    {
    }

    protected virtual void CollisionPlatform()
    {
    }

    #endregion


    private void SpawnCharacter()
    {
        if (Move)
        {
            Move = false;
            characterAnimations.SetFalling(true);
        }
        else if (!isSpawning && transform.position.y < -10f)
        {
            transform.position = spawnPos;
            isSpawning = true;
        }
    }

    private IEnumerator StopMovements()
    {
        //TODO: animasyonun birden fazla tekrar baslamasini durdur
        characterAnimations.SetFallingDown(true);
        Move = false;
        yield return new WaitForSeconds(0.7f);
        characterAnimations.SetFallingDown(false);
    }

    private void SetSpawnPos(Vector3 pos)
    {
        spawnPos = new Vector3(pos.x, 30f, pos.z);
    }

    //for standing up animation event
    protected internal void SetMoveActive()
    {
        SetMoveActiveOverride();
        characterAnimations.SetFallingDown(false);
        Move = true;
    }

    protected virtual void SetMoveActiveOverride()
    {
    }
}