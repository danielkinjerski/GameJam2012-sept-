using UnityEngine;
using System.Collections;

public class Player : CharacterBasics {
	
	#region Fields
	private Vector3 velocity;
	
	//Ledgehanging
    public float torsoOffset = 0, topOffset = 0;
    public bool torsoHit, ledgehanging, topHit, wallClimbing;
	
	#region Sliding
	public float rayDistance = 2;
	private Vector3 contactPoint;
	private RaycastHit hit;
	public float levitate = 2;
	#endregion
	
	#region WallJump
	private CollisionFlags collisionFlags;
	public float touchWallJumpTime;
	public float lastJumpButtonTime;
	public float wallJumpTimeout;
	public Vector3 wallJumpContactNormal;
	
	#endregion
	
	#region Bool List
	public bool canJump = true;
	#endregion
	
	#endregion
	
	protected override void Awake () 
	{
		base.Awake();
		
		//normalColor = mesh.material.GetColor("_Ambient");
		rayDistance = controller.height * .5f + controller.radius;
	}
	
	public virtual void Movement()
    {
		//AnimationFramework();

		if (ledgehanging)	
		{return;}
		
        //Directional movement time
        var tempDir = Camera.mainCamera.transform.forward * InputMovement().y + InputMovement().x * Camera.mainCamera.transform.right;
        
		if (pushing)
			direction = new Vector3(direction.x, direction.y, direction.z); //Keeps movement from happening
			else
			direction = new Vector3(tempDir.x, direction.y, tempDir.z); //Input movement is used

        targetSpeed = (tempDir.magnitude != 0) ? maxSpeed : 0;
        
        //Snap speed to 0 if going SUPER slow
        if (speed < .01f) speed = 0;

        //snap movement
        if (new Vector3(tempDir.x, 0, tempDir.z).normalized.magnitude > 0.9f)
            trans.forward = new Vector3(direction.x, 0, direction.z);
		
		//Extra abilities for the player
		PushRigibodies();
	
		//Lerping of speed, etc.
		//////////////////////////////////////
		//Catch the current vertical movement (for jumping/falling)
        var _holdTheJump = direction.y;

        //Don't want our character pointing up if we jump
        direction.y = 0;

        //We only want direction at this point
        direction.Normalize();

        //This is our "friction"
        speed = Mathf.Lerp(speed, targetSpeed, accelerationSpeed);

        //If we've got a signification magnitude, continue moving forward ;; if were are recieving movement, apply it 
        direction = (direction.magnitude > 1) ? new Vector3(direction.x * speed, _holdTheJump, direction.z * speed)
                                              : new Vector3(trans.forward.x * speed, _holdTheJump, trans.forward.z * speed);

        if (!ledgehanging )
        {
			Gravity();
		}
		//////////////////////////////////////
		
		//Allows for knockback and rush to work
        if (force.magnitude > 1)
        {
            force *= Mathf.Lerp(force.magnitude, 0, .03f) / force.magnitude;
            controller.Move(force * Time.deltaTime);
        }
        else if (force.magnitude > 0)
            force = Vector3.zero;
		
		//SlideOnSlopes(); //Must happen last


        //Reapply the vertical force and speed to the movement and override basic gravity
        controller.Move(direction * Time.deltaTime);
    }
	
	public void LedgeChecking()
    {		
    	
        RaycastHit torsoHitCast, topHitCast;

        var fwd = transform.TransformDirection(Vector3.forward);
        Vector3 t; t = transform.position;

        if (!Physics.Raycast(new Vector3(t.x, t.y + topOffset, t.z), fwd, out topHitCast, 1)
            && Physics.Raycast(new Vector3(t.x, t.y + torsoOffset, t.z), fwd, out torsoHitCast, 1)
            && direction.y < 2   && !controller.isGrounded)
        {
            if (torsoHitCast.collider.GetType() == typeof(BoxCollider) && (torsoHitCast.collider.tag != "Prop" || topHitCast.collider.tag != "Prop"))
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
					//A brief delay before you can grab the wall again
					Invoke("CanWallClimb", .2f);
					Launch(10);
					ledgehanging = false;
                }
                else 
                {
					direction.y = 0;
                    trans.rotation = Quaternion.LookRotation(new Vector3(-torsoHitCast.normal.x,0,-torsoHitCast.normal.z));
					
					//Falsfy all when on ledge
                    falling = false;
					jumping = false;
					
					//anim.Play("Ledge_Hang_Pose");
                    ledgehanging = true;
                }
            }
        }
        else //When nothing is detected any longer, not ledgehanging
            ledgehanging = false;

        Debug.DrawRay(new Vector3(t.x, t.y + topOffset, t.z), fwd, Color.green);
        Debug.DrawRay(new Vector3(t.x, t.y + torsoOffset, t.z), fwd, Color.green);
    }
	
	/// <summary>
	/// This allows for pushing rigibodies.
	/// </summary>
	public void PushRigibodies()
	{	
        RaycastHit forwardCast;

        var fwd = transform.TransformDirection(Vector3.forward);
        Vector3 t; t = transform.position;
		
        //Torso Hit Confirmed
        if (Physics.Raycast(new Vector3(t.x, t.y + torsoOffset, t.z), fwd, out forwardCast, .8f)
            && direction.y < 2)
        {
            if ((forwardCast.collider.GetType() == typeof(BoxCollider)) && (forwardCast.collider.tag == "Prop"))
            {
				var pushObj = forwardCast.collider.gameObject.GetComponent<PushableObject>();
				pushObj.Push(trans.forward);
				pushing = true;

	            anim.Blend("Push");
	            trans.rotation = Quaternion.LookRotation(new Vector3(-forwardCast.normal.x,0,-forwardCast.normal.z));
				pushing = false;
			}
        }
        else
            pushing = false;
		
        Debug.DrawRay(new Vector3(t.x, t.y + torsoOffset, t.z), fwd, Color.blue);
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
		Movement();
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Launch();
		}
	}
}
