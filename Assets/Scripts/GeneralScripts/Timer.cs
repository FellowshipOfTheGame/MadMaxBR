using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    private float MinCount;
    private float SecCount;
    private float MilliCount;

    private bool isTimerPaused;

    // Start is called before the first frame update
    void Start() {
        ResetTimer();
        isTimerPaused = false;
    }

     // Update is called once per frame
    void Update() {
        if (!isTimerPaused) {
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
    }

    public void ResetTimer() {
        MinCount = 0;
        SecCount = 0;
        MilliCount = 0;
    }

    public void PauseTimer() {
        isTimerPaused = true;
    }

    public void ResumeTimer() {
        isTimerPaused = false;
    }

    public float GetMilliseconds() {
        return MilliCount;
    }

    public float GetSeconds() {
        return SecCount;
    }

    public float GetMinutes() {
        return MinCount;
    }

    public float GetTimeInSeconds() {
        return SecCount + MinCount * 60;
    }
}
