using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GraphManagement : GraphCreator
{
    public Slider resolutionSlider;
    public Slider wavesAmountSlider;
    public Slider heightSlider;
    public Button randomValuesButton;
    // public Slider leftBoundarySlider, rightBoundarySlider;
    [SerializeField] int resolutionMin, resolutionMax;
    [SerializeField] float wavesAmountMin,wavesAmountMax;
    [SerializeField] float heightMin,heightMax;

    private void Awake()
    {
        InitializeSliders();
    }

    private void InitializeSliders()
    {
        resolutionSlider.minValue = resolutionMin;
        resolutionSlider.maxValue = resolutionMax;
        heightSlider.minValue = heightMin;
        heightSlider.maxValue = heightMax;
        wavesAmountSlider.minValue = wavesAmountMin;
        wavesAmountSlider.maxValue = wavesAmountMax;
    }

    private void OnEnable()
    {
        resolutionSlider.onValueChanged.AddListener(OnResolutionChanged);
        wavesAmountSlider.onValueChanged.AddListener(OnWavesAmountChanged);
        heightSlider.onValueChanged.AddListener(OnHeightChanged);
        randomValuesButton.onClick.AddListener(OnRandomValues);
    }
       

    private void OnDisable()
    {
        resolutionSlider.onValueChanged.RemoveListener(OnResolutionChanged);
        wavesAmountSlider.onValueChanged.RemoveListener(OnHeightChanged);
        heightSlider.onValueChanged.RemoveListener(OnHeightChanged);
    }

    private void OnHeightChanged(float arg0)
    {
        heightMultiplier = Mathf.Clamp(arg0,heightMin,heightMax);
    }

    private void OnResolutionChanged(float arg0)
    {
        resolution =(int) Mathf.Clamp(arg0, resolutionMin, resolutionMax);
    }
    private void OnWavesAmountChanged(float arg0)
    {
        wavesMultiplier = Mathf.Clamp(arg0, wavesAmountMin, wavesAmountMax);
    }
    private void OnRandomValues()
    {
        OnResolutionChanged(Random.Range(resolutionMin,resolutionMax));
        OnWavesAmountChanged(Random.Range(wavesAmountMin,wavesAmountMax));
        OnHeightChanged(Random.Range(heightMin,heightMax));
    }
}