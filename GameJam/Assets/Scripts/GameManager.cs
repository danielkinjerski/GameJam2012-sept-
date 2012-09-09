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

    public GameObject OpeningWindow, GameOverWindow, BlackWorld, WhiteWorld;
    public Material BlackMat, WhiteMat;
    public static GameState gameState;
    public static CurrentPlayMode currentPlayMode;
    private bool toggle;

	// Use this for initialization
	void Awake () {
        BlackMat.color = new Color(BlackMat.color.r, BlackMat.color.g, BlackMat.color.b, 1f);
        WhiteMat.color = new Color(WhiteMat.color.r, WhiteMat.color.g, WhiteMat.color.b, 1f);
        gameState = GameState.OpeningWindow;
        OpeningWindow.SetActiveRecursively(true);
        GameOverWindow.SetActiveRecursively(false);
	
	}

    void OnApplicationQuit()
    {
        BlackMat.color = new Color(BlackMat.color.r, BlackMat.color.g, BlackMat.color.b, 1);
        WhiteMat.color = new Color(WhiteMat.color.r, WhiteMat.color.g, WhiteMat.color.b, 1);
    }
	
	void Update () {

        if (toggle)
        {
            if (currentPlayMode == CurrentPlayMode.Black)
            {
                if (Toggle(ref BlackMat, true) && Toggle(ref WhiteMat, false))
                    toggle = false;
            }
            else if (currentPlayMode == CurrentPlayMode.White)
            {
                if (Toggle(ref BlackMat, true) && Toggle(ref WhiteMat, false))
                    toggle = false;
            }
        }
	}

    void Play()
    {
        gameState = GameState.PlayGame;
        OpeningWindow.SetActiveRecursively( false );
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
        switch (currentPlayMode)
        {
            case CurrentPlayMode.Black :
                BlackMode(true);
                WhiteMode(false);
                currentPlayMode = CurrentPlayMode.White;
                break;
            case CurrentPlayMode.White :
                WhiteMode(true);
                BlackMode(false);
                currentPlayMode = CurrentPlayMode.Black;
                break;
        }
        toggle = true;
    }

    bool Toggle(ref Material mat, bool pulse)
    {
        //if (mat.color.a <= .2f || mat.color.a >= 1f) { pulse = !pulse; }    
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, (pulse) ? (mat.color.a - .005f) : (mat.color.a + .005f));

        if ((mat.color.a <= 0 && !pulse)|| (mat.color.a >= 1 && !pulse))
            return true;
        else
            return false;

    }

    void BlackMode(bool active)
    {
        BlackWorld.SetActiveRecursively(active);
    }
    void WhiteMode(bool active)
    {
        WhiteWorld.SetActiveRecursively(active);
    }


}
