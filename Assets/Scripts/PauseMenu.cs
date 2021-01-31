using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private void Update()
    {
        if (!(Game.CurrentState == Game.State.Game || Game.CurrentState == Game.State.Pause)) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Game.CurrentState == Game.State.Pause)
            {
                Resume();
            }
            else
            {
                ShowMenu();
            }
        }
    }

    private void ShowMenu()
    {
        m_Canvas.enabled = true;
        Time.timeScale = 0;
        Game.CurrentState = Game.State.Pause;
    }

    public void Resume()
    {
        m_Canvas.enabled = false;
        Time.timeScale = 1;
        Game.CurrentState = Game.State.Game;
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [SerializeField] private CanvasGroup m_Canvas;
}