using UnityEngine;
using System.Collections;

public class CharacterBasics : MonoBehaviour {

    private float speed, targetSpeed;
	
    public Vector3 direction, force, velocity;

    public float jumpHeight, maxSpeed = 8, accelerationSpeed = 1f, gravity = 20;
	
	
	#region Animation Names
	public string walk = "Walk";
	public string run = "Run";
	public string idle = "Idle_Chilling";
	public string fall = "Falling_Pose";
	public string jump = "Jump";
	#endregion
	
	#region Bool List (Oh NOES!)
	public bool attacking = false, falling = false, jumping = false;
	#endregion
	
	#region Important
	protected Transform trans;
	protected CharacterController controller;
	protected Animation anim;
	protected SkinnedMeshRenderer mesh;
	#endregion
	
	
	// Use this for initialization
	protected virtual void Awake () 
	{
		controller = GetComponent<CharacterController>();
		trans = this.transform;
        anim = GetComponent<Animation>();
	}
	
	//The Animation component of the character
	protected void AnimationFramework()
    {
		if (controller.isGrounded && !attacking)
		{
			#region Running
			if (speed > maxSpeed / 2)
	        {
	            //if we are coming from idle or walk ;; force chance
	            if (anim.IsPlaying(walk) || anim.IsPlaying(idle))
	                anim.CrossFade(run);
	
	            //if we are already playing out anim ;; wait till its over, then play again
	            else if (!anim.isPlaying)
	                anim.Play(run);
	
	            //set animation speed based on the percentage of current speed against maxspeed
	            anim[run].speed = speed / maxSpeed;
	        }
			#endregion
			else
			#region Walking
			if (speed > maxSpeed / 20)
	        {
	            //if we are coming from idle or run ;; force chance
	            if (anim.IsPlaying(run) || anim.IsPlaying(idle))
	                anim.CrossFade(walk);
	
	            //if we are already playing out anim ;; wait till its over, then play again
	            else if (!anim.isPlaying)
	                anim.Play(walk);
	
	            //set animation speed based on the percentage of current speed against maxspeed
	            anim[run].speed = speed / maxSpeed;
	        }
			#endregion
	
			#region Moving Still
			else if (speed == 0)
	        {
	            //if we are playing any other animation of than idle ;; force chance
	            if (anim.IsPlaying(run) || anim.IsPlaying(walk))
	                anim.CrossFade(idle);			
	            //if nothing is playing ;; play again
	            else if (!anim.isPlaying)
	                anim.Play(idle);
	
	        }
			#endregion
		}
		
		#region Falling
		if (falling && (!controller.isGrounded && !attacking))	
		{
			  if (!anim.IsPlaying(fall))
                 anim.Blend(fall);
               else if (!anim.isPlaying)
                 anim.Play(fall);
		}
		#endregion
    }
	
	#region Physics
		
	protected virtual void Gravity()
	{

        //If not on the ground, assume normal gravity
        if (!controller.isGrounded)
        {
            RaycastHit hit;
            Vector3 down = new Vector3(trans.position.x, trans.position.y - 2f, trans.position.z);
            Debug.DrawLine(trans.position, down, Color.cyan);

            if (!Physics.Raycast(trans.position, Vector3.down, out hit, 2f) && !jumping)
            {

            }
            direction.y -= (direction.y > -gravity) ? gravity * Time.deltaTime : 0;
        }
        else
        {
            if (direction.y != 0 && falling)
            {
                falling = false;
                direction.y = 0;
            }
        }

        //Needed to make the corresponding animations work
        if (controller.isGrounded)
        {
            jumping = false;
            falling = false;
        }
	}


    /// <summary>
    /// Basic movment function
    /// </summary>
    /// <param name="input">Direction (x,z)</param>
    /// <param name="speed">Speed (0 - 1) where 1 is max speed</param>
    /// <returns>true if we are moving false if we are not</returns>
    public virtual bool BaseMovement(Vector2 input, float speed)
    {
        //build our movement vector
        Vector3 moveDir = new Vector3(input.x,direction.y,input.y);

        //prevent snapping forward
        if (moveDir.x == 0 && moveDir.z == 0)
            moveDir = new Vector3(trans.forward.x, direction.y, trans.forward.z);

        //are we accelerationg or decelerating?
        targetSpeed = (speed != 0) ? maxSpeed : 0;

        if (speed > 0.9f)
            trans.forward = new Vector3(moveDir.x, 0, moveDir.z);


        //Lerping of speed, etc.
        //////////////////////////////////////
        //Catch the current vertical movement (for jumping/falling)
        var _holdTheJump = moveDir.y;

        //Don't want our character pointing up if we jump
        moveDir.y = 0;

        //We only want direction at this point
        moveDir.Normalize();

        if (Vector3.Dot(moveDir, trans.forward) < .5f)
            trans.forward = Vector3.Lerp(trans.forward, moveDir, 1 / speed);
        else
            trans.forward = new Vector3(moveDir.x, 0, moveDir.z);

        //This is our "friction"
        speed = Mathf.Lerp(speed, targetSpeed, accelerationSpeed);

        //If we've got a signification magnitude, continue moving forward ;; if were are recieving movement, apply it 
        direction = (speed > .9f) ? new Vector3(moveDir.x * speed, _holdTheJump, moveDir.z * speed)
                                              : new Vector3(trans.forward.x * speed, _holdTheJump, trans.forward.z * speed);
        //apply gravity
        Gravity();

        float currentHeight = 0;
        if(_holdTheJump>0)
            currentHeight = trans.position.y;

        controller.Move(direction * Time.deltaTime);

        if (currentHeight == trans.position.y)
            direction.y = 0;

        //are we still moving?
        return (speed == 0)? false: true;
    }

    public virtual void Update()
    {
        if (GameManager.gameState != GameState.PlayGame)
            return;
    }
	
	/// <summary>
	/// Used for jumping and launching
	/// </summary>
	public void Launch()
	{
        if (controller.isGrounded)
        {
            direction.y = jumpHeight;
            //anim.CrossFade(jump, .01f);
            jumping = true;
        }
	}
	
	/// <summary>
	/// Overloaded to support inserting launch force
	/// </summary>
	/// <param name='_force'>
	/// _force.
	/// </param>
	public virtual void Launch(float _force)
	{

		direction.y = _force;
	}
	
	public void Rush(Vector3 _dir, float _amount)
	{
		force = _dir * _amount;
	}
	#endregion
    
    /// <summary>
    /// Flash this character
    /// </summary>
    /// <returns></returns>
    public IEnumerator FlashCharacter()
    {
          mesh.material.SetColor("_Ambient", Color.white);
          yield return new WaitForSeconds(.1f);
          //mesh.material.SetColor("_Ambient", normalColor);
    }
	
}
