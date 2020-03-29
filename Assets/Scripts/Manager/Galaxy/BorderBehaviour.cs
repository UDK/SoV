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

        private readonly Dictionary<Collider2D, Rigidbody2D> _registeredGameObjects =
            new Dictionary<Collider2D, Rigidbody2D>();

        private readonly IForceAdapter _movementForceAdapter
            = new MovementForceAdapter();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.transform.parent != null &&
                !_registeredGameObjects.ContainsKey(collision))
            {
                _registeredGameObjects.Add(
                    collision,
                    collision.GetComponent<Rigidbody2D>());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.parent != null)
            {
                var rigidBody = collision.GetComponent<Rigidbody2D>();

                var oneSideX = PushDirection.x != 0 && Mathf.Sign(PushDirection.x) == Mathf.Sign(rigidBody.velocity.x);
                var oneSideY = PushDirection.y != 0 && Mathf.Sign(PushDirection.y) == Mathf.Sign(rigidBody.velocity.y);
                if(!oneSideX || !oneSideY)
                {
                    rigidBody.velocity = PushDirection * 10f;
                }
                _registeredGameObjects.Remove(collision);
            }
        }

        private void FixedUpdate()
        {
            foreach(var pair in _registeredGameObjects)
            {
                var force = _movementForceAdapter.PullForce(pair.Value, PushDirection, 10f);
                pair.Value.AddForce(force, ForceMode2D.Force);
            }
        }

    }
}
