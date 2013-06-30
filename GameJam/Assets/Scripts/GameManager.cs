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
    White = 2,
    Grey = 3
}
#endregion
public class GameManager : MonoBehaviour
{

    #region Variables

    public GameObject OpeningWindow, GameOverWindow, SelectionWindow, TutorialWindow,
                    BlackWorld, WhiteWorld,
                    BlackCam, WhiteCam, MainCam,
                    Character,
                    CharBRender, CharWRender;
    private GameObject Facebook, fbbutton, fbsuccess, fbpost;
    UILabel fb;
    public UILabel deathLbl, timeLbl;
    public Material BlackMat, WhiteMat;
    public static GameState gameState = GameState.OpeningWindow;
    public static CurrentPlayMode currentPlayMode = CurrentPlayMode.Black;
    private bool toggle;
    public bool cheats;
    private int deaths;
    private float time, maxTime;

    public MaterialManager matMan;

    #endregion

    #region Mono Inherit Functions

    void Awake () {
        //BlackMat.color = new Color(BlackMat.color.r, BlackMat.color.g, BlackMat.color.b, 1f);
        //WhiteMat.color = new Color(WhiteMat.color.r, WhiteMat.color.g, WhiteMat.color.b, 1f);
        gameState = GameState.OpeningWindow;
        OpeningWindow.SetActive(true);
        GameOverWindow.SetActive(false);
        SelectionWindow.SetActive(false);
        TutorialWindow.SetActive(false);
        

        BlackCam.camera.rect = new Rect(0.5f, 0, 0.5f, 1);
        WhiteCam.camera.rect = new Rect(0, 0, 0.5f, 1);

        MainCam.camera.rect = new Rect(0, 0, 1, 1);
        MainCam.SetActive(true);
        BlackCam.SetActive(false);
        WhiteCam.SetActive(false);

        //fb = fbsuccess.GetComponent<UILabel>();

        deathLbl.text = "You died: @ time(s)!";
        timeLbl.text = "Your max time: @";

        //White();
	}

    #endregion

    #region UI Events
    void BackToMain()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    void Quit()
    {
        Application.Quit();
    }
    void Switch()
    {
        if (currentPlayMode != CurrentPlayMode.Grey && Character.transform.parent != null)
        {
            Character.transform.parent = null;
        }

        switch (GameManager.currentPlayMode)
        {
            case CurrentPlayMode.Black:
                ActivateWhiteMode(true);
                ActivateBlackMode(false);
                break;
            case CurrentPlayMode.White:
                ActivateBlackMode(true);
                ActivateWhiteMode(false);
                break;
            case CurrentPlayMode.Grey:
                ToggleGrayMode();
                break;
        }
    }
    void Play()
    {
        OpeningWindow.SetActive(false);
        SelectionWindow.SetActive(true);
    }
    void Gray() 
    {
        WhiteCam.SetActive(true);
        BlackCam.SetActive(true);
        SelectionWindow.SetActive(false);
        gameState = GameState.PlayGame;
        currentPlayMode = CurrentPlayMode.Grey;
        time = Time.timeSinceLevelLoad;
        CharWRender.SetActive(true);
        CharBRender.SetActive(true);

        MainCam.SetActive(false);

        SetVanishingPoint(WhiteCam.camera, new Vector2(0.1955f, 0));
        SetVanishingPoint(BlackCam.camera, new Vector2(-0.1955f, 0));

        TutorialWindow.SetActive(true);
    }
    void Black() 
    {
        ActivateWhiteMode(false);
        ActivateBlackMode(true);
        gameState = GameState.PlayGame;        
        time = Time.timeSinceLevelLoad;
    }
    void White() 
    {
        ActivateBlackMode(false);
        ActivateWhiteMode(true);
        gameState = GameState.PlayGame;
        time = Time.timeSinceLevelLoad;
    }

    void EndGame()
    {
        float timer = Time.timeSinceLevelLoad - time;        
        string minutes = Mathf.Floor(maxTime / 60).ToString("00");
        string seconds = (maxTime % 60).ToString("00");
        gameState = GameState.GameOver;
        OpeningWindow.SetActive(false);
        GameOverWindow.SetActive(true);
        deathLbl.text = "You WIN! CONGRATULATIONS!";
        timeLbl.text = "Time taken to complete this win:" + minutes + " minutes " + seconds + " seconds";
        TutorialWindow.SetActive(false);
    }

    void Pause()
    {
        gameState = GameState.Pause;
    }

    void GameOver()
    {
        deaths++;
        float timer = Time.timeSinceLevelLoad - time;
        if (timer > maxTime)
            maxTime = timer;
        string minutes = Mathf.Floor(maxTime / 60).ToString("00");
        string seconds = (maxTime % 60).ToString("00");
        gameState = GameState.GameOver;
        OpeningWindow.SetActive(false);
        GameOverWindow.SetActive(true);
        deathLbl.text = "You died: @ time(s)!";
        timeLbl.text = "Your max time: @";
        deathLbl.text = deathLbl.text.Replace("@", deaths.ToString());
        timeLbl.text = timeLbl.text.Replace("@", minutes + " minutes " + seconds + " seconds");
        TutorialWindow.SetActive(false);
    }
    void Replay()
    {
        gameState = GameState.PlayGame;
        time = Time.timeSinceLevelLoad;
        Character.SetActive(true);
        GameOverWindow.SetActive(false);
        TutorialWindow.SetActive(true);
        if(currentPlayMode == CurrentPlayMode.Black)
            CharWRender.active = false;
        if (currentPlayMode == CurrentPlayMode.White)
            CharBRender.active = false;
    }

    void OnCheats(bool isChecked)
    {
        print("Enabled cheats"+isChecked);
        cheats = isChecked;
    }
    #endregion

    #region Utilities

    void ActivateBlackMode(bool active)
    {
        BlackWorld.SetActive(active);
        if (active)
        {
            matMan.BeginBlackMaterialChange(1);
            matMan.BeginWhiteMaterialChange(0);
            MainCam.camera.backgroundColor = new Color(.85f, .85f, .85f);
            CharBRender.SetActive(true);
            CharWRender.SetActive(false);
            GameManager.currentPlayMode = CurrentPlayMode.Black;
        }
        
    }
    void ActivateWhiteMode(bool active)
    {
        WhiteWorld.SetActive(active);
        if (active)
        {
            matMan.BeginBlackMaterialChange(0);
            matMan.BeginWhiteMaterialChange(1);
            MainCam.camera.backgroundColor = new Color(.29f, .29f, .29f);
            CharBRender.SetActive(false);
            CharWRender.SetActive(true);
            GameManager.currentPlayMode = CurrentPlayMode.White;
        }
    }

    void ToggleGrayMode()
    {
        // if we have this on black, then turn off.
        // Hopefully this switched
        if ((BlackCam.camera.cullingMask & (1 << LayerMask.NameToLayer("CharBlack"))) != 0)
        {
            BlackCam.camera.cullingMask &= ~(1 << LayerMask.NameToLayer("CharBlack"));
            BlackCam.camera.cullingMask &= ~(1 << LayerMask.NameToLayer("Black"));
            BlackCam.camera.cullingMask |= (1 << LayerMask.NameToLayer("CharWhite"));
            BlackCam.camera.cullingMask |= (1 << LayerMask.NameToLayer("White"));

            WhiteCam.camera.cullingMask |= (1 << LayerMask.NameToLayer("CharBlack"));
            WhiteCam.camera.cullingMask |= (1 << LayerMask.NameToLayer("Black"));
            WhiteCam.camera.cullingMask &= ~(1 << LayerMask.NameToLayer("CharWhite"));
            WhiteCam.camera.cullingMask &= ~(1 << LayerMask.NameToLayer("White"));
        }
        else
        {
            WhiteCam.camera.cullingMask &= ~(1 << LayerMask.NameToLayer("CharBlack"));
            WhiteCam.camera.cullingMask &= ~(1 << LayerMask.NameToLayer("Black"));
            WhiteCam.camera.cullingMask |= (1 << LayerMask.NameToLayer("CharWhite"));
            WhiteCam.camera.cullingMask |= (1 << LayerMask.NameToLayer("White"));

            BlackCam.camera.cullingMask |= (1 << LayerMask.NameToLayer("CharBlack"));
            BlackCam.camera.cullingMask |= (1 << LayerMask.NameToLayer("Black"));
            BlackCam.camera.cullingMask &= ~(1 << LayerMask.NameToLayer("CharWhite"));
            BlackCam.camera.cullingMask &= ~(1 << LayerMask.NameToLayer("White"));
        }


    }

    void SetVanishingPoint (Camera cam, Vector2 perspectiveOffset) 
    {
        Matrix4x4 m = cam.projectionMatrix;
        float w = 2 * cam.nearClipPlane / m.m00;
        float h = 2 * cam.nearClipPlane / m.m11;
        
	    float left = -w/2 - perspectiveOffset.x;
	    float right = left+w;
	    float bottom = -h/2 - perspectiveOffset.y;
	    float top = bottom+h;
 
	    cam.projectionMatrix = PerspectiveOffCenter(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane);
    }
 
    Matrix4x4 PerspectiveOffCenter (float left, float right, float bottom, float top, float near, float far)
    {
	    float x =  (2.0f * near) / (right - left);
	    float y =  (2.0f * near) / (top - bottom);
	    float a =  (right + left) / (right - left);
	    float b =  (top + bottom) / (top - bottom);
	    float c = -(far + near)	/ (far - near);
	    float d = -(2.0f * far * near) / (far - near);
	    float e = -1.0f;
 
	    Matrix4x4 m = new Matrix4x4();
	    m[0,0] =    x;  m[0,1] = 0.0f;  m[0,2] = a;   m[0,3] = 0.0f;
	    m[1,0] = 0.0f;  m[1,1] =    y;  m[1,2] = b;   m[1,3] = 0.0f;
	    m[2,0] = 0.0f;  m[2,1] = 0.0f;  m[2,2] = c;   m[2,3] =   d;
	    m[3,0] = 0.0f;  m[3,1] = 0.0f;  m[3,2] = e;   m[3,3] = 0.0f;
        return m;
    }
    
    #endregion


}
