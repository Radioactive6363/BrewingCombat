using UnityEngine;

public class LoseMenuUI : MonoBehaviour
{
    public void RetryButton()
    {
        if (GameManager.Instance != null)
        {
            GameRestarted();
            GameManager.Instance.LoadScene("MapSelection");
        }
    }

    public void MainMenuButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadScene("MainMenu");
        }
    }

    private void GameRestarted()
    {
        GameManager.Instance.onGameRestart?.Invoke();
    }
}
