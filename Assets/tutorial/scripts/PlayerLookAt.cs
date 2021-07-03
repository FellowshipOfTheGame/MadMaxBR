using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerLookAt : MonoBehaviour
{


    public float turnSpeed = 15f;
    public GameObject cinemachineFreeLookCamera;
    private Camera mainCamera;
    private bool isTargetMode = false;
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            if (isTargetMode)
            {
                mainCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
                mainCamera.transform.localPosition = new Vector3(0, 3, -5);
                cinemachineFreeLookCamera.SetActive(false);
            }
            else
            {
                cinemachineFreeLookCamera.transform.localPosition = new Vector3(0, 0, -1);
                cinemachineFreeLookCamera.SetActive(true);
            }
            isTargetMode = !isTargetMode;
        }

    }
    void Start()
    {
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        float xawCamera = mainCamera.transform.rotation.eulerAngles.x;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(xawCamera, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }
}