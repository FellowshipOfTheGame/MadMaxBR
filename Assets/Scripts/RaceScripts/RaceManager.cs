using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour {
    public float NumberOfLaps;
    public List<GameObject> Racers;

    public void StartRace() {
        for (int i = 0; i < Racers.Count; i++) {
            Racers[i].GetComponent<CarController>().enabled = true; // active control for the racer 'i'
            Racers[i].GetComponent<VehicleRaceData>().enabled = true; // active the racer 'i' data script
        }
    }

    public void FinishRace() {

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
    }
}
