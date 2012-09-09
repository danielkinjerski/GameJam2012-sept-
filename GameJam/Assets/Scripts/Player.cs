using UnityEngine;
using System.Collections;

public class Player : CharacterBasics {
	
	#region Fields

    private GameObject manager;
    private Vector3 respawn;
	
	#endregion

    private void Start()
    {
        manager = GameObject.Find("GameManager");
        respawn = transform.position;
    }
	
	/// <summary>
	/// 	Recieves the input from the axes
	/// </summary>
	/// <returns>
	/// The movement.
	/// </returns>
	private Vector2 InputMovement()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
	
	public void Update()
	{
        if (GameManager.gameState != GameState.PlayGame)
            return;


        BaseMovement(InputMovement(), InputMovement().magnitude );
		if (Input.GetKeyDown(KeyCode.Space)  || Input.GetKeyDown(KeyCode.JoystickButton1))
		{
			base.Launch();
		}
        if (Input.GetKeyUp(KeyCode.X))
        {
            respawn = transform.position;
            manager.SendMessage("Switch");
        }
	}

    void Respawn()
    {
        this.ForceStopEverything();
        this.transform.position = respawn;
    }

}
