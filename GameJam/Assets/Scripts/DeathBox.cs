using UnityEngine;
using System.Collections;

public class DeathBox : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        Destroy(collider.gameObject);
        GameObject.Find("GameManager").SendMessage("GameOver");
    }
}
