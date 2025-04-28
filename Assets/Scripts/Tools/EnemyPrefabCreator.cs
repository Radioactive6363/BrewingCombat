using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
// using UnityEngine.Animations; // Not strictly needed for RuntimeAnimatorController

// Ensure AbilityStruct is accessible (either defined above or in a separate file)
// If defined in EnemyClass.cs, you MUST move it out and mark it [System.Serializable]

public class EnemyPrefabCreator : EditorWindow
{
    private string _enemyName = "NewEnemy"; // DefaultName
    private int _health = 100; // DefaultHealth
    private float _speed = 5.0f; // DefaultSpeed
    private float _damage = 5.0f; // DefaultDamage

    // Field to select a base model GameObject/Prefab asset
    private GameObject _baseModelPrefab;

    private EnemyData _enemyData; // Field to Store the created Data Asset

    // --- Fields for Animator ---
    private RuntimeAnimatorController _animatorController;
    // ------------------------------

    // --- New field for Abilities ---
    private List<AbilityStruct> _abilitiesToAssign = new List<AbilityStruct>();
    // -------------------------------

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

        GUILayout.Space(10); // Add some spacing
        GUILayout.Label("Appearance Settings", EditorStyles.boldLabel);

        // Field to select the base model prefab/GameObject asset
        _baseModelPrefab = (GameObject)EditorGUILayout.ObjectField("Base Model Prefab:", _baseModelPrefab, typeof(GameObject), false);

        // --- Section for Animator ---
        GUILayout.Space(10); // Add some spacing
        GUILayout.Label("Animation Settings", EditorStyles.boldLabel);
        _animatorController = (RuntimeAnimatorController)EditorGUILayout.ObjectField("Animator Controller:", _animatorController, typeof(RuntimeAnimatorController), false);
        // ----------------------------------

        // --- New section for Abilities ---
        GUILayout.Space(10);
        GUILayout.Label("Abilities Settings", EditorStyles.boldLabel);

        // Scroll view for abilities if the list gets long
        // Vector2 scrollPosition = Vector2.zero; // If you need scrolling
        // scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);


        // Loop through the abilities list and draw fields for each
        for (int i = 0; i < _abilitiesToAssign.Count; i++)
        {
            // Use BeginVertical and EndVertical to group fields for one ability
            GUILayout.BeginVertical(GUI.skin.box); // Use a box style for visual grouping
            GUILayout.Label($"Ability {i + 1}", EditorStyles.miniBoldLabel);

            // --- FIX: Get a copy of the struct first ---
            AbilityStruct currentAbility = _abilitiesToAssign[i];

            // Ensure the name field is not null (optional, but good practice)
            if (currentAbility.name == null) currentAbility.name = "";

            // --- Modify the copy's fields using the EditorGUILayout results ---
            currentAbility.name = EditorGUILayout.TextField("Name:", currentAbility.name);
            currentAbility.animationName = EditorGUILayout.TextField("Animation Name:", currentAbility.animationName);
            currentAbility.damage = EditorGUILayout.IntField("Damage:", currentAbility.damage);
            currentAbility.probability = EditorGUILayout.IntField("Probability:", currentAbility.probability);
            currentAbility.cooldown = EditorGUILayout.FloatField("Cooldown:", currentAbility.cooldown);
            currentAbility.chargeTime = EditorGUILayout.FloatField("Charge Time:", currentAbility.chargeTime);

            // --- FIX: Assign the modified copy back to the list ---
            _abilitiesToAssign[i] = currentAbility;
            // -----------------------------------------------------


            // Button to remove THIS specific ability
            if (GUILayout.Button($"Remove Ability {i + 1}"))
            {
                _abilitiesToAssign.RemoveAt(i);
                // Exit GUI to prevent errors because the list count changed during iteration
                GUIUtility.ExitGUI();
            }

            GUILayout.EndVertical(); // End the vertical group for this ability
            GUILayout.Space(5); // Add a little space between abilities
        }

        // EditorGUILayout.EndScrollView(); // If using scrolling

        // Buttons to add/remove abilities from the list
        if (GUILayout.Button("Add New Ability"))
        {
            // Add a new default AbilityStruct to the list
            _abilitiesToAssign.Add(new AbilityStruct { name = "New Ability", animationName = "Idle", damage = 10, probability = 1, cooldown = 1f, chargeTime = 0f }); // Provide default values
        }
        if (_abilitiesToAssign.Count > 0) // Only show remove last button if there are abilities
        {
            if (GUILayout.Button("Remove Last Ability"))
            {
                _abilitiesToAssign.RemoveAt(_abilitiesToAssign.Count - 1);
            }
        }
        // -----------------------------------

        GUILayout.Space(20); // Add some spacing before the button

        if (GUILayout.Button("Create Prefab"))
        {
            // Basic validation
            if (string.IsNullOrEmpty(_enemyName))
            {
                EditorUtility.DisplayDialog("Error", "Enemy Name cannot be empty.", "OK");
                return;
            }
            if (_baseModelPrefab == null)
            {
                EditorUtility.DisplayDialog("Error", "Base Model Prefab cannot be empty. Please assign a GameObject or FBX model.", "OK");
                return;
            }
            // Consider adding validation for abilities if at least one is required

            // Create EnemyData first (it's an asset)
            CreateEnemyData(_enemyName, _health, _speed, _damage);

            // Then create the Prefab instance based on the data, selected model, and abilities
            CreatePrefabAsset(_enemyName, _enemyData, _baseModelPrefab, _animatorController, _abilitiesToAssign); // Pass the abilities list

            // Close the window only if creation was attempted (or successful)
            this.Close();
        }
    }

    // CreateEnemyData remains the same
    private void CreateEnemyData(string ename, int hp, float spd, float dm)
    {
        string path = "Assets/Scripts/Databases/Enemies/EnemyDatabases";

        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets/Scripts/Databases/Enemies", "EnemyDatabases");
        }

        _enemyData = ScriptableObject.CreateInstance<EnemyData>();
        _enemyData.Name = ename;
        _enemyData.Health = hp;
        _enemyData.Speed = spd;
        _enemyData.Damage = dm;

        string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{path}/{ename}_Data.asset");
        AssetDatabase.CreateAsset(_enemyData, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"EnemyData created in: {assetPath}");
    }


    // --- Modified CreatePrefabAsset method to accept Abilities List ---
    private void CreatePrefabAsset(string ename, EnemyData data, GameObject baseModel, RuntimeAnimatorController controller, List<AbilityStruct> abilities)
    {
        string folderPath = "Assets/Prefabs/Enemies";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs", "Enemies");
        }

        // Instantiate the base model prefab to get an editable instance
        GameObject newEnemyInstance = PrefabUtility.InstantiatePrefab(baseModel) as GameObject;

        if (newEnemyInstance == null)
        {
            Debug.LogError("Failed to instantiate base model prefab.");
            return;
        }

        newEnemyInstance.name = ename;

        // Get or Add the EnemyClass Script component
        // Use GetOrAddComponent just in case the baseModelPrefab already has one
        EnemyClass enemyComponent = newEnemyInstance.GetComponent<EnemyClass>();
        if (enemyComponent == null)
        {
            enemyComponent = newEnemyInstance.AddComponent<EnemyClass>();
        }


        // --- Add Animator component and assign controller if provided ---
        // Use GetOrAddComponent - the base model might already have an Animator
        Animator animator = newEnemyInstance.GetComponent<Animator>();
        if (controller != null)
        {
            if (animator == null) animator = newEnemyInstance.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
        }
        else // If no controller is assigned in the tool, remove Animator if it exists
        {
            if (animator != null) DestroyImmediate(animator);
        }
        // --------------------------------------------------------------

        // --- Assign the abilities list to the EnemyClass component ---
        // Check if the component exists and if the list is not null
        if (enemyComponent != null && abilities != null)
        {
            // Create a NEW list to assign, preventing modification issues with the list in the Editor window
            enemyComponent.Abilities = new List<AbilityStruct>(abilities);
            Debug.Log($"Assigned {enemyComponent.Abilities.Count} abilities to {newEnemyInstance.name}.");
        }
        // ------------------------------------------------------------


        // Assign data BEFORE the creation of a prefab
        if (enemyComponent != null)
        {
            enemyComponent.enemyData = data;
        }
        else
        {
            Debug.LogError($"EnemyClass component not found on {newEnemyInstance.name}. Cannot assign EnemyData.");
        }


        // Save the modified INSTANCE as a NEW prefab asset
        string prefabPath = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{ename}.prefab");
        bool success;
        // PrefabUtility.SaveAsPrefabAsset returns the saved prefab asset, which is useful
        GameObject savedPrefab = PrefabUtility.SaveAsPrefabAsset(newEnemyInstance, prefabPath, out success);

        if (success)
        {
            Debug.Log($"Prefab created successfully at: {prefabPath}");
            // Optionally, ping the newly created asset in the Project window
            if (savedPrefab != null) EditorGUIUtility.PingObject(savedPrefab);
        }
        else
        {
            Debug.LogError($"Failed to create prefab at: {prefabPath}");
        }

        // Destroy the temporary instantiated object
        DestroyImmediate(newEnemyInstance);

        // Update Asset database of Unity
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    // ----------------------------------------------------------------
}
