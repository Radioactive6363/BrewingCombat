using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [FormerlySerializedAs("OnGameRestart")] public UnityEvent onGameRestart;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Avoids destroying this object when reloading scene
        }
        else
        {
            Destroy(gameObject); // If instance already exists, destroy this one
        }
    }

    // Cambiar a una escena
    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading Scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
        FindFirstObjectByType<InventorySystem>().OnChangedScene();
    }

    // Cambiar a una escena por su índice
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Cargar la siguiente escena 
    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No hay más escenas para cargar.");
        }
    }

    // Cerrar el juego
    public void QuitGame()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }

    public void GameRestartInvoke()
    {
        onGameRestart?.Invoke();
    }
}
