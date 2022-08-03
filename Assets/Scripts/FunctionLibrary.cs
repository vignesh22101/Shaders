using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public static float timeMultiplier = 0.5f;
    public delegate float Function(float x, float t);
    public enum FunctionName { Wave, MultiWave, Ripple }

    static Function[] functions = { Wave, MultiWave, Ripple };
    public static Function GetFunction(FunctionName functionName)
    {
        return functions[(int)functionName];
    }
    public static float Wave(float x, float t)
    {
        float y = GetSin(x, t);
        return y;
    }

    public static float MultiWave(float x, float t)
    {
        float y = GetSin(x, t);
        y += Sin(2 * PI * (x + t)) * (1f / 2f);
        return y * (1f / 1.5f);
    }

    private static float GetSin(float x, float t)
    {
        return Sin(PI * (x + timeMultiplier * t));
    }

    public static float Ripple(float x, float t)
    {
        float d = Abs(x);
        float y = Sin(PI * (4f * d - t));
        return y / (1f + 10f * d);
    }
}
