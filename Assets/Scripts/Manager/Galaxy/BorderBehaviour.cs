using Assets.Scripts.Helpers;
using Assets.Scripts.Physics;
using Assets.Scripts.Physics.Adapters.ForceAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Manager.Galaxy
{
    /// <summary>
    /// TODO
    /// make tag border and avoid influencing of object's gravity on it
    /// finish adding of objects based on tags (eg dont touch satellites)
    /// add special processing of player
    /// </summary>
    public class BorderBehaviour : MonoBehaviour
    {
        public Vector2 PushDirection = Vector2.zero;

        private readonly Dictionary<Collider2D, MovementBehaviour> _registeredGameObjects =
            new Dictionary<Collider2D, MovementBehaviour>();

        private readonly IForce _movementForceAdapter
            = new MovementForce();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var movement = collision.GetComponent<MovementBehaviour>();
            movement.SetVelocity(PushDirection * 0.5f);
            /*if(collision.tag == EnumTags.FreeSpaceBody &&
                !_registeredGameObjects.ContainsKey(collision))
            {
                _registeredGameObjects.Add(
                    collision,
                    collision.GetComponent<MovementBehaviour>());
            }*/
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == EnumTags.FreeSpaceBody)
            {
                var movement = collision.GetComponent<MovementBehaviour>();

                var oneSideX = PushDirection.x != 0 && Mathf.Sign(PushDirection.x) == Mathf.Sign(movement.Velocity.x);
                var oneSideY = PushDirection.y != 0 && Mathf.Sign(PushDirection.y) == Mathf.Sign(movement.Velocity.y);
                if(!oneSideX || !oneSideY)
                {
                    movement.SetVelocity(PushDirection * 0.5f);
                }
                _registeredGameObjects.Remove(collision);
            }
        }

        private void FixedUpdate()
        {
            /*foreach(var pair in _registeredGameObjects)
            {
                pair.Value.MoveInDirection(PushDirection, 0.5f);
            }*/
        }

    }
}
