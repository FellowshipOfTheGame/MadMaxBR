using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{
    //[Range(0.0f, 100.0f)]
    //public float limiteRotacaoX = 45.0f;
    //[Range(0.0f, 100.0f)]
    //public float limiteRotacaoY = 45.0f;
    //Vector3 mouse;
    //Vector3 mouseWorld;
    //Vector3 forward;
    [Range(0.0f, 3.0f)]
    public float velocidadeRotacao = 0.5f;

    public Camera cam;
    public Transform rectTranform;
    private Vector3 rectPosition;
    private Vector3 rectNormal;
    private float forwardInput;
    private float rotationInput;
    private Vector3 finalTurretLookDir;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        // Cursor.visible = false;
    }
    private void FixedUpdate()
    {
        HandleInputs();
        HandleReticle();
        HandleTurret();
    }


    //void Update()
    //{
    //    mouse = Input.mousePosition;
    //    mouseWorld = cam.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 180));
    //    forward = mouseWorld - transform.position;
    //    //forward.x = 0;
    //    //forward.z = 0;
    //    // transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    //    if (transform.rotation.x > (limiteRotacaoY * -1) && transform.rotation.x < limiteRotacaoY)
    //    {
    //        transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    //    }
    //}

    public Vector3 RectPosition()
    {
        return rectPosition;
    }

    public Vector3 RectNomal()
    {
        return rectNormal;
    }

    public float ForwardInput()
    {
        return forwardInput;
    }

    public float RotationInput()
    {
        return forwardInput;
    }

    protected virtual void HandleInputs()
    {
        Ray screenRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(screenRay, out hit))
        {
            rectNormal = hit.normal;
            rectPosition = hit.point;
        }
        forwardInput = Input.GetAxis("Vertical");
        rotationInput = Input.GetAxis("Horizontal");
    }

    protected virtual void HandleTurret()
    {
        Vector3 turretLookDir = rectPosition - transform.position;
        finalTurretLookDir = Vector3.Lerp(finalTurretLookDir, turretLookDir, Time.deltaTime * velocidadeRotacao);
        transform.rotation = Quaternion.LookRotation(turretLookDir);
    }

    protected virtual void HandleReticle()
    {
        if (rectTranform)
        {
            rectTranform.position = rectPosition;
        }
    }

}
