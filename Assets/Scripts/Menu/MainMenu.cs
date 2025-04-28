using UnityEngine;

public class MainMenu : MonoBehaviour
{
     public void PlayGame()
    {
        
        GameManager.Instance.LoadScene("MapSelection");
    }

    // Método para el botón Exit
    public void ExitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
