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
        public Vector3 direction;
        public Quaternion rotation;
        public Transform transform;
    }
    FractalPart[][] parts;

    private void Awake()
    {
        parts = new FractalPart[depth][];

        for (int i = 0, length = 1; i < parts.Length; i++, length *= 5)
        {
            parts[i] = new FractalPart[length];
        }

        float scale = 1f;
        parts[0][0] = CreatePart(0, 0, scale);
        for (int li = 1; li < depth; li++)
        {
            FractalPart[] levelParts = parts[li];
            scale *= 0.5f;
            for (int fpi = 0; fpi < levelParts.Length; fpi += 5)
            {
                for (int ci = 0; ci < 5; ci++)
                {
                    levelParts[fpi + ci] = CreatePart(li, ci, scale);
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
        return new FractalPart { direction = directions[childIndex], rotation = rotations[childIndex], transform = fractal.transform };
    }

    private void Update()
    {
        Quaternion deltaRotation = Quaternion.Euler(0f, 22.5f * Time.deltaTime, 0f);
        FractalPart rootPart = parts[0][0];
        rootPart.rotation *= deltaRotation;
        rootPart.transform.localRotation = rootPart.rotation;
        parts[0][0] = rootPart;

        for (int li = 1; li < parts.Length; li++)
        {
            FractalPart[] levelParts = parts[li];
            FractalPart[] parentParts = parts[li - 1];
            //print($"LevelParts,{levelParts.Length}");
            for (int fpi = 0; fpi < levelParts.Length; fpi++)
            {
                Transform parentTransform = parentParts[fpi / 5].transform;
                FractalPart part = levelParts[fpi];
                Debug.Log($"ParentTransform, {li}_{fpi % 5}", parentTransform.gameObject);
                part.rotation *= deltaRotation;

                part.transform.localRotation = parentTransform.localRotation * part.rotation;
                part.transform.localPosition =
                     parentTransform.localPosition +
                     parentTransform.localRotation *
                         (1.5f * part.transform.localScale.x * part.direction);
                //part.transform.localPosition = parentTransform.localPosition + (part.direction * (part.transform.localScale.x / 2 + parentTransform.localScale.x / 2));
                levelParts[fpi] = part;
            }
        }
    }
}
