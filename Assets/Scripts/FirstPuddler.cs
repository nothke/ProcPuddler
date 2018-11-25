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

        yield return null;
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
