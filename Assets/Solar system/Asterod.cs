using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Debug = System.Diagnostics.Debug;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Asteroid: MonoBehaviour
{
    [Range(.01f, 1f)]public float radius = .1f;
    [Range(.01f, 1f)]public float distortion = .1f;
    public Material material;
    public float angleSpeed;
    
    
    public void run()
    {
        init();

        var mesh = AsteroidMeshGenerator.generateAsteroid(radius, distortion);

        collider.sharedMesh = mesh;
        filter.sharedMesh = mesh;
        renderer.material = material;

        transform.localRotation = Random.rotation;
    }


    private MeshCollider collider;
    private MeshRenderer renderer;
    private MeshFilter filter;
    
    private void init()
    {
        collider = GetComponent<MeshCollider>();
        filter = GetComponent<MeshFilter>();
        renderer = GetComponent<MeshRenderer>();
    }
    
    
    private void FixedUpdate()
    {
        transform.RotateAround(transform.parent.position, Vector3.up, angleSpeed);
    }
}


[CustomEditor(typeof(Asteroid))]
class AsteroidCustomEditor: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var applier = target as Asteroid;
        Debug.Assert(applier != null, nameof(applier) + " != null");

        if (GUILayout.Button("run"))
        {
            applier.run();
        }
    }
}
