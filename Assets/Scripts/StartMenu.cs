using Player;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    private void Awake()
    {
        Game.CurrentState = Game.State.StartMenu;
        m_Player.BlockInputs = true;
    }
    
    //////////////////////////////////////////////////////////////////////////
    
    public void Play()
    {
        LeanTween.value(this.gameObject, (value) => { m_Canvas.alpha = value; }, 1, 0, m_FadeOutTime).setOnComplete(() => { m_Player.BlockInputs = false; });
        Game.CurrentState = Game.State.Game;
    }
    
    //////////////////////////////////////////////////////////////////////////

    public void Quit()
    {
        Application.Quit();
    }
    
    //////////////////////////////////////////////////////////////////////////

    [SerializeField] private float m_FadeOutTime;
    [SerializeField] private PlayerController m_Player;
    [SerializeField] private CanvasGroup m_Canvas;
}


public static class Game
{
    public enum State
    {
        StartMenu,
        Pause,
        Game,
        Victory
    }

    public static State CurrentState;
}