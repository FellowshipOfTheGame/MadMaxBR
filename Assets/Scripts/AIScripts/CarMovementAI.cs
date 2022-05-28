using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiState
{
    FollowingTrack = 1, Avoiding = 2
}

public class CarMovementAI : MonoBehaviour
{
    [Header("Sensors")]
    [SerializeField] private Transform leftOuterSensorTransform;
    [SerializeField] private Transform leftInnerSensorTransform;
    [SerializeField] private Transform rightOuterSensorTransform;
    [SerializeField] private Transform rightInnerSensorTransform;
    [SerializeField] private Transform horizontalSensorTransform;
    [SerializeField] private Transform muretaLeftSensorTransform;
    [SerializeField] private Transform muretaRightSensorTransform;
    [SerializeField] private float sensorLength = 5f;
    [SerializeField] private float horizontalSensorLength = 2f;
    [SerializeField] private float frontSensorInnerAngle = 15f;
    [SerializeField] private float frontSensorOuterAngle = 30f;
    [SerializeField] private LayerMask ignoredLayerMasks;
    [SerializeField] private LayerMask roadLayerMask;
    [SerializeField] private LayerMask carLayerMask;
    [SerializeField] private LayerMask muretaLayerMask;

    [Header("Path to follow")]
    [SerializeField] private float trackWidth = 15.6f;
    
    public Transform path;
    public List<BoxCollider> nodes;
    public int currentNode;

    [Header("AI variables")]
    [SerializeField] private float minPositiveThrottle = 0.1f;
    [SerializeField] private float maxThrottle = 1f;
    [SerializeField] private float maxAngleForMinThrottle = 35f;
    [SerializeField] private float minDistanceToReverse = 3f;
    [SerializeField] private float distanceFromTrackToBreak = 30f;
    
    public AiState aiState = AiState.FollowingTrack;
    public float steer;
    public float throttle;
    public bool brake;

    private Rigidbody _rb;
    private BoxCollider[] _nodesBoxCollider;
    private Collider[] _colliders;
    private CarController _carController;
    private RaycastHit[] _raycastHitSensor = new RaycastHit[7];
    private Vector3 _directionToNextNode;
    private Vector3 _position;
    private Vector3 _currentNodePosition;
    private Vector3 _previousNodePosition;
    private Vector3 _trackDirection;
    private Vector3 _nextTrackDirection;
    private Vector3 _carToWaypoint;
    private Vector3 _relativeVector;
    private Vector3 _interpolatedDirection;
    private Vector3 _trackDirectionPosition;
    private Vector2 _currentNodePositionXZ;
    private Vector2 _previousNodePositionXZ;
    private Vector2 _carPositionXZ;
    private CarMovementAI[] _cars;
    private Transform _carTransform;
    private float _curveAngle;
    private float _percentThrottle;
    private float _newSteer;
    private float _percentDistance;
    private int _i;
    private int _nextNode;
    private int _previousNode;

    private void Awake()
    {
        _carController = GetComponent<CarController>();
        _cars = FindObjectsOfType<CarMovementAI>();
        _rb = GetComponent<Rigidbody>();
        _nodesBoxCollider = path.GetComponentsInChildren<BoxCollider>(true);
        _carTransform = transform;
        _colliders = new Collider[15];
    }

    private void Start()
    {
        nodes = new List<BoxCollider>();

        for (_i = 0; _i < _nodesBoxCollider.Length; _i++)
        {
            nodes.Add(_nodesBoxCollider[_i]);
        }
    }

    private void FixedUpdate()
    {
        if (_carController.CurrentSpeed < 40f)
        {
            Sensors();
        }
        
        if (aiState == AiState.FollowingTrack)
        {
            FollowPath();
            GetThrottle();
        }

        if (DistanceFromTrack() > trackWidth || IsOverturned())
        {
            StartCoroutine(Teleport());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == muretaLayerMask)
        {
            StartCoroutine(Teleport());
        }
    }

    private bool IsOverturned()
    {
        return !Physics.Raycast(_carTransform.position, -_carTransform.up, 5f, roadLayerMask);
    }

    private IEnumerator ChangeCollision()
    {
        Vector3 rotationDirection = nodes[currentNode].transform.position - nodes[_previousNode].transform.position;
        _rb.rotation = Quaternion.LookRotation(rotationDirection, Vector3.up);
        _rb.position = nodes[_previousNode].transform.position + new Vector3(0f, 3f, 0f);
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        foreach (CarMovementAI car in _cars)
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), car.GetComponent<Collider>(), true);
        }

        yield return new WaitForSeconds(5f);

        StartCoroutine(VerifyCollision());

        foreach (CarMovementAI car in _cars)
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), car.GetComponent<Collider>(), false);
        }
    }

    private IEnumerator VerifyCollision()
    {
        while (Physics.OverlapSphereNonAlloc(nodes[currentNode].transform.position, 30f, _colliders, carLayerMask) > 1)
        {
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator Teleport()
    {
        StartCoroutine(VerifyCollision());

        yield return new WaitForSeconds(2f);

        StartCoroutine(ChangeCollision());
    }

    private void GetThrottle()
    {
        _curveAngle = CurveAngle();

        if (_curveAngle > maxAngleForMinThrottle && _curveAngle < 90f) //Entre 35 e 90
        {
            throttle = minPositiveThrottle;

            //print(rb.velocity.magnitude + " velocity");

            brake = //DistanceFromTrack() < distanceFromTrackToBreak &&
                    _carController.CurrentSpeed > 60f;
        }
        else if (_curveAngle > 90f)
        {
            throttle = 0.5f;
        }
        else
        {
            brake = false;
            _percentThrottle = 1 - _curveAngle / maxAngleForMinThrottle;
            _percentThrottle *= _percentThrottle;// * _percentThrottle;
            throttle = (maxThrottle - minPositiveThrottle) * _percentThrottle + minPositiveThrottle;
        }
    }

    private void Sensors()
    {
        if (Physics.Raycast(horizontalSensorTransform.position, 
                transform.right, out _raycastHitSensor[4], horizontalSensorLength, ~ignoredLayerMasks))
        {
            //Debug.DrawLine(horizontalSensorTransform.position, _raycastHitSensor[4].point, Color.blue);
            //Debug.Log("Sensor horizontal");

            brake = false;
            aiState = AiState.Avoiding;
            throttle = -1f;
            steer = -1f;
        }
        
        else if (Physics.Raycast(muretaLeftSensorTransform.position, 
                     Quaternion.AngleAxis(-frontSensorOuterAngle, _carTransform.up) * _carTransform.forward, 
                     out _raycastHitSensor[5], sensorLength, muretaLayerMask))
        {
            //Debug.DrawLine(muretaLeftSensorTransform.position, _raycastHitSensor[5].point);
            //Debug.Log("Sensor esquerdo da mureta");

            aiState = AiState.Avoiding;
            steer = 1f;
            throttle = minPositiveThrottle;
            brake = true;
            
            if (Vector3.Distance(_raycastHitSensor[5].point, muretaLeftSensorTransform.position) <=
                  minDistanceToReverse && _carController.CurrentSpeed < 1f && !_carController.backward)
            
            if (!(Vector3.Distance(_raycastHitSensor[5].point, muretaLeftSensorTransform.position) <=
                  minDistanceToReverse) || !(_carController.CurrentSpeed < 1f)) return;
            brake = false;
            throttle = -1f;
            steer = -1f;
        }
        
        else if (Physics.Raycast(muretaRightSensorTransform.position, 
                     Quaternion.AngleAxis(frontSensorOuterAngle, _carTransform.up) * _carTransform.forward, 
                     out _raycastHitSensor[6], sensorLength, muretaLayerMask))
        {
            //Debug.DrawLine(muretaRightSensorTransform.position, _raycastHitSensor[6].point);
            //Debug.Log("Sensor direito da mureta");

            aiState = AiState.Avoiding;
            steer = -1f;
            throttle = minPositiveThrottle;
            brake = true;

            if (!(Vector3.Distance(_raycastHitSensor[6].point, muretaRightSensorTransform.position) <=
                  minDistanceToReverse) || !(_carController.CurrentSpeed < 1f)) return;

            brake = false;
            throttle = -1f;
            steer = 1f;
        }

        // first front left sensor
        else if (Physics.Raycast(leftOuterSensorTransform.position, 
                     Quaternion.AngleAxis(-frontSensorOuterAngle, _carTransform.up) * _carTransform.forward, 
                     out _raycastHitSensor[0], sensorLength, ~ignoredLayerMasks))
        {
            //Debug.DrawLine(leftOuterSensorTransform.position, _raycastHitSensor[0].point);
            //Debug.Log("Sensor 1 da esquerda");

            aiState = AiState.Avoiding;
            steer = 1f;
            throttle = minPositiveThrottle;
            brake = true;

            if (!(Vector3.Distance(_raycastHitSensor[0].point, leftOuterSensorTransform.position) <=
                  minDistanceToReverse) || !(_carController.CurrentSpeed < 1f)) return;

            brake = false;
            throttle = -1f;
            steer = -1f;
        }

        // second front right angle
        else if (Physics.Raycast(rightOuterSensorTransform.position, 
                     Quaternion.AngleAxis(frontSensorOuterAngle, _carTransform.up) * _carTransform.forward,
                     out _raycastHitSensor[3], sensorLength, ~ignoredLayerMasks))
        {
            //Debug.DrawLine(rightOuterSensorTransform.position, _raycastHitSensor[3].point);
            //Debug.Log("Sensor 2 da direita");

            aiState = AiState.Avoiding;
            steer = -1f;
            throttle = minPositiveThrottle;
            brake = true;

            if (!(Vector3.Distance(_raycastHitSensor[3].point, rightOuterSensorTransform.position) <=
                  minDistanceToReverse) || !(_carController.CurrentSpeed < 1f)) return;
            
            brake = false;
            throttle = -1f;
            steer = 1f;
        }

        // second front left sensor
        else if (Physics.Raycast(leftInnerSensorTransform.position, 
                     Quaternion.AngleAxis(-frontSensorInnerAngle, _carTransform.up) * _carTransform.forward, 
                     out _raycastHitSensor[1], sensorLength, ~ignoredLayerMasks))
        {
            //Debug.DrawLine(leftInnerSensorTransform.position, _raycastHitSensor[1].point);
            //Debug.Log("Sensor 2 da esquerda");

            aiState = AiState.Avoiding;
            steer = 0.5f;
            throttle = minPositiveThrottle;
            brake = true;

            if (!(Vector3.Distance(_raycastHitSensor[1].point, leftInnerSensorTransform.position) <=
                  minDistanceToReverse) || !(_carController.CurrentSpeed < 1f)) return;
            
            brake = false;
            throttle = -1f;
            steer = -0.5f;
        }

        // first front right sensor;
        else if (Physics.Raycast(rightInnerSensorTransform.position, 
                     Quaternion.AngleAxis(frontSensorInnerAngle, _carTransform.up) * _carTransform.forward, 
                     out _raycastHitSensor[2], sensorLength, ~ignoredLayerMasks))
        {
            //Debug.DrawLine(rightInnerSensorTransform.position, _raycastHitSensor[2].point);
            //Debug.Log("Sensor 1 da direita");

            aiState = AiState.Avoiding;
            steer = -0.5f;
            throttle = minPositiveThrottle;
            brake = true;

            if (!(Vector3.Distance(_raycastHitSensor[2].point, rightInnerSensorTransform.position) <=
                  minDistanceToReverse) || !(_carController.CurrentSpeed < 1f)) return;
            
            brake = false;
            throttle = -1f;
            steer = 0.5f;
        }

        else
        {
            aiState = AiState.FollowingTrack;
            brake = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform || !other.transform.parent) return;
        if (!other.CompareTag("Node") ||
            currentNode != GetSiblingIndex(other.transform, other.transform.parent)) return;
        
        currentNode = (currentNode + 1) % nodes.Count;
        _nextNode = (currentNode + 1) % nodes.Count;
        _previousNode = (currentNode + nodes.Count - 1) % nodes.Count;
    }

    private float CurveAngle()
    {
        _directionToNextNode = nodes[_nextNode].transform.position - nodes[currentNode].transform.position;
        //Debug.DrawRay(_carTransform.position, _directionToNextNode);

        return Vector3.Angle(_directionToNextNode, _carTransform.forward);
    }

    private void FollowPath()
    {
        _position = _carTransform.position;
        _currentNodePosition = nodes[currentNode].transform.position;
        _previousNodePosition = nodes[_previousNode].transform.position;
        _trackDirection = _currentNodePosition - _previousNodePosition;
        _nextTrackDirection = nodes[_nextNode].transform.position - _currentNodePosition;
        _carToWaypoint = _currentNodePosition - _position;

        _percentDistance = _carToWaypoint.magnitude * Mathf.Cos(Vector3.Angle(_trackDirection, _carToWaypoint) * Mathf.Deg2Rad) / _trackDirection.magnitude;
        
        if (DistanceFromTrack() > trackWidth || Vector3.Angle(_trackDirection, _carToWaypoint) > 90f) // Distancia maior que a largura da pista
        {
            _relativeVector = _carTransform.InverseTransformPoint(_currentNodePosition);
        }
        else
        {
            _interpolatedDirection = Vector3.Lerp(_trackDirection, _nextTrackDirection,
                Mathf.Clamp((1 - _percentDistance) * (1 - _percentDistance), 0, 1)); // ease function
            _trackDirectionPosition = _position + _interpolatedDirection;
            _relativeVector = _carTransform.InverseTransformPoint(_trackDirectionPosition);
        }

        //Debug.DrawLine(_position, _carTransform.TransformPoint(_relativeVector), Color.red);

        _newSteer = _relativeVector.x / _relativeVector.magnitude;
        steer = _newSteer;
    }

    private float DistanceFromTrack()
    {
        _currentNodePositionXZ = new Vector2(_currentNodePosition.x, _currentNodePosition.z);
        _previousNodePositionXZ = new Vector2(_previousNodePosition.x, _previousNodePosition.z);
        _carPositionXZ = new Vector2(_position.x, _position.z);

        return Mathf.Abs((_previousNodePositionXZ.x - _currentNodePositionXZ.x) * (_currentNodePositionXZ.y - _carPositionXZ.y) - (_previousNodePositionXZ.y - _currentNodePositionXZ.y) * 
            (_currentNodePositionXZ.x - _carPositionXZ.x)) / Mathf.Sqrt((_previousNodePositionXZ.x - _currentNodePositionXZ.x) * (_previousNodePositionXZ.x - _currentNodePositionXZ.x) + 
            (_previousNodePositionXZ.y - _currentNodePositionXZ.y) * (_previousNodePositionXZ.y - _currentNodePositionXZ.y));

        //return Mathf.Abs((p2.x - p1.x) * (p1.y - carPosition.y) - (p2.y - p1.y) * (p1.x - carPosition.x)) / Mathf.Sqrt((p2.x - p1.x) * (p2.x - p1.x) + (p2.y - p1.y) * (p2.y - p1.y));
    }

    private int GetSiblingIndex(Transform child, Transform parent)
    {
        for (_i = 0; _i < parent.childCount; ++_i)
        {
            if (child == parent.GetChild(_i))
                return _i;
        }
        Debug.LogWarning("Child doesn't belong to this parent.");
        return -1;
    }
}