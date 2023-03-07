using UnityEngine;

public class Fractal : MonoBehaviour
{
    [SerializeField, Range(1, 6)] int depth;

    private void Start()
    {
        name = $"Fractal:{depth}";

        if (depth <= 0)
            return;
        Fractal childA = CreateChild(Vector3.right);
        Fractal childB = CreateChild(Vector3.up);

        childA.transform.SetParent(transform, false);
        childB.transform.SetParent(transform, false);
    }

    private Fractal CreateChild(Vector3 direction)
    {
        Fractal child = Instantiate(this);
        //child.transform.parent = transform;
        child.transform.localPosition = 0.75f * direction;
        child.transform.localScale = 0.5f * Vector3.one;
        child.depth -= 1;
        return child;
    }
}
