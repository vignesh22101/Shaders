using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public static float timeMultiplier = 0.5f;
    public delegate float Function(float x,float z, float t);
    public enum FunctionName { Wave, MultiWave, Ripple }

    static Function[] functions = { Wave, MultiWave, Ripple };
    public static Function GetFunction(FunctionName functionName)
    {
        return functions[(int)functionName];
    }
    public static float Wave(float x,float z, float t)
    {
        float y = GetSin(x, z,t);
        return y;
    }

    public static float MultiWave(float x,float z, float t)
    {
        float y = GetSin(x, t);
        y += Sin(2 * PI * (z + t)) * (1f / 2f);
        y += Sin(PI * (x + z + 0.25f * t));
        return y * (1f / 2.5f);
    }

    private static float GetSin(float x,float z, float t)
    {
        return Sin(PI * (x + z+ timeMultiplier * t));
    }

    private static float GetSin(float x, float t)
    {
        return Sin(PI * (x + timeMultiplier * t));
    }

    public static float Ripple(float x,float z, float t)
    {
        float d = Sqrt(x * x + z * z);
        float y = Sin(PI * (4f * d - t));
        //float y = Sin(PI * (4f * d - t));
        //return y / (1f + 10f * d);
        return y / (1f+10f * d);
    }
}
