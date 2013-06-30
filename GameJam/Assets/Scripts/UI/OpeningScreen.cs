using UnityEngine;
using System.Collections;

public class OpeningScreen : BaseMenu
{
    public event ButtonClickHandler OnOuyaButton;
    public event ButtonClickHandler OnGrayClick;
    public event ButtonClickHandler OnWhiteClick;

    void Start()
    {
        OnOuyaButton += BaseMenu_OnOuyaButton;
    }

    void BaseMenu_OnOuyaButton()
    {
        MenuManager.GoToMenu<MainScreen>();
    }

    void Update()
    {
        if (InputHandler.jbO || Input.GetKey(KeyCode.Joystick1Button1))
        {
            Debug.Log("Begin Game");
            if (OnOuyaButton != null)
            {
                OnOuyaButton();
            }
        }

    }
}
