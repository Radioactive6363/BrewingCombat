using UnityEngine;

public enum BlockType
{
    Normal,
    Obstructed,
    NavigationPoint
}

public class BlockClass : MonoBehaviour
{
    [Header("Block Object Spawning Properties")]
    public Transform objectParent;
    public Transform obstacleParent;

    [Header("Navigation Points")]
    public GameObject combatPoint;
    public GameObject frontierCombatPoint;
    public GameObject exchangePoint;

    [Header("Obstacle Points")]
    private GameObject treePoint;

    private bool spawnedNavigationPoint = false;
    private bool spawnedObstaclePoint = false;

    public void Init(int navigationPointSpawnRoll, int obstacleSpawnRoll)
    {
        if (navigationPointSpawnRoll == 1)
        {
            if (combatPoint != null)
            {
                Instantiate(combatPoint, objectParent.position, treePoint.transform.rotation, objectParent);
            }

            spawnedNavigationPoint = true;
        }
        else if (obstacleSpawnRoll == 1)
        {
            if (treePoint != null)
            {
                Instantiate(treePoint, obstacleParent.position, treePoint.transform.rotation, obstacleParent);
            }

            spawnedObstaclePoint = true;
        }
    }

    public void SetTreePoint(GameObject treePrefab)
    {
        treePoint = treePrefab;
    }

    public bool CheckSpawnNavPoint()
    {
        return spawnedNavigationPoint;
    }

    public bool CheckSpawnObsPoint()
    {
        return spawnedObstaclePoint;
    }
}
