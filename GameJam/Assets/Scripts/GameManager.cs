using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public GameObject OpeningWindow, GameOverWindow;
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
