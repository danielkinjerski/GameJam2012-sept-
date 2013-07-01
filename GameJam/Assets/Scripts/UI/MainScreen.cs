using UnityEngine;
using System.Collections;

public class MainScreen : BaseMenu
{
    public event ButtonClickHandler OnBlackClick;
    public event ButtonClickHandler OnGrayClick;
    public event ButtonClickHandler OnWhiteClick;

    void Start()
    {
        OnBlackClick += PlayGame;
        OnGrayClick += PlayGame;
        OnWhiteClick += PlayGame;

    }

    void PlayGame()
    {
        MenuManager.GoToMenu<Hud>();
    }

    void Black()
    {
        Debug.Log("Black");
        if (OnBlackClick != null)
        {
            OnBlackClick();
        }
    }

    void Gray()
    {
        Debug.Log("Gray");
        if (OnGrayClick != null)
        {
            OnGrayClick();
        }
    }

    void White()
    {
        Debug.Log("White");
        if (OnWhiteClick != null)
        {
            OnWhiteClick();
        }
    }
}
