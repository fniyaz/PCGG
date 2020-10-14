
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IslandsSquasher))]
public class TerrainSquacherCustomEditor: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var squassher = target as IslandsSquasher;

        if (GUILayout.Button("Squash!"))
        {
            squassher.squash();
        }
    }
}
