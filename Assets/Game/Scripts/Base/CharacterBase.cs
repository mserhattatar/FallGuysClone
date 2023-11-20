using System.Collections;
using Game.Scripts.Concretes;
using Game.Scripts.Manager;
using Game.Scripts.Pool;
using UnityEngine;

namespace Game.Scripts.Base
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class CharacterBase : PooledObjectBehaviour
    {
        private CharacterAnimations _characterAnimations;
        private bool _isSpawning;
        private Rigidbody _rb;
        private Vector3 _spawnPos;
        protected bool CanMove;

        protected bool IsFalling;
        protected float ObstacleForce = 2000f;
        private float RunAniSpeed => SetRunAniSpeed();


        private void LateUpdate()
        {
            _characterAnimations.SetRun(RunAniSpeed);

            var position = transform.position;
            IsFalling = IsFalling switch
            {
                false when position.y < 0 => true,
                true when position.y >= 0 => false,
                _ => IsFalling
            };

            if (IsFalling && !_isSpawning && transform.position.y < -10f)
                StartCoroutine(StartCharacterSpawning());
        }

        public override void OnCreatedFromPool(ObjectPool objectPool, Transform parentTransform,
            PoolObjectType poolObjectType)
        {
            Init();
            base.OnCreatedFromPool(objectPool, parentTransform, poolObjectType);
        }

        protected virtual float SetRunAniSpeed()
        {
            return 0f;
        }

        protected virtual void Init()
        {
            _isSpawning = true;
            CanMove = false;

            _rb = GetComponent<Rigidbody>();
            _characterAnimations = new CharacterAnimations(GetComponent<Animator>());

            SetSpawnPos((int)transform.position.z);
        }


        private IEnumerator StartCharacterSpawning()
        {
            _isSpawning = true;

            OnCharacterSpawning();

            CanMove = false;
            _characterAnimations.SetFalling(true);

            yield return new WaitForSeconds(1f);

            _rb.velocity = Vector3.zero;
            transform.position = _spawnPos;
        }

        private void SetSpawnPos(int posZ)
        {
            if ((int)_spawnPos.z == posZ)
                return;
            _spawnPos = new Vector3(0, 25f, posZ);
        }

        // for standing up animation event
        protected internal void StandingAniFinished()
        {
            _characterAnimations.SetStandingUp(false);

            StandingUpAniFinished();
        }

        // for falling down animation event
        protected internal void FallingDownAniFinished()
        {
            _characterAnimations.SetFallingDown(false);

            _characterAnimations.SetStandingUp(true);
        }


        #region Collision

        private void OnCollisionStay(Collision collisionInfo)
        {
            if (collisionInfo.gameObject.CompareTag("RotatingPlatform"))
            {
                OnRotatingPlatform();

                var turnPos = collisionInfo.gameObject.GetComponent<ObstacleMover>().PlayerRotationPos;
                var randomForce = Random.Range(1, 4);
                transform.Translate(turnPos * randomForce * Time.deltaTime, Space.World);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (CanMove && other.gameObject.CompareTag("SpawnPoint"))
            {
                OnSpawnPoint();

                SetSpawnPos((int)other.transform.position.z);
            }

            if (CanMove && other.gameObject.CompareTag("FinishLine"))
            {
                OnFinishLine();

                CanMove = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (CanMove &&
                (collision.gameObject.CompareTag("AddForceObstacle") || collision.gameObject.CompareTag("Player")))
            {
                OnAddForceObstacle();

                CanMove = false;
                _characterAnimations.SetFallingDown(true);
                _rb.AddExplosionForce(ObstacleForce, collision.transform.position, 360, 0.2f);
            }

            if (CanMove && collision.gameObject.CompareTag("SpawnObstacle"))
            {
                OnSpawnObstacle();

                StartCoroutine(StartCharacterSpawning());
            }

            if (_isSpawning && !CanMove && collision.gameObject.CompareTag("Platform"))
            {
                _isSpawning = false;
                OnPlatform();
                _characterAnimations.SetFalling(false);
                _characterAnimations.SetFallingDown(false);
                _characterAnimations.SetStandingUp(true);
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        ///     This function runs when standing up animation finished
        /// </summary>
        protected virtual void StandingUpAniFinished()
        {
            CanMove = true;
        }

        /// <summary>
        ///     This function runs when character is spawning from air
        /// </summary>
        protected virtual void OnCharacterSpawning()
        {
        }

        /// <summary>
        ///     This function runs when character is on finish line
        /// </summary>
        protected virtual void OnFinishLine()
        {
        }

        /// <summary>
        ///     This function runs when character hit spawn obstacle
        /// </summary>
        protected virtual void OnSpawnObstacle()
        {
        }

        /// <summary>
        ///     This function runs when character hit AddForce obstacle
        /// </summary>
        protected virtual void OnAddForceObstacle()
        {
        }

        /// <summary>
        ///     This function runs when character hit spawn point(position) object
        /// </summary>
        protected virtual void OnSpawnPoint()
        {
        }

        /// <summary>
        ///     This function runs when character hit the platform first time after spawning
        /// </summary>
        protected virtual void OnPlatform()
        {
        }

        /// <summary>
        ///     This function runs during character  is on rotating platform
        /// </summary>
        protected virtual void OnRotatingPlatform()
        {
        }

        #endregion
    }
}