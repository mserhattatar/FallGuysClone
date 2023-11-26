using System;
using System.Collections;
using Game.Scripts.Concretes;
using Game.Scripts.Manager;
using Game.Scripts.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private bool _isFalling;
        private float RunAniSpeed => SetRunAniSpeed();

        protected float ObstacleForce = 2000f;
        protected virtual bool CanMove { get; set; }
        public string CharacterName { get; protected set; }

        private (bool, int) _characterGameCompletionInfo = new(false, 99);

        public (bool, int) CharacterGameCompletionInfo
        {
            get => _characterGameCompletionInfo;

            set
            {
                if (!_characterGameCompletionInfo.Item1 || value.Item1)
                {
                    _characterGameCompletionInfo = value;
                }
            }
        }

        protected virtual float SetRunAniSpeed()
        {
            return 0f;
        }

        private void LateUpdate()
        {
            _characterAnimations.SetRun(RunAniSpeed);
        }

        protected virtual void Init()
        {
            _rb = GetComponent<Rigidbody>();
            _characterAnimations = new CharacterAnimations(GetComponent<Animator>());

            ResetCharacter();
        }

        protected virtual void ResetCharacter()
        {
            _isSpawning = true;
            CanMove = false;
            _characterGameCompletionInfo = new ValueTuple<bool, int>(false, 99);
            SetSpawnPos((int)transform.position.z);
            StopAllCoroutines();
        }

        public void Activate()
        {
            _isSpawning = true;
            gameObject.SetActive(true);

            StartCoroutine(CharacterFallCheck());
        }

        public override void OnCreatedFromPool(ObjectPool objectPool, Transform parentTransform,
            PoolObjectType poolObjectType)
        {
            Init();
            base.OnCreatedFromPool(objectPool, parentTransform, poolObjectType);
        }


        private IEnumerator CharacterFallCheck()
        {
            var updateSecond = new WaitForSeconds(0.8f);
            while (isActiveAndEnabled)
            {
                var position = transform.position;
                if (!_isFalling && position.y < 0)
                    _isFalling = true;

                if (!_isSpawning && _isFalling && transform.position.y < -10f)
                {
                    StartCoroutine(StartCharacterSpawning());
                    yield return new WaitForSeconds(1f);
                }

                yield return updateSecond;
            }
        }

        private IEnumerator StartCharacterSpawning()
        {
            _isSpawning = true;
            CanMove = false;
            _characterAnimations.SetFalling(true);

            yield return new WaitForSeconds(1f);

            _rb.velocity = Vector3.zero;
            _rb.position = _spawnPos;
        }

        private void SetSpawnPos(int posZ)
        {
            if ((int)_spawnPos.z == posZ)
                return;
            _spawnPos = new Vector3(0, 35f, posZ);
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
                OnRotatingPlatform(collisionInfo);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (CanMove && other.gameObject.CompareTag("SpawnPoint"))
            {
                OnSpawnPoint(other);
            }

            if (CanMove && other.gameObject.CompareTag("FinishLine"))
            {
                OnFinishLine();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (CanMove &&
                (collision.gameObject.CompareTag("AddForceObstacle") || collision.gameObject.CompareTag("Player")))
            {
                OnAddForceObstacle(collision);
            }

            if (_isSpawning && collision.gameObject.CompareTag("Platform"))
            {
                OnPlatform();
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
        ///     This function runs when character is on finish line
        /// </summary>
        protected virtual void OnFinishLine()
        {
            CanMove = false;
            _characterGameCompletionInfo = new ValueTuple<bool, int>(true, _characterGameCompletionInfo.Item2);
        }

        /// <summary>
        ///     This function runs when character hit AddForce obstacle
        /// </summary>
        protected virtual void OnAddForceObstacle(Collision collision)
        {
            CanMove = false;
            _characterAnimations.SetFallingDown(true);
            _rb.AddExplosionForce(ObstacleForce, collision.transform.position, 360, 0.2f);
        }

        /// <summary>
        ///     This function runs when character hit spawn point(position) object
        /// </summary>
        protected virtual void OnSpawnPoint(Collider other)
        {
            SetSpawnPos((int)other.transform.position.z);
        }

        /// <summary>
        ///     This function runs when character hit the platform first time after spawning
        /// </summary>
        protected virtual void OnPlatform()
        {
            _isFalling = false;
            _isSpawning = false;
            _characterAnimations.SetFalling(false);
            _characterAnimations.SetFallingDown(false);
            _characterAnimations.SetStandingUp(true);
        }

        /// <summary>
        ///     This function runs during character  is on rotating platform
        /// </summary>
        protected virtual void OnRotatingPlatform(Collision collisionInfo)
        {
            var turnPos = collisionInfo.gameObject.GetComponent<ObstacleMover>().PlayerRotationPos;
            var randomForce = Random.Range(1, 4);
            transform.Translate(turnPos * randomForce * Time.deltaTime, Space.World);
        }

        #endregion
    }
}