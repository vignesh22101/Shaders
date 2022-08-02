using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphCreator : MonoBehaviour
{
    [SerializeField] protected int resolution;
    [SerializeField] GameObject prefab;
    [SerializeField] Transform parent;
    [SerializeField] protected float minPos, maxPos;
    [SerializeField] List<GameObject> prefabsList = new List<GameObject>();
    [SerializeField] protected float wavesMultiplier = 1, heightMultiplier = 1;
    [SerializeField, Range(1, 2)] int function;
    private void Start()
    {
        HandleGraphCreation();
    }

    protected void HandleGraphCreation()
    {
        ClearAndRestart();
        CreateGraph();
    }

    //Clears the existing prefabs and creates new one
    private void ClearAndRestart()
    {
        prefabsList.Clear();
        if (parent != null)
            Destroy(parent.gameObject);
        parent = new GameObject("Graph").transform;
    }

    private void CreateGraph()
    {
        for (int i = 0; i < resolution; i++)
        {
            GameObject instantiatedPrefab = GameObject.Instantiate(prefab, parent);
            Vector3 position = Vector3.zero;
            Vector3 scale = ((maxPos - minPos) / resolution) * Vector3.one;
            instantiatedPrefab.transform.localScale = scale;
            position.x = i * scale.x + (minPos);
            position.y = Mathf.Sin(i / (resolution - 1)) * scale.y;
            instantiatedPrefab.transform.position = position;
            prefabsList.Add(instantiatedPrefab);
        }
    }

    private void Update()
    {
        for (int i = 0; i < prefabsList.Count; i++)
        {
            Transform prefab = prefabsList[i].transform;
            Vector3 newPos = prefab.position;
            newPos.y = Function(newPos.x);
            prefab.position = newPos;
        }
    }

    private float Function(float x)
    {
        if (function == 1) return FunctionLibrary.Wave(x, Time.time);
        else if (function == 2) return FunctionLibrary.MultiWave(x, Time.time);
        return 0;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying)
            HandleGraphCreation();
    }
#endif
}
