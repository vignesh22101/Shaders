using System;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    [SerializeField, Range(1, 6)] int depth;
    [SerializeField] Mesh mesh;
    [SerializeField] Material material;

    static Vector3[] directions = { Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back };
    static Quaternion[] rotations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f), Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f)
    };

    struct FractalPart
    {
        public Vector3 position;
        public Quaternion rotation;
        public Transform transform;
    }
    FractalPart[][] fractalParts;

    private void Awake()
    {
        fractalParts = new FractalPart[depth][];

        for (int i = 0, length = 1; i < fractalParts.Length; i++, length *= 5)
        {
            fractalParts[i] = new FractalPart[length];
        }

        float scale = 1f;
        fractalParts[0][0] = CreatePart(0, 0, scale);
        for (int li = 1; li < depth; li++)
        {
            FractalPart[] fractalPart = fractalParts[li];

            for (int fpi = 0; fpi < fractalPart.Length; fpi++, scale *= 0.5f, fpi += 5)
            {
                for (int ci = 0; ci < 5; ci++)
                {
                    fractalPart[fpi + ci] = CreatePart(li, ci, scale);
                }
            }
        }
    }

    FractalPart CreatePart(int levelIndex, int childIndex, float localScale)
    {
        GameObject fractal = new GameObject($"Fractal L:{levelIndex}, C:{childIndex}");
        fractal.transform.SetParent(transform, true);
        fractal.transform.localScale = Vector3.one * localScale;
        fractal.AddComponent<MeshFilter>().mesh = mesh;
        fractal.AddComponent<MeshRenderer>().material = material;
        return new FractalPart { position = directions[childIndex], rotation = rotations[childIndex], transform = fractal.transform };
    }
}
