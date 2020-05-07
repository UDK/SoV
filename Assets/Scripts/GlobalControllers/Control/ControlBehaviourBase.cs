using Assets.Scripts.Physics;
using Assets.Scripts.Physics.Fabric.ForceFabrics;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.GlobalControllers.Control
{
    public abstract class ControlBehaviourBase : MonoBehaviour
    {
        public float Acceleration = 1f;

        private MovementBehaviour _movement { get; set; }
        private Actions _currentAction { get; set; } = Actions.EmptyInstance;
        private IForce _movementForceAdapter;

        public ControlBehaviourBase()
        {
            _currentAction = Actions.EmptyInstance;
        }

        public void ReleaseDrive()
        {
            ProcessMove();
        }

        public void RegisterDrive()
        {
            this._currentAction = Drive();
        }

        protected virtual Actions Drive()
        {
            return Actions.EmptyInstance;
        }

        private void ProcessMove()
        {
            try
            {
                Vector2 direction = Vector2.zero;
                if ((_currentAction.Move & Move.Up) != 0)
                {
                    direction.y = 1;
                }
                if ((_currentAction.Move & Move.Down) != 0)
                {
                    direction.y = -1;
                }
                if ((_currentAction.Move & Move.Rigth) != 0)
                {
                    direction.x = 1;
                }
                if ((_currentAction.Move & Move.Left) != 0)
                {
                    direction.x = -1;
                }

                _movement.SmoothlySetVelocity(direction * Acceleration);
            }
            catch
            {
                Debug.Log("Fuck this control. This is again null.");
            }
        }

        // Update is called once per frame
        void Awake()
        {
            _movement = this.GetComponent<MovementBehaviour>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            ReleaseDrive();
        }

        // Update is called once per frame
        void Update()
        {
            RegisterDrive();
        }
    }
}
