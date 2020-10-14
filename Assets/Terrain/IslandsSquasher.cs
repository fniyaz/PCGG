using UnityEngine;


[RequireComponent(typeof(TerrainTest))]
public class IslandsSquasher : MonoBehaviour
{
    [Range(1, 10)] public float scale = 1;
    [Range(.5f, 2)] public float power = 1;

    public void squash()
    {
        var test = GetComponent<TerrainTest>();

        test.init();
        var threshold = test.seaLevel;

        var terrainData = test.terrain.terrainData;
        var wid = terrainData.heightmapResolution;
        var hei = terrainData.heightmapResolution;
        var heights = terrainData.GetHeights(0, 0, wid, hei);
        var newHeights = new float[wid, hei];

        for (var x = 0; x < wid; x++)
        {
            for (var y = 0; y < hei; y++)
            {
                if (heights[x, y] > threshold)
                {
                    newHeights[x, y] = Mathf.Pow((heights[x, y] - threshold) / (1 - threshold), power)
                        * (1 - threshold)
                        / scale + threshold;
                }
                else
                {
                    newHeights[x, y] = heights[x, y];
                }
            }
        }

        test.terrain.terrainData.SetHeights(0, 0, newHeights);
        test.terrainCollider.terrainData.SetHeights(0, 0, newHeights);
    }
}