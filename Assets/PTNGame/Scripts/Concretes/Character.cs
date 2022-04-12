using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    private CharacterAnimations characterAnimations;
    private Rigidbody rb;

    private Vector3 spawnPos;
    [SerializeField] private bool isSpawning;
    [SerializeField] protected bool canMove;
    protected float ObstacleForce = 2000f;
    private float RunAniSpeed => SetRunAniSpeed();

    protected virtual float SetRunAniSpeed() => 0f;

    protected void Init()
    {
        isSpawning = true;
        canMove = false;
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


    private IEnumerator SpawnCharacter()
    {
        Debug.Log(gameObject.name + "  =  started character Spawn prosedur because of its y position");
        CharacterFalling();
        canMove = false;
        isSpawning = true;
        characterAnimations.SetFalling(true);

        yield return new WaitForSeconds(1f);
        Debug.Log(gameObject.name + "  =  characterSpawned in spawnden");
        rb.velocity = Vector3.zero;
        transform.position = spawnPos;
        isSpawning = false;
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
        Debug.Log(gameObject.name + "  =  StandingAniFinished");
        characterAnimations.SetStandingUp(false);
        canMove = true;

        StandingUpAniFinished();
    }

    //for Falling Down animation event
    protected internal void FallingDownAniFinished()
    {
        Debug.Log(gameObject.name + "  =  FallingDownAniFinished");
        characterAnimations.SetFallingDown(false);
        
        characterAnimations.SetStandingUp(true);
    }


    #region Collision

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("RotatingPlatform"))
        {
            Debug.Log(gameObject.name + "  = OnCollisionStay RotatingPlatform");
            OnRotatingPlatform();

            var turnPos = collisionInfo.gameObject.GetComponent<ObstacleMover>().PlayerRotationPos;
            transform.Translate(turnPos * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canMove && other.gameObject.CompareTag("SpawnPoint"))
        {
            Debug.Log(gameObject.name + "  = Collision RotatingPlatform");

            OnSpawnPointSave();

            SetSpawnPos((int) other.transform.position.z);
        }


        if (canMove && other.gameObject.CompareTag("FinishLine"))
        {
            Debug.Log(gameObject.name + "  = Collision RotatingPlatform");

            OnFinishLine();

            canMove = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canMove &&
            (collision.gameObject.CompareTag("AddForceObstacle") || collision.gameObject.CompareTag("Player")))
        {
            Debug.Log(gameObject.name + "  = Collision AddForceObstacle or Player ");

            OnAddForceObstacle();

            canMove = false;
            characterAnimations.SetFallingDown(true);
            rb.AddExplosionForce(ObstacleForce, collision.transform.position, 360, 0.2f);
        }

        if (canMove && collision.gameObject.CompareTag("SpawnObstacle"))
        {
            Debug.Log(gameObject.name + "  = Collision SpawnObstacle");

            OnSpawnObstacle();

            canMove = false;
            characterAnimations.SetFalling(true);
            transform.position = spawnPos;
        }

        if (isSpawning && !canMove && collision.gameObject.CompareTag("Platform"))
        {
            Debug.Log(gameObject.name + "  = Collision Platform");

            OnPlatform();
            characterAnimations.SetStandingUp(true);
        }
    }

    protected virtual void StandingUpAniFinished()
    {
    }

    protected virtual void CharacterFalling()
    {
    }

    protected virtual void OnFinishLine()
    {
    }

    protected virtual void OnSpawnPointSave()
    {
    }

    protected virtual void OnPlatform()
    {
    }

    protected virtual void OnRotatingPlatform()
    {
    }

    protected virtual void OnSpawnObstacle()
    {
    }

    protected virtual void OnAddForceObstacle()
    {
    }

    #endregion
}