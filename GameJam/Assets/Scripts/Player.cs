using UnityEngine;
using System.Collections;

public class Player : CharacterBasics {
	
	#region Fields
	
	#region Bool List
	public bool canJump = true;
	#endregion
	
	#endregion
		
	
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
        BaseMovement(InputMovement(), InputMovement().magnitude );
		if (Input.GetKeyDown(KeyCode.Space)  || Input.GetKeyDown(KeyCode.JoystickButton1))
		{
			base.Launch();
		}
	}
}
