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
    public static GameState gameState;
    public static CurrentPlayMode currentPlayMode;

	// Use this for initialization
	void Awake () {
        gameState = GameState.OpeningWindow;
        OpeningWindow.SetActiveRecursively(true);
        GameOverWindow.SetActiveRecursively(false);
	
	}
	
	// Update is called once per frame
	void Update () {
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
                BlackMode(true);
                currentPlayMode = CurrentPlayMode.Black;
                break;
        }
    }
    void BlackMode(bool active)
    {
        Material mat = BlackWorld.GetComponentInChildren<Renderer>().material as Material;
        BlackWorld.SetActiveRecursively(active);
    }
    void WhiteMode(bool active)
    {
        WhiteWorld.SetActiveRecursively(active);
    }


}
