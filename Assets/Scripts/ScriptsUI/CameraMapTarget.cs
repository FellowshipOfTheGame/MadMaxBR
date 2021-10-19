using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMapTarget : MonoBehaviour {
    public GameObject TargetCar;

    // Update is called once per frame
    void Update() {
        this.gameObject.transform.position = new Vector3(TargetCar.transform.position.x, this.gameObject.transform.position.y, TargetCar.transform.position.z);   
    }
}
