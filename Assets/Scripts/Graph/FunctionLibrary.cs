using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public static float Wave(float x, float t)
    {
        float y = Sin(PI * (x + t));
        return y;
    }

    public static float MultiWave(float x, float t)
    {
        float y = Sin(PI * (x +.5f* t));
        y += Sin(2 * PI * (x + t))* (1f/ 2f);
        return y*(1f/1.5f);
    }
}
