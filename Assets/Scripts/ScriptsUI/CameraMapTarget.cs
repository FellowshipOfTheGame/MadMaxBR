using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMapTarget : MonoBehaviour {

    // Update is called once per frame
    void Update() {
        this.gameObject.transform.position = new Vector3(RaceManager.Instance.Player.transform.position.x, this.gameObject.transform.position.y, RaceManager.Instance.Player.transform.position.z);   
    }
}
