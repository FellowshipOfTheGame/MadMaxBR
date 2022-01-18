using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script Destroys the gameObject this script is attached to after a determined time.
/// </summary>
public class DestroyTimer : MonoBehaviour {
    public float TimeInSeconds;

    // Update is called once per frame
    void Awake() {
        Destroy(gameObject, TimeInSeconds);
    }
}
