using SineOfMadness;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRenderable : MonoBehaviour {

    public GameplaySettings settings;

	void Start () {
        if (settings == null)
            return;

        LineRenderer line = GetComponent<LineRenderer>();
        line.SetPosition(0, new Vector3(settings.playfield.xMin, 0, settings.playfield.yMin));
        line.SetPosition(1, new Vector3(settings.playfield.xMin, 0, settings.playfield.yMax));
        line.SetPosition(2, new Vector3(settings.playfield.xMax, 0, settings.playfield.yMax));
        line.SetPosition(3, new Vector3(settings.playfield.xMax, 0, settings.playfield.yMin));
        line.SetPosition(4, new Vector3(settings.playfield.xMin, 0, settings.playfield.yMin));
    }
}
