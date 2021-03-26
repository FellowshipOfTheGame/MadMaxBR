using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapCompleteTriggerManager : MonoBehaviour {

    public GameObject LapCompleteTrigger;
    public GameObject HalfLapTrigger;
    public GameObject MinutesDisplayBest;
    public GameObject SecondsDisplayBest;
    public GameObject MillisDisplayBest;
    public GameObject LapCounterDisplay;
    public GameObject LapTimeBox;

    private int MinCountBest;
    private int SecCountBest;
    private float MilliCountBest;
    private bool NewBest;
    private int LapCounter;

    private void Start() {
        MinCountBest = 100;
        SecCountBest = 100;
        MilliCountBest = 100;
        LapCounter = 0;
        LapCompleteTrigger.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        NewBest = false;

        if (LapTimeManager.MinCount < MinCountBest) {
            NewBest = true;
        } else if (LapTimeManager.MinCount == MinCountBest && LapTimeManager.SecCount < SecCountBest) {
            NewBest = true;
        } else if (LapTimeManager.MinCount == MinCountBest && LapTimeManager.SecCount == SecCountBest && LapTimeManager.MilliCount < MilliCountBest) {
            NewBest = true;
        }

        if (NewBest) {
            MinCountBest = LapTimeManager.MinCount;
            SecCountBest = LapTimeManager.SecCount;
            MilliCountBest = LapTimeManager.MilliCount;

            if (SecCountBest <= 9) {
                SecondsDisplayBest.GetComponent<Text>().text = "0" + SecCountBest + ".";
            } else {
                SecondsDisplayBest.GetComponent<Text>().text = "" + SecCountBest + ".";
            }

            if (MinCountBest <= 9) {
                MinutesDisplayBest.GetComponent<Text>().text = "0" + MinCountBest + ".";
            } else {
                MinutesDisplayBest.GetComponent<Text>().text = "" + MinCountBest + ".";
            }

            MillisDisplayBest.GetComponent<Text>().text = "" + MilliCountBest.ToString("F0");
        }

        LapCounter++;
        LapCounterDisplay.GetComponent<Text>().text = "" + LapCounter;

        LapTimeManager.MinCount = 0;
        LapTimeManager.SecCount = 0;
        LapTimeManager.MilliCount = 0;

        HalfLapTrigger.SetActive(true);
        LapCompleteTrigger.SetActive(false);
    }
}
