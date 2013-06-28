using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        transform.GetComponentInChildren<TweenScale>().enabled = true;
        GameObject.Find("GameManager").SendMessage("EndGame");
    }
}
