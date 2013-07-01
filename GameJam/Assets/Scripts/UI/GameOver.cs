using UnityEngine;
using System.Collections;
using System;

public class GameOver : BaseMenu
{
    public event ButtonClickHandler OnReplayClick;
    public event ButtonClickHandler OnQuitClick;

    public UILabel deathLbl, timeLbl;

    void Start()
    {
        OnReplayClick += GameOver_OnReplayClick;
        OnQuitClick += GameOver_OnQuitClick;
    }

    void GameOver_OnReplayClick()
    {
        MenuManager.GoToMenu<Hud>();
    }

    void GameOver_OnQuitClick()
    {
        MenuManager.GoToMenu<GameOver>();
    }

    void Replay()
    {
        Debug.Log("Replay");
        if (OnReplayClick != null)
        {
            OnReplayClick();
        }
    }

    void Quit()
    {
        Debug.Log("Quit");
        if (OnQuitClick != null)
        {
            OnQuitClick();
        }
    }
}
