using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface GraphTDA
{
    void InitializeGraph();
    void AddVertex(int v);
    void RemoveVertex(int v);
    SetTDA GetVertices();
    void AddEdge(int v1, int v2, int weight);
    void RemoveEdge(int v1, int v2);
    bool EdgeExists(int v1, int v2);
    int GetEdgeWeight(int v1, int v2);
}

public class AdjacencyMatrixGraph : GraphTDA
{
    static int maxVertices = 100;
    public int[,] adjacencyMatrix;
    public int[] labels;
    public int nodeCount;

    public void InitializeGraph()
    {
        adjacencyMatrix = new int[maxVertices, maxVertices];
        labels = new int[maxVertices];
        nodeCount = 0;
    }

    public void AddVertex(int v)
    {
        labels[nodeCount] = v;
        for (int i = 0; i <= nodeCount; i++)
        {
            adjacencyMatrix[nodeCount, i] = 0;
            adjacencyMatrix[i, nodeCount] = 0;
        }
        nodeCount++;
    }

    public void RemoveVertex(int v)
    {
        int index = VertexToIndex(v);

        for (int k = 0; k < nodeCount; k++)
        {
            adjacencyMatrix[k, index] = adjacencyMatrix[k, nodeCount - 1];
        }

        for (int k = 0; k < nodeCount; k++)
        {
            adjacencyMatrix[index, k] = adjacencyMatrix[nodeCount - 1, k];
        }

        labels[index] = labels[nodeCount - 1];
        nodeCount--;
    }

    public int VertexToIndex(int v)
    {
        int i = nodeCount - 1;
        while (i >= 0 && labels[i] != v)
        {
            i--;
        }
        return i;
    }

    public SetTDA GetVertices()
    {
        SetTDA vertexSet = new LinkedListSet();
        vertexSet.InitializeSet();
        for (int i = 0; i < nodeCount; i++)
        {
            vertexSet.Add(labels[i]);
        }
        return vertexSet;
    }

    public void AddEdge(int v1, int v2, int weight)
    {
        int from = VertexToIndex(v1);
        int to = VertexToIndex(v2);
        adjacencyMatrix[from, to] = weight;
    }

    public void RemoveEdge(int v1, int v2)
    {
        int from = VertexToIndex(v1);
        int to = VertexToIndex(v2);
        adjacencyMatrix[from, to] = 0;
    }

    public bool EdgeExists(int v1, int v2)
    {
        int from = VertexToIndex(v1);
        int to = VertexToIndex(v2);
        return adjacencyMatrix[from, to] != 0;
    }

    public int GetEdgeWeight(int v1, int v2)
    {
        int from = VertexToIndex(v1);
        int to = VertexToIndex(v2);
        return adjacencyMatrix[from, to];
    }
}
