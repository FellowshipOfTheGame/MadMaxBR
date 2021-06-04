using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

// THIS SCRIPT DOESN'T WORK

public class AIJobSystem : MonoBehaviour
{
    [SerializeField] private CarMovementAI[] AiCars;
    [SerializeField] private float trackWidth = 15f;
    public Transform path;
    private RaycastHit[] hits = new RaycastHit[5];
    private NativeArray<float3> nodePositionArray;
    private int nodesQuantity;

    void Start()
    {
        BoxCollider[] nodesBoxCollider = path.GetComponentsInChildren<BoxCollider>();
        nodePositionArray = new NativeArray<float3>(nodesBoxCollider.Length, Allocator.Persistent);
        nodesQuantity = nodesBoxCollider.Length;

        for (int i = 0; i < nodesBoxCollider.Length; i++)
        {
            nodePositionArray[i] = nodesBoxCollider[i].transform.position;
        }
        nodePositionArray.Dispose();
    }

    void FixedUpdate()
    {
        NativeArray<float3> carPositionArray = new NativeArray<float3>(AiCars.Length, Allocator.TempJob);
        NativeArray<float> steerArray = new NativeArray<float>(AiCars.Length, Allocator.TempJob);
        NativeArray<int> currentIndexArray = new NativeArray<int>(AiCars.Length, Allocator.TempJob);
        NativeArray<AiState> aiStatesArray = new NativeArray<AiState>(AiCars.Length, Allocator.TempJob);
        NativeArray<Matrix4x4> localToWorldPosMatrix = new NativeArray<Matrix4x4>(AiCars.Length, Allocator.TempJob);

        for (int i = 0; i < AiCars.Length; i++)
        {
            carPositionArray[i] = AiCars[i].transform.position;
            currentIndexArray[i] = AiCars[i].currentNode;
            steerArray[i] = AiCars[i].steer;
            aiStatesArray[i] = AiCars[i].aiState;
            localToWorldPosMatrix[i].SetTRS(AiCars[i].transform.position, AiCars[i].transform.rotation, AiCars[i].transform.localScale);
        }

        FollowPathJob followPathJob = new FollowPathJob()
        {
            carPositionArrayJob = carPositionArray,
            nodePositionArrayJob = nodePositionArray,
            steerJob = steerArray,
            trackWidthJob = trackWidth,
            nodesQuantityJob = nodesQuantity,
            currentNode = currentIndexArray,
            aiStates = aiStatesArray,
            localToWorldPosMatrix = localToWorldPosMatrix
        };

        JobHandle followPathJobHandle = followPathJob.Schedule(AiCars.Length, 1);
        followPathJobHandle.Complete();

        for (int i = 0; i < AiCars.Length; i++)
        {
            AiCars[i].steer = steerArray[i];
        }

        carPositionArray.Dispose();
        steerArray.Dispose();
        currentIndexArray.Dispose();
        aiStatesArray.Dispose();
        localToWorldPosMatrix.Dispose();
    }
}

[BurstCompile]
public struct FollowPathJob : IJobParallelFor
{
    [NativeDisableParallelForRestriction] public NativeArray<float3> carPositionArrayJob;
    [NativeDisableParallelForRestriction] public NativeArray<float3> nodePositionArrayJob;
    public NativeArray<float> steerJob;
    public float trackWidthJob;
    public int nodesQuantityJob;
    [NativeDisableParallelForRestriction] public NativeArray<int> currentNode;
    public NativeArray<AiState> aiStates;
    public NativeArray<Matrix4x4> localToWorldPosMatrix;

    public void Execute(int index)
    {
        if (aiStates[index] == AiState.FollowingTrack)
        {
            int previousNode = (currentNode[index] + nodesQuantityJob - 1) % nodesQuantityJob;
            int nextNode = (currentNode[index] + 1) % nodesQuantityJob;
            float3 position = carPositionArrayJob[index];
            float3 currentNodePosition = nodePositionArrayJob[currentNode[index]];
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
                relativeVector = math.transform(math.inverse(localToWorldPosMatrix[index]), currentNodePosition[index]);
            }
            else
            {
                float3 interpolatedDirection = Vector3.Lerp(trackDirection, nextTrackDirection, math.clamp((1 - percentDistance) * (1 - percentDistance), 0, 1)); // ease function
                float3 trackDirectionPosition = position + interpolatedDirection;
                relativeVector = math.transform(math.inverse(localToWorldPosMatrix[index]), trackDirectionPosition);
            }

            //Debug.DrawLine(position, transform.TransformPoint(relativeVector), Color.red);

            float newSteer = relativeVector.x / Vector3Magnitude(relativeVector);
            steerJob[index] = newSteer;
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
public struct SensorJob : IJobParallelFor
{
    public void Execute(int index)
    {

    }
}
