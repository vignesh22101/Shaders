using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimedDisabler : MonoBehaviour
{
    [SerializeField] public UnityEvent OnDisableEvent;
    [SerializeField] float duration=5f;

    public void OnEnable()
    {
        RestartTimer();
    }

    public void OnDisable()
    {
        OnDisableEvent?.Invoke();
        StopAllCoroutines();
    }

    public void RestartTimer()
    {
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}