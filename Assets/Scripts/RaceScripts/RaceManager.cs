using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour {

    public static RaceManager Instance { get; private set; }

    public float NumberOfLaps;
    public GameObject Player;
    public List<GameObject> Racers;
    // HUDs
    public GameObject GameHUD;
    public GameObject RaceResults;
    public GameObject DeathScreen;

    private void Awake() {
        Instance = this;
    }

    public void StartRace() {
        for (int i = 0; i < Racers.Count; i++) {
            Racers[i].GetComponent<CarUserControl>().ControlActive = true; // active control for the racer 'i'
            Racers[i].GetComponent<VehicleRaceData>().ActiveTimer(true); // start timer of race data of vehicle
        }
    }
    /// <summary>
    /// Called when the player finishes the race. This function sets an AI to control the player's car
    /// and show a table with all the racers and their information: Name, Car Name, Time and Kills.
    /// </summary>
    public void FinishRace() {
        // sets AI on player car
        Player.GetComponent<CarUserControl>().SetAIControl(true);
        Player.transform.GetChild(4).gameObject.SetActive(true);

        // show Final results
        GameHUD.gameObject.SetActive(false);
        RaceResults.gameObject.SetActive(true);
    }
    private void UpdateRaceResultsTable() {
        GameObject RunnersList = RaceResults.transform.GetChild(0).gameObject;
        for (int i = 0; i < Racers.Count; i++) {
            int position = i + 1;
            VehicleData VehicleInfo = Racers[i].GetComponent<VehicleData>();
            VehicleRaceData VehicleRaceInfo = Racers[i].GetComponent<VehicleRaceData>();
            GameObject RunnerRow = RunnersList.transform.GetChild(position).gameObject;
            // position
            if (position < 10) {
                RunnerRow.transform.GetChild(0).gameObject.GetComponent<Text>().text = "0" + position.ToString();
            } else {
                RunnerRow.transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + position.ToString();
            }
            // runner name
            RunnerRow.transform.GetChild(1).gameObject.GetComponent<Text>().text = VehicleInfo.RunnerName;
            // car name
            RunnerRow.transform.GetChild(2).gameObject.GetComponent<Text>().text = Racers[i].name;
            // kills
            RunnerRow.transform.GetChild(3).gameObject.GetComponent<Text>().text = VehicleInfo.KillsCount.ToString();
            // time
            if (!VehicleInfo.IsDead()) {
                string MinCount;
                string SecCount;
                string MilliCount;
                if (VehicleRaceInfo.HasCompletedRace()) {
                    if (Racers[i].CompareTag("Player")) {
                        RunnerRow.GetComponent<Image>().color = new Color32(0, 0, 200, 100);
                    }
                    if (VehicleRaceInfo.GetMinCountTotal() <= 9) {
                        MinCount = "0" + VehicleRaceInfo.GetMinCountTotal().ToString();
                    } else {
                        MinCount = VehicleRaceInfo.GetMinCountTotal().ToString();
                    }
                
                    if (VehicleRaceInfo.GetSecCountTotal() <= 9) {
                        SecCount = "0" + VehicleRaceInfo.GetSecCountTotal().ToString();
                    } else {
                        SecCount = VehicleRaceInfo.GetSecCountTotal().ToString();
                    }
                
                    if (VehicleRaceInfo.GetMilliCountTotal() < 10) {
                        MilliCount = "00" + VehicleRaceInfo.GetMilliCountTotal().ToString("F0");
                    } else if (VehicleRaceInfo.GetMilliCountTotal() < 100 && VehicleRaceInfo.GetMilliCountTotal() >= 10) {
                        MilliCount = "0" + VehicleRaceInfo.GetMilliCountTotal().ToString("F0");
                    } else {
                        MilliCount = "" + VehicleRaceInfo.GetMilliCountTotal().ToString("F0");
                    }
                } else {
                    if (VehicleRaceInfo.RaceTime.GetMinutes() <= 9) {
                        MinCount = "0" + VehicleRaceInfo.RaceTime.GetMinutes().ToString();
                    } else {
                        MinCount = VehicleRaceInfo.RaceTime.GetMinutes().ToString();
                    }

                    if (VehicleRaceInfo.RaceTime.GetSeconds() <= 9) {
                        SecCount = "0" + VehicleRaceInfo.RaceTime.GetSeconds().ToString();
                    } else {
                        SecCount = VehicleRaceInfo.RaceTime.GetSeconds().ToString();
                    }

                    if (VehicleRaceInfo.RaceTime.GetMilliseconds() < 10) {
                        MilliCount = "00" + VehicleRaceInfo.RaceTime.GetMilliseconds().ToString("F0");
                    } else if (VehicleRaceInfo.RaceTime.GetMilliseconds() < 100 && VehicleRaceInfo.RaceTime.GetMilliseconds() >= 10) {
                        MilliCount = "0" + VehicleRaceInfo.RaceTime.GetMilliseconds().ToString("F0");
                    } else {
                        MilliCount = "" + VehicleRaceInfo.RaceTime.GetMilliseconds().ToString("F0");
                    }
                }
                RunnerRow.transform.GetChild(4).gameObject.GetComponent<Text>().text = "" + MinCount + ":" + SecCount + "." + MilliCount;
            } else {
                RunnerRow.GetComponent<Image>().color = new Color32(255,0,0,100);
                RunnerRow.transform.GetChild(4).gameObject.GetComponent<Text>().text = "ELIMINADO";
                //RunnerRow.transform.GetChild(4).gameObject.GetComponent<Text>().color = new Color32(255, 0, 0, 100);
            }
            RunnerRow.gameObject.SetActive(true);
        }
    }

    private void UpdateRacersPositions() {
        for (int i = 0; i < Racers.Count; i++) {
            for (int j = i + 1; j < Racers.Count; j++) {
                if (!Racers[i].GetComponent<VehicleRaceData>().HasCompletedRace() && !Racers[j].GetComponent<VehicleRaceData>().HasCompletedRace()) {
                    // if last tracker node of car i and last tracker node of car j are the same
                    if (Racers[j].GetComponent<VehicleRaceData>().TrackerNode == Racers[i].GetComponent<VehicleRaceData>().TrackerNode) {
                        // if car j passed car i, change their race positions and resort list of racers
                        if (Racers[j].GetComponent<VehicleRaceData>().TrackerNode.GetDistance(Racers[j]) > Racers[i].GetComponent<VehicleRaceData>().TrackerNode.GetDistance(Racers[i]) && Racers[j].GetComponent<VehicleRaceData>().GetRacePosition() > Racers[i].GetComponent<VehicleRaceData>().GetRacePosition() && Racers[j].GetComponent<VehicleRaceData>().GetLapCount() == Racers[i].GetComponent<VehicleRaceData>().GetLapCount()) {
                            float aux = Racers[i].GetComponent<VehicleRaceData>().GetRacePosition();
                            Racers[i].GetComponent<VehicleRaceData>().SetRacePosition(Racers[j].GetComponent<VehicleRaceData>().GetRacePosition());
                            Racers[j].GetComponent<VehicleRaceData>().SetRacePosition(aux);
                            // sort list of racers based on race position
                            Racers.Sort(delegate (GameObject car1, GameObject car2) {
                                if (car1.GetComponent<VehicleRaceData>().GetRacePosition() > car2.GetComponent<VehicleRaceData>().GetRacePosition()) {
                                    return 1;
                                } else {
                                    return -1;
                                }
                            });
                        }
                    }
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < Racers.Count; i++) {
            Racers[i].GetComponent<VehicleRaceData>().SetRacePosition(i + 1);
        }
    }

    // Update is called once per frame
    void Update() {
        if (Player.GetComponent<VehicleData>().isDead) {
            if (!DeathScreen.activeSelf) {
                GameHUD.SetActive(false);
                DeathScreen.SetActive(true);
            }
        } else {
            UpdateRacersPositions();
            for (int i = 0; i < Racers.Count; i++) {
                // if a car completes the last lap and hasnt completed the race yet
                if (!Racers[i].GetComponent<VehicleRaceData>().HasCompletedRace() && Racers[i].GetComponent<VehicleRaceData>().GetLapCount() == NumberOfLaps) {
                    Racers[i].GetComponent<VehicleRaceData>().CompleteRace();
                    if (Racers[i].tag == "Player") {
                        FinishRace();
                    }
                }
            }
            UpdateRaceResultsTable();
        }
    }
}
