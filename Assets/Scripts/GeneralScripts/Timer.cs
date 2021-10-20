using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    private float MinCount;
    private float SecCount;
    private float MilliCount;

    // Start is called before the first frame update
    void Start() {
        ResetTimer();
    }

     // Update is called once per frame
    void Update() {
        // counts how many milliseconds have passed since the last frame
        MilliCount += Time.deltaTime * 1000;

        if (MilliCount >= 1000) {
            MilliCount = 0;
            SecCount++;
        }

        if (SecCount >= 60) {
            SecCount = 0;
            MinCount++;
        }
    }

    public void ResetTimer() {
        MinCount = 0;
        SecCount = 0;
        MilliCount = 0;
    }

    public float GetMilliseconds() {
        return (int)MilliCount;
    }

    public float GetSeconds() {
        return (int)SecCount;
    }

    public float GetMinutes() {
        return (int)MinCount;
    }

    public float GetTimeInSeconds() {
        return SecCount + MinCount * 60;
    }
}
