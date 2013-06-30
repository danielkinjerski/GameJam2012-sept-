using UnityEngine;
using System.Collections;

public class MainScreen : BaseMenu
{
    public event ButtonClickHandler OnBlackClick;
    public event ButtonClickHandler OnGrayClick;
    public event ButtonClickHandler OnWhiteClick;

    void Login()
    {
        Debug.Log("Login");
        if (OnBlackClick != null)
        {
            OnBlackClick();
        }
    }

    void Register()
    {
        Debug.Log("Register");
        if (OnGrayClick != null)
        {
            OnGrayClick();
        }
    }

    void Editor()
    {
        Debug.Log("Editor");
        if (OnWhiteClick != null)
        {
            OnWhiteClick();
        }
    }
}
