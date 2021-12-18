using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWallCollider : MonoBehaviour
{
    private Mesh mesh;
    private MeshCollider meshCollider;
    private Vector3[] newMeshPoints;
    private Vector2[] newMeshUV;
    private int[] newMeshTriangles;

#pragma warning disable CS0414
    [SerializeField] bool updateCollider;
#pragma warning restore CS0414

    private void OnValidate()
    {
        CalculateMesh();

        UpdateMesh();

        updateCollider = false;
    }

    private void CalculateMesh()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
        newMeshPoints = mesh.vertices;
        newMeshUV = mesh.uv;
        newMeshTriangles = mesh.triangles;
    }

    private void UpdateMesh()
    {
        mesh.vertices = newMeshPoints;
        mesh.uv = newMeshUV;
        mesh.triangles = newMeshTriangles;

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
