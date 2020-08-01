using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers.CompareObjects
{
    public class Comparable : MonoBehaviour
    {
        [SerializeField]
        public string ObjectUniqueId;

        public static bool operator ==(Comparable obj1, Comparable obj2)
        {
            return obj1.ObjectUniqueId == obj2.ObjectUniqueId;
        }

        public static bool operator !=(Comparable obj1, Comparable obj2)
        {
            return obj1.ObjectUniqueId != obj2.ObjectUniqueId;
        }
        
        public static bool operator ==(Comparable obj1, GameObject obj2)
        {
            return obj1 == obj2.GetComponent<Comparable>();
        }
        
        public static bool operator !=(Comparable obj1, GameObject obj2)
        {
            return obj1 != obj2.GetComponent<Comparable>();
        }
        
        public static bool operator ==(GameObject obj1, Comparable obj2)
        {
            return obj1.GetComponent<Comparable>() == obj2;
        }
        
        public static bool operator !=(GameObject obj1, Comparable obj2)
        {
            return obj1.GetComponent<Comparable>() != obj2;
        }

        public override bool Equals(object other)
        {
            if(other is Comparable c)
            {
                return this == c;
            }
            else if(other is GameObject go)
            {
                return this == go;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class ComparableExtensions
    {
        public static bool CompareEquality(
            this GameObject first,
            GameObject second)
        {
            return first.GetComponent<Comparable>() == second.GetComponent<Comparable>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Comparable c(
            this GameObject first)
        {
            return first.GetComponent<Comparable>();
        }
    }
}
