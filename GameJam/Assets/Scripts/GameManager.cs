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
    public GameObject OpeningWindow, GameOverWindow;
    public static GameState gameState;
    public static int CurrentPlayMode;
	// Use this for initialization
	void Awake () {
        OpeningWindow.SetActiveRecursively(true);
        GameOverWindow.SetActiveRecursively(false);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void Play()
    {
        OpeningWindow.SetActiveRecursively( false );
    }
    void Pause()
    {

    }
    void GameOver()
    {
        OpeningWindow.SetActiveRecursively(false);
        GameOverWindow.SetActiveRecursively(true);
    }
}
