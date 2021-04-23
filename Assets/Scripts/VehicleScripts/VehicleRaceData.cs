using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleRaceData : MonoBehaviour {
    [HideInInspector]
    public Timer RaceTime;
    [HideInInspector]
    public Timer LapTime;

    private int LapCounter;
    private int MinCountBest;
    private int SecCountBest;
    private float MilliCountBest;
    private bool firstTrigger;
    public void LapCompleted() {
        LapCounter++;
        
        bool NewBest = false;

        if (LapTime.GetMinutes() < MinCountBest) {
            NewBest = true;
        } else if (LapTime.GetMinutes() == MinCountBest && LapTime.GetSeconds() < SecCountBest) {
            NewBest = true;
        } else if (LapTime.GetMinutes() == MinCountBest && LapTime.GetSeconds() == SecCountBest && LapTime.GetMilliseconds() < MilliCountBest) {
            NewBest = true;
        }

        if (NewBest) {
            MinCountBest = LapTime.GetMinutes();
            SecCountBest = LapTime.GetSeconds();
            MilliCountBest = LapTime.GetMilliseconds();
        }

        LapTime.ResetTimer();
    }

    public int GetLapCount() {
        return LapCounter;
    }
    public int GetMinCountBest() {
        return MinCountBest;
    }
    public int GetSecCountBest() {
        return SecCountBest;
    }
    public float GetMilliCountBest() {
        return MilliCountBest;
    }

    // Start is called before the first frame update
    void Start() {
        RaceTime = gameObject.AddComponent<Timer>();
        LapTime = gameObject.AddComponent<Timer>();
        MinCountBest = 9999;
        SecCountBest = 9999;
        MilliCountBest = 9999;
        LapCounter = 0;
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
