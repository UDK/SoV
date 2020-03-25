using UnityEngine;
using UnityEditor;

public interface IGravity
{
    /// <summary>
    /// Gives Vector2 force
    /// </summary>
    /// <param name="who">Object that pulls or is pulled</param>
    /// <param name="iv">Influence vector</param>
    /// <param name="influence">Influence parameter that charges end force</param>
    /// <returns>Force related to who or sob</returns>
    Vector2 PullForce(Rigidbody2D who, Vector2 iv, float influence);
}