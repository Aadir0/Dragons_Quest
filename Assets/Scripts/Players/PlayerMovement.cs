using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement parameters")]
    // Serialize Field is used to make any variable accesible and editible directly from unit. But it is diffrent from public since it make any variable accesible from any other script.
    public float speed;
    [SerializeField] private float jumpPower;

    // Layers to detect ground and walls for jumping on walls or jumping checks.
    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Refrences")]
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;

    // To stop from infinite wall-jumping 
    [Header ("Wall Jumping")]
    private float wallJumpCooldown;
    private float horizontalInput;
    [SerializeField] private float WallJumpX;  //Horizontal Wall jump force
    [SerializeField] private float WallJumpY;  //Vertical Wall jump force

    [Header("Audio Clips")]
    [SerializeField] private AudioClip JumpAudio;

    [Header("coyote counter")]
    [SerializeField] private float coyoteTime; //How much time player will be hanging in air
    private float coyoteCounter; //How much time passed since player ran off the edge

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJump;
    private int jumpCounter;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private AudioClip dashAudio;
    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private float dashCooldownTimer = 0f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        //Grab references for rigidbody and animator or box collider from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            extraJump = 1;
        }
    }

    private void Update()
    {
        //Update dash cooldown timer
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;

        //Handle dashing
        if (isDashing)
        {
            HandleDash();
            return; //Skip normal movement while dashing
        }

        //Check for dash input (Left Ctrl)
        if (Input.GetKeyDown(KeyCode.LeftControl) && dashCooldownTimer <= 0 && !isDashing && isGrounded())
        {
            StartDash();
            return;
        }

        //To get horizontal input keys.
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one; //Facing right
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1); //Facing left

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //------------------Wall jump logic-----------------------

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.Space) && body.linearVelocity.y > 0)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y / 2);
        }

        if (onWall())
        {
            body.gravityScale = 7;
            body.linearVelocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //Reset coyote counter
                jumpCounter = extraJump; //Reset jumpCounter to extra jump value
            }
            else
            {
                coyoteCounter -= Time.deltaTime; //Start decreasing time when not on ground
            }
        }
    }

    //----------------Jump logic---------------------
    private void Jump()
    {
        if (coyoteCounter < 0 && !onWall() && jumpCounter <= 0) return;
        //Ifcoyote counter is 0 or less or on wall or no extra jumps left don't do anything
        if (SoundManager.instance != null)
            SoundManager.instance.Playsound(JumpAudio);

        if (onWall())
        {
            WallJump();
        }
        else
        {
            if (isGrounded())
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            }
            else
            {
                //If not on ground or coyote counter greater than 0 do a normal  jump
                if (coyoteCounter > 0)
                {
                    body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
                }
                else
                {
                    if (jumpCounter > 0)
                    {
                        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }
            //Resets the coyoteCounter to zero to avoid double jump
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * WallJumpX, WallJumpY));
        wallJumpCooldown = 0;
    }

    //----------------Collision checks------------------
    private bool isGrounded()
    {
        //Using Raycast to check if bottom of player touches ground layer
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        //Using Raycast to check which direction player is facing
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    //public so that other scripts can use it
    public bool canAttack()
    {
        //Player can't shoot while on wall and dashing
        return !onWall() && !isDashing;
    }

    //----------------Dash logic---------------------
    private void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        dashCooldownTimer = dashCooldown;
        
        //Play dash animation
        anim.SetTrigger("dash");
        
        //Play dash sound
        if (SoundManager.instance != null && dashAudio != null)
            SoundManager.instance.Playsound(dashAudio);
        
        //Enable invincibility - ignore collision with Enemy/Trap layer (layer 10)
        Physics2D.IgnoreLayerCollision(8, 10, true);
        Physics2D.IgnoreLayerCollision(8, 9, true);
    }

    private void HandleDash()
    {
        dashTimeLeft -= Time.deltaTime;
        
        //Perform dash movement in the direction player is facing
        float dashDirection = Mathf.Sign(transform.localScale.x);
        body.linearVelocity = new Vector2(dashDirection * dashSpeed, 0);
        
        //End dash when time runs out
        if (dashTimeLeft <= 0)
        {
            EndDash();
        }
    }

    private void EndDash()
    {
        isDashing = false;
        dashTimeLeft = 0f;
        
        //Disable invincibility - re-enable collision with Enemy/Trap layer (layer 10)
        Physics2D.IgnoreLayerCollision(8, 10, false);
        Physics2D.IgnoreLayerCollision(8, 9, false);
        
        //Reset velocity
        body.linearVelocity = new Vector2(0, body.linearVelocity.y);
        
        //Update animator parameters to return to idle/run
        anim.SetBool("run", false);
        anim.SetBool("grounded", isGrounded());
    }

    public bool IsDashing()
    {
        return isDashing;
    }
}