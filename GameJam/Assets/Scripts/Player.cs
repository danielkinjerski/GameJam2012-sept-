using UnityEngine;
using System.Collections;

public class Player : CharacterBasics {
	
	#region Fields

    private GameObject manager;
	
	#endregion

    private void Start()
    {
        manager = GameObject.Find("GameManager");
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
            manager.SendMessage("Switch");
        }
	}
}
