using Nothke.Math.Coord;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class FirstPuddler : MonoBehaviour
{

    float[,] heights;

    void Start()
    {
        StartCoroutine(Co());
    }

    private void Update()
    {
        DrawGrid();
    }

    const int size = 128;
    bool[,] filled;

    float H(Coord c)
    {
        return heights[c.x, c.y];
    }

    bool F(Coord c)
    {
        return filled[c.x, c.y];
    }

    bool Within(Coord c)
    {
        return c.x >= 0 && c.x < size && c.y >= 0 && c.y < size;
    }

    bool OnEdge(Coord c)
    {
        return !(c.x > 0 && c.x < size - 1 && c.y > 0 && c.y < size - 1);
    }

    List<Coord> Neighbors(Coord c)
    {
        List<Coord> neighbors = new List<Coord>();
        Coord c0 = c + new Coord(-1, -1);
        Coord c1 = c + new Coord(0, -1);
        Coord c2 = c + new Coord(1, -1);
        Coord c3 = c + new Coord(-1, 0);
        Coord c4 = c + new Coord(1, 0);
        Coord c5 = c + new Coord(-1, 1);
        Coord c6 = c + new Coord(0, 1);
        Coord c7 = c + new Coord(1, 1);

        if (Within(c0)) neighbors.Add(c0);
        if (Within(c1)) neighbors.Add(c1);
        if (Within(c2)) neighbors.Add(c2);
        if (Within(c3)) neighbors.Add(c3);
        if (Within(c4)) neighbors.Add(c4);
        if (Within(c5)) neighbors.Add(c5);
        if (Within(c6)) neighbors.Add(c6);
        if (Within(c7)) neighbors.Add(c7);

        return neighbors;
    }

    List<Coord> Expand(List<Coord> coords)
    {
        HashSet<Coord> expanded = new HashSet<Coord>();
        expanded.UnionWith(coords);

        foreach (var c in coords)
        {
            // get neighbors of each coord and add to hashset
            var neis = Neighbors(c);
            expanded.UnionWith(neis);
        }

        return new List<Coord>(expanded);
    }

    IEnumerator Co()
    {
        // fill heights
        heights = new float[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                heights[x, y] = Mathf.PerlinNoise(x * 0.123f, y * 0.123f) * 10;
                heights[x, y] += Mathf.PerlinNoise(x * 0.04312f, y * 0.043243f) * 20;

                //heights[x, y] += Mathf.PerlinNoise(x * 0.312f, y * 0.3243f) * 20;


            }
        }

        filled = new bool[size, size];
        List<Coord> minima = new List<Coord>();

        for (int x = 1; x < size - 1; x++)
        {
            for (int y = 1; y < size - 1; y++)
            {
                int highers = 0;

                float h = heights[x, y];

                var c = new Coord(x, y);
                var neis = Neighbors(c);

                foreach (var nei in neis)
                {
                    if (h <= H(nei)) highers++;
                }

                if (highers == 8)
                {
                    filled[x, y] = true;
                    minima.Add(c);

                    yield return null;
                    Ray(c, 1, Color.cyan, 10);
                }
            }
        }

        minima = minima.OrderBy(c => H(c)).ToList();

        List<List<Coord>> allCollected = new List<List<Coord>>();

        int skipct = 0;

        foreach (var min in minima)
        {
            // Get the lowest point of the neighbors
            var queue = Neighbors(min);

            HashSet<Coord> taken = new HashSet<Coord>();
            taken.Add(min);
            taken.UnionWith(queue);

            // order by height
            queue = queue.OrderBy(c => H(c)).ToList();

            List<Coord> passed = new List<Coord>();
            passed.Add(min);

            float saddleHeight = 0;

            while (queue.Count != 0)
            {
                var next = queue[0];
                queue.RemoveAt(0);

                passed.Add(next);

                var nextNeighbors = Neighbors(next);

                bool isSaddle = false;

                // Check 
                foreach (var nextNeighbor in nextNeighbors)
                {
                    if (taken.Contains(nextNeighbor)) continue;

                    taken.Add(nextNeighbor);

                    isSaddle = H(nextNeighbor) < H(next) || OnEdge(next);

                    if (isSaddle)
                    {
                        // check if already exists
                        bool exists = false;

                        foreach (var collected in allCollected)
                        {
                            if (collected.Contains(next))
                            {
                                taken.UnionWith(collected);
                                exists = true;
                                break;
                            }
                        }

                        if (!exists)
                        {
                            // SADDLE!!
                            isSaddle = true;
                            saddleHeight = H(next);
                            Line(nextNeighbor, next, Color.red, 1);
                            goto Bail;
                        }

                    }
                    else // if not saddle
                    {
                        queue.Add(nextNeighbor);
                        Line(nextNeighbor, next, Color.yellow, 1);
                    }

                    //yield return null;
                }

                skipct++;
                if (skipct > 25)
                {
                    yield return null;
                    skipct = 0;
                }

                queue = queue.OrderBy(c => H(c)).ToList();
            }

            Bail:

            allCollected.Add(passed);

            foreach (var c in passed)
            {
                Ray(c, 1, Color.green, 1);

                // modify height
                heights[c.x, c.y] = saddleHeight;
                filled[c.x, c.y] = true;
            }

            yield return null;
        }
    }

    Vector3 P(Coord c)
    {
        return new Vector3(c.x, heights[c.x, c.y], c.y);
    }

    void Line(Coord c0, Coord c1, Color c, float duration = 0)
    {
        if (duration == 0)
            Debug.DrawLine(P(c0), P(c1), c);
        else
            Debug.DrawLine(P(c0), P(c1), c, duration);
    }

    void Ray(Coord c0, float scale, Color c, float duration = 0)
    {
        if (duration == 0)
            Debug.DrawRay(P(c0), Vector3.up * scale, c);
        else
            Debug.DrawRay(P(c0), Vector3.up * scale, c, duration);
    }

    void DrawGrid()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector3 p0 = new Vector3(x, heights[x, y], y);

                if (x < size - 1)
                {
                    Vector3 pr = new Vector3(x + 1, heights[x + 1, y], y);
                    Debug.DrawLine(p0, pr);
                }

                if (y < size - 1)
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
