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
    public bool onPlatform = false;
    public void CheckForPlatforms()
    {
        RaycastHit hitCast;
        Vector3 down = new Vector3(trans.position.x, trans.position.y + 1f, trans.position.z);

        if (Physics.Raycast(down, Vector3.down, out hitCast, 1f) && hitCast.collider.tag == "Platform")
        {
            if (!onPlatform && hitCast.collider != null )
            {
                onPlatform = true;
                transform.parent = hitCast.transform;
                print("Hi!");
            }

        }
        else if (onPlatform || hitCast.collider == null)
        {
            onPlatform = false;
            transform.parent = null;
        }
    }

    public void Update()
    {
        CheckForPlatforms();

        if (InputHandler.jbO || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            base.Launch();
        }

        AnimationFramework();

        if (InputHandler.jbR2 || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            transform.parent = null;

            manager.SendMessage("Switch");
        }

        if (Input.GetAxis("Joy1 Axis 3") > 0.5)//(Input.GetKey(KeyCode.LeftShift) || InputHandler.bL2Held || InputHandler.jbL2Held))
        {
            sprinting = true;
        }
        else
        {
            sprinting = false;
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
        return new Vector2(Input.GetAxis("Joy1 Axis 1") + Input.GetAxis("Horizontal"), Input.GetAxis("Joy1 Axis 2")+ Input.GetAxis("Vertical"));
    }

    #endregion


}
