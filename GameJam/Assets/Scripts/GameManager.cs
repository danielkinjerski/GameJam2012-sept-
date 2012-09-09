using UnityEngine;
using System.Collections;
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
public class GameManager : MonoBehaviour {

    public GameObject OpeningWindow, GameOverWindow, SelectionWindow,
                    BlackWorld, WhiteWorld, 
                    BlackCam, WhiteCam, MainCam;
    public Material BlackMat, WhiteMat;
    public static GameState gameState = GameState.OpeningWindow;
    public static CurrentPlayMode currentPlayMode = CurrentPlayMode.Black;
    private bool toggle;

	void Awake () {
        BlackMat.color = new Color(BlackMat.color.r, BlackMat.color.g, BlackMat.color.b, 1f);
        WhiteMat.color = new Color(WhiteMat.color.r, WhiteMat.color.g, WhiteMat.color.b, 1f);
        gameState = GameState.OpeningWindow;
        OpeningWindow.SetActiveRecursively(true);
        GameOverWindow.SetActiveRecursively(false);
        SelectionWindow.SetActiveRecursively(false);

        //WhiteCam.camera.rect = new Rect(0.5f, 0, 0.5f, 1);
        //BlackCam.camera.rect = new Rect(0, 0, 0.5f, 1);

        MainCam.camera.rect = BlackCam.camera.rect = new Rect(0, 0, 1, 1);
        MainCam.active = false;

	
	}

    void OnApplicationQuit()
    {
        BlackMat.color = new Color(BlackMat.color.r, BlackMat.color.g, BlackMat.color.b, 1);
        WhiteMat.color = new Color(WhiteMat.color.r, WhiteMat.color.g, WhiteMat.color.b, 1);
    }
	
	void Update () {

        if (toggle)
        {
            print("toggle" + GameManager.currentPlayMode);
            if (GameManager.currentPlayMode == CurrentPlayMode.Black)
            {
                print("black"+WhiteMat.color.a);
                if (Toggle(ref WhiteMat, true)&&Toggle(ref BlackMat, false))
                {
                    print("done");
                    toggle = false;
                    ActivateBlackMode(false);
                    GameManager.currentPlayMode = CurrentPlayMode.White;
                }
            }
            else if (GameManager.currentPlayMode == CurrentPlayMode.White)
            {
                print("white");
                if(Toggle(ref BlackMat, true)&& Toggle(ref WhiteMat, false))
                {
                    print("done");
                    toggle = false;
                    ActivateWhiteMode(false);
                    GameManager.currentPlayMode = CurrentPlayMode.Black;
                }
            }
        }
	}

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
    }
    
    void Pause()
    {
        gameState = GameState.Pause;
    }
    void GameOver()
    {
        gameState = GameState.GameOver;
        OpeningWindow.SetActiveRecursively(false);
        GameOverWindow.SetActiveRecursively(true);
    }
    void Replay()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
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
        //if (mat.color.a <= .2f || mat.color.a >= 1f) { pulse = !pulse; }    

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
            MainCam.camera.backgroundColor = new Color(.85f, .85f, .85f);
        
    }
    void ActivateWhiteMode(bool active)
    {
        WhiteWorld.SetActiveRecursively(active);
        if (active)
            MainCam.camera.backgroundColor = new Color(.29f, .29f, .29f);
    }


}
