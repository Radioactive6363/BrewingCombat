using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockSpawnManager : MonoBehaviour
{
    [Header("Dirt Spawning Variables")]
    public BlockClass dirtBlock;
    public Transform dirtParent;
    public Vector2 dirtAreaSize;
    public float dirtSeparationAmount;

    [Header("Object Density Variables")]
    public int obstacleChance;

    private bool spawnedNavPointLeft = false;
    private bool spawnedNavPointRight = false;
    private bool spawnedNavPointForward = false;

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

        for (int i = 0; i < dirtAreaSize.y; i++)
        {
            for (int j = 0; j < dirtAreaSize.x; j++)
            {
                BlockClass block = Instantiate(dirtBlock, dirtParent.position + Vector3.right * dirtSeparationAmount * j + Vector3.forward * dirtSeparationAmount * i, Quaternion.identity, dirtParent);

                if (j == (int)dirtAreaSize.x / 2 && i == 0)
                {
                    continue;
                }
                else if (i != 0 && j == 0 || j == dirtAreaSize.x - 1)
                {
                    if (spawnedNavPointLeft && j == 0 || spawnedNavPointRight && j == dirtAreaSize.x - 1)
                    {
                        block.Init(0, (int)Random.Range(1f, obstacleChance));
                    }
                    else
                    {
                        block.Init((int)Random.Range(1f, dirtAreaSize.y - i), (int)Random.Range(1f, obstacleChance));

                        if (j == 0 && block.CheckSpawnNavPoint())
                        {
                            spawnedNavPointLeft = true;
                        }
                        else if (j == dirtAreaSize.x - 1 && block.CheckSpawnNavPoint())
                        {
                            spawnedNavPointRight = true;
                        }
                    }
                }
                else if (j < dirtAreaSize.x - 1 && i != dirtAreaSize.y - 1 || spawnedNavPointForward)
                {
                    block.Init(0, (int)Random.Range(1f, obstacleChance));
                }
                else
                {
                    block.Init((int)Random.Range(1f, dirtAreaSize.x - j), (int)Random.Range(1f, obstacleChance));

                    if (block.CheckSpawnNavPoint())
                    {
                        spawnedNavPointForward = true;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("MapSelection");
        }
    }
}
