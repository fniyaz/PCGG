using System;
using UnityEngine;

public class Sun : MonoBehaviour
{
    private static readonly int EmissionMap = Shader.PropertyToID("_EmissionMap");
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    public Color c1, c2;
    private Texture2D[] textures;
    private Renderer render;
    
    private float aS = 1f;

    private void Awake()
    {
        render = GetComponent<Renderer>();
        var noise = new PerlinNoise3D(10, 10, 5, 5);
        
        textures = new Texture2D[100];
        for (var z = 0; z < 100; z++)
        {
            textures[z] = new Texture2D(100, 100, TextureFormat.RGBA32, false);
            for (var x = 0; x < 100; x++)
            {
                for (var y = 0; y < 100; y++)
                {
                    var c = noise.at(x / 10f, y / 10f, z / 20f) / 2 + .5f;
                    var color = Color.Lerp(c1, c2, c); 
                    textures[z].SetPixel(x, y, color);
                }
            }
            
            textures[z].Apply();
        }
        
        setTexture(textures[0]);
    }

    public void generateSkin(Color c1, Color c2)
    {
        this.c1 = c1;
        this.c2 = c2;
        var noise = new PerlinNoise2D(10, 10, 5);
        var texture = new Texture2D(100, 100, TextureFormat.RGBA32, false);

        for (var x = 0; x < 100; x++)
        for (var y = 0; y < 100; y++)
        {
            var c = noise.at(x / 10f, y / 10f) / 2 + .5f;
            var color = Color.Lerp(c1, c2, c);
            texture.SetPixel(x, y, color);
        }

        texture.Apply();

        var renderer = GetComponent<Renderer>();
        var tempMaterial = new Material(renderer.sharedMaterial);
        tempMaterial.mainTexture = texture;
        tempMaterial.SetTexture(EmissionMap, texture);
        tempMaterial.SetColor(EmissionColor, Color.white);

        renderer.sharedMaterial = tempMaterial;
    }

    private void setTexture(Texture2D tex)
    {
        render.material.mainTexture = tex;
        render.material.SetTexture(EmissionMap, tex);
        render.material.SetColor(EmissionColor, Color.white);
    }

    private int counter = 0;
    private int texNum = 0;
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, aS);
        
        counter++;
        if (counter > 1)
        {
            counter = 0;
            texNum++;
            texNum %= textures.Length;
            setTexture(textures[texNum]);
        }
    }
}
