using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private void Awake()
    {
        HideMenu();
    }

    //////////////////////////////////////////////////////////////////////////

    private void Update()
    {
        if (!(Game.CurrentState == Game.State.Game || Game.CurrentState == Game.State.Pause)) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Game.CurrentState == Game.State.Pause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////

    public void PauseGame()
    {
        ShowMenu();
        Time.timeScale = 0;
        Game.CurrentState = Game.State.Pause;
    }

    //////////////////////////////////////////////////////////////////////////

    public void ResumeGame()
    {
        HideMenu();
        Time.timeScale = 1;
        Game.CurrentState = Game.State.Game;
    }

    //////////////////////////////////////////////////////////////////////////

    private void ShowMenu()
    {
        m_Canvas.interactable = true;
        m_Canvas.blocksRaycasts = true;
        m_Canvas.alpha = 1;
    }

    //////////////////////////////////////////////////////////////////////////

    private void HideMenu()
    {
        m_Canvas.interactable = false;
        m_Canvas.blocksRaycasts = false;
        m_Canvas.alpha = 0;
    }

    //////////////////////////////////////////////////////////////////////////

    public void QuitToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //////////////////////////////////////////////////////////////////////////

    [SerializeField] private CanvasGroup m_Canvas;
}