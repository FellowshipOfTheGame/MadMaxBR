using System.Collections.Generic;
using UnityEngine;

public enum AiState
{
    FollowingTrack = 1, Avoiding = 2, Overtaking = 3
}

public class CarMovementAI : MonoBehaviour
{
    [Header("Sensors")]
    public Transform[] sensorsTransforms;
    public float sensorLength = 20f;
    public float frontSideSensorPosition = 1f;
    public float frontSensorInnerAngle = 15f;
    public float frontSensorOuterAngle = 30f;
    [SerializeField] private LayerMask nodeTerrainLayerMask;

    [Header("Path to follow")]
    public Transform path;
    private List<BoxCollider> nodes;
    [SerializeField] private int currentNode = 0;
    
    [Header("AI variables")]
    public float steer = 0f;
    public float throttle = 0f;
    public bool brake = false;
    public bool nitroEnabled = false;
    private float minThrottle = 0.03f;
    private float maxThrottle = 0.5f;
    private float maxAngleforMinThrottle = 45f;
    private bool startedReverse = false;

    private Rigidbody rb;
    private VehicleControl vehicleControl;

    public AiState aiState = AiState.FollowingTrack;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vehicleControl = GetComponent<VehicleControl>();
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

    private void GetThrottle()
    {
        float curveAngle = CurveAngle();
        if (curveAngle > maxAngleforMinThrottle)
        {
            throttle = minThrottle;
        }

        else
        {
            float percentThrottle = 1 - curveAngle / maxAngleforMinThrottle;
            percentThrottle *= percentThrottle * percentThrottle;
            throttle = (maxThrottle - minThrottle) * percentThrottle + minThrottle;
        }
    }

    private void Sensors()
    {
        RaycastHit[] hits = new RaycastHit[4];

        // first front left sensor
        if (Physics.Raycast(sensorsTransforms[0].position, Quaternion.AngleAxis(-frontSensorOuterAngle, transform.up) * transform.forward, out hits[0], sensorLength, ~nodeTerrainLayerMask))
        {
            Debug.DrawLine(sensorsTransforms[0].position, hits[0].point);
            Debug.Log("Sensor 1 da esquerda");

            aiState = AiState.Avoiding;
            steer = Mathf.MoveTowards(steer, 1f, 0.2f);
            
            if (Vector3.Distance(hits[0].point, sensorsTransforms[0].position) <= .3f || rb.velocity.magnitude <= 5f)
            {
                brake = false;
                throttle = -1f;
            }
            else if (Vector3.Distance(hits[0].point, sensorsTransforms[0].position) > 2f)
            {
                throttle = 0.1f;
            }
            else
            {
                brake = true;
            }

            if (vehicleControl.Backward && !startedReverse)
            {
                startedReverse = true;
                steer = -steer;
            }
            else if (!vehicleControl.Backward)
            {
                startedReverse = false;
            }
        }

        // second front left sensor
        if (Physics.Raycast(sensorsTransforms[1].position, Quaternion.AngleAxis(-frontSensorInnerAngle, transform.up) * transform.forward, out hits[1], sensorLength, ~nodeTerrainLayerMask))
        {
            Debug.DrawLine(sensorsTransforms[1].position, hits[1].point);
            Debug.Log("Sensor 2 da esquerda");

            aiState = AiState.Avoiding;
            steer = Mathf.MoveTowards(steer, 0.5f, 0.2f);
            brake = true;

            if (Vector3.Distance(hits[1].point, sensorsTransforms[1].position) <= .3f || rb.velocity.magnitude <= 5f)
            {
                brake = false;
                throttle = -1f;
            }
            else if (Vector3.Distance(hits[1].point, sensorsTransforms[1].position) > 2f)
            {
                throttle = 0.1f;
            }
            else
            {
                brake = true;
            }

            if (vehicleControl.Backward && !startedReverse)
            {
                startedReverse = true;
                steer = -steer;
            }
            else if (!vehicleControl.Backward)
            {
                startedReverse = false;
            }
        }

        // first front right sensor;
        if (Physics.Raycast(sensorsTransforms[2].position, Quaternion.AngleAxis(frontSensorInnerAngle, transform.up) * transform.forward, out hits[2], sensorLength, ~nodeTerrainLayerMask))
        {
            Debug.DrawLine(sensorsTransforms[2].position, hits[2].point);
            Debug.Log("Sensor 1 da direita");

            aiState = AiState.Avoiding;
            steer = Mathf.MoveTowards(steer, -0.5f, 0.2f);
            brake = true;

            if (Vector3.Distance(hits[2].point, sensorsTransforms[2].position) <= .3f || rb.velocity.magnitude <= 5f)
            {
                brake = false;
                throttle = -1f;
            }
            else if (Vector3.Distance(hits[2].point, sensorsTransforms[2].position) > 2f)
            {
                throttle = 0.1f;
            }
            else
            {
                brake = true;
            }

            if (vehicleControl.Backward && !startedReverse)
            {
                startedReverse = true;
                steer = -steer;
            }
            else if (!vehicleControl.Backward)
            {
                startedReverse = false;
            }
        }

        // second front right angle
        if (Physics.Raycast(sensorsTransforms[3].position, Quaternion.AngleAxis(frontSensorOuterAngle, transform.up) * transform.forward, out hits[3], sensorLength, ~nodeTerrainLayerMask))
        {
            Debug.DrawLine(sensorsTransforms[3].position, hits[3].point);
            Debug.Log("Sensor 2 da direita");

            aiState = AiState.Avoiding;
            steer = Mathf.MoveTowards(steer, -1f, 0.2f);
            brake = true;

            if (Vector3.Distance(hits[3].point, sensorsTransforms[3].position) <= .3f || rb.velocity.magnitude <= 5f)
            {
                brake = false;
                throttle = -1f;
            }
            else if (Vector3.Distance(hits[3].point, sensorsTransforms[3].position) > 2f)
            {
                throttle = 0.1f;
            }
            else
            {
                brake = true;
            }

            if (vehicleControl.Backward && !startedReverse)
            {
                startedReverse = true;
                steer = -steer;
            }
            else if (!vehicleControl.Backward)
            {
                startedReverse = false;
            }
        }

        if (hits[0].collider && hits[1].collider && hits[2].collider && hits[3].collider)
        {
            throttle = -1f;

            if (vehicleControl.Backward)
            {
                steer = -steer;
            }
        }
        else if (!hits[0].collider && !hits[1].collider && !hits[2].collider && !hits[3].collider)
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

        Vector3 direcao = nodes[nextNode].transform.position - nodes[currentNode].transform.position;
        Debug.DrawRay(transform.position, direcao);

        return Vector3.Angle(nodes[nextNode].transform.position - nodes[currentNode].transform.position, transform.forward);
    }

    private void FollowPath()
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

        Vector2 currentNodePositionXZ = new Vector2(currentNodePosition.x, currentNodePosition.z);
        Vector2 previousNodePositionXZ = new Vector2(previousNodePosition.x, previousNodePosition.z);
        Vector2 carPositionXZ = new Vector2(position.x, position.z);

        float percentDistance = carToWaypoint.magnitude * Mathf.Cos(Vector3.Angle(trackDirection, carToWaypoint) * Mathf.Deg2Rad) / trackDirection.magnitude;

        if (DistanceFromTrack(currentNodePositionXZ, previousNodePositionXZ, carPositionXZ) > 13f || Vector3.Angle(trackDirection, carToWaypoint) > 90) // Distancia maior que a largura da pista
        {
            relativeVector = transform.InverseTransformPoint(currentNodePosition);
        }
        else
        {
            Vector3 interpolatedDirection = Vector3.Lerp(trackDirection, nextTrackDirection, Mathf.Clamp01((1 - percentDistance) * (1 - percentDistance))); // ease function
            Vector3 trackDirectionPosition = position + interpolatedDirection;
            relativeVector = transform.InverseTransformPoint(trackDirectionPosition);
        }

        Debug.DrawLine(position, transform.TransformPoint(relativeVector), Color.red);

        float newSteer = relativeVector.x / relativeVector.magnitude;
        //steer = Mathf.MoveTowards(steer, newSteer, 0.2f);
        steer = newSteer;
        /*
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].transform.position);
        float newSteer = relativeVector.x / relativeVector.magnitude;
        steer = Mathf.MoveTowards(steer, newSteer, 0.2f);*/
    }

    private float DistanceFromTrack(Vector2 p1, Vector2 p2, Vector2 carPosition)
    {
        return Mathf.Abs((p2.x - p1.x) * (p1.y - carPosition.y) - (p2.y - p1.y) * (p1.x - carPosition.x)) / Mathf.Sqrt((p2.x - p1.x) * (p2.x - p1.x) + (p2.y - p1.y) * (p2.y - p1.y));
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