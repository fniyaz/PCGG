

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SolarSystem))]
public class SolarSystemCustomEditor: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var system = target as SolarSystem;

        if (GUILayout.Button("generate"))
        {
            system.generate();
        }
    }
}
