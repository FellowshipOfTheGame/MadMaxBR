using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour {
    public float NumberOfLaps;
    public GameObject Player;
    public List<GameObject> Racers;
    // HUDs
    public GameObject GameHUD;
    public GameObject RaceResults;
    public void StartRace() {
        for (int i = 0; i < Racers.Count; i++) {
            Racers[i].GetComponent<CarController>().enabled = true; // active control for the racer 'i'
            Racers[i].GetComponent<VehicleRaceData>().enabled = true; // active the racer 'i' data script
        }
    }

    public void FinishRace() {
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
            RunnerRow.transform.GetChild(3).gameObject.GetComponent<Text>().text = VehicleInfo.GetKillsCount().ToString();
            // time

            string minCount;
        if (VehicleRaceInfo.GetMinCountTotal() <= 9) {
                secCount = VehicleRaceInfo.GetMinCountTotal().ToString();
        } else {
            SecondsDisplay.GetComponent<Text>().text = "" + SecCount + ".";
        }

        if (MinCount <= 9) {
            MinutesDisplay.GetComponent<Text>().text = "0" + MinCount + ":";
        } else {
            MinutesDisplay.GetComponent<Text>().text = "" + MinCount + ":";
        } 
            
            RunnerRow.transform.GetChild(4).gameObject.GetComponent<Text>().text = "" +  + ":" + VehicleRaceInfo.GetSecCountTotal().ToString() + "." + VehicleRaceInfo.GetMilliCountTotal().ToString("F0");
            RunnerRow.gameObject.SetActive(true);
        }
    }

    private void UpdateRacersPositions() {
        for (int i = 0; i < Racers.Count; i++) {
            for (int j = i + 1; j < Racers.Count; j++) {
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

    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < Racers.Count; i++) {
            Racers[i].GetComponent<VehicleRaceData>().SetRacePosition(i + 1);
        }
    }

    // Update is called once per frame
    void Update() {
        UpdateRacersPositions();
        for (int i = 0; i < Racers.Count; i++) {
            // if a car completes the last lap and hasnt completed the race yet
            if (!Racers[i].GetComponent<VehicleRaceData>().HasCompletedRace() && Racers[i].GetComponent<VehicleRaceData>().GetLapCount() == NumberOfLaps) {
                Racers[i].GetComponent<VehicleRaceData>().CompleteRace();
                UpdateRaceResultsTable();
                if (Racers[i].tag == "Player") {
                    FinishRace();
                }
            }
        }
    }
}
