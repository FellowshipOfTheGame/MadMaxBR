using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialPointTriggerManager : MonoBehaviour {

    public GameObject InitialPointTrigger;
    public GameObject HalfLapTrigger;
    public GameObject LapTimeManager;
    [HideInInspector]
    public bool firstActivation;
    private void Start() {
        firstActivation = true;
    }

    private void OnTriggerEnter() {
        if (firstActivation) {
            firstActivation = false;
            HalfLapTrigger.SetActive(true);
            InitialPointTrigger.SetActive(false);
        } else {
            //LapTimeManager.GetComponent<RaceDataDisplayer>().LapCompleted();
            HalfLapTrigger.SetActive(true);
            InitialPointTrigger.SetActive(false);
        }
    }
}
