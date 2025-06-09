using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DijkstraAlgorithm
{
    public static int[] distances;
    public static string[] paths;

    private static int GetMinimumDistanceIndex(int[] distances, bool[] isInShortestPathSet, int vertexCount)
    {
        int min = int.MaxValue;
        int minIndex = 0;

        for (int v = 0; v < vertexCount; ++v)
        {
            // Always get the node with the smallest known distance
            // Only consider nodes that haven't been finalized yet
            if (!isInShortestPathSet[v] && distances[v] <= min)
            {
                min = distances[v];
                minIndex = v;
            }
        }

        // Return the index of the node with the smallest distance
        return minIndex;
    }

    public static void Dijkstra(AdjacencyMatrixGraph graph, int sourceLabel)
    {
        int[,] matrix = graph.adjacencyMatrix;
        int vertexCount = graph.nodeCount;
        int source = graph.VertexToIndex(sourceLabel);

        distances = new int[vertexCount];
        bool[] isInShortestPathSet = new bool[vertexCount];

        int[] previousNodes = new int[vertexCount];
        int[] currentNodes = new int[vertexCount];

        for (int i = 0; i < vertexCount; ++i)
        {
            // Set all distances to max (unreachable initially)
            distances[i] = int.MaxValue;
            isInShortestPathSet[i] = false;
            previousNodes[i] = currentNodes[i] = -1;
        }

        // Distance to the source node is 0
        distances[source] = 0;
        previousNodes[source] = currentNodes[source] = graph.labels[source];

        // Loop over all vertices
        for (int count = 0; count < vertexCount - 1; ++count)
        {
            int u = GetMinimumDistanceIndex(distances, isInShortestPathSet, vertexCount);
            isInShortestPathSet[u] = true;

            // Check neighbors of the selected node
            for (int v = 0; v < vertexCount; ++v)
            {
                if (!isInShortestPathSet[v] && matrix[u, v] != 0 &&
                    distances[u] != int.MaxValue &&
                    distances[u] + matrix[u, v] < distances[v])
                {
                    distances[v] = distances[u] + matrix[u, v];
                    previousNodes[v] = graph.labels[u];
                    currentNodes[v] = graph.labels[v];
                }
            }
        }

        // Build path strings
        paths = new string[vertexCount];
        int originLabel = graph.labels[source];

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

                for (int j = 0; j < pathList.Count; j++)
                {
                    if (j == 0)
                    {
                        paths[i] = pathList[j].ToString();
                    }
                    else
                    {
                        paths[i] += "," + pathList[j].ToString();
                    }
                }
            }
        }
    }
}
