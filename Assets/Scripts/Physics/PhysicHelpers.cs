using UnityEngine;
using UnityEditor;

public class PhysicHelpers
{
    public static float GetFinalVelocity(float aVelocityChange, float aDrag)
    {
        return aVelocityChange * (1 / Mathf.Clamp01(aDrag * Time.fixedDeltaTime) - 1);
    }
    public static float GetFinalVelocityFromAcceleration(float aAcceleration, float aDrag)
    {
        return GetFinalVelocity(aAcceleration * Time.fixedDeltaTime, aDrag);
    }



    public static float GetDrag(float aVelocityChange, float aFinalVelocity)
    {
        return aVelocityChange / ((aFinalVelocity + aVelocityChange) * Time.deltaTime);
    }

    public static float GetDragFromAcceleration(float aAcceleration, float aFinalVelocity)
    {
        return GetDrag(aAcceleration * Time.fixedDeltaTime, aFinalVelocity);
    }



    public static float GetRequiredVelocityChange(float aFinalSpeed, float aDrag)
    {
        float m = Mathf.Clamp01(aDrag * Time.fixedDeltaTime);
        return aFinalSpeed * m / (1 - m);
    }

    public static float GetRequiredAcceleraton(float aFinalSpeed, float aDrag)
    {
        return GetRequiredVelocityChange(aFinalSpeed, aDrag) / Time.fixedDeltaTime;
    }



    public static float GetRequiredVelocity(float aFinalSpeed, float currentSpeed)
    {
        float m = Mathf.Clamp01(Time.fixedDeltaTime);
        return (aFinalSpeed - currentSpeed) * m / (1 - m);
    }

    public static float GetAcceleraton(float aFinalSpeed, float currentSpeed)
    {
        return GetRequiredVelocity(aFinalSpeed, currentSpeed) / Time.fixedDeltaTime;
    }

    public static Vector2 GetGravitationPullForce(BodyBase who, BodyBase whom, float gravityForce)
    {
        // fuck this formulas
        /*
        // Do the Force calculation (refer universal gravitation for more info)
        // Use numbers to adjust force, distance will be changing over time!
        float force = GravConstrant * (who.Mass * whom.Mass) / Mathf.Pow(distance, 2);
        */
        var distance = Vector2.Distance(who.transform.position, whom.transform.position);
        float force = gravityForce / Mathf.Pow(distance, 2);
        // Find the Normal direction
        Vector2 normalDirection = (who.transform.position - whom.transform.position).normalized;

        // calculate the force on the object from the planet
        Vector2 normalForce = normalDirection * force;

        return normalForce;
    }
}