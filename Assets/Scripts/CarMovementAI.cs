using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovementAI : MonoBehaviour
{
    [Header("Sensors")]
    public Transform offset;
    public float sensorLength = 3f;
    //public Vector3 frontSensorPosition = new Vector3(0f, 1f, 0.5f);
    public float frontSideSensorPosition = 0.2f;
    public float frontSensorAngle = 30f;

    [Header("Path to follow")]
    public Transform path;
    private List<Transform> nodes;
    private int currentNode = 0;

    private bool avoiding = false;
    private Vector3 acceleration = Vector3.zero;
    private Vector3 lastVelocity = Vector3.zero;

    
    [Header("AI variables")]
    public float steer = 0f;
    public float throttle = 0f;
    public bool brake = false;
    public bool nitroEnabled = false;

    private Rigidbody rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    void FixedUpdate()
    {
        acceleration = (rb.velocity - lastVelocity) / Time.fixedDeltaTime;
        lastVelocity = rb.velocity;

        Sensors();
        FollowPath();
        CheckWaypointDistance();
        throttle = 1f;
        /*
        if (TimeToCollision() < 5f)
        {
            brake = true;
        }
        */
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = offset.position;
        float avoidMultiplier = 0f;
        avoiding = false;

        // front right sensor
        sensorStartPos += offset.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 1f;
            }
        }

        // front right angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 0.5f;
            }
        }

        // front left sensor;
        sensorStartPos -= 2 * offset.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 1f;
            }
        }

        // front left angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 0.5f;
            }
        }

        // front center sensor
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;

                    if (hit.normal.x < 0)
                    {
                        avoidMultiplier = -1f;
                    }
                    else
                    {
                        avoidMultiplier = 1f;
                    }
                }
            }
        }
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 1f)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }

    private void FollowPath()
    {
        if (avoiding) return;

        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = relativeVector.x / relativeVector.magnitude;
        steer = Mathf.MoveTowards(steer, newSteer, 0.2f);
    }

    private float TimeToCollision(float initialVelocityA, float initialVelocityB, float accelerationA, float accelerationB, float distance)
    {
        float a = accelerationB - accelerationA;
        float b = 2 * (initialVelocityB - initialVelocityA);
        float c = -2 * distance;

        float time = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);

        if (time < 0)
        {
            return Mathf.Infinity;
        }

        return time;
    }
}