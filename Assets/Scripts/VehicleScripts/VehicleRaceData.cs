using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleRaceData : MonoBehaviour {
    [HideInInspector]
    public Timer RaceTime;
    [HideInInspector]
    public Timer LapTime;

    private float lapCounter;
    private float racePosition;

    private float minCountBest;
    private float secCountBest;
    private float milliCountBest;

    private bool firstTrigger;
    public void LapCompleted() {
        lapCounter++;
        
        bool NewBest = false;

        if (LapTime.GetMinutes() < minCountBest) {
            NewBest = true;
        } else if (LapTime.GetMinutes() == minCountBest && LapTime.GetSeconds() < secCountBest) {
            NewBest = true;
        } else if (LapTime.GetMinutes() == minCountBest && LapTime.GetSeconds() == secCountBest && LapTime.GetMilliseconds() < milliCountBest) {
            NewBest = true;
        }

        if (NewBest) {
            minCountBest = LapTime.GetMinutes();
            secCountBest = LapTime.GetSeconds();
            milliCountBest = LapTime.GetMilliseconds();
        }

        LapTime.ResetTimer();
    }

    public float GetLapCount() {
        return lapCounter;
    }
    public float GetMinCountBest() {
        return minCountBest;
    }
    public float GetSecCountBest() {
        return secCountBest;
    }
    public float GetMilliCountBest() {
        return milliCountBest;
    }
    public float GetPosition() {
        return racePosition;
    }

    public void SetPosition(float pos) {
        racePosition = pos;
    }

    // Start is called before the first frame update
    void Start() {
        RaceTime = gameObject.AddComponent<Timer>();
        LapTime = gameObject.AddComponent<Timer>();
        minCountBest = 9999;
        secCountBest = 9999;
        milliCountBest = 9999;
        lapCounter = 0;
        firstTrigger = true;
    }

    private void OnTriggerEnter(Collider collider) {
        Debug.Log("PASSOU");
        if (collider.gameObject.CompareTag("Finish")) {
            if (firstTrigger) {
                firstTrigger = false;
            } else {
                LapCompleted();
            }
        }
    }
}
