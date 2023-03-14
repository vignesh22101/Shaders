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
        public Vector3 direction, worldPosition;
        public Quaternion rotation, worldRotation;
        public float spinAngle;
    }
    FractalPart[][] parts;
    Matrix4x4[][] matrices;

    ComputeBuffer[] matricesBuffer;

    private void OnEnable()
    {
        parts = new FractalPart[depth][];
        matrices = new Matrix4x4[depth][];
        matricesBuffer = new ComputeBuffer[depth];
        int stride = 16 * 4;

        for (int i = 0, length = 1; i < parts.Length; i++, length *= 5)
        {
            parts[i] = new FractalPart[length];
            matrices[i] = new Matrix4x4[length];
        }


        parts[0][0] = CreatePart(0);
        for (int li = 1; li < depth; li++)
        {
            FractalPart[] levelParts = parts[li];

            for (int fpi = 0; fpi < levelParts.Length; fpi += 5)
            {
                for (int ci = 0; ci < 5; ci++)
                {
                    levelParts[fpi + ci] = CreatePart(ci);
                }
            }
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < matricesBuffer.Length; i++)
        {
            matricesBuffer[i].Release();
        }

        parts = null;
        matrices = null;
        matricesBuffer = null;
    }

    FractalPart CreatePart(int childIndex)
    {
        return new FractalPart { direction = directions[childIndex], rotation = rotations[childIndex] };
    }

    private void Update()
    {
        float spinAngleDelta = 22.5f * Time.deltaTime;
        FractalPart rootPart = parts[0][0];
        rootPart.spinAngle += spinAngleDelta;
        rootPart.worldRotation = rootPart.rotation;
        parts[0][0] = rootPart;
        matrices[0][0] = Matrix4x4.TRS(
            rootPart.worldPosition, rootPart.worldRotation, Vector3.one
        );

        float scale = 1f;

        for (int li = 1; li < parts.Length; li++)
        {
            scale *= .5f;
            FractalPart[] levelParts = parts[li];
            FractalPart[] parentParts = parts[li - 1];
            Matrix4x4[] levelMatrices = matrices[li];

            //print($"LevelParts,{levelParts.Length}");
            for (int fpi = 0; fpi < levelParts.Length; fpi++)
            {
                FractalPart parent = levelParts[fpi / 5];
                FractalPart part = levelParts[fpi];
                Debug.Log($"ParentTransform, {li}_{fpi % 5}");
                part.spinAngle += spinAngleDelta;

                part.worldRotation = parent.worldRotation * (part.rotation * Quaternion.Euler(0f, part.spinAngle, 0f));
                part.worldPosition =
                     parent.worldPosition +
                     parent.worldRotation *
                         (1.5f * scale * part.direction);
                //part.transform.localPosition = parentTransform.localPosition + (part.direction * (part.transform.localScale.x / 2 + parentTransform.localScale.x / 2));
                levelParts[fpi] = part;

                levelMatrices[fpi] = Matrix4x4.TRS(
                    part.worldPosition, part.worldRotation, scale * Vector3.one
                );
            }

            for (int i = 0; i < matricesBuffer.Length; i++)
            {
                matricesBuffer[i].SetData(matrices[i]);
            }
        }
    }

    private void OnValidate()
    {
        if (parts != null)
        {
            OnDisable();
            OnEnable();
        }
    }
}
