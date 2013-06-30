using UnityEngine;
using System.Collections;

public abstract class BaseMenu : MonoBehaviour
{
    public delegate void ButtonClickHandler();
    public event ButtonClickHandler OnBackClick;
    protected MenuManager mm;
    public bool isStartMenu = false;

    // Use this for initialization
    protected virtual void Awake () 
    {
        MenuManager.Register(this, isStartMenu);	
    }

    protected virtual void GoBack()
    {
        Debug.Log("GoBack");
        if (OnBackClick != null)
        {
            OnBackClick();
        }
    }
}
