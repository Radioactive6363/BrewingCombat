using UnityEngine;

public class BlockClass : MonoBehaviour
{
    [Header("Block Object Spawning Properties")]
    public Transform objectParent;

    [Header("Navigation Points")]
    public GameObject combatPoint;
    public GameObject frontierCombatPoint;
    public GameObject exchangePoint;

    [Header("Obstacle Points")]
    public GameObject treePoint;

    private bool spawnedNavigationPoint = false;

    public void Init(int navigationPointSpawnRoll, int obstacleSpawnRoll)
    {
        if (navigationPointSpawnRoll == 1)
        {
            Instantiate(combatPoint, objectParent.position, Quaternion.identity, objectParent);
            spawnedNavigationPoint = true;
        }
        else if (obstacleSpawnRoll == 1)
        {
            Instantiate(treePoint, objectParent.position, Quaternion.identity, objectParent);
        }
    }

    public bool CheckSpawnNavPoint()
    {
        return spawnedNavigationPoint;
    }
}
