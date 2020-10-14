

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WoodenTextureApplier))]
public class WoodenTextureCustomMenu: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var applier = target as WoodenTextureApplier;

        if (GUILayout.Button("apply"))
        {
            applier.apply();
        }
    }
}