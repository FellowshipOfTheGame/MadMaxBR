using UnityEngine;
using UnityEngine.UI;

public class LapTimeManager : MonoBehaviour {

    private int lapCounter;
    private int MinCountTotal;
    private int MinCountLap;
    private int minCountBest;
    private int SecCountTotal;
    private int SecCountLap;
    private int secCountBest;
    private float MilliCountTotal;
    private float MilliCountLap;
    private float milliCountBest;
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
        minCountBest = 9999;
        secCountBest = 9999;
        milliCountBest = 9999;
        lapCounter = 0;
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
        lapCounter++;
        LapCounterDisplay.GetComponent<Text>().text = "" + lapCounter;

        NewBest = false;

        if (MinCountLap < minCountBest) {
            NewBest = true;
        } else if (MinCountLap == minCountBest && SecCountLap < secCountBest) {
            NewBest = true;
        } else if (MinCountLap == minCountBest && SecCountLap == secCountBest && MilliCountLap < milliCountBest) {
            NewBest = true;
        }

        if (NewBest) {
            minCountBest = MinCountLap;
            secCountBest = SecCountLap;
            milliCountBest = MilliCountLap;
            UpdateTimeDisplay(secCountBest, minCountBest, milliCountBest, SecondsDisplayBest, MinutesDisplayBest, MillisDisplayBest);
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
