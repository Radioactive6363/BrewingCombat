using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockSpawnManager : MonoBehaviour
{
    public static BlockSpawnManager Instance { get; private set; }

    public readonly Dictionary<Vector3, BlockType> BlockCoordinateSystem = new Dictionary<Vector3, BlockType>();
    public Vector3 playerSpawnBlock = Vector3.zero;

    [Header("Player Spawning Variables")]
    public GameObject playerPrefab;
    public Vector3 playerSpawnOffset;

    [Header("Dirt Spawning Variables")]
    public BlockClass dirtBlock;
    public Transform dirtParent;
    public Vector2 dirtAreaSize;
    public float dirtSeparationAmount;

    [Header("Tree Spawning Variables")]
    public GameObject[] trees;

    [Header("Object Density Variables")]
    public int obstacleChance;
    
    private bool _spawnedNavPointLeft = false;
    private bool _spawnedNavPointRight = false;
    private bool _spawnedNavPointForward = false;

    private AdjacencyMatrixGraph _adjacencyMatrixGraph = new AdjacencyMatrixGraph();
    private List<int> NavigableVertices = new List<int>();

    private int sourceNode;
    private List<int> DestinationNodes = new List<int>();
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        _adjacencyMatrixGraph.InitializeGraph();
    }

    private void Start()
    {
        if (dirtBlock == null)
        {
            Debug.LogError("Dirt Block prefab not set!");
        }
        else if (dirtParent == null)
        {
            Debug.LogError("Dirt Block parent not set!");
        }

        StartCoroutine(CreateGridCoroutine());
    }

    private IEnumerator CreateGridCoroutine()
    {
        int index = -1;
        
        for (int i = 0; i < dirtAreaSize.y; i++)
        {
            for (int j = 0; j < dirtAreaSize.x; j++)
            {
                yield return new WaitForSeconds(0.1f);
                index++;
                Vector3 blockPosition = dirtParent.position + Vector3.right * dirtSeparationAmount * j + Vector3.forward * dirtSeparationAmount * i;
                BlockClass block = Instantiate(dirtBlock, blockPosition, Quaternion.identity, dirtParent);
                block.SetTreePoint(trees[(int)Random.Range(0f, trees.Length - 1)]);

                if (j == (int)dirtAreaSize.x / 2 && i == 0)
                {
                    playerSpawnBlock = dirtParent.position + Vector3.right * dirtSeparationAmount * j + Vector3.forward * dirtSeparationAmount * i;
                    Instantiate(playerPrefab, playerSpawnBlock + playerSpawnOffset, Quaternion.identity);
                    BlockCoordinateSystem.Add(playerSpawnBlock, BlockType.Normal);
                    
                    _adjacencyMatrixGraph.AddVertex(index); // We add the vertex of the middle point.
                    NavigableVertices.Add(index); // We add the vertex as a navigable vertex.
                    sourceNode = index; // We set the sourceNode as the vertex where the player spawns in.
                    Debug.Log("SOURCE NODE IS: " + sourceNode);
                    
                    continue;
                }
                else if (i != 0 && j == 0 || Mathf.Approximately(j, dirtAreaSize.x - 1))
                {
                    if (_spawnedNavPointLeft && j == 0 || _spawnedNavPointRight && Mathf.Approximately(j, dirtAreaSize.x - 1))
                    {
                        block.Init(0, (int)Random.Range(1f, obstacleChance));
                    }
                    else
                    {
                        block.Init((int)Random.Range(1f, dirtAreaSize.y - i), (int)Random.Range(1f, obstacleChance));

                        if (j == 0 && block.CheckSpawnNavPoint())
                        {
                            _spawnedNavPointLeft = true;
                        }
                        else if (Mathf.Approximately(j, dirtAreaSize.x - 1) && block.CheckSpawnNavPoint())
                        {
                            _spawnedNavPointRight = true;
                        }
                    }
                }
                else if (j < dirtAreaSize.x - 1 && !Mathf.Approximately(i, dirtAreaSize.y - 1) || _spawnedNavPointForward)
                {
                    block.Init(0, (int)Random.Range(1f, obstacleChance));
                }
                else
                {
                    block.Init((int)Random.Range(1f, dirtAreaSize.x - j), (int)Random.Range(1f, obstacleChance));

                    if (block.CheckSpawnNavPoint())
                    {
                        _spawnedNavPointForward = true;
                    }
                }

                if (block.CheckSpawnNavPoint())
                {
                    BlockCoordinateSystem.Add(dirtParent.position + Vector3.right * dirtSeparationAmount * j + Vector3.forward * dirtSeparationAmount * i, BlockType.NavigationPoint);
                    _adjacencyMatrixGraph.AddVertex(index); // We add a possible navigation point as a vertex for the adjacencyMatrixGraph
                    NavigableVertices.Add(index); // We add the vertex as a navigable vertex.
                    DestinationNodes.Add(index);
                    Debug.Log("FOUND A NAVPOINT!!!, INDEX: " + index);
                }
                else if (block.CheckSpawnObsPoint())
                {
                    BlockCoordinateSystem.Add(dirtParent.position + Vector3.right * dirtSeparationAmount * j + Vector3.forward * dirtSeparationAmount * i, BlockType.Obstructed);
                }
                else
                {
                    BlockCoordinateSystem.Add(dirtParent.position + Vector3.right * dirtSeparationAmount * j + Vector3.forward * dirtSeparationAmount * i, BlockType.Normal);
                    _adjacencyMatrixGraph.AddVertex(index); // We add a possible normal navigation point as a vertex for the adjacencyMatrixGraph
                    NavigableVertices.Add(index); // We add the vertex as a navigable vertex.
                    Debug.Log("FOUND A BASIC NAVPOINT, INDEX: " + index);
                }
            }
        }
        
                
        // Creating an octagonal direction system to check for each vertex neighbor availability.
        int width = (int)dirtAreaSize.y;
        int[] neighborOffsets = {
            -width - 1, -width, -width + 1,
            -1,             1,
            width - 1,  width,  width + 1
        };

        // Setting the neighbors of each vertex in the system
        foreach (var vertex in NavigableVertices)
        {
            foreach (int offset in neighborOffsets)
            {
                if (_adjacencyMatrixGraph.Labels.Contains(vertex + offset))
                    _adjacencyMatrixGraph.AddEdge(vertex, vertex + offset, 1);
            }
        }
        
        DijkstraAlgorithm.Dijkstra(_adjacencyMatrixGraph, sourceNode);
        
        if (DijkstraAlgorithm.Distances[DestinationNodes[0]] == int.MaxValue)
        {
            Debug.Log($"\nNo hay camino hacia el nodo {DestinationNodes[0]}");
        }
        else
        {
            string path = DijkstraAlgorithm.Paths[DestinationNodes[0]];
            Debug.Log($"\nCamino del nodo {sourceNode} hacia el nodo {DestinationNodes[0]}: Distancia = {DijkstraAlgorithm.Distances[DestinationNodes[0]]}, Camino = {path}");
        }
        
        yield break;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("MapSelection");
        }
    }
}
