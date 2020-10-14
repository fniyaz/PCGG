using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TerrainTest : MonoBehaviour
{
    [Range(0.0f, 1.0f)] public float seaLevel = 0.5f;
    [Range(1, 9)] public int noiseRandomScale = 5;
    [Range(0.0f, 1.0f)] public float DSRandomLevel = 0.1f;
    public DiamondSquareInitial DSInitialValues = DiamondSquareInitial.SmoothstepWhiteNoise;

    [SerializeField] [CanBeNull] private GameObject plane;

    public enum TerrainGeneratorType
    {
        Noise,
        DiamondSquare
    }

    public enum DiamondSquareInitial
    {
        WhiteNoise,
        SmoothstepWhiteNoise,
        Constant
    }
    
    
    // private Texture2D mapTexture;
    internal Terrain terrain;
    internal TerrainCollider terrainCollider;

    internal void init()
    {
        if (terrain == null)
        {
            terrain = GetComponent<Terrain>();
            terrainCollider = GetComponent<TerrainCollider>();
        }
    }

    void OnValidate()
    {
        init();
        var terrainData = terrain.terrainData;
        var maxHeight = terrainData.size.y;
        if (plane != null)
        {
            var position = plane.transform.position;
            var (olx, olz) = (position.x, position.z);
            plane.transform.SetPositionAndRotation(
                new Vector3(olx, seaLevel * maxHeight, olz),
                plane.transform.rotation
            );
        }
    }

    
    public void generateTerrain(TerrainGeneratorType tpye)
    {
        init();
        var terrainData = terrain.terrainData;

        int heightmapWidth = terrainData.heightmapResolution;
        int heightmapHeight = terrainData.heightmapResolution;

        var heights = new float[heightmapWidth, heightmapHeight];

        switch (tpye)
        {
            case TerrainGeneratorType.Noise:
                calculateTerrainNoise(ref heights);
                break;
            case TerrainGeneratorType.DiamondSquare:
                calculateTerrainDS(ref heights);
                break;
        }

        // mapTexture = generateMap(ref heights, seaLevel);

        terrainData.SetHeights(0, 0, heights);

        terrainCollider.terrainData = terrainData;
        terrain.terrainData = terrainData;
    }

    private void calculateTerrainNoise(ref float[,] heights)
    {
        var scale = 1 << noiseRandomScale;
        var fscale = (float) scale; 
        var wid = heights.GetLength(0);
        var hei = heights.GetLength(1);
        var noise = new PerlinNoise2D(wid / scale, hei / scale);
        for (var x = 0; x < wid; x++)
        for (var y = 0; y < hei; y++)
            heights[x, y] = (noise.at(x / fscale, y / fscale) / 0.5f + 1) / 2;
    }

    private void calculateTerrainDS(ref float[,] heights)
    {
        var copy = new float[heights.GetLength(0) - 1, heights.GetLength(1) - 1];
        var size = 64;
        for (int x = 0; x < copy.GetLength(1); x += size)
        for (int y = 0; y < copy.GetLength(0); y += size)
        {
            switch (DSInitialValues)
            {
                case DiamondSquareInitial.Constant:
                    copy[x, y] = .5f;
                    break;
                case DiamondSquareInitial.WhiteNoise:
                    copy[x, y] = Random.Range(0, 1f);
                    break;
                case DiamondSquareInitial.SmoothstepWhiteNoise:
                    copy[x, y] = smoothstep(Random.Range(0, 1f));
                    break;
            }
        }

        DiamondSquare.run(ref copy, size, (-DSRandomLevel, DSRandomLevel));
        for (var x = 0; x < copy.GetLength(0); x++)
        for (var y = 0; y < copy.GetLength(1); y++)
            heights[x, y] = copy[x, y];
        for (var x = 0; x < copy.GetLength(0); x++)
            heights[x, copy.GetLength(1)] = copy[x, 0];
        for (var y = 0; y < copy.GetLength(1); y++)
            heights[copy.GetLength(0), y] = copy[0, y];
        heights[copy.GetLength(0), copy.GetLength(1)] = copy[0, 0];
    }
    
    private static float smoothstep(float x)
    {
        return x * x * (3 - 2 * x);
    }
}
