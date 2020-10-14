using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class PerlinNoise2D
{
    private Vector2[,] data;
    private PerlinNoise2D p;

    public PerlinNoise2D(int wid, int hei, int octaves)
    {
        if (octaves < 1)
        {
            throw new ArgumentException("Can't be negative", "octaves");
        }

        if (octaves == 1)
        {
            setUpGrads(wid, hei);
            p = null;
        }
        else
        {
            setUpGrads(wid, hei);
            p = new PerlinNoise2D(wid * 2, hei * 2, octaves - 1);
        }
    }

    public PerlinNoise2D(int wid, int hei)
    {
        setUpGrads(wid, hei);
    }

    public float at(float x, float y)
    {
        return at(new Vector2(x, y));
    }

    public float at(Vector2 point)
    {
        int sx = (int) Math.Floor(point.x);
        int sy = (int) Math.Floor(point.y);

        var qp = point - new Vector2(sx, sy);

        var w = data.GetLength(0);
        var h = data.GetLength(1);

        var sx1 = sx + 1;
        var sy1 = sy + 1;

        var noise =
            Mathf.Lerp(
                Mathf.Lerp(
                    Vector2.Dot(data[sx % w, sy % h], point - new Vector2(sx, sy)),
                    Vector2.Dot(data[sx1 % w, sy % h], point - new Vector2(sx1, sy)),
                    smoother(qp.x)
                ),
                Mathf.Lerp(
                    Vector2.Dot(data[sx % w, sy1 % h], point - new Vector2(sx, sy1)),
                    Vector2.Dot(data[sx1 % w, sy1 % h], point - new Vector2(sx1, sy1)),
                    smoother(qp.x)
                ),
                smoother(qp.y)
            );

        if (p != null)
        {
            noise += p.at(point * 2) / 2;
        }
        
        return noise;
    }

    private void setUpGrads(int wid, int hei)
    {
        data = new Vector2[wid, hei];

        for (int x = 0; x < wid; x++)
        {
            for (int y = 0; y < hei; y++)
            {
                data[x, y] = Random.insideUnitCircle;
            }
        }
    }

    private float smoother(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }
}
