using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCameraSize : MonoBehaviour {
    public float ScaleFactor;
    // Update is called once per frame
    void Update() {
        this.gameObject.transform.position = new Vector3(this.transform.parent.gameObject.transform.position.x * ScaleFactor, this.gameObject.transform.position.y, this.transform.parent.gameObject.transform.position.z * ScaleFactor);
    }
}
