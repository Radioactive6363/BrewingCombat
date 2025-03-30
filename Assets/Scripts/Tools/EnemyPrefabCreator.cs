using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyPrefabCreator : EditorWindow
{
    private string enemyName = "NewEnemy"; // DefaultName

    [MenuItem("Tools/Create Enemy Prefab")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<EnemyPrefabCreator>("Create Enemy Prefab");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create new Enemy", EditorStyles.boldLabel);

        enemyName = EditorGUILayout.TextField("Name of Enemy:", enemyName);

        if (GUILayout.Button("Create Prefab"))
        {
            CreatePrefab(enemyName);
            EditorWindow.GetWindow<EnemyPrefabCreator>().Close(); // Cerrar la ventana después de crear el prefab
        }
    }
    static void CreatePrefab(string name)
    {
        // Creates a NewGameObject
        GameObject enemy = new GameObject(name);

        // Add EnemyClass Script
        enemy.AddComponent<EnemyClass>();

        // Creates Prefab Folder (if not setted)
        string folderPath = "Assets/Prefabs";
        if (!Directory.Exists(folderPath)) 
        { 
                Directory.CreateDirectory(folderPath);
        }

        // Saves prefab on the folder
        string prefabPath = $"{folderPath}/NewEnemy.prefab";
        PrefabUtility.SaveAsPrefabAsset(enemy, prefabPath);

        // Destroy object (in case is added to the scene)
        Object.DestroyImmediate(enemy);

        // Confirmation.
        Debug.Log($"Prefab created in: {prefabPath}");
    }
}

