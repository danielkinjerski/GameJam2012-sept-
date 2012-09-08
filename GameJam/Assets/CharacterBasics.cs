using UnityEngine;
using System.Collections;

public class CharacterBasics : MonoBehaviour {
	
	public float speed, maxSpeed = 8, targetSpeed, accelerationSpeed = 1f, gravity = 20;
	
    public Vector3 direction, force;
	
	public float jumpHeight;
	
	public Color normalColor;

	
	
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
	public bool pushing = false;
	public bool gliding = false;
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
		//stats = gameObject.AddComponent<GustStats>();
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
	            if (anim.IsPlaying(walk) || anim.IsPlaying(idle) && !pushing)
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
		if (falling && (!controller.isGrounded && !attacking && !gliding))	
		{
			  if (!anim.IsPlaying(fall))
                 anim.Blend(fall);
               else if (!anim.isPlaying)
                 anim.Play(fall);
		}
		#endregion

		#region Gliding
		if (gliding && (!controller.isGrounded && !attacking))
		{
			if (speed != 0)
	        {
	            if (!anim.IsPlaying(glide))
	                anim.Blend(glide);
	            else if (!anim.isPlaying)
	                anim.Blend(glide);
	        }
			else
				 anim.Blend(fall);
		}
		#endregion
    }
	
	#region Physics
	public virtual void Gravity()	
	{
		var velocity = direction;
				
		velocity.y = direction.y;
		direction.y = velocity.y;
		
		direction.y -= (direction.y > -gravity) ? gravity * Time.deltaTime : 0;	
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
	
	public void Rush(Vector3 dir, float amount)
	{
		force = dir * amount;
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
