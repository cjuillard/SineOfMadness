using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryRenderable : MonoBehaviour
{
    void Awake()
    {
        LineRenderer renderer = GetComponent<LineRenderer>();
        Vector3[] bounds = new Vector3[4];
        Rect playArea = Boot.Settings.PlayArea;
        bounds[0] = new Vector3(playArea.min.x, playArea.min.y, 0);
        bounds[1] = new Vector3(playArea.min.x, playArea.max.y, 0);
        bounds[2] = new Vector3(playArea.max.x, playArea.max.y, 0);
        bounds[3] = new Vector3(playArea.max.x, playArea.min.y, 0);
        
        renderer.positionCount = 4;
        renderer.SetPositions(bounds);
    }
}
