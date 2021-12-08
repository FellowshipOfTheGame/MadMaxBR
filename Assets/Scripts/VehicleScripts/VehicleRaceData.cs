using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleRaceData : MonoBehaviour {
    [HideInInspector]
    public Timer RaceTime;
    [HideInInspector]
    public Timer LapTime;

    public TrackerNode TrackerNode;
    public TriggerPoint TriggerPoint;

    private bool isTimerStarted;

    private float lapCounter;
    private float racePosition;

    private float minCountBest;
    private float secCountBest;
    private float milliCountBest;

    private float minCountTotal;
    private float secCountTotal;
    private float milliCountTotal;
    // controls if the car has completed the race or not
    private bool completedRace;
    // variable to control when the car reach the finish line for the first time, when the race starts
    private bool firstTrigger;

    public void ActiveTimer(bool val) {
        isTimerStarted = val;
    }

    // when a Lap is completed, the time of the best lap is stored
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
            minCountBest = RaceTime.GetMinutes();
            secCountBest = RaceTime.GetSeconds();
            milliCountBest = RaceTime.GetMilliseconds();
        }

        LapTime.ResetTimer();
    }
    // when the race is completed, the total time spent on the race is stored
    public void CompleteRace() {
        completedRace = true;
        minCountTotal = RaceTime.GetMinutes();
        secCountTotal = RaceTime.GetSeconds();
        milliCountTotal = RaceTime.GetMilliseconds();
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
    public float GetMinCountTotal() {
        return minCountTotal;
    }
    public float GetSecCountTotal() {
        return secCountTotal;
    }
    public float GetMilliCountTotal() {
        return milliCountTotal;
    }
    public float GetRacePosition() {
        return racePosition;
    }
    public bool HasCompletedRace() {
        return completedRace;
    }

    public void SetRacePosition(float pos) {
        racePosition = pos;
    }

    // Start is called before the first frame update
    void Start() {
        RaceTime = gameObject.AddComponent<Timer>();
        LapTime = gameObject.AddComponent<Timer>();
        minCountBest = 9999;
        secCountBest = 9999;
        milliCountBest = 9999;
        minCountTotal = 0;
        secCountTotal = 0;
        milliCountTotal = 0;
        lapCounter = 0;
        isTimerStarted = false;
        completedRace = false;
        firstTrigger = true;
    }

    private void Update() {
        if (!isTimerStarted) {
            RaceTime.ResetTimer();
            LapTime.ResetTimer();
        }
    }

    // if the car reaches the finish line
    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("InitialPointTrigger")) {
            if (TriggerPoint.gameObject.CompareTag("HalfPointTrigger")) {
                LapCompleted();
            }
            TriggerPoint = collider.gameObject.GetComponent<TriggerPoint>();
        } else if (collider.gameObject.CompareTag("HalfPointTrigger")) {
            TriggerPoint = collider.gameObject.GetComponent<TriggerPoint>();
        }
    }
}
