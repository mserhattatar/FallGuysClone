using System;
using Game.Scripts.Base;
using Pathfinding;
using UnityEngine;

namespace Game.Scripts.Concretes
{
    public class AIOpponent : CharacterBase
    {
        [SerializeField] private AIDestinationSetter _aiDestinationSetter;
        [SerializeField] private AIPath _aiPath;
        private bool _canMove;

        protected override bool CanMove
        {
            get => _canMove;

            set
            {
                _canMove = value;
                _aiPath.canMove = value;
            }
        }

        protected override void Init()
        {
            base.Init();
            ObstacleForce = 3000f;
        }

        public void CreateAgent(string agentName, Vector3 startPos, Transform destinationTarget)
        {
            OpponentName = agentName;
            transform.position = startPos;
            _aiDestinationSetter.target = destinationTarget;
        }

        protected override float SetRunAniSpeed()
        {
            return Math.Abs(_aiPath.velocity.normalized.magnitude);
        }

        protected override void OnFinishLine()
        {
            base.OnFinishLine();
            gameObject.SetActive(false);
        }
    }
}