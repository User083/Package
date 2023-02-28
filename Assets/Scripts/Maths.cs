using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Maths
{
    public static float Magnitude(Vector2 a)
    {
       if(a.GetType() == typeof(Vector2))
       {
            return Mathf.Sqrt((a.x * a.x) + (a.y * a.y));
       }
       else
       {
            return 0;
       }
        
    }

    public static Vector2 Normalise(Vector2 a)
    {
        float mag = Magnitude(a);
        if(mag < 0.01f)
        {
            return Vector2.zero;
        }

        return new(a.x/Magnitude(a), a.y/Magnitude(a));  
    }

    public static float Dot(Vector2 lhs, Vector2 rhs)
    {
        Normalise(lhs);
        Normalise(rhs);

        return (lhs.x * rhs.x) + (lhs.y * rhs.y);
    }

    public static float Angle(Vector2 lhs, Vector2 rhs)
    {
        float dot = Dot(lhs, rhs);
        return Mathf.Acos(dot);
    }

    public static Vector2 RotateVector(Vector2 vector, float angle)
    {
        float ang = angle * Mathf.Deg2Rad; //convert from degrees to float value
        float xPos = (vector.x * Mathf.Cos(ang)) - (vector.y * Mathf.Sin(ang));
        float yPos = (vector.x * Mathf.Sin(ang)) + (vector.y * Mathf.Cos(ang));
        Vector2 rotVect = new Vector2(xPos, yPos);
        return rotVect;
    }
}
