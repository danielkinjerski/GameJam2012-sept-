using UnityEngine;
using System.Collections;

public class CharacterBasics : MonoBehaviour {
	
	public float speed, maxSpeed = 8, targetSpeed, accelerationSpeed = 1f, gravity = 20;
	
    public Vector3 direction, force, velocity;
	
	public float jumpHeight;
	
	public Color normalColor;
	public bool ledgehanging;
	
	
	#region Animation Names
	public string walk = "Walk";
	public string run = "Run";
	public string idle = "Idle_Chilling";
	public string fall = "Falling_Pose";
	public string glide = "Gliding";
	public string jump = "Jump";
	#endregion
	
	#region Bool List (Oh NOES!)
	public bool attacking = false;
	public bool falling = false;
	public bool jumping = false;
	#endregion
	
	#region Important
	public Transform trans;
	public CharacterController controller;
	public Animation anim;
	public SkinnedMeshRenderer mesh;
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
		    RaycastHit hit;
			Vector3 down = new Vector3(trans.position.x, trans.position.y - 2f, trans.position.z);
			Debug.DrawLine(trans.position, down, Color.cyan);
			
			//If not on the ground, assume normal gravity
	        if (!controller.isGrounded && jumping)	
			{
				direction.y -= (direction.y > -gravity) ? gravity * Time.deltaTime : 0;
				if (direction.y < 1)
					falling = true;
			}
			else
			if (!Physics.Raycast(trans.position, Vector3.down, out hit,2f) && !jumping)
			{			
				if (!falling)
				{
					direction.y = 0;
					falling = true;
				}
				//direction.y = velocity.y;
				
				direction.y -= (direction.y > -gravity) ? gravity * Time.deltaTime : 0;
			}
			
			//Needed to make the corresponding animations work
			if (controller.isGrounded)
			{
				jumping = false;
				falling = false;
			}
		
            ////Allows for jumping
            //if (controller.isGrounded) 
            //{
            //        //justJumped = true;
            //        Launch();
            //}
	}

    public virtual void BaseMovement(Vector3 direction, float speed)
    {
        targetSpeed = (speed != 0) ? maxSpeed : 0;

        if (speed > 0.9f)
            trans.forward = new Vector3(direction.x, 0, direction.z);


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
        direction = (speed > .9f) ? new Vector3(direction.x * speed, _holdTheJump, direction.z * speed)
                                              : new Vector3(trans.forward.x * speed, _holdTheJump, trans.forward.z * speed);

        Gravity();

        controller.Move(direction * Time.deltaTime);
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
          mesh.material.SetColor("_Ambient", normalColor);
    }
	
}
