using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class PerlinNoise3D
{
    private Vector3[,,] data;
    private PerlinNoise3D p;

    public PerlinNoise3D(int wid, int hei, int depth, int octaves)
    {
        if (octaves < 1)
        {
            throw new ArgumentException("Can't be negative", "octaves");
        }

        if (octaves == 1)
        {
            setUpGrads(wid, hei, depth);
            p = null;
        }
        else
        {
            setUpGrads(wid, hei, depth);
            p = new PerlinNoise3D(wid * 2, hei * 2, depth * 2, octaves - 1);
        }
    }

    public PerlinNoise3D(int wid, int hei, int depth)
    {
        setUpGrads(wid, hei, depth);
    }

    public float at(float x, float y, float z)
    {
        return at(new Vector3(x, y, z));
    }

    public float at(Vector3 point)
    {
        int sx = (int) Math.Floor(point.x);
        int sy = (int) Math.Floor(point.y);
        int sz = (int) Math.Floor(point.z);

        var qp = point - new Vector3(sx, sy, sz);

        var w = data.GetLength(0);
        var h = data.GetLength(1);
        var d = data.GetLength(2);

        var sx1 = sx + 1;
        var sy1 = sy + 1;
        var sz1 = sz + 1;

        var noise =
            Mathf.Lerp(
                Mathf.Lerp(
                    Mathf.Lerp(
                        Vector3.Dot(data[sx % w, sy % h, sz % d], point - new Vector3(sx, sy, sz)),
                        Vector3.Dot(data[sx1 % w, sy % h, sz % d], point - new Vector3(sx1, sy, sz)),
                        smoother(qp.x)
                    ),
                    Mathf.Lerp(
                        Vector3.Dot(data[sx % w, sy1 % h, sz % d], point - new Vector3(sx, sy1, sz)),
                        Vector3.Dot(data[sx1 % w, sy1 % h, sz % d], point - new Vector3(sx1, sy1, sz)),
                        smoother(qp.x)
                    ),
                    smoother(qp.y)
                ),
                Mathf.Lerp(
                    Mathf.Lerp(
                        Vector3.Dot(data[sx % w, sy % h, sz1 % d], point - new Vector3(sx, sy, sz1)),
                        Vector3.Dot(data[sx1 % w, sy % h, sz1 % d], point - new Vector3(sx1, sy, sz1)),
                        smoother(qp.x)
                    ),
                    Mathf.Lerp(
                        Vector3.Dot(data[sx % w, sy1 % h, sz1 % d], point - new Vector3(sx, sy1, sz1)),
                        Vector3.Dot(data[sx1 % w, sy1 % h, sz1 % d], point - new Vector3(sx1, sy1, sz1)),
                        smoother(qp.x)
                    ),
                    smoother(qp.y)
                ),
                smoother(qp.z)
            );

        if (p != null)
        {
            noise += p.at(point * 2) / 2;
        }

        return noise;
    }

    private void setUpGrads(int wid, int hei, int depth)
    {
        data = new Vector3[wid, hei, depth];

        for (int x = 0; x < wid; x++)
        {
            for (int y = 0; y < hei; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    data[x, y, z] = Random.insideUnitSphere;
                }
            }
        }
    }

    private float smoother(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }
}