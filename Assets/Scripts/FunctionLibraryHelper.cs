using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionLibraryHelper : MonoBehaviour
{
    [SerializeField] float timeMultiplier;
    [SerializeField] bool applyValues;

    private void OnValidate()
    {
        HandleValues();
    }

    private void HandleValues()
    {
        if (applyValues)
        {
            FunctionLibrary.timeMultiplier = timeMultiplier;
        }
    }
}
