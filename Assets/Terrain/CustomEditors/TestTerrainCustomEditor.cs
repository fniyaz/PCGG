using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TerrainTest))]
public class TestTerrainCustomEditor: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        var terrainTest = (TerrainTest) target;

        if (GUILayout.Button("Run Noise"))
        {
            terrainTest.generateTerrain(TerrainTest.TerrainGeneratorType.Noise);
        }
        
        if (GUILayout.Button("Run Diamond Square"))
        {
            terrainTest.generateTerrain(TerrainTest.TerrainGeneratorType.DiamondSquare);
        }
    }
}
