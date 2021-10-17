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

    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < Racers.Count; i++) {
            Racers[i].GetComponent<VehicleRaceData>().SetPosition(i + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
