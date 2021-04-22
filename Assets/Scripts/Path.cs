using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Color lineColor;

    private List<Transform> nodes = new List<Transform>();

    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        Transform[] pathTransform = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != transform)
            {
                nodes.Add(pathTransform[i]);
            }
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = nodes[(nodes.Count - 1 + i) % nodes.Count].position;
            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 5f);
        }
    }
}
