using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTimeManager : MonoBehaviour {

    public static int MinCount;
    public static int SecCount;
    public static float MilliCount;

    public GameObject MinutesBox;
    public GameObject SecondsBox;
    public GameObject MillisBox;

    // Update is called once per frame
    void Update() {
        // counts how many milliseconds have passed since the last frame and convert it into text
        MilliCount += Time.deltaTime * 100; 
        MillisBox.GetComponent<Text>().text = "" + MilliCount.ToString("F0");

        if (MilliCount >= 100) {
            MilliCount = 0;
            SecCount++;
        }

        if (SecCount <= 9) {
            SecondsBox.GetComponent<Text>().text = "0" + SecCount + ".";
        } else {
            SecondsBox.GetComponent<Text>().text = "" + SecCount + ".";
        }

        if (SecCount >= 60) {
            SecCount = 0;
            MinCount++;
        }

        if (MinCount <= 9) {
            MinutesBox.GetComponent<Text>().text = "0" + MinCount + ":";
        }
        else {
            MinutesBox.GetComponent<Text>().text = "" + MinCount + ":";
        }

    }
}
