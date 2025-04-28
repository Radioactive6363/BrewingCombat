using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager: MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Crear instancia 
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Cambiar a una escena
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
}
