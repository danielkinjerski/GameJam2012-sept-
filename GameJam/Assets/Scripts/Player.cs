using UnityEngine;
using System.Collections;

public class Player : RigidCharacterBasics {
	
    #region Mono Inherit Functions
	
	public void FixedUpdate()
	{
        if (GameManager.gameState != GameState.PlayGame)
            return;

        //RaycastHit hitCast;
        var down = transform.TransformDirection(Vector3.down);

        BaseMovement(InputMovement(), InputMovement().magnitude );
		

	}

    public void Update()
    {
        AnimationFramework();

        if (InputHandler.jbO || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            base.Launch();
        }
        if (InputHandler.jbR2 || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            manager.SendMessage("Switch");
        }
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Recieves the input from the axes
    /// </summary>
    /// <returns>
    /// The movement.
    /// </returns>
    private Vector2 InputMovement()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    #endregion


}
