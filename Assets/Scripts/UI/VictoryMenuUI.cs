using UnityEngine;

public class VictoryMenuUI : MonoBehaviour
{
    public void ContinueButton()
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
        GameManager.Instance.OnGameRestart?.Invoke();
    }
}
