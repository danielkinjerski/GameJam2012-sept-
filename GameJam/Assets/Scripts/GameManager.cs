using UnityEngine;
using System.Collections;

#region Enums
public enum GameState
{
    OpeningWindow = 1,
    PlayGame = 2,
    GameOver = 3,
    Pause = 4
}
public  enum CurrentPlayMode
{
    Black = 1,
    White = 2
}
#endregion
public class GameManager : MonoBehaviour
{

    #region Variables

    public GameObject OpeningWindow, GameOverWindow, SelectionWindow,
                    BlackWorld, WhiteWorld,
                    BlackCam, WhiteCam, MainCam,
                    Facebook, fbbutton, fbsuccess,
                    Character;
    public Material BlackMat, WhiteMat, CharMat;
    public Texture2D blackTexture, whiteTexture;
    public static GameState gameState = GameState.OpeningWindow;
    public static CurrentPlayMode currentPlayMode = CurrentPlayMode.Black;
    private bool toggle;
    public bool cheats;
    private int deaths;
    private float time;

    #endregion

    #region Mono Inherit Functions

    void Awake () {
        BlackMat.color = new Color(BlackMat.color.r, BlackMat.color.g, BlackMat.color.b, 1f);
        WhiteMat.color = new Color(WhiteMat.color.r, WhiteMat.color.g, WhiteMat.color.b, 1f);
        gameState = GameState.OpeningWindow;
        OpeningWindow.SetActiveRecursively(true);
        GameOverWindow.SetActiveRecursively(false);
        SelectionWindow.SetActiveRecursively(false);
        fbsuccess.active = false;

        //WhiteCam.camera.rect = new Rect(0.5f, 0, 0.5f, 1);
        //BlackCam.camera.rect = new Rect(0, 0, 0.5f, 1);

        MainCam.camera.rect = BlackCam.camera.rect = new Rect(0, 0, 1, 1);
        MainCam.active = false;
        Facebook.active = true;

	
	}
	
	void Update ()
    {

        #region Fade in/out
        if (toggle)
        {
            if (GameManager.currentPlayMode == CurrentPlayMode.Black)
            {
                if (Toggle(ref WhiteMat, true)&&Toggle(ref BlackMat, false))
                {
                    toggle = false;
                    ActivateBlackMode(false);
                    GameManager.currentPlayMode = CurrentPlayMode.White;
                }
            }
            else if (GameManager.currentPlayMode == CurrentPlayMode.White)
            {
                if(Toggle(ref BlackMat, true)&& Toggle(ref WhiteMat, false))
                {
                    toggle = false;
                    ActivateWhiteMode(false);
                    GameManager.currentPlayMode = CurrentPlayMode.Black;
                }
            }
        }
        #endregion
    }

    #endregion

    #region UI Events
    void Play()
    {
        OpeningWindow.SetActiveRecursively( false );
        SelectionWindow.SetActiveRecursively(true);
    }

    void Gray() 
    {
        WhiteCam.active = BlackCam.active = true;
        BlackCam.camera.rect = new Rect(0, 0, 0.5f, 1);
        SelectionWindow.SetActiveRecursively(false);
        gameState = GameState.PlayGame;
        time = Time.timeSinceLevelLoad;
    }
    void Black() 
    {
        WhiteCam.active = BlackCam.active = false;
        MainCam.active = true;
        ActivateWhiteMode(false);
        ActivateBlackMode(true);
        SelectionWindow.SetActiveRecursively(false);
        gameState = GameState.PlayGame;
        GameManager.currentPlayMode = CurrentPlayMode.Black;
        time = Time.timeSinceLevelLoad;
    }
    void White() 
    {
        WhiteCam.active = BlackCam.active = false;
        MainCam.active = true;
        ActivateBlackMode(false);
        ActivateWhiteMode(true);
        SelectionWindow.SetActiveRecursively(false);
        gameState = GameState.PlayGame;
        GameManager.currentPlayMode = CurrentPlayMode.White;
        time = Time.timeSinceLevelLoad;
    }
    void FaceBook()
    {
        Facebook.SendMessage("GetToken");
    }
    void PostResults()
    {
        float timer = Time.timeSinceLevelLoad - time;
        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");
        Facebook.GetComponent<Facebook>().Publish("I died "+deaths+" times and played for " + minutes + " minutes " + seconds + " seconds!"  );
    }
    void SuccessFacebookLink()
    {
        if (fbbutton.active)
            fbsuccess.active = true;
        fbbutton.SetActiveRecursively(false);
    }
    void Pause()
    {
        gameState = GameState.Pause;
    }
    void GameOver()
    {
        deaths++;
        float timer = Time.timeSinceLevelLoad - time;
        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");
        gameState = GameState.GameOver;
        OpeningWindow.SetActiveRecursively(false);
        GameOverWindow.SetActiveRecursively(true);
        UILabel deathLbl = GameObject.Find("lblDeaths").GetComponent<UILabel>();
        UILabel timeLbl = GameObject.Find("lblTime").GetComponent<UILabel>();
        deathLbl.text = deathLbl.text.Replace("0", deaths.ToString());
        timeLbl.text = timeLbl.text.Replace("0", minutes + " minutes " + seconds + " seconds");
    }
    void Replay()
    {
        Character.SetActiveRecursively(true);
        GameOverWindow.SetActiveRecursively(false);
        gameState = GameState.PlayGame;
        time = Time.timeSinceLevelLoad;
    }
    #endregion

    #region Utilities

    void Switch()
    {
        toggle = true;
        switch (GameManager.currentPlayMode)
        {
            case CurrentPlayMode.Black:
                ActivateWhiteMode(true);
                WhiteMat.color = new Color(WhiteMat.color.r, WhiteMat.color.g, WhiteMat.color.b, 0);
                break;
            case CurrentPlayMode.White:
                ActivateBlackMode(true);
                BlackMat.color = new Color(BlackMat.color.r, BlackMat.color.g, BlackMat.color.b, 0);
                break;
        }
    }

    bool Toggle(ref Material mat, bool pulse)
    {
        if ((mat.color.a <= 0 && !pulse) || (mat.color.a >= 1 && pulse))
            return true;

        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, (pulse) ? (mat.color.a + .1f) : (mat.color.a - .1f));
        print(mat.color.a + pulse.ToString()+mat.name);

        return false;
    }

    void ActivateBlackMode(bool active)
    {
        BlackWorld.SetActiveRecursively(active);
        if (active)
        {
            MainCam.camera.backgroundColor = new Color(.85f, .85f, .85f);
            CharMat.mainTexture = blackTexture;
        }
        
    }
    void ActivateWhiteMode(bool active)
    {
        WhiteWorld.SetActiveRecursively(active);
        if (active)
        {
            MainCam.camera.backgroundColor = new Color(.29f, .29f, .29f);
            CharMat.mainTexture = whiteTexture;
        }
    }

    void OnApplicationQuit()
    {
        BlackMat.color = new Color(BlackMat.color.r, BlackMat.color.g, BlackMat.color.b, 1);
        WhiteMat.color = new Color(WhiteMat.color.r, WhiteMat.color.g, WhiteMat.color.b, 1);
    }

    #endregion


}
