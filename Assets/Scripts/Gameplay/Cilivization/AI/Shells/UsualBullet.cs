using Assets.Scripts.Gameplay.SpaceObject;
using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Shells
{
    public class UsualBullet : ShellBase
    {
        public float ReloadTimeOfBullet;

        public float LifeDistance { get; set; } = 10f;

        private void Update()
        {
            if(Target == null)
            {
                ShellCollection.Destroy(gameObject);
            }
            if(Vector3.Distance(transform.position, _startPosition) > LifeDistance)
            {
                ShellCollection.Destroy(gameObject);
            }
        }
    }
}
