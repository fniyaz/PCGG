using UnityEngine;

public static class CloudsTexture
{
    private static Color c1 = Color.black;
    private static Color c2 = Color.white;

    public static Texture2D[] run(
        (int, int) wid,
        (int, int) hei,
        (int, int) depth,
        Color c1,
        Color c2,
        float threshhold = 0
    )
    {
        var noise = new PerlinNoise3D(wid.Item1, wid.Item1, depth.Item1, 3);
        var texture = new Texture2D[unfold(depth)];

        for (var k = 0; k < unfold(depth); k++)
        {
            texture[k] = new Texture2D(unfold(wid), unfold(hei), TextureFormat.ARGB32, false);
            // texture[k].filterMode = FilterMode.Point;

            for (var i = 0; i < unfold(wid); i++)
            {
                for (var j = 0; j < unfold(hei); j++)
                {
                    var c = noise.at((float) i / wid.Item2, (float) j / hei.Item2, (float) k / depth.Item2) / 2 + 0.5f;

                    var color = Color.Lerp(c2, c1, c);
                    color[3] = c > threshhold ? (c - threshhold) / (1 - threshhold) : 0;

                    texture[k].SetPixel(i, j, color);
                }
            }

            texture[k].Apply();
        }

        return texture;
    }

    private static int unfold((int, int) a)
    {
        return a.Item1 * a.Item2;
    }
}
