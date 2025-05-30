using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool isGamePaused = false;

    public GameObject pauseMenuUI;

    public void TogglePause()
    {
        if (isGamePaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }
}
