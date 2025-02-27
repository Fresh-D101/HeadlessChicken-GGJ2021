﻿using Player;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    private void Awake()
    {
        m_Canvas.interactable = true;
        m_Canvas.blocksRaycasts = true;
        m_Canvas.alpha = 1;
        Game.CurrentState = Game.State.StartMenu;
        m_Player.BlockInputs = true;
    }

    //////////////////////////////////////////////////////////////////////////

    public void Play()
    {
        m_Canvas.interactable = false;
        m_Canvas.blocksRaycasts = false;
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