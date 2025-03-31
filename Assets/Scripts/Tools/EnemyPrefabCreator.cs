using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class EnemyPrefabCreator : EditorWindow
{
    private string _enemyName = "NewEnemy"; // DefaultName
    private int _health = 100; // DefaultHealth
    private float _speed = 5.0f; // DefaultSpeed
    private float _damage = 5.0f; // DefaultDamage
    private Mesh _enemyMesh;
    private List<Material> _materials = new();
    private EnemyData _enemyData; // Field to Store Data.

    [MenuItem("Tools/Create Enemy Prefab")]
    public static void ShowWindow()
    {
        GetWindow<EnemyPrefabCreator>("Create Enemy Prefab");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create new Enemy", EditorStyles.boldLabel);

        _enemyName = EditorGUILayout.TextField("Name of Enemy:", _enemyName);
        _health = EditorGUILayout.IntField("Health:", _health);
        _speed = EditorGUILayout.FloatField("Speed:", _speed);
        _damage = EditorGUILayout.FloatField("Damage:", _damage);
        _enemyMesh = (Mesh)EditorGUILayout.ObjectField("Enemy Mesh:", _enemyMesh, typeof(Mesh), false);
        
        GUILayout.Label("Materials:", EditorStyles.boldLabel);
        for (int i = 0; i < _materials.Count; i++)
        {
            _materials[i] = (Material)EditorGUILayout.ObjectField($"Material {i + 1}:", _materials[i], typeof(Material), false);
        }

        // Botón para agregar un nuevo material
        if (GUILayout.Button("Add Material"))
        {
            _materials.Add(null);
        }

        // Botón para eliminar el último material
        if (_materials.Count > 0 && GUILayout.Button("Remove Last Material"))
        {
            _materials.RemoveAt(_materials.Count - 1);
        }
        
        if (GUILayout.Button("Create Prefab"))
        {
            CreateEnemyData(_enemyName, _health, _speed, _damage, _enemyMesh);
            CreatePrefab(_enemyName,_enemyData, _enemyMesh,_materials);
            this.Close(); // Closes window on finished
        }
    }

    private void CreateEnemyData(string ename, int hp, float spd, float dm, Mesh em)
    {
        string path = "Assets/EnemyDatabases";
        
        // Create folder if it doesnt exist
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets", "EnemyDatabases");
        }

        // Create Instancy of EnemyData
        _enemyData = ScriptableObject.CreateInstance<EnemyData>();
        _enemyData.Name = ename;
        _enemyData.Health = hp;
        _enemyData.Speed = spd;
        _enemyData.Damage = dm;
        _enemyData.enemyMesh = em;

        // Save ScriptableObject
        string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{path}/{ename}_Data.asset");
        AssetDatabase.CreateAsset(_enemyData, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"EnemyData created in: {assetPath}");
    }
    
    private void CreatePrefab(string ename, EnemyData data, Mesh em, List<Material> mats)
    {
        // Create "Prefab" Folder if it doesnt exist
        string folderPath = "Assets/Prefabs";
        
        // Creates folder if it doesnt exist
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        
        // Create a new GameObject for the enemy
        GameObject newEnemy = new GameObject(ename);
        
        // Add EnemyClass Script
        EnemyClass enemyComponent = newEnemy.AddComponent<EnemyClass>();
        
        // Add MeshFilter and assign Mesh
        MeshFilter meshFilter = newEnemy.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = newEnemy.AddComponent<MeshRenderer>();
        if (em != null)
        {
            meshFilter.mesh = em;
        }
        if (mats.Count > 0)
        {
            meshRenderer.materials = mats.ToArray();
        }

        
        // Assign data BEFORE the creation of a prefab
        enemyComponent.enemyData = data;
        
        // Save as prefab   
        string prefabPath = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{ename}.prefab");
        PrefabUtility.SaveAsPrefabAsset(newEnemy, prefabPath);
        DestroyImmediate(newEnemy);

        // Update Asset database of Unity
        AssetDatabase.Refresh();

        Debug.Log($"Prefab created in: {prefabPath} with {data.Name}");
    }
}
