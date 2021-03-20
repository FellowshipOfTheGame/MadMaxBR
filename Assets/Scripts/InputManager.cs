using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float throttle;
    public float steer;
    public bool braking;


    void Update()
    {
        throttle = Input.GetAxis("Vertical");
        steer = Input.GetAxis("Horizontal");
        braking = Input.GetKey(KeyCode.Space);
    }
}
