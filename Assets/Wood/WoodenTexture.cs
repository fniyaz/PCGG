using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class WoodenTexture
{
    // static Color dark = new Color(139 / 255f, 69 / 255f, 19 / 255f);
    // static Color light = new Color(200 / 255f, 199 / 255f, 137 / 255f);

    public static Texture2D getLines(
        bool vertical,
        int width,
        int height,
        int num,
        Color dark,
        Color light,
        float highGrain = 1,
        float lowGrain = 1/5f,
        float _2dNoise = 1/5f
    )
    {
        var noise = new PerlinNoise2D(width, height, 5);
        var widthNoise = new PerlinNoise2D(width, 1, 5);
        var highGrainNoise = new PerlinNoise2D(width, 1, 1);
        var texture = new Texture2D(width * num, height * num, TextureFormat.RGBA32, false);
        var offset = Random.Range(0, Mathf.PI * 2);

        if (vertical)
        {
            (width, height) = (height, width);
        }

        for (int y = 0; y < height * num; y++)
        {
            var fy = (float) y / num;
            var widthDisturb = widthNoise.at(fy, 0.5f) * highGrain;
            widthDisturb += highGrainNoise.at(fy, 0.5f) * lowGrain;

            for (int x = 0; x < width * num; x++)
            {
                var fx = (float) x / num;
                var noiseVal = noise.at(fx, fy);

                var pos = new Vector2(fx, fy);
                pos = shift(pos, noiseVal * _2dNoise);

                var linePattern = getLinePattern(pos, widthDisturb, offset);

                var col = Color.Lerp(
                    light,
                    dark,
                    Mathf.Pow(linePattern, 1f / 8)
                );

                if (linePattern > 0.95f)
                {
                    col = Color.Lerp(Color.black, dark, 0.5f + 1 / 0.1f * (1 - linePattern));
                    // col = Color.Lerp(Color.black, dark, 0.5f);
                }

                var disCol = new Color(col.r, col.g, col.b);

                if (vertical)
                {
                    texture.SetPixel(y, x, disCol);
                }
                else
                {
                    texture.SetPixel(x, y, disCol);
                }
            }
        }

        texture.Apply();

        return texture;
    }

    private static float getLinePattern(Vector2 pos, float disturb, float offset)
    {
        var y = pos.y;

        var sin = Mathf.Sin(offset + (y + disturb) * 19) / 2 + .5f;

        return sin;
    }

    private static Vector2 shift(Vector2 pos, float dy)
    {
        return new Vector2(pos.x, pos.y + dy);
    }

    // todo probably don't need
    public static Texture2D getCracks(int width, int height)
    {
        var xyPeriod = 8.0f; //number of rings
        var turbPower = 0.2f; //makes twists
        var turbSize = 32.0f; //initial size of the turbulence

        var noise = new PerlinNoise2D(width / 10, height / 100, 1);

        var texWid = width;
        var texHei = height;
        var texture = new Texture2D(texWid, texHei, TextureFormat.ARGB32, false);

        for (int x = 0; x < texWid; x++)
        {
            for (int y = 0; y < texHei; y++)
            {
                var noiseVal = noise.at((float) x / 10, (float) y / 100) / (.5f * Mathf.Sqrt(2)) + .5f;
                noiseVal = noiseVal > 0.9 ? (noiseVal - 0.9f) / (1 - 0.9f) : 0;


                var noiseColor = new Color(noiseVal, noiseVal, noiseVal);
                var treeColor = noiseColor;

                texture.SetPixel(x, y, Color.Lerp(treeColor, noiseColor, 0f));
            }
        }

        texture.Apply();

        return texture;
    }
}