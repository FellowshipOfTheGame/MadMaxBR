using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class InitialPointTriggerManager : MonoBehaviour {

    public GameObject InitialPointTrigger;
    public GameObject HalfLapTrigger;

    [HideInInspector]
    public bool firstActivation;
    private void Start() {
        firstActivation = true;
    }

    private void OnTriggerEnter(Collider col) {
        if (firstActivation) {
            firstActivation = false;
            HalfLapTrigger.SetActive(true);
            InitialPointTrigger.SetActive(false);
        } else {
            HalfLapTrigger.SetActive(true);
            InitialPointTrigger.SetActive(false);
        }
    }
}
