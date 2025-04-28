using UnityEngine;

public class LoseMenuUI : MonoBehaviour
{
    public void RetryButton()
    {
        if (GameManager.Instance != null)
        {
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
}
