using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    [Range(1, 10)] public int count = 5;
    [Range(.5f, 10)] public float minSize = .5f;
    [Range(.5f, 10)] public float maxSize = 10;
    [Range(5, 20)] public float sunSize = 15;
    [Range(0.05f, 1f)] public float minSpeed = .5f;
    [Range(0.05f, 1f)] public float maxSpeed = 1f;

    public Color sunColor1 = Color.cyan;
    public Color sunColor2 = Color.yellow;
    public Color[] availableColors = new[]
    {
        Color.white,
        Color.green,
        Color.red,
        Color.yellow,
        Color.blue,
        Color.Lerp(Color.blue, Color.red, 0.7f)
    };
    
    internal void generate()
    {
        clear();

        var sun = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sun.transform.parent = transform;
        sun.transform.localScale = new Vector3(sunSize, sunSize, sunSize);
        sun.tag = "Generated planet";
        sun.AddComponent<Sun>().generateSkin(sunColor1, sunColor2);
        
        var sysrand = new System.Random();
        
        var notLessThan = 1.1f * sunSize;
        for (int i = 0; i < count; i++)
        {
            var size = Random.Range(minSize, maxSize);
            
            var distance = Random.Range(notLessThan + size / 2, notLessThan + size + 3);
            notLessThan = distance + 1.1f * size / 2;
            var position = Vector3.right * distance;

            var c1i = sysrand.Next(availableColors.Length);
            var c2i = sysrand.Next(availableColors.Length - 1);
            if (c2i >= c1i)
                c2i += 1;
            generatePlanet(sun, $"Planet {i+1}", size, position, availableColors[c1i], availableColors[c2i],
                Random.Range(minSpeed, maxSpeed));
        }
    }
    
    private GameObject generatePlanet(GameObject sun, string name, float size, Vector3 position, Color c1, Color c2, float angleSPeed)
    {
        var planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        planet.name = name;
        planet.tag = "Generated planet";
        
        planet.transform.parent = transform;
        planet.transform.localPosition = position;
        planet.transform.localScale = new Vector3(size,size, size);
        planet.transform.RotateAround(sun.transform.position, Vector3.up, Random.Range(0, 360));

        var planetComp = planet.AddComponent<Planet>();
        planetComp.generateSkin(sun, c1, c2, angleSPeed,  Random.Range(5f, 15f));

        return planet;
    }

    private void clear()
    {
        foreach (var planet in GameObject.FindGameObjectsWithTag("Generated planet"))
        {
            DestroyImmediate(planet);
        }
    }
}