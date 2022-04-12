using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterAnimations characterAnimations;
    private Rigidbody rb;

    private Vector3 spawnPos;
    private bool isSpawning = true;
    protected bool Move;
    protected float ObstacleForce = 2000f;
    private float RunAniSpeed => SetRunAniSpeed();

    protected virtual float SetRunAniSpeed() => 0f;

    protected void Init()
    {
        rb = GetComponent<Rigidbody>();
        characterAnimations = new CharacterAnimations(GetComponent<Animator>());
        SetSpawnPos((int) transform.position.z);
    }

    private void LateUpdate()
    {
        characterAnimations.SetRun(RunAniSpeed);

        if (!isSpawning && transform.position.y < -2f)
            StartCoroutine(SpawnCharacter());
    }

    protected internal IEnumerator SpawnCharacter()
    {
        isSpawning = true;
        Move = false;
        SetAgent(false);
        characterAnimations.SetFalling(true);

        yield return new WaitForSeconds(1.5f);
        rb.velocity = Vector3.zero;
        transform.position = spawnPos;

        yield return new WaitForSeconds(5f);
        if (!Move || transform.position.y < -1)
            StartCoroutine(SpawnCharacter());
    }

    private void SetSpawnPos(int posZ)
    {
        if ((int) spawnPos.z == posZ)
            return;
        spawnPos = new Vector3(0, 25f, posZ);
    }

    //for standing up animation event
    protected internal void StandingAniFinished()
    {
        Move = true;
        SetAgent(true);
    }

    //for Falling Down animation event
    protected internal void FallingDownAniFinished()
    {
        characterAnimations.SetFallingDown(false);
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

    private void OnTriggerEnter(Collider other)
    {
        if (Move && other.gameObject.CompareTag("SpawnPoint"))
            SetSpawnPos((int) other.transform.position.z);

        if (Move && other.gameObject.CompareTag("FinishLine"))
        {
            Move = false;
            CollisionFinisLine();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Move && (collision.gameObject.CompareTag("AddForceObstacle") || collision.gameObject.CompareTag("Player")))
        {
            SetAgent(false);

            characterAnimations.SetFallingDown(true);
            Move = false;
            rb.AddExplosionForce(ObstacleForce, collision.transform.position, 360, 0.2f);
        }

        if (Move && collision.gameObject.CompareTag("SpawnObstacle"))
        {
            SetAgent(false);
            isSpawning = true;
            Move = false;
            characterAnimations.SetFalling(true);
            transform.position = spawnPos;
        }

        if (isSpawning && collision.gameObject.CompareTag("Platform"))
        {
            characterAnimations.SetFalling(false);
            isSpawning = false;
        }
    }

    //TODO: rename the method
    protected virtual void SetAgent(bool active)
    {
    }

    protected virtual void CollisionFinisLine()
    {
    }

    #endregion
}