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
    private GameObject _treePoint;

    private bool _spawnedNavigationPoint = false;
    private bool _spawnedObstaclePoint = false;

    public void Init(int navigationPointSpawnRoll, int obstacleSpawnRoll)
    {
        if (navigationPointSpawnRoll == 1)
        {
            if (combatPoint != null)
            {
                Instantiate(combatPoint, objectParent.position, _treePoint.transform.rotation, objectParent);
            }

            _spawnedNavigationPoint = true;
        }
        else if (obstacleSpawnRoll == 1)
        {
            if (_treePoint != null)
            {
                Instantiate(_treePoint, obstacleParent.position, _treePoint.transform.rotation, obstacleParent);
            }

            _spawnedObstaclePoint = true;
        }
    }

    public void SetTreePoint(GameObject treePrefab)
    {
        _treePoint = treePrefab;
    }

    public bool CheckSpawnNavPoint()
    {
        return _spawnedNavigationPoint;
    }

    public bool CheckSpawnObsPoint()
    {
        return _spawnedObstaclePoint;
    }
}
