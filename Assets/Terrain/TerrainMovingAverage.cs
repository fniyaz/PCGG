using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMovingAverage : MonoBehaviour
{
    [Range(1, 10)] public int windowingLevel = 1;
    
    
    public void run()
    {
        var terrina = GetComponent<Terrain>();
        var terrainCollider = GetComponent<TerrainCollider>();

        var data = terrina.terrainData;
        
        int width = data.heightmapResolution;
        int height = data.heightmapResolution;

        var heights = data.GetHeights(0, 0, width, height);
        var newHeights = new float[heights.GetLength(0), heights.GetLength(1)];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var window = windowAt(ref heights, x, y, windowingLevel * 2 + 1);
                newHeights[x, y] = sumUp(ref heights, window) / window.Count;
            }
        }
        
        data.SetHeights(0, 0, newHeights);

        terrina.terrainData = data;
        terrainCollider.terrainData = data;
    }

    private static List<(int, int)> windowAt(ref float[,] arr, int xAt, int yAt, int width)
    {
        var tr = new List<(int, int)>();

        for (var x = xAt - width / 2; x <= xAt + width / 2; x++)
        {
            for (var y = yAt - width / 2; y <= yAt + width / 2; y++)
            {
                // if (x >= 0 && x < arr.GetLength(0) && y >= 0 && y < arr.GetLength(1))
                tr.Add((x, y));
            }
        }

        return tr;
    }

    private static float sumUp(ref float[,] arr, List<(int, int)> toSum)
    {
        float sum = 0;
        foreach (var (x, y) in toSum)
        {
            sum += arr[safeX(ref arr, x), safeY(ref arr, y)];
        }

        return sum;
    }
    
    private static int safeX(ref float[,] arr, int x)
    {
        var wid = arr.GetLength(0);
        var safeX = (x % wid + wid) % wid;
        return safeX;
    }
    
    private static int safeY(ref float[,] arr, int y)
    {
        var hei = arr.GetLength(1);
        var safeY = (y % hei + hei) % hei;
        return safeY;
    }
}
