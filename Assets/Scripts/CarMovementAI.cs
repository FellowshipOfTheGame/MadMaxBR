using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovementAI : MonoBehaviour
{
    [Header("Sensors")]
    public Transform offset;
    public float sensorLength = 5f;
    public float frontSideSensorPosition = 1f;
    public float frontSensorAngle = 30f;

    [Header("Path to follow")]
    public Transform path;
    private List<BoxCollider> nodes;
    [SerializeField] private int currentNode = 0;

    private bool avoiding = false;
    private Vector2 acceleration = Vector2.zero;
    private Vector2 lastVelocity = Vector2.zero;

    
    [Header("AI variables")]
    public float steer = 0f;
    public float throttle = 0f;
    public bool brake = false;
    public bool nitroEnabled = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        BoxCollider[] pathTransforms = path.GetComponentsInChildren<BoxCollider>();
        nodes = new List<BoxCollider>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            nodes.Add(pathTransforms[i]);         
        }
    }

    void FixedUpdate()
    {
        Sensors();
        FollowPath();
        throttle = 0.1f;
        if (rb.velocity.x <= Mathf.Abs(Mathf.Epsilon) || rb.velocity.z <= Mathf.Abs(Mathf.Epsilon))
        {

        }
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = offset.position;
        avoiding = false;

        // front right sensor
        sensorStartPos += offset.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);

                Vector2 thisVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
                Vector2 otherVelocity;
                Vector2 otherAcceleration;

                if (hit.rigidbody)
                {
                    otherVelocity = new Vector2(hit.rigidbody.velocity.x, hit.rigidbody.velocity.z);
                    otherAcceleration = hit.transform.GetComponent<VehicleControl>().acceleration;
                }
                else
                {
                    otherVelocity = Vector2.zero;
                    otherAcceleration = Vector2.zero;
                }

                if (Vector2.Distance(TimeToCollisionV(thisVelocity, otherVelocity, acceleration, otherAcceleration, hit.distance), Vector2.zero) <= 5f)
                {
                    avoiding = true;
                    steer = Mathf.MoveTowards(steer, -1f, 0.2f);
                    throttle = -1f;
                    brake = true;

                    if (rb.velocity.x <= Mathf.Abs(5f) || rb.velocity.z <= Mathf.Abs(5f))
                    {
                        brake = false;
                    }
                }
            }
        }

        // front right angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);

                Vector2 thisVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
                Vector2 otherVelocity;
                Vector2 otherAcceleration;

                if (hit.rigidbody)
                {
                    otherVelocity = new Vector2(hit.rigidbody.velocity.x, hit.rigidbody.velocity.z);
                    otherAcceleration = hit.transform.GetComponent<VehicleControl>().acceleration;
                }
                else
                {
                    otherVelocity = Vector2.zero;
                    otherAcceleration = Vector2.zero;
                }

                if (Vector2.Distance(TimeToCollisionV(thisVelocity, otherVelocity, acceleration, otherAcceleration, hit.distance), Vector2.zero) <= 5f)
                {
                    avoiding = true;
                    steer = Mathf.MoveTowards(steer, -0.5f, 0.2f);
                    throttle = -1f;
                    brake = true;

                    if (rb.velocity.x <= Mathf.Abs(5f) || rb.velocity.z <= Mathf.Abs(5f))
                    {
                        brake = false;
                    }
                }
            }
        }

        // front left sensor;
        sensorStartPos -= 2 * offset.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                Vector2 thisVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
                Vector2 otherVelocity;
                Vector2 otherAcceleration;

                if (hit.rigidbody)
                {
                    otherVelocity = new Vector2(hit.rigidbody.velocity.x, hit.rigidbody.velocity.z);
                    otherAcceleration = hit.transform.GetComponent<VehicleControl>().acceleration;
                }
                else
                {
                    otherVelocity = Vector2.zero;
                    otherAcceleration = Vector2.zero;
                }

                if (Vector2.Distance(TimeToCollisionV(thisVelocity, otherVelocity, acceleration, otherAcceleration, hit.distance), Vector2.zero) <= 5f)
                {
                    avoiding = true;
                    steer = Mathf.MoveTowards(steer, 1f, 0.2f);
                    throttle = -1f;
                    brake = true;

                    if (rb.velocity.x <= Mathf.Abs(5f) || rb.velocity.z <= Mathf.Abs(5f))
                    {
                        brake = false;
                    }
                }
            }
        }

        // front left angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);

                Vector2 thisVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
                Vector2 otherVelocity;
                Vector2 otherAcceleration;

                if (hit.rigidbody)
                {
                    otherVelocity = new Vector2(hit.rigidbody.velocity.x, hit.rigidbody.velocity.z);
                    otherAcceleration = hit.transform.GetComponent<VehicleControl>().acceleration;
                }
                else
                {
                    otherVelocity = Vector2.zero;
                    otherAcceleration = Vector2.zero;
                }

                if (Vector2.Distance(TimeToCollisionV(thisVelocity, otherVelocity, acceleration, otherAcceleration, hit.distance), Vector2.zero) <= 5f)
                {
                    avoiding = true;
                    steer = Mathf.MoveTowards(steer, 0.5f, 0.2f);
                    throttle = -1f;
                    brake = true;

                    if (rb.velocity.x <= Mathf.Abs(5f) || rb.velocity.z <= Mathf.Abs(5f))
                    {
                        brake = false;
                    }
                }
            }
            brake = false;
        }

        // front center sensor

        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);

                Vector2 thisVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
                Vector2 otherVelocity;
                Vector2 otherAcceleration;

                if (hit.rigidbody)
                {
                    otherVelocity = new Vector2(hit.rigidbody.velocity.x, hit.rigidbody.velocity.z);
                    otherAcceleration = hit.transform.GetComponent<VehicleControl>().acceleration;
                }
                else
                {
                    otherVelocity = Vector2.zero;
                    otherAcceleration = Vector2.zero;
                }

                if (Vector2.Distance(TimeToCollisionV(thisVelocity, otherVelocity, acceleration, otherAcceleration, hit.distance), Vector2.zero) <= 5f)
                {
                    avoiding = true;
                    throttle = -1f;
                    brake = true;

                    if (hit.normal.x < 0)
                    {
                        steer = Mathf.MoveTowards(steer, -1f, 0.2f);
                    }
                    else
                    {
                        steer = Mathf.MoveTowards(steer, 1f, 0.2f);
                    }

                    if (rb.velocity.x <= Mathf.Abs(5f) || rb.velocity.z <= Mathf.Abs(5f))
                    {
                        brake = false;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Node") && currentNode == GetSiblingIndex(other.transform, other.transform.parent))
        {
            currentNode = (currentNode + 1) % nodes.Count;
        }
    }

    private void FollowPath()
    {
        if (avoiding) return;

        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].transform.position);
        float newSteer = relativeVector.x / relativeVector.magnitude;
        steer = Mathf.MoveTowards(steer, newSteer, 0.2f);
    }

    private float TimeToCollisionF(float initialVelocityA, float initialVelocityB, float accelerationA, float accelerationB, float distance)
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

    private Vector2 TimeToCollisionV(Vector2 initialVelocityA, Vector2 initialVelocityB, Vector2 accelerationA, Vector2 accelerationB, float distance)
    {
        float ax = accelerationB.x - accelerationA.x;
        float bx = 2 * (initialVelocityB.x - initialVelocityA.x);
        float c = -2 * distance;

        float timex = (-bx + Mathf.Sqrt(bx * bx - 4 * ax * c)) / (2 * ax);

        float ay = accelerationB.y - accelerationA.y;
        float by = 2 * (initialVelocityB.y - initialVelocityA.y);

        float timez = (-by + Mathf.Sqrt(by * by - 4 * ay * c)) / (2 * ay);

        if (timex < 0f && timez < 0f)
        {
            return new Vector2(Mathf.Infinity, Mathf.Infinity);
        }
        else if (timex < 0f)
        {
            return new Vector2(Mathf.Infinity, timez);
        }
        else if (timez < 0f)
        {
            return new Vector2(timex, Mathf.Infinity);
        }

        return new Vector2(timex, timez);
    }

    int GetSiblingIndex(Transform child, Transform parent)
    {
        for (int i = 0; i < parent.childCount; ++i)
        {
            if (child == parent.GetChild(i))
                return i;
        }
        Debug.LogWarning("Child doesn't belong to this parent.");
        return 0;
    }
}