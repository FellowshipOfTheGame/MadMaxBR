using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This Script is responsible to show on the UI information about the player in the race, as time, position and laps completed.
/// </summary>
public class RaceTimeDisplayer : MonoBehaviour {
    public GameObject Player;
    //public GameObject RaceManager;

    //public GameObject LapCounterDisplay;
    //public GameObject MaxNumberOfLaps;
    //public GameObject RacePositionDisplay;
    //public GameObject MaxRacePositionDisplay;
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

    void UpdateTimeDisplay(float SecCount, float MinCount, float MilliCount, GameObject SecondsDisplay, GameObject MinutesDisplay, GameObject MillisecondsDisplay) {
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

    private void Awake() {
        
    }
    private void Start() {
        PlayerRaceData = Player.GetComponent<VehicleRaceData>();
    }

    // Update is called once per frame
    void Update() {
        //LapCounterDisplay.GetComponent<Text>().text = "" + PlayerRaceData.GetLapCount();
        //MaxRacePositionDisplay.GetComponent<Text>().text = "" + RaceManager.GetComponent<RaceManager>().Racers.Count;
        //MaxNumberOfLaps.GetComponent<Text>().text = "" + RaceManager.GetComponent<RaceManager>().NumberOfLaps;
        UpdateTimeDisplay(PlayerRaceData.RaceTime.GetSeconds(), PlayerRaceData.RaceTime.GetMinutes(), PlayerRaceData.RaceTime.GetMilliseconds(), SecondsDisplayTime, MinutesDisplayTime, MillisDisplayTime);
        UpdateTimeDisplay(PlayerRaceData.LapTime.GetSeconds(), PlayerRaceData.LapTime.GetMinutes(), PlayerRaceData.LapTime.GetMilliseconds(), SecondsDisplayLap, MinutesDisplayLap, MillisDisplayLap);
        if (PlayerRaceData.GetMinCountBest() != 9999) {
            UpdateTimeDisplay(PlayerRaceData.GetSecCountBest(), PlayerRaceData.GetMinCountBest(), PlayerRaceData.GetMilliCountBest(), SecondsDisplayBest, MinutesDisplayBest, MillisDisplayBest);
        }
    }
}
