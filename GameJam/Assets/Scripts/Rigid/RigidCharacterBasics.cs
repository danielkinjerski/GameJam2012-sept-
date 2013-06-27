using UnityEngine;
using System.Collections;

public class RigidCharacterBasics : MonoBehaviour
{
    public bool sprinting = false;
    #region Variables

    public float speed, targetSpeed;
    private Vector3 force, velocity, respawn, initialPos, initialRot;
    public Vector3 direction;
    public float jumpHeight, maxSpeed = 8, accelerationSpeed = 1f, gravity = 20;
    public string walk = "Walking", idle = "Standing", fall = "Default Take", jump = "Jump", sprint = "dash";
    public bool attacking = false, falling = false, jumping = false;

    public bool isGrounded;

    public Transform trans;
    protected BoxCollider controller;
    public Rigidbody rigid;
    protected Animation anim = new Animation();
    protected SkinnedMeshRenderer mesh;
    protected GameObject manager;

    #endregion

    #region Mono Inherit Functions

    /// <summary>
    /// Initizalize
    /// </summary>
    protected virtual void Start()
    {
        controller = GetComponent<BoxCollider>();
        trans = this.transform;
        initialPos = trans.position;
        initialRot = trans.rotation.eulerAngles;
        anim = this.animation;
        anim[jump].wrapMode = WrapMode.Clamp;
        respawn = transform.position;
        manager = GameObject.Find("GameManager");
    }

    #endregion

    #region Player Updates

    protected virtual void Gravity()
    {
        //If not on the ground, assume normal gravity

        RaycastHit hitCast;
        Vector3 down = new Vector3(trans.position.x, trans.position.y + 1f, trans.position.z);
        //Debug.DrawLine(trans.position, down, Color.cyan);

        if (!Physics.Raycast(down, Vector3.down, out hitCast, 1f))
        {
            isGrounded = false;
            RaycastHit hit;
            Debug.DrawLine(trans.position, down, Color.cyan);

            if (!Physics.Raycast(trans.position, Vector3.down, out hit, 1f) && !jumping)
            {

            }
            direction.y -= (direction.y > -gravity) ? gravity * Time.deltaTime : 0;
            if (direction.y < 0 && jumping)
            {
                respawn = transform.position;
                falling = true;
                jumping = false;
            }
        }
        else
        {
            if (!hitCast.collider.isTrigger)
            {
                isGrounded = true;
                if (direction.y != 0 && falling)
                {
                    falling = false;
                    direction.y = 0;
                }
                if (!jumping && !falling)
                {
                    direction.y = 0;
                }
            }

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
        Vector3 moveDir = new Vector3(input.x, direction.y, input.y);

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

        if (sprinting)
        {
            speed = Mathf.Lerp(speed, targetSpeed * 2, accelerationSpeed);
        }
        else
        {
            //This is our "friction"
            speed = Mathf.Lerp(speed, targetSpeed, accelerationSpeed);
        }

        //If we've got a signification magnitude, continue moving forward ;; if were are recieving movement, apply it 
        direction = (speed > .9f) ? new Vector3(moveDir.x * speed, _holdTheJump, moveDir.z * speed)
                                              : new Vector3(trans.forward.x * speed, _holdTheJump, trans.forward.z * speed);
        //apply gravity
        Gravity();

        float currentHeight = 0;
        if (_holdTheJump > 0)
            currentHeight = trans.position.y;

        //controller.Move(direction * Time.deltaTime);
        //rigid.AddForce(direction * Time.deltaTime);
        //rigid.velocity = new Vector3(direction.x,-gravity,direction.z);
        rigid.velocity = direction;
        //rigid.AddForce(Vector3.up * direction.y);


        //if (currentHeight == trans.position.y)
        //    direction.y = 0;

        //are we still moving?
        return (speed == 0) ? false : true;
    }
    #endregion

    #region Utilities
    protected void ForceStopEverything()
    {
        direction = Vector3.zero;
        speed = 0;
    }

    /// <summary>
    /// Used for jumping and launching
    /// </summary>
    public void Launch()
    {
        if (!falling && !jumping)
        {
            direction.y = jumpHeight;
            //rigid.AddForce (Vector3.up * jumpHeight,ForceMode.Impulse);
            //rigidbody.velocity += Vector3.up * jumpHeight;

            //anim.CrossFade(jump, .01f);
            jumping = true;
            anim.Play(jump);
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

    void Respawn()
    {
        if (manager.gameObject.GetComponent<GameManager>().cheats)
        {
            this.ForceStopEverything();
            this.transform.position = respawn;
        }
        else
        {
            manager.SendMessage("GameOver");
            trans.position = initialPos;
            trans.rotation = Quaternion.Euler(initialRot);
            gameObject.SetActive(false);
        }
    }

    #endregion

    public void AnimationFramework()
    {
        if (!isGrounded)
        {
            if (jumping)
            {
                if (!anim.IsPlaying(jump))
                    anim.Play(jump);
            }
            if (falling)
            {
                if (!anim.IsPlaying(fall))
                    anim.Play(fall);
            }
        }

        if (isGrounded)//&!falling&&!jumping
        {                // Idle
            if (targetSpeed == 0 && direction.magnitude == 0)//!falling && !jumping
            {
                //if we are playing any other animation of than idle ;; force chance
                if (!anim.IsPlaying(idle))
                    anim.Play(idle);
            }

            if (!sprinting)
            {

                if (targetSpeed > 0 && direction.magnitude != 0)
                {
                    //if we are coming from idle or run ;; force chance
                    if (!anim.IsPlaying(walk))
                        anim.Play(walk);
                }
            }
            else if(sprinting && direction.magnitude != 0)
            {
                //if we are coming from idle or run ;; force chance
                if (!anim.IsPlaying(sprint))
                    anim.Play(sprint);
            }


        }
    }
}
