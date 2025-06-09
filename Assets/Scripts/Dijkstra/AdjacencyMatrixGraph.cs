using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IGraphADT
{
    void InitializeGraph();
    void AddVertex(int v);
    void RemoveVertex(int v);
    ISetADT GetVertices();
    void AddEdge(int v1, int v2, int weight);
    void RemoveEdge(int v1, int v2);
    bool EdgeExists(int v1, int v2);
    int GetEdgeWeight(int v1, int v2);
}

public class AdjacencyMatrixGraph : IGraphADT
{
    static int _maxVertices = 100;
    public int[,] AdjacencyMatrix;
    public int[] Labels;
    public int NodeCount;

    public void InitializeGraph()
    {
        AdjacencyMatrix = new int[_maxVertices, _maxVertices];
        Labels = new int[_maxVertices];
        NodeCount = 0;
    }

    public void AddVertex(int v)
    {
        Labels[NodeCount] = v;
        for (int i = 0; i <= NodeCount; i++)
        {
            AdjacencyMatrix[NodeCount, i] = 0;
            AdjacencyMatrix[i, NodeCount] = 0;
        }
        NodeCount++;
    }

    public void RemoveVertex(int v)
    {
        int index = VertexToIndex(v);

        for (int k = 0; k < NodeCount; k++)
        {
            AdjacencyMatrix[k, index] = AdjacencyMatrix[k, NodeCount - 1];
        }

        for (int k = 0; k < NodeCount; k++)
        {
            AdjacencyMatrix[index, k] = AdjacencyMatrix[NodeCount - 1, k];
        }

        Labels[index] = Labels[NodeCount - 1];
        NodeCount--;
    }

    public int VertexToIndex(int v)
    {
        int i = NodeCount - 1;
        while (i >= 0 && Labels[i] != v)
        {
            i--;
        }
        return i;
    }

    public ISetADT GetVertices()
    {
        ISetADT vertexSet = new LinkedListSet();
        vertexSet.InitializeSet();
        for (int i = 0; i < NodeCount; i++)
        {
            vertexSet.Add(Labels[i]);
        }
        return vertexSet;
    }

    public void AddEdge(int v1, int v2, int weight)
    {
        int from = VertexToIndex(v1);
        int to = VertexToIndex(v2);
        AdjacencyMatrix[from, to] = weight;
    }

    public void RemoveEdge(int v1, int v2)
    {
        int from = VertexToIndex(v1);
        int to = VertexToIndex(v2);
        AdjacencyMatrix[from, to] = 0;
    }

    public bool EdgeExists(int v1, int v2)
    {
        int from = VertexToIndex(v1);
        int to = VertexToIndex(v2);
        return AdjacencyMatrix[from, to] != 0;
    }

    public int GetEdgeWeight(int v1, int v2)
    {
        int from = VertexToIndex(v1);
        int to = VertexToIndex(v2);
        return AdjacencyMatrix[from, to];
    }
}
