using System;
using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour, IDelayService {
    public static CoroutineRunner Instance { get; private set; }

    public void DelayAction(float delayTimeInMilliseconds, Action onComplete) {
        StartCoroutine(DelayCoroutine(delayTimeInMilliseconds, onComplete));
    }

    private IEnumerator DelayCoroutine(float delayTimeInSeconds, Action onComplete) {
        yield return new WaitForSeconds(delayTimeInSeconds);
        onComplete?.Invoke();
    }

    void Awake() {

        if (Instance == null) {
            Instance = this;
        }
    }
}
public interface IDelayService {
    void DelayAction(float delayTimeInMilliseconds, Action onComplete);
}