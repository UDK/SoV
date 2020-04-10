using Assets.Scripts.Physics.Sattellite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ObjectsBase.Containers
{
    public class BodyGameplay : IGameplayBody, ISatelliteBody
    {

        public bool Civilication;

        public float Mass { get; set; }

        private readonly MonoBehaviour _subject;

        public BodyGameplay(MonoBehaviour subject)
        {
            _subject = subject;
        }

        public void Destroy()
        {

        }
    }
}
