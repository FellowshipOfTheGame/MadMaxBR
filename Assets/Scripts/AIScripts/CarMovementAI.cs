using System.Collections.Generic;
using UnityEngine;

public enum AiState
{
    FollowingTrack = 1, Avoiding = 2, Overtaking = 3
}

public class CarMovementAI : MonoBehaviour
{
    [Header("Sensors")]
    [SerializeField] private Transform[] sensorsTransforms;
    [SerializeField] private float sensorLength = 5f;
    [SerializeField] private float horizontalSensorLength = 2f;
    [SerializeField] private float frontSensorInnerAngle = 15f;
    [SerializeField] private float frontSensorOuterAngle = 30f;
    [SerializeField] private LayerMask ignoredLayerMasks;

    [Header("Path to follow")]
    public Transform path;
    public int currentNode = 0;
    [SerializeField] private float trackWidth = 15f;
    private List<BoxCollider> nodes;
    
    [Header("AI variables")]
    public float steer = 0f;
    public float throttle = 0f;
    public bool brake = false;
    public bool nitroEnabled = false;
    [SerializeField] private float minPositiveThrottle = 0.03f;
    [SerializeField] private float maxThrottle = 1f;
    [SerializeField] private float maxAngleForMinThrottle = 35f;
    [SerializeField] private float minDistanceToReverse = 3f;
    [SerializeField] private float distanceFromTrackToBreak = 30f;

    private Rigidbody rb;

    public AiState aiState = AiState.FollowingTrack;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        BoxCollider[] nodesBoxCollider = path.GetComponentsInChildren<BoxCollider>();
        nodes = new List<BoxCollider>();

        for (int i = 0; i < nodesBoxCollider.Length; i++)
        {
            nodes.Add(nodesBoxCollider[i]);
        }
    }

    void FixedUpdate()
    {
        Sensors();
        
        if (aiState == AiState.FollowingTrack)
        {
            FollowPath();
            GetThrottle();
        }
    }

    public void GetThrottle()
    {
        float curveAngle = CurveAngle();

        if (curveAngle > maxAngleForMinThrottle)
        {
            throttle = minPositiveThrottle;

            print(rb.velocity.magnitude + " velocity");
            
            if (rb.velocity.magnitude > 10f && DistanceFromTrack() < distanceFromTrackToBreak)
            {
                brake = true;
            }
            else
            {
                brake = false;
            }
        }

        else
        {
            brake = false;
            float percentThrottle = 1 - curveAngle / maxAngleForMinThrottle;
            percentThrottle *= percentThrottle;
            throttle = (maxThrottle - minPositiveThrottle) * percentThrottle + minPositiveThrottle;
        }
    }

    public void Sensors()
    {
        RaycastHit[] hits = new RaycastHit[5];

        // first front left sensor
        if (Physics.Raycast(sensorsTransforms[0].position, Quaternion.AngleAxis(-frontSensorOuterAngle, transform.up) * transform.forward, out hits[0], sensorLength, ~ignoredLayerMasks))
        {
            Debug.DrawLine(sensorsTransforms[0].position, hits[0].point);
            Debug.Log("Sensor 1 da esquerda");

            aiState = AiState.Avoiding;
            steer = 1f;
            throttle = minPositiveThrottle;

            if (Vector3.Distance(hits[0].point, sensorsTransforms[0].position) <= minDistanceToReverse)
            {
                throttle = -1f;
                steer = -1f;
            }
        }

        // second front right angle
        else if (Physics.Raycast(sensorsTransforms[3].position, Quaternion.AngleAxis(frontSensorOuterAngle, transform.up) * transform.forward, out hits[3], sensorLength, ~ignoredLayerMasks))
        {
            Debug.DrawLine(sensorsTransforms[3].position, hits[3].point);
            Debug.Log("Sensor 2 da direita");

            aiState = AiState.Avoiding;
            steer = -1f;
            throttle = minPositiveThrottle;

            if (Vector3.Distance(hits[3].point, sensorsTransforms[3].position) <= minDistanceToReverse)
            {
                throttle = -1f;
                steer = 1f;
            }
        }

        // second front left sensor
        else if (Physics.Raycast(sensorsTransforms[1].position, Quaternion.AngleAxis(-frontSensorInnerAngle, transform.up) * transform.forward, out hits[1], sensorLength, ~ignoredLayerMasks))
        {
            Debug.DrawLine(sensorsTransforms[1].position, hits[1].point);
            Debug.Log("Sensor 2 da esquerda");

            aiState = AiState.Avoiding;
            steer = 0.5f;
            throttle = minPositiveThrottle;

            if (Vector3.Distance(hits[1].point, sensorsTransforms[1].position) <= minDistanceToReverse)
            {
                throttle = -1f;
                steer = -0.5f;
            }
        }

        // first front right sensor;
        else if (Physics.Raycast(sensorsTransforms[2].position, Quaternion.AngleAxis(frontSensorInnerAngle, transform.up) * transform.forward, out hits[2], sensorLength, ~ignoredLayerMasks))
        {
            Debug.DrawLine(sensorsTransforms[2].position, hits[2].point);
            Debug.Log("Sensor 1 da direita");

            aiState = AiState.Avoiding;
            steer = -0.5f;
            throttle = minPositiveThrottle;

            if (Vector3.Distance(hits[2].point, sensorsTransforms[2].position) <= minDistanceToReverse)
            {
                throttle = -1f;
                steer = 0.5f;
            }
        }
        else if (Physics.Raycast(sensorsTransforms[4].position, transform.right, out hits[4], horizontalSensorLength, ~ignoredLayerMasks))
        {
            Debug.DrawLine(sensorsTransforms[4].position, hits[4].point, Color.blue);
            Debug.Log("Sensor horizontal");

            aiState = AiState.Avoiding;
            throttle = -1f;
            steer = -1f;
        }
        else
        {
            aiState = AiState.FollowingTrack;
            brake = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Node") && currentNode == GetSiblingIndex(other.transform, other.transform.parent))
        {
            currentNode = (currentNode + 1) % nodes.Count;
        }
    }

    private float CurveAngle()
    {
        int nextNode = (currentNode + 1) % nodes.Count;

        Vector3 direction = nodes[nextNode].transform.position - nodes[currentNode].transform.position;
        Debug.DrawRay(transform.position, direction);

        return Vector3.Angle(direction, transform.forward);
    }

    public void FollowPath()
    {
        int previousNode = (currentNode + nodes.Count - 1) % nodes.Count;
        int nextNode = (currentNode + 1) % nodes.Count;
        Vector3 position = transform.position;
        Vector3 currentNodePosition = nodes[currentNode].transform.position;
        Vector3 previousNodePosition = nodes[previousNode].transform.position;
        Vector3 trackDirection = currentNodePosition - previousNodePosition;
        Vector3 nextTrackDirection = nodes[nextNode].transform.position - currentNodePosition;
        Vector3 carToWaypoint = currentNodePosition - position;
        Vector3 relativeVector;

        float percentDistance = carToWaypoint.magnitude * Mathf.Cos(Vector3.Angle(trackDirection, carToWaypoint) * Mathf.Deg2Rad) / trackDirection.magnitude;

        if (DistanceFromTrack() > trackWidth || Vector3.Angle(trackDirection, carToWaypoint) > 90f) // Distancia maior que a largura da pista
        {
            relativeVector = transform.InverseTransformPoint(currentNodePosition);
        }
        else
        {
            Vector3 interpolatedDirection = Vector3.Lerp(trackDirection, nextTrackDirection, Mathf.Clamp((1 - percentDistance) * (1 - percentDistance), 0, 1)); // ease function
            Vector3 trackDirectionPosition = position + interpolatedDirection;
            relativeVector = transform.InverseTransformPoint(trackDirectionPosition);
        }

        Debug.DrawLine(position, transform.TransformPoint(relativeVector), Color.red);

        float newSteer = relativeVector.x / relativeVector.magnitude;
        steer = newSteer;
    }

    private float DistanceFromTrack()
    {
        int previousNode = (currentNode + nodes.Count - 1) % nodes.Count;
        Vector3 position = transform.position;
        Vector3 currentNodePosition = nodes[currentNode].transform.position;
        Vector3 previousNodePosition = nodes[previousNode].transform.position;
        Vector2 currentNodePositionXZ = new Vector2(currentNodePosition.x, currentNodePosition.z);
        Vector2 previousNodePositionXZ = new Vector2(previousNodePosition.x, previousNodePosition.z);
        Vector2 carPositionXZ = new Vector2(position.x, position.z);

        return Mathf.Abs((previousNodePositionXZ.x - currentNodePositionXZ.x) * (currentNodePositionXZ.y - carPositionXZ.y) - (previousNodePositionXZ.y - currentNodePositionXZ.y) * 
            (currentNodePositionXZ.x - carPositionXZ.x)) / Mathf.Sqrt((previousNodePositionXZ.x - currentNodePositionXZ.x) * (previousNodePositionXZ.x - currentNodePositionXZ.x) + 
            (previousNodePositionXZ.y - currentNodePositionXZ.y) * (previousNodePositionXZ.y - currentNodePositionXZ.y));

        //return Mathf.Abs((p2.x - p1.x) * (p1.y - carPosition.y) - (p2.y - p1.y) * (p1.x - carPosition.x)) / Mathf.Sqrt((p2.x - p1.x) * (p2.x - p1.x) + (p2.y - p1.y) * (p2.y - p1.y));
    }

    int GetSiblingIndex(Transform child, Transform parent)
    {
        for (int i = 0; i < parent.childCount; ++i)
        {
            if (child == parent.GetChild(i))
                return i;
        }
        Debug.LogWarning("Child doesn't belong to this parent.");
        return -1;
    }
}