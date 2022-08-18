using System;
using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public static float timeMultiplier = 0.5f;
    public delegate Vector3 Function(float u,float v, float t);
    public enum FunctionName { Wave, MultiWave, Ripple,Sphere }

    static Function[] functions = { Wave, MultiWave, Ripple,Sphere };

    /*public static Vector3 Sphere(float u, float v, float t)
    {
        float r = Cos(0.5f * PI * v); 
        Vector3 result;
        result.x =r* Cos(PI*u);
        result.y = Sin(PI * 0.5f * v);
        result.z = r*Sin(PI*u);
        return result;
    }*/

    public static Vector3 Sphere(float u, float v, float t)
    {
        float r = 0.5f + 0.5f * Sin(PI * t);
        float s = r * Cos(0.5f * PI * v);
        Vector3 result;
        result.x = s * Cos(PI * u);
        result.y = r*Sin(PI * 0.5f * v);
        result.z = s * Sin(PI * u);
        return result;
    }

    public static Function GetFunction(FunctionName functionName)
    {
        return functions[(int)functionName];
    }
    public static Vector3 Wave(float u,float v, float t)
    {
        Vector3 result;
        result.x = u;
        result.z = v;
        float y = GetSin(u, v,t);
        result.y = y;
        return result;
    }

    public static Vector3 MultiWave(float u,float v, float t)
    {
        Vector3 result;
        result.x = u;
        result.z = v;
        float y = GetSin(u, t);
        y += Sin(2 * PI * (v + t)) * (1f / 2f);
        y += Sin(PI * (u + v + 0.25f * t));
       result.y= y * (1f / 2.5f);
        return result;
    }

    private static float GetSin(float u,float v, float t)
    {
        return Sin(PI * (u + v+ timeMultiplier * t));
    }

    private static float GetSin(float u, float t)
    {
        return Sin(PI * (u + timeMultiplier * t));
    }

    public static Vector3 Ripple(float u,float v, float t)
    {
        Vector3 result;
        result.x = u;
        result.z = v;
        float d = Sqrt(u * u + v * v);
        float y = Sin(PI * (4f * d - t));
        //float y = Sin(PI * (4f * d - t));
        //return y / (1f + 10f * d);
        result.y= y / (1f+10f * d);
        return result;
    }
}
