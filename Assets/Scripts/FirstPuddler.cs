using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Nothke.Math.Coord;

public class FirstPuddler : MonoBehaviour
{

    float[,] heights;

    void Start()
    {
        StartCoroutine(Co());
    }

    IEnumerator Co()
    {
        // fill heights
        heights = new float[32, 32];

        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                heights[x, y] = Mathf.PerlinNoise(x * 0.232f, y * 0.232f);
                heights[x, y] += Mathf.PerlinNoise(x * 0.312f, y * 0.3243f);
            }
        }

        List<Coord> minima = new List<Coord>();

        for (int x = 1; x < 31; x++)
        {
            for (int y = 1; y < 31; y++)
            {
                int highers = 0;

                float h = heights[x, y];
                if (h < heights[x + 1, y]) highers++;
                if (h < heights[x - 1, y]) highers++;
                if (h < heights[x, y + 1]) highers++;
                if (h < heights[x, y - 1]) highers++;

                if (highers == 4) minima.Add(new Coord(x, y));
            }
        }

        yield return null;

        while (true)
        {
            // show minima
            foreach (var min in minima)
            {
                Debug.DrawRay(new Vector3(min.x, 0, min.y), Vector3.up * 2, Color.red);
            }

            yield return null;
        }
    }

    private void Update()
    {
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                Vector3 p0 = new Vector3(x, heights[x, y], y);

                if (x < 32 - 1)
                {
                    Vector3 pr = new Vector3(x + 1, heights[x + 1, y], y);
                    Debug.DrawLine(p0, pr);
                }

                if (y < 32 - 1)
                {
                    Vector3 pf = new Vector3(x, heights[x, y + 1], y + 1);
                    Debug.DrawLine(p0, pf);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {

    }
}
