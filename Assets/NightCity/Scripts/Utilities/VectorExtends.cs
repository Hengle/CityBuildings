﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;

using Random = UnityEngine.Random;

namespace NightCity.Utilities
{
    public static class VectorExtends
    {
        public static float Rand(this Vector2 vec2)
        {
            return Random.Range(vec2.x, vec2.y);
        }

        public static float Average(this Vector2 vec2)
        {
            return (vec2.x + vec2.y) * 0.5f;
        }

        public static Vector3 Surplus(this Vector3 v3, float v)
        {
            return new Vector3(v3.x % v, v3.y % v, v3.z % v);
        }

        public static Vector2 Surplus(this Vector2 v2, float v)
        {
            return new Vector2(v2.x % v, v2.y % v);
        }

        public static Vector2 XZ(this Vector3 v3)
        {
            return new Vector2(v3.x, v3.z);
        }

        public static Vector3 ToVector3(this Vector2 v2, float y = 0f)
        {
            return new Vector3(v2.x, y, v2.y);
        }
    }
}
