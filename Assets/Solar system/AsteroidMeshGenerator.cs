using System.Collections.Generic;
using UnityEngine;


public class AsteroidMeshGenerator
{
    public static Mesh generateAsteroid(float radius, float distortion)
    {
        var mesh = new Mesh();
        mesh.name = "Asteroid";

        var horizontalLines = 40;
        var verticalLines = 40;

        var vertices = new Vector3[horizontalLines * verticalLines];
        var posMap = new Dictionary<(int, int), int>();

        List<Vector2> newUVs = new List<Vector2>();
        List<int> newTriangles = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        
        
        var noise = new PerlinNoise2D(5, 5, 5);

        int index = 0;
        var origin = new Vector3(0, 0, 0);
        for (int m = 0; m < horizontalLines; m++)
        {
            for (int n = 0; n < verticalLines - 1; n++)
            {
                var sample = noise.at(m / 8f, n / 8f) * distortion;
                sample *= Mathf.Sin( Mathf.PI * m / (horizontalLines - 1));

                var pos = at(n, m, verticalLines, horizontalLines, radius + sample);
                vertices[index] = pos;
                posMap[(n, m)] = index;
                index++;

                newUVs.Add(new Vector2(n / (verticalLines - 1f), m / (horizontalLines - 1f)));
                normals.Add(- origin + pos);
            }
            vertices[index] = vertices[posMap[(0, m)]];
            posMap[(verticalLines - 1, m)] = index;
            index++;
            newUVs.Add(new Vector2(1, m / (horizontalLines - 1f)));
            normals.Add(normals[posMap[(0, m)]]);
        }

        for (int m = 0; m < horizontalLines; m++)
        {
            for (int n = 0; n < verticalLines; n++)
            {
                var n1 = (n + 1) % (verticalLines);
                var m1 = (m + 1) % (horizontalLines);

                newTriangles.Add(posMap[(n, m1)]);
                newTriangles.Add(posMap[(n1, m)]);
                newTriangles.Add(posMap[(n, m)]);

                newTriangles.Add(posMap[(n1, m1)]);
                newTriangles.Add(posMap[(n1, m)]);
                newTriangles.Add(posMap[(n, m1)]);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = newUVs.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.normals = normals.ToArray();

        return mesh;
    }

    private static Vector3 at(int n, int m, int N, int M, float rad)
    {
        N = N - 1;
        M = M - 1;
        float x = Mathf.Sin(Mathf.PI * m / M) * Mathf.Cos(2 * Mathf.PI * n / N);
        float y = Mathf.Sin(Mathf.PI * m / M) * Mathf.Sin(2 * Mathf.PI * n / N);
        float z = Mathf.Cos(Mathf.PI * m / M);
        return new Vector3(x, y, z) * rad;
    }
}