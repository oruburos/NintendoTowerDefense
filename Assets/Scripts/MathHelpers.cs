using System;
using UnityEngine;


namespace Assets.Scripts
{
    static class MathHelpers
    {
        static public float Angle(Vector2 a, Vector2 b)
        {
            Vector2 an = a.normalized;
            Vector2 bn = b.normalized;
            float x = an.x * bn.x + an.y * bn.y;
            var y = an.y * bn.x - an.x * bn.y;
            return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }
    }
}
