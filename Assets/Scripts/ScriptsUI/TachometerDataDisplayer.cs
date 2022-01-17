using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TachometerDataDisplayer : MonoBehaviour {
    //public GameObject Player;
    public GameObject VelocityDisplay;
    public GameObject GearDisplay;
    public GameObject TachometerNeedle;

    private float thisAngle = -150f;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        int curGear = RaceManager.Instance.Player.GetComponent<CarController>().CurrentGear;
        float curSpeed = RaceManager.Instance.Player.GetComponent<CarController>().CurrentSpeed;

        VelocityDisplay.GetComponent<Text>().text = "" + (int)curSpeed;
        /*
        if (RaceManager.Instance.Player.GetComponent<CarController>().CurrentGear == -1) {
            GearDisplay.GetComponent<Text>().color = new Color(255, 255, 255);
            GearDisplay.GetComponent<Text>().text = "N";
        } else if (RaceManager.Instance.Player.GetComponent<CarController>().CurrentGear == -2) {
            GearDisplay.GetComponent<Text>().color = new Color(255, 0, 0);
            GearDisplay.GetComponent<Text>().text = "R";
        } else {
            GearDisplay.GetComponent<Text>().color = new Color(0, 255, 0);
            GearDisplay.GetComponent<Text>().text = "" + (RaceManager.Instance.Player.GetComponent<CarController>().CurrentGear + 1);
        }
        */
        if (curGear > 0 && curSpeed > 1) {
            GearDisplay.GetComponent<Text>().color = Color.green;
            GearDisplay.GetComponent<Text>().text = curGear.ToString();
        } else if (curSpeed > 1) {
            GearDisplay.GetComponent<Text>().color = Color.red;
            GearDisplay.GetComponent<Text>().text = "R";
        } else {
            GearDisplay.GetComponent<Text>().color = Color.white;
            GearDisplay.GetComponent<Text>().text = "N";
        }

        //TachometerNeedle.GetComponent<Image>().rectTransform.rotation = Quaternion.Euler(0, 0, 180 - (225 * RaceManager.Instance.Player.GetComponent<CarController>().Revs));
        thisAngle = (RaceManager.Instance.Player.GetComponent<CarController>().MotorRPM / 20) - 175;
        thisAngle = Mathf.Clamp(thisAngle, -180, 90);

        TachometerNeedle.GetComponent<Image>().rectTransform.rotation = Quaternion.Euler(0, 0, -thisAngle);
    }
}
