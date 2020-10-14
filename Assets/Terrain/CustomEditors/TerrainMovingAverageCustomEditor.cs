using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TerrainMovingAverage))]
public class TerrainMovingAverageCustomEditor: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var movingAverage = target as TerrainMovingAverage;

        if (GUILayout.Button("Run"))
        {
            movingAverage.run();
        }
    }
}
