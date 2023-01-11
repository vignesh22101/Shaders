using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    [SerializeField, Range(10, 200)]
    int resolution = 10;
    [SerializeField] protected float minPos, maxPos;

    [SerializeField]
    FunctionLibrary.FunctionName functionName;

    public enum TransitionMode { Cycle, Random }

    [SerializeField]
    TransitionMode transitionMode = TransitionMode.Cycle;

    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;

    float duration;

    bool transitioning;

    [SerializeField] ComputeShader computeShader;
    [SerializeField] ComputeBuffer positionsBuffer;
    [SerializeField]
    Material material;

    [SerializeField]
    Mesh mesh;
    FunctionLibrary.FunctionName transitionFunction;

    static readonly int
        positionsId = Shader.PropertyToID("_Positions"),
        resolutionId = Shader.PropertyToID("_Resolution"),
        stepId = Shader.PropertyToID("_Step"),
        timeId = Shader.PropertyToID("_Time");

    private void OnEnable()
    {
        print($"OnEnable on GPUGraph 2");
        positionsBuffer = new ComputeBuffer(resolution * resolution, 4 * 3);
    }

    private void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
    }

    void UpdateFunctionOnGPU()
    {
        float step = 2f / resolution;
        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);
        computeShader.SetBuffer(0, positionsId, positionsBuffer);
        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(0, groups, groups, 1);
        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionsBuffer.count);
    }

    void Update()
    {
        for (int i = 0, x = 0, z = 0; i < resolution * resolution; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
            }

            Transform prefab = transform;
            Vector3 scale = ((maxPos - minPos) / resolution) * Vector3.one;
            float u = x * scale.x + (minPos);
            float v = z * scale.z + (minPos);
            prefab.position = Function(u, v);
        }

        UpdateFunctionOnGPU();
    }

    private Vector3 Function(float x, float z)
    {
        FunctionLibrary.Function function = FunctionLibrary.GetFunction(functionName);
        return function(x, z, Time.time);
    }
}
