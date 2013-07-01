using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

    #region Variables

    /// <summary>
    /// Sets of containers for menus and HUD and shit
    /// </summary>
    private List<Component> menus = new List<Component>();

    /// <summary>
    /// Current Menu Id
    /// </summary>
    private int selectedMenuId;

    /// <summary>
    /// Current Menu Nam
    /// </summary>
    private string selectedMenuName;

    /// <summary>
    /// Reference to menu manager
    /// </summary>
    static private MenuManager _mm = null;   

    #endregion

    #region Properties

    /// <summary>
    /// Will change menu by id or return current menu id
    /// </summary>
    static public int CurrentMenuId
    {
        set 
        {
            if (value == mm.selectedMenuId)
                return;
            mm.ChangeMenu(value);
        }
        get { return mm.selectedMenuId; }
    }

    /// <summary>
    /// Will change menu by name or return current menu name
    /// </summary>
    static public string CurrentMenuName
    {
        set
        {
            if (value == mm.selectedMenuName)
                return;
            mm.ChangeMenu(value);
        }
        get { return mm.selectedMenuName; }
    }

    /// <summary>
    /// Will return current menu component - cast to target menu
    /// </summary>
    static public Component CurrentMenu
    {
        get { return mm.menus[mm.selectedMenuId]; }
    }

    static public bool isNull
    {
        get
        {
            return (_mm == null);
        }
    }

    static private MenuManager mm
    {
        get
        {
            if (_mm == null)
            {
                _mm = GameObject.FindObjectOfType(typeof(MenuManager)) as MenuManager;
            }

            return _mm;
        }
    }

    #endregion   

    #region Access Functions

    /// <summary>
    /// Will set current menu based on name
    /// </summary>
    /// <param name="menuName"></param>
    private void ChangeMenu(string menuName)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].gameObject.name != menuName)
            {
                menus[i].gameObject.SetActive(false);
            }
            else
            {
                menus[i].gameObject.SetActive(true);
                this.selectedMenuName = menuName;
                this.selectedMenuId = i;
            }

        }
    }

    /// <summary>
    /// Will set current menu based on name
    /// </summary>
    /// <param name="menuName"></param>
    private void ChangeMenu<T>() where T : Component
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i] is T)
            {
                menus[i].gameObject.SetActive(true);
                this.selectedMenuName = menus[i].name;
                this.selectedMenuId = i;   
            }
            else
            {
                menus[i].gameObject.SetActive(false);
            }
            
        }
    }

    /// <summary>
    /// Will set current menu based on id
    /// </summary>
    /// <param name="menuName"></param>
    private void ChangeMenu(int menuId)
    {
        if (menuId >= menus.Count)
        {
            menuId = 0;
        }

        for (int i = 0; i < menus.Count; i++ )
        {
            if (i != menuId)
            {
                menus[i].gameObject.SetActive(false);                
            }
            else
            {
                menus[i].gameObject.SetActive(true);
                this.selectedMenuName = menus[i].gameObject.name;
                this.selectedMenuId = i;
            }
        }
    }

    /// <summary>
    /// Get Menu by type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private T GetMenu<T>() where T : Component
    {
        foreach (Component menu in this.menus)
        {
            if (menu is T)
            {
                return menu as T;
            }
        }
        Debug.LogWarning("Menu manager has returned a null Menu");
        return null;
    }

    /// <summary>
    /// Find Menu Manager and get a menu by type. Don't use this in an update or anything
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    static public T FindMenu<T>() where T : Component
    {
        return mm.GetMenu<T>();
    }

    /// <summary>
    /// Change menu by id
    /// </summary>
    /// <param name="menuId"></param>
    static public void GoToMenu(int menuId)
    {
        mm.ChangeMenu(menuId);
    }


    /// <summary>
    /// Change menu by type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static public void GoToMenu<T>() where T : Component
    {
        mm.ChangeMenu<T>();
    }

    /// <summary>
    /// Change menu by name
    /// </summary>
    /// <param name="menuName"></param>
    static public void GoToMenu(string menuName)
    {
        mm.ChangeMenu(menuName);
    }

    /// <summary>
    /// Register menu
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="initialMenu"></param>
    static public void Register(Component menu, bool initialMenu)
    {
        mm.menus.Add(menu);
        if (initialMenu)
        {
            mm.selectedMenuId = mm.menus.Count - 1;
            mm.selectedMenuName = menu.gameObject.name;
        }
    }

    #endregion

    #region Monobehaviour Functions

    // Use this for initialization
    void Start () {
        bool foundSelected = false;
        foreach (Component menu in this.menus)
        {
            if (menu.gameObject.name != this.selectedMenuName)
            {
                menu.gameObject.SetActive(false);
                foundSelected = true;
            }
        }
        if (!foundSelected)
        {
            throw new Exception( "Missing Default Starting Menu..." ); 
        }
    }
    // Update is called once per frame
    void Update () {

        if (Input.GetKeyUp(KeyCode.X))
        {
            CurrentMenuId++;
        }

    }

    #endregion
}
