using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {
    public TweenScale ts;
    GameObject player;
    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
            player = collider.gameObject;
        ts.enabled = true;
    }

    void Finished()
    {
        ts.transform.localScale = Vector3.one;
        player.SendMessage("Respawn");
        GameObject.Find("GameManager").SendMessage("EndGame");
    }
}
