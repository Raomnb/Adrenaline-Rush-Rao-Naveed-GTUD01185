using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Color lineColor;
    private List<Transform> wayPoint = new List<Transform>();
    private Transform[] transforms;
    Vector3 currentNode = Vector3.zero;
    Vector3 previousNode = Vector3.zero;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = lineColor;
        transforms = GetComponentsInChildren<Transform>();
        for(int i =0; i<transforms.Length; i++)
        {
            if(transforms[i].transform != transform)
            {
                wayPoint.Add(transforms[i]);
            }
        }
        for(int i =0; i<wayPoint.Count;i++)
        {
            currentNode = wayPoint[i].position;
            if(i>0)
            {
                previousNode = wayPoint[i - 1].position;
            }
            else
            {
                previousNode = wayPoint[wayPoint.Count-1].position;
            }
            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawSphere(currentNode, 0.3f);
        }

    }
}
