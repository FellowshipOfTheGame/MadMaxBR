using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPU : MonoBehaviour {
    //private GameObject CarTarget; 
    
    // Start is called before the first frame update
    void Start() {
        
    }

    public void Activate() {
        GameObject CarTarget = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached
        CarTarget.GetComponent<VehicleData>().SetCurrentShield(500);
    }
}
