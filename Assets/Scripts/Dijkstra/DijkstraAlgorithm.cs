using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class DijkstraAlgorithm
{
    public static int[] Distances;
    public static string[] Paths;
    public static int[][] PathsInt;
    public static int GridWidth;
    
    private static int GetMinimumDistanceIndex(int[] distances, bool[] isInShortestPathSet, int vertexCount)
    {
        int min = int.MaxValue;
        int minIndex = 0;

        for (int v = 0; v < vertexCount; ++v)
        {
            if (!isInShortestPathSet[v] && distances[v] <= min)
            {
                min = distances[v];
                minIndex = v;
            }
        }

        return minIndex;
    }

    public static void Dijkstra(AdjacencyMatrixGraph graph, int sourceLabel, int gridWidth)
    {
        int[,] matrix = graph.AdjacencyMatrix;
        int vertexCount = graph.NodeCount;
        int source = graph.VertexToIndex(sourceLabel);

        Distances = new int[vertexCount];
        bool[] isInShortestPathSet = new bool[vertexCount];

        int[] previousNodes = new int[vertexCount];
        int[] currentNodes = new int[vertexCount];

        GridWidth = gridWidth;
        
        for (int i = 0; i < vertexCount; ++i)
        {
            // Set all distances to max (unreachable initially)
            Distances[i] = int.MaxValue;
            isInShortestPathSet[i] = false;
            previousNodes[i] = currentNodes[i] = -1;
        }

        // Distance to the source node is 0
        Distances[source] = 0;
        previousNodes[source] = currentNodes[source] = graph.Labels[source];

        // Loop over all vertices
        for (int count = 0; count < vertexCount - 1; ++count)
        {
            int u = GetMinimumDistanceIndex(Distances, isInShortestPathSet, vertexCount);
            isInShortestPathSet[u] = true;

            // Check neighbors of the selected node
            for (int v = 0; v < vertexCount; ++v)
            {
                if (!isInShortestPathSet[v] && matrix[u, v] != 0 &&
                    Distances[u] != int.MaxValue &&
                    Distances[u] + matrix[u, v] < Distances[v])
                {
                    Distances[v] = Distances[u] + matrix[u, v];
                    previousNodes[v] = graph.Labels[u];
                    currentNodes[v] = graph.Labels[v];
                }
            }
        }

        // Build path strings and PathsInt
        Paths = new string[vertexCount];
        PathsInt = new int[vertexCount][];
        
        int originLabel = graph.Labels[source];

        for (int i = 0; i < vertexCount; i++)
        {
            if (previousNodes[i] != -1)
            {
                List<int> pathList = new List<int>();
                pathList.Add(previousNodes[i]);
                pathList.Add(currentNodes[i]);

                while (pathList[0] != originLabel)
                {
                    for (int j = 0; j < vertexCount; j++)
                    {
                        if (j != source && pathList[0] == currentNodes[j])
                        {
                            pathList.Insert(0, previousNodes[j]);
                            break;
                        }
                    }
                }

                // Initialize PathsInt[i] with the correct size
                PathsInt[i] = new int[pathList.Count];

                // Build path string and populate PathsInt
                for (int j = 0; j < pathList.Count; j++)
                {
                    PathsInt[i][j] = pathList[j];

                    if (j == 0)
                    {
                        Paths[i] = pathList[j].ToString();
                    }
                    else
                    {
                        Paths[i] += "," + pathList[j].ToString();
                    }
                }
            }
            else
            {
                PathsInt[i] = new int[0]; // Empty array if no path exists
            }
        }
    }

    public static Vector3[] GetPathsXZ(int index)
    {
        // Check if the index is valid and if there's a path at that index
        if (index < 0 || index >= PathsInt.Length || PathsInt[index] == null || PathsInt[index].Length <= 1)
        {
            return new Vector3[0]; // Return empty array if no valid path
        }

        Vector3[] path = new Vector3[PathsInt[index].Length - 1];

        // Build PathsXZ with direction vectors
        for (int j = 0; j < PathsInt[index].Length - 1; j++)
        {
            int direction = PathsInt[index][j + 1] - PathsInt[index][j];
            Vector3 directionVector = Vector3.zero;

            switch (direction)
            {
                case 1: // right
                    directionVector.x = 1;
                    break;
                case -1: // left
                    directionVector.x = -1;
                    break;
                case var n when n == -GridWidth: // down
                    directionVector.z = -1;
                    break;
                case var n when n == GridWidth: // up
                    directionVector.z = 1;
                    break;
                case var n when n == -GridWidth + 1: // down-right (diagonal)
                    directionVector.x = 1;
                    directionVector.z = -1;
                    break;
                case var n when n == -GridWidth - 1: // down-left (diagonal)
                    directionVector.x = -1;
                    directionVector.z = -1;
                    break;
                case var n when n == GridWidth + 1: // up-right (diagonal)
                    directionVector.x = 1;
                    directionVector.z = 1;
                    break;
                case var n when n == GridWidth - 1: // up-left (diagonal)
                    directionVector.x = -1;
                    directionVector.z = 1;
                    break;
                default:
                    // unexpected
                    directionVector = Vector3.zero;
                    break;
            }

            path[j] = directionVector;
        }

        return path;
    }
}