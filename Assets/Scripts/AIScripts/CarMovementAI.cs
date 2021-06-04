using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

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
    [SerializeField] private float frontSideSensorPosition = 1f;
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
    [SerializeField] private float maxAngleforMinThrottle = 35f;
    [SerializeField] private float minDistanceToReverse = 3f;

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

        if (curveAngle > maxAngleforMinThrottle)
        {
            throttle = minPositiveThrottle;

            print(rb.velocity.magnitude + " velocity");
            
            if (rb.velocity.magnitude > 10f && DistanceFromTrack() < 30f)
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
            float percentThrottle = 1 - curveAngle / maxAngleforMinThrottle;
            percentThrottle *= percentThrottle * percentThrottle;
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
            throttle = 0.1f;

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
            throttle = 0.1f;

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
            throttle = 0.1f;

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
            throttle = 0.1f;

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

        return Vector3.Angle(nodes[nextNode].transform.position - nodes[currentNode].transform.position, transform.forward);
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

        float percentDistance = carToWaypoint.magnitude * math.cos(Vector3.Angle(trackDirection, carToWaypoint) * Mathf.Deg2Rad) / trackDirection.magnitude;

        if (DistanceFromTrack() > trackWidth || Vector3.Angle(trackDirection, carToWaypoint) > 90f) // Distancia maior que a largura da pista
        {
            relativeVector = transform.InverseTransformPoint(currentNodePosition);
        }
        else
        {
            Vector3 interpolatedDirection = Vector3.Lerp(trackDirection, nextTrackDirection, math.clamp((1 - percentDistance) * (1 - percentDistance), 0, 1)); // ease function
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

        return math.abs((previousNodePositionXZ.x - currentNodePositionXZ.x) * (currentNodePositionXZ.y - carPositionXZ.y) - (previousNodePositionXZ.y - currentNodePositionXZ.y) * (currentNodePositionXZ.x - carPositionXZ.x)) / math.sqrt((previousNodePositionXZ.x - currentNodePositionXZ.x) * (previousNodePositionXZ.x - currentNodePositionXZ.x) + (previousNodePositionXZ.y - currentNodePositionXZ.y) * (previousNodePositionXZ.y - currentNodePositionXZ.y));

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

[BurstCompile]
public struct FollowJob : IJob
{
    public float3 carPositionArrayJob;
    public float3 nodePositionArrayJob;
    public float steerJob;
    public float trackWidthJob;
    public int nodesQuantityJob;
    public int currentNode;
    public AiState aiStates;
    public Matrix4x4 localToWorldPosMatrix;

    public void Execute()
    {
        if (aiStates == AiState.FollowingTrack)
        {
            int previousNode = (currentNode + nodesQuantityJob - 1) % nodesQuantityJob;
            int nextNode = (currentNode + 1) % nodesQuantityJob;
            float3 position = carPositionArrayJob;
            float3 currentNodePosition = nodePositionArrayJob[currentNode];
            float3 previousNodePosition = nodePositionArrayJob[previousNode];
            float3 trackDirection = currentNodePosition - previousNodePosition;
            float3 nextTrackDirection = nodePositionArrayJob[nextNode] - currentNodePosition;
            float3 carToWaypoint = currentNodePosition - position;
            float3 relativeVector;

            float percentDistance = Vector3Magnitude(carToWaypoint) * math.cos(Vector3.Angle(trackDirection, carToWaypoint) * math.PI / 180) / Vector3Magnitude(trackDirection);

            float2 currentNodePositionXZ = new float2(currentNodePosition.x, currentNodePosition.z);
            float2 previousNodePositionXZ = new float2(previousNodePosition.x, previousNodePosition.z);
            float2 carPositionXZ = new float2(position.x, position.z);

            if (DistanceFromTrack(currentNodePositionXZ, previousNodePositionXZ, carPositionXZ) > trackWidthJob || Vector3.Angle(trackDirection, carToWaypoint) > 90f) // Distancia maior que a largura da pista
            {
                relativeVector = math.transform(math.inverse(localToWorldPosMatrix), currentNodePosition);
            }
            else
            {
                float3 interpolatedDirection = Vector3.Lerp(trackDirection, nextTrackDirection, math.clamp((1 - percentDistance) * (1 - percentDistance), 0, 1)); // ease function
                float3 trackDirectionPosition = position + interpolatedDirection;
                relativeVector = math.transform(math.inverse(localToWorldPosMatrix), trackDirectionPosition);
            }

            //Debug.DrawLine(position, transform.TransformPoint(relativeVector), Color.red);

            float newSteer = relativeVector.x / Vector3Magnitude(relativeVector);
            steerJob = newSteer;
        }
    }

    public float Vector3Magnitude(float3 vector)
    {
        return math.sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
    }

    public float DistanceFromTrack(float2 p1, float2 p2, float2 carPosition)
    {
        return math.abs((p2.x - p1.x) * (p1.y - carPosition.y) - (p2.y - p1.y) * (p1.x - carPosition.x)) / math.sqrt((p2.x - p1.x) * (p2.x - p1.x) + (p2.y - p1.y) * (p2.y - p1.y));
    }
}

public struct GetThrottle : IJob
{
    public float3 TransformForward;
    public float3 CarPosition;
    public float3 NodePosition;
    public float Throttle;
    public float MinPositiveThrottle;
    public float MaxThrottle;
    public float MaxAngleforMinThrottle;
    public bool Brake;
    public int CurrentNode;
    public int NodesQuantity;

    public void Execute()
    {
        float curveAngle = 1;//CurveAngle();

        if (curveAngle > MaxAngleforMinThrottle)
        {
            Throttle = MinPositiveThrottle;

            //print(rb.velocity.magnitude + " velocity");

            int previousNode = (CurrentNode + NodesQuantity - 1) % NodesQuantity;
            int nextNode = (CurrentNode + 1) % NodesQuantity;
            float3 position = CarPosition;
            float3 currentNodePosition = NodePosition[CurrentNode];
            float3 previousNodePosition = NodePosition[previousNode];
            float2 currentNodePositionXZ = new float2(currentNodePosition.x, currentNodePosition.z);
            float2 previousNodePositionXZ = new float2(previousNodePosition.x, previousNodePosition.z);
            float2 carPositionXZ = new float2(position.x, position.z);
            /*
            if (rb.velocity.magnitude > 10f && DistanceFromTrack(currentNodePositionXZ, previousNodePositionXZ, carPositionXZ) < 30f)
            {
                Brake = true;
            }
            else
            {
                Brake = false;
            }*/
        }

        else
        {
            Brake = false;
            float percentThrottle = 1 - curveAngle / MaxAngleforMinThrottle;
            percentThrottle *= percentThrottle * percentThrottle;
            Throttle = (MaxThrottle - MinPositiveThrottle) * percentThrottle + MinPositiveThrottle;
        }
    }

    public float DistanceFromTrack(float2 p1, float2 p2, float2 carPosition)
    {
        return math.abs((p2.x - p1.x) * (p1.y - carPosition.y) - (p2.y - p1.y) * (p1.x - carPosition.x)) / math.sqrt((p2.x - p1.x) * (p2.x - p1.x) + (p2.y - p1.y) * (p2.y - p1.y));
    }
    /*
    public float CurveAngle()
    {
        int nextNode = (CurrentNode + 1) % NodesQuantity;

        Vector3 direction = nodes[nextNode].transform.position - nodes[CurrentNode].transform.position;

        return Vector3.Angle(nodes[nextNode].transform.position - nodes[CurrentNode].transform.position, TransformForward);
    }*/
}