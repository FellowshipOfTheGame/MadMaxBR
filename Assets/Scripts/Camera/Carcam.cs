using UnityEngine;
using System.Collections;

public class Carcam : MonoBehaviour
{
    private Transform rootNode;
    private Transform cam;
    private Transform car;
    public Rigidbody rbCar;

    public float rotationThreshold = 1f;
    public float cameraStickness = 10f;
    public float cameraRotationSpeed = 5f;

    private void Awake()
    {
        rootNode = transform;
        cam = GetComponentInChildren<Camera>().transform;
        car = rootNode.parent.transform;
    }

    private void Start()
    {
        rootNode.parent = null;
    }

    private void FixedUpdate()
    {
        Quaternion lookAt;

        rootNode.position = Vector3.Lerp(rootNode.position, car.position, cameraStickness * Time.fixedDeltaTime);

        if (rbCar.velocity.magnitude < rotationThreshold)
        {
            lookAt = Quaternion.LookRotation(car.forward);
        }
        else
        {
            lookAt = Quaternion.LookRotation(rbCar.velocity.normalized);
        }

        lookAt = Quaternion.Slerp(rootNode.rotation, lookAt, cameraRotationSpeed * Time.fixedDeltaTime);
        rootNode.rotation = lookAt;
    }
}