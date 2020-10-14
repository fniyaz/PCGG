using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class DiamondSquare
{
    public static void run(ref float[,] arr, int startStep, (float, float) rndBounds)
    {
        if (!isPot(startStep))
        {
            throw new ArgumentException($"startStep={startStep} is not a Power Of Two!");
        }

        var width = arr.GetLength(0);
        var height = arr.GetLength(1);

        for (var step = startStep; step > 1; step /= 2)
        {
            var stepF = 1 - (startStep - step) / (float) (startStep - 2);

            for (int x = 0; x < width - step + 1; x += step)
            {
                for (int y = 0; y < height - step + 1; y += step)
                {
                    var xm = x + step / 2;
                    var ym = y + step / 2;

                    // square
                    arr[safeX(ref arr, xm), safeY(ref arr, ym)] = (arr[safeX(ref arr, x), safeY(ref arr, y)]
                                   + arr[safeX(ref arr, x), safeY(ref arr, y + step)]
                                   + arr[safeX(ref arr, x + step), safeY(ref arr, y)]
                                   + arr[safeX(ref arr, x + step), safeY(ref arr, y + step)]
                                  ) / 4
                                  + Random.Range(rndBounds.Item1, rndBounds.Item2) * stepF;
                }
            }

            for (int x = 0; x < width - step + 1; x += step)
            {
                for (int y = 0; y < height - step + 1; y += step)
                {
                    // diamond
                    var hs = step / 2;
                    foreach (var (nx, ny) in coordsForDiamond(ref arr, x + hs, y + hs, step))
                    {
                        var dmd = coordsForDiamond(ref arr, nx, ny, step);

                        arr[safeX(ref arr, nx), safeY(ref arr, ny)] = sumUp(ref arr, dmd) / dmd.Count
                                      + Random.Range(rndBounds.Item1, rndBounds.Item2) * stepF;
                    }
                }
            }
        }
    }

    public static void Main()
    {
        var arr = new float[5, 5];

        arr[safeX(ref arr, 0), safeY(ref arr, 0)] = 1;
        arr[safeX(ref arr, 0), safeY(ref arr, 4)] = 5;
        arr[safeX(ref arr, 4), safeY(ref arr, 4)] = 8;
        arr[safeX(ref arr, 2), safeY(ref arr, 2)] = 1;

        run(ref arr, 2, (0, 0));

        for (var x = 0; x < 5; x++)
        {
            for (var y = 0; y < 5; y++)
            {
                Console.Write($"{arr[safeX(ref arr, x), safeY(ref arr, y)]:0.0} ");
            }

            Console.WriteLine();
        }
    }

    private static bool isPot(int a)
    {
        return (a & (a - 1)) == 0;
    }

    private static List<(int, int)> coordsForDiamond(ref float[,] arr, int x, int y, int step)
    {
        var hs = step / 2;
        return validCoords(ref arr, new[]
        {
            (x, y + hs),
            (x, y - hs),
            (x + hs, y),
            (x - hs, y)
        });
    }

    private static List<(int, int)> validCoords(ref float[,] arr, (int, int)[] toValidate)
    {
        var tr = new List<(int, int)>();

        var wid = arr.GetLength(0);
        var hei = arr.GetLength(1);

        foreach (var (x, y) in toValidate)
        {
            if (x >= 0 && y >= 0)
                tr.Add((x, y));
            // tr.Add(
            //     (
            //         (x % wid + wid) % wid,
            //         (y % hei + hei) % hei
            //     )
            // );
        }

        return tr;
    }

    private static float sumUp(ref float[,] arr, List<(int, int)> coords)
    {
        float sum = 0;
        foreach (var (x, y) in coords)
        {
            sum += arr[safeX(ref arr, x), safeY(ref arr, y)];
        }

        return sum;
    }

    private static float smoothstep(float x)
    {
        return x * x * (3 - 2 * x);
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
