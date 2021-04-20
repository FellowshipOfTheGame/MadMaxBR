using UnityEngine;
using UnityEngine.UI;

public class RaceDataDisplayer : MonoBehaviour {
    public GameObject Player;

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

    private VehicleRaceData PlayerRaceData;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

    private void Start() {
        PlayerRaceData = Player.transform.GetChild(0).GetComponent<VehicleRaceData>();
    }

    // Update is called once per frame
    void Update() {
        LapCounterDisplay.GetComponent<Text>().text = "" + PlayerRaceData.GetLapCount();
        UpdateTimeDisplay(PlayerRaceData.RaceTime.GetSeconds(), PlayerRaceData.RaceTime.GetMinutes(), PlayerRaceData.RaceTime.GetMilliseconds(), SecondsDisplayTime, MinutesDisplayTime, MillisDisplayTime);
        UpdateTimeDisplay(PlayerRaceData.LapTime.GetSeconds(), PlayerRaceData.LapTime.GetMinutes(), PlayerRaceData.LapTime.GetMilliseconds(), SecondsDisplayLap, MinutesDisplayLap, MillisDisplayLap);
        if (PlayerRaceData.GetMinCountBest() != 9999) {
            UpdateTimeDisplay(PlayerRaceData.GetSecCountBest(), PlayerRaceData.GetMinCountBest(), PlayerRaceData.GetMilliCountBest(), SecondsDisplayBest, MinutesDisplayBest, MillisDisplayBest);
        }
    }
}
