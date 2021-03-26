using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class CarSpeed : MonoBehaviour {

    public GameObject CarSpeedDisplay;

    public CarController m_CarController;

    private int CarCurrentSpeed;
    // Update is called once per frame
    void Update() {
        CarCurrentSpeed = (int) m_CarController.CurrentSpeed;
        CarSpeedDisplay.GetComponent<Text>().text = "" + CarCurrentSpeed;
    }
}
