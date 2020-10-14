using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    public GameObject sun;
    public float speed = 1f;
    public float aS;

    public void generateSkin(GameObject sun, Color c1, Color c2, float speed, float aS)
    {
        this.aS = aS; 
        this.sun = sun;
        this.speed = speed;
        var noise = new PerlinNoise2D(7, 7, 5);
        var texture = new Texture2D(70, 70, TextureFormat.RGBA32, false);
        
        for (var x = 0; x < 70; x++)
        for (var y = 0; y < 70; y++)
        {
            var c = noise.at(x / 10f, y / 10f) / 2 + .5f;
            var color = Color.Lerp(c1, c2, c);
            texture.SetPixel(x, y, color);
        }
        texture.Apply();

        var renderer = GetComponent<Renderer>();
        var tempMaterial = new Material(renderer.sharedMaterial);
        tempMaterial.mainTexture = texture;

        renderer.sharedMaterial = tempMaterial;
    }

    private void cloud()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, aS);
        if (sun != null)
            transform.RotateAround(sun.transform.position, Vector3.up, speed);
    }
}
