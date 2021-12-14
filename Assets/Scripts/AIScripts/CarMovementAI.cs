using System.Collections.Generic;
using UnityEngine;

public enum AiState
{
    FollowingTrack = 1, Avoiding = 2, Overtaking = 3
}

public class CarMovementAI : MonoBehaviour
{
    [Header("Sensors")]
    [SerializeField] private Transform leftSensorTransform;
    [SerializeField] private Transform leftInnerSensorTransform;
    [SerializeField] private Transform rightSensorTransform;
    [SerializeField] private Transform rightInnerSensorTransform;
    [SerializeField] private Transform horizontalSensorTransform;
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
    public bool NitroEnabled = false;
    [SerializeField] private float minPositiveThrottle = 0.03f;
    [SerializeField] private float maxThrottle = 1f;
    [SerializeField] private float maxAngleForMinThrottle = 35f;
    [SerializeField] private float minDistanceToReverse = 3f;
    [SerializeField] private float distanceFromTrackToBreak = 30f;

    private Rigidbody rb;
    private BoxCollider[] nodesBoxCollider;
    private float curveAngle;
    private float percentThrottle;
    private RaycastHit[] raycastHitSensor = new RaycastHit[5];
    private int i;
    private int nextNode;
    private Vector3 directionToNextNode;
    private int previousNode;
    private Vector3 position;
    private Vector3 currentNodePosition;
    private Vector3 previousNodePosition;
    private Vector3 trackDirection;
    private Vector3 nextTrackDirection;
    private Vector3 carToWaypoint;
    private Vector3 relativeVector;
    private float percentDistance;
    private Vector3 interpolatedDirection;
    private Vector3 trackDirectionPosition;
    private float newSteer;
    private Vector2 currentNodePositionXZ;
    private Vector2 previousNodePositionXZ;
    private Vector2 carPositionXZ;

    public AiState aiState = AiState.FollowingTrack;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        nodesBoxCollider = path.GetComponentsInChildren<BoxCollider>();
        nodes = new List<BoxCollider>();

        for (i = 0; i < nodesBoxCollider.Length; i++)
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
        curveAngle = CurveAngle();

        if (curveAngle > maxAngleForMinThrottle)
        {
            throttle = minPositiveThrottle;

            //print(rb.velocity.magnitude + " velocity");
            
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
            percentThrottle = 1 - curveAngle / maxAngleForMinThrottle;
            percentThrottle *= percentThrottle;
            throttle = (maxThrottle - minPositiveThrottle) * percentThrottle + minPositiveThrottle;
        }
    }

    public void Sensors()
    {
        // first front left sensor
        if (Physics.Raycast(leftSensorTransform.position, Quaternion.AngleAxis(-frontSensorOuterAngle, transform.up) * transform.forward, out raycastHitSensor[0], sensorLength, ~ignoredLayerMasks))
        {
            Debug.DrawLine(leftSensorTransform.position, raycastHitSensor[0].point);
            Debug.Log("Sensor 1 da esquerda");

            aiState = AiState.Avoiding;
            steer = 1f;
            throttle = minPositiveThrottle;

            if (Vector3.Distance(raycastHitSensor[0].point, leftSensorTransform.position) <= minDistanceToReverse)
            {
                throttle = -1f;
                steer = -1f;
            }
        }

        // second front right angle
        else if (Physics.Raycast(rightSensorTransform.position, Quaternion.AngleAxis(frontSensorOuterAngle, transform.up) * transform.forward, out raycastHitSensor[3], sensorLength, ~ignoredLayerMasks))
        {
            Debug.DrawLine(rightSensorTransform.position, raycastHitSensor[3].point);
            Debug.Log("Sensor 2 da direita");

            aiState = AiState.Avoiding;
            steer = -1f;
            throttle = minPositiveThrottle;

            if (Vector3.Distance(raycastHitSensor[3].point, rightSensorTransform.position) <= minDistanceToReverse)
            {
                throttle = -1f;
                steer = 1f;
            }
        }

        // second front left sensor
        else if (Physics.Raycast(leftInnerSensorTransform.position, Quaternion.AngleAxis(-frontSensorInnerAngle, transform.up) * transform.forward, out raycastHitSensor[1], sensorLength, ~ignoredLayerMasks))
        {
            Debug.DrawLine(leftInnerSensorTransform.position, raycastHitSensor[1].point);
            Debug.Log("Sensor 2 da esquerda");

            aiState = AiState.Avoiding;
            steer = 0.5f;
            throttle = minPositiveThrottle;

            if (Vector3.Distance(raycastHitSensor[1].point, leftInnerSensorTransform.position) <= minDistanceToReverse)
            {
                throttle = -1f;
                steer = -0.5f;
            }
        }

        // first front right sensor;
        else if (Physics.Raycast(rightInnerSensorTransform.position, Quaternion.AngleAxis(frontSensorInnerAngle, transform.up) * transform.forward, out raycastHitSensor[2], sensorLength, ~ignoredLayerMasks))
        {
            Debug.DrawLine(rightInnerSensorTransform.position, raycastHitSensor[2].point);
            Debug.Log("Sensor 1 da direita");

            aiState = AiState.Avoiding;
            steer = -0.5f;
            throttle = minPositiveThrottle;

            if (Vector3.Distance(raycastHitSensor[2].point, rightInnerSensorTransform.position) <= minDistanceToReverse)
            {
                throttle = -1f;
                steer = 0.5f;
            }
        }
        else if (Physics.Raycast(horizontalSensorTransform.position, transform.right, out raycastHitSensor[4], horizontalSensorLength, ~ignoredLayerMasks))
        {
            Debug.DrawLine(horizontalSensorTransform.position, raycastHitSensor[4].point, Color.blue);
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
        nextNode = (currentNode + 1) % nodes.Count;

        directionToNextNode = nodes[nextNode].transform.position - nodes[currentNode].transform.position;
        Debug.DrawRay(transform.position, directionToNextNode);

        return Vector3.Angle(directionToNextNode, transform.forward);
    }

    public void FollowPath()
    {
        previousNode = (currentNode + nodes.Count - 1) % nodes.Count;
        nextNode = (currentNode + 1) % nodes.Count;
        position = transform.position;
        currentNodePosition = nodes[currentNode].transform.position;
        previousNodePosition = nodes[previousNode].transform.position;
        trackDirection = currentNodePosition - previousNodePosition;
        nextTrackDirection = nodes[nextNode].transform.position - currentNodePosition;
        carToWaypoint = currentNodePosition - position;

        percentDistance = carToWaypoint.magnitude * Mathf.Cos(Vector3.Angle(trackDirection, carToWaypoint) * Mathf.Deg2Rad) / trackDirection.magnitude;

        if (DistanceFromTrack() > trackWidth || Vector3.Angle(trackDirection, carToWaypoint) > 90f) // Distancia maior que a largura da pista
        {
            relativeVector = transform.InverseTransformPoint(currentNodePosition);
        }
        else
        {
            interpolatedDirection = Vector3.Lerp(trackDirection, nextTrackDirection, Mathf.Clamp((1 - percentDistance) * (1 - percentDistance), 0, 1)); // ease function
            trackDirectionPosition = position + interpolatedDirection;
            relativeVector = transform.InverseTransformPoint(trackDirectionPosition);
        }

        Debug.DrawLine(position, transform.TransformPoint(relativeVector), Color.red);

        newSteer = relativeVector.x / relativeVector.magnitude;
        steer = newSteer;
    }

    private float DistanceFromTrack()
    {
        currentNodePositionXZ = new Vector2(currentNodePosition.x, currentNodePosition.z);
        previousNodePositionXZ = new Vector2(previousNodePosition.x, previousNodePosition.z);
        carPositionXZ = new Vector2(position.x, position.z);

        return Mathf.Abs((previousNodePositionXZ.x - currentNodePositionXZ.x) * (currentNodePositionXZ.y - carPositionXZ.y) - (previousNodePositionXZ.y - currentNodePositionXZ.y) * 
            (currentNodePositionXZ.x - carPositionXZ.x)) / Mathf.Sqrt((previousNodePositionXZ.x - currentNodePositionXZ.x) * (previousNodePositionXZ.x - currentNodePositionXZ.x) + 
            (previousNodePositionXZ.y - currentNodePositionXZ.y) * (previousNodePositionXZ.y - currentNodePositionXZ.y));

        //return Mathf.Abs((p2.x - p1.x) * (p1.y - carPosition.y) - (p2.y - p1.y) * (p1.x - carPosition.x)) / Mathf.Sqrt((p2.x - p1.x) * (p2.x - p1.x) + (p2.y - p1.y) * (p2.y - p1.y));
    }

    private int GetSiblingIndex(Transform child, Transform parent)
    {
        for (i = 0; i < parent.childCount; ++i)
        {
            if (child == parent.GetChild(i))
                return i;
        }
        Debug.LogWarning("Child doesn't belong to this parent.");
        return -1;
    }
}