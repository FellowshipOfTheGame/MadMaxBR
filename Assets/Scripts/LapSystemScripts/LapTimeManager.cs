using UnityEngine;
using UnityEngine.UI;

public class LapTimeManager : MonoBehaviour {

    private int LapCounter;
    private int MinCountTotal;
    private int MinCountLap;
    private int MinCountBest;
    private int SecCountTotal;
    private int SecCountLap;
    private int SecCountBest;
    private float MilliCountTotal;
    private float MilliCountLap;
    private float MilliCountBest;
    private bool NewBest;

    public GameObject LapCompleteTrigger;
    public GameObject HalfLapTrigger;
    public GameObject LapCounterDisplay;
    public GameObject MinutesDisplayTime;
    public GameObject SecondsDisplayTime;
    public GameObject MillisDisplayTime;
    public GameObject MinutesDisplayLap;
    public GameObject SecondsDisplayLap;
    public GameObject MillisDisplayLap;
    public GameObject MinutesDisplayBest;
    public GameObject SecondsDisplayBest;
    public GameObject MillisDisplayBest;

    private void Start() {
        MinCountTotal = 0;
        SecCountTotal = 0;
        MilliCountTotal = 0;
        MinCountLap = 0;
        SecCountLap = 0;
        MilliCountLap = 0;
        MinCountBest = 9999;
        SecCountBest = 9999;
        MilliCountBest = 9999;
        LapCounter = 0;
    }

    void UpdateTimeDisplay(int SecCount, int MinCount, float MilliCount, GameObject SecondsDisplay, GameObject MinutesDisplay, GameObject MillisecondsDisplay) {
        MillisecondsDisplay.GetComponent<Text>().text = "" + MilliCount.ToString("F0");

        if (SecCount <= 9) {
            SecondsDisplay.GetComponent<Text>().text = "0" + SecCount + ".";
        } else {
            SecondsDisplay.GetComponent<Text>().text = "" + SecCount + ".";
        }

        if (MinCount <= 9) {
            MinutesDisplay.GetComponent<Text>().text = "0" + MinCount + ":";
        } else {
            MinutesDisplay.GetComponent<Text>().text = "" + MinCount + ":";
        }
    }

    public void LapCompleted() {
        LapCounter++;
        LapCounterDisplay.GetComponent<Text>().text = "" + LapCounter;

        NewBest = false;

        if (MinCountLap < MinCountBest) {
            NewBest = true;
        } else if (MinCountLap == MinCountBest && SecCountLap < SecCountBest) {
            NewBest = true;
        } else if (MinCountLap == MinCountBest && SecCountLap == SecCountBest && MilliCountLap < MilliCountBest) {
            NewBest = true;
        }

        if (NewBest) {
            MinCountBest = MinCountLap;
            SecCountBest = SecCountLap;
            MilliCountBest = MilliCountLap;
            UpdateTimeDisplay(SecCountBest, MinCountBest, MilliCountBest, SecondsDisplayBest, MinutesDisplayBest, MillisDisplayBest);
        }

        MinCountLap = 0;
        SecCountLap = 0;
        MilliCountLap = 0;

        HalfLapTrigger.SetActive(true);
        LapCompleteTrigger.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        // counts how many milliseconds have passed since the last frame
        MilliCountLap += Time.deltaTime * 1000;
        MilliCountTotal += MilliCountLap;

        if (MilliCountLap >= 1000) {
            MilliCountLap = 0;
            SecCountLap++;
            MilliCountTotal = 0;
            SecCountTotal++;
        }
        // seconds counting for the lap timer
        if (SecCountLap >= 60) {
            SecCountLap = 0;
            MinCountLap++;

        }
        // seconds counting for the time timer
        if (SecCountTotal >= 60) {
            SecCountTotal = 0;
            MinCountTotal++;
        }
        // put the timers on its respective display
        UpdateTimeDisplay(SecCountLap, MinCountLap, MilliCountLap, SecondsDisplayLap, MinutesDisplayLap, MillisDisplayLap);
        UpdateTimeDisplay(SecCountTotal, MinCountTotal, MilliCountTotal, SecondsDisplayTime, MinutesDisplayTime, MillisDisplayTime);
    }
}
