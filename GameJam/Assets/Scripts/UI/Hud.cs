using UnityEngine;
using System.Collections;

public class Hud : BaseMenu
{

    public UILabel death, timer;

    float time = 0;

    // Use this for initialization
    void Start () {
        time = Time.timeSinceLevelLoad;        
    }

    void Update()
    {
        float timeSoFar = Time.timeSinceLevelLoad - time;
        string minutes = Mathf.Floor(timeSoFar / 60).ToString("00");
        string seconds = (timeSoFar % 60).ToString("00");
        if(timer != null)
        timer.text = minutes + ":" + seconds;
    }
}
