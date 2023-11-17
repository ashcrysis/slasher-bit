using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 2f;
    private float jumpingPower = 12f;
    public float speedVeloc = 20f;
    private bool isFacingRight = true;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    
    private float origSpeed;


    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.1f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(6f, 12f);

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 1f;
     private float jumpCooldown = 1f;
    private bool untouchable;
    private bool house = false;
    public bool cutscene = false;
    public float acceleration = 2f;
    private int jumpCount = 1;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform abletoDash;
    [SerializeField] private Transform fallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    public bool moving;
    public float hitForce = 10f;
    public AudioSource Jump;
    public AudioSource Land;
    public AudioSource SnowWalk;
    public AudioSource SnowRun;
    public AudioSource Dash;
    Animator anim;
    private void Start(){
            origSpeed = speed;
            anim = GetComponent<Animator>();
     
    
    }

  /// <summary>
  /// This function updates the character's movement and animation based on player input.
  /// </summary>
  /// <returns>
  /// The code does not explicitly return anything.
  /// </returns>
    private void Update()
    {
        
        horizontal = Input.GetAxisRaw("Horizontal");    
         moving = horizontal!= 0 ? true : false;

        anim.SetBool("landanim",ableDash());
       
        anim.SetBool("isMoving",moving);
        anim.SetBool("isGrounded",IsGrounded());
        anim.SetBool("fallCheck",IsFalling());
        
        if (!cutscene){
        if (isDashing)
        {   
            untouchable = true;
            anim.SetBool("isDashing",true);
            return;
        }else{
            anim.SetBool("isDashing",false);
            untouchable = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && ableDash() && canDash && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jumping"))
        {
            StartCoroutine(Dashh());
        }

        if(!IsGrounded()){

        anim.SetBool("isMoving",false);

        }


        if (Input.GetButtonDown("Jump") && ableDash() )
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            anim.SetBool("isJumping",true);
            Jump.Play();
        }
        else{
               anim.SetBool("isJumping",false);
        }

        WallSlide();
        WallJump();
        if (cutscene){
            speed = 2f;
        }
}

        if (!isWallJumping && !anim.GetCurrentAnimatorStateInfo(0).IsName("hold_hit_walking") && !anim.GetCurrentAnimatorStateInfo(0).IsName("roll") && !anim.GetCurrentAnimatorStateInfo(0).IsName("hold_hit")){
        Flip();
        }
            // Check for Shift key input to run
            if (!cutscene){
        speed = Input.GetButton("Fire3") ? speedVeloc : origSpeed;
        }else{
            speed = origSpeed;
        }
        anim.SetFloat("speed",speed);
        }
        
    



  /// <summary>
  /// This function updates the velocity of the Rigidbody component based on the input and acceleration
  /// variables.
  /// </summary>
  /// <returns>
  /// If the condition "isDashing" is true, then the function will return and exit without executing any
  /// further code.
  /// </returns>
    private void FixedUpdate()
    {
      if (isDashing)
        {
            return;
        }
        if (!isWallJumping ){
                
            float targetVelocityX = horizontal * speed;
            float smoothVelocityX = Mathf.Lerp(rb.velocity.x, targetVelocityX, Time.deltaTime * acceleration);
            
            rb.velocity = new Vector2(smoothVelocityX, rb.velocity.y);
           }
      
    }


/// <summary>
/// The WallJump function allows the player to perform a wall jump if they are currently wall sliding or
/// have recently wall jumped.
/// </summary>
 private void WallJump()
    {
        if (isWallSliding)
        {
            
            isWallJumping = false;
            wallJumpingDirection = - transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }


        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

   /// <summary>
   /// The Dashh function allows the player to dash in a specific direction for a certain amount of
   /// time, with a cooldown period before they can dash again.
   /// </summary>
    private IEnumerator Dashh()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }

/// <summary>
/// The IsGrounded function checks if the player is currently touching the ground using a 2D physics
/// overlap circle.
/// </summary>
/// <returns>
/// The method is returning a boolean value.
/// </returns>
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

    }
 /// <summary>
 /// The function "ableDash" checks if there is a ground layer within a certain radius of a given
 /// position.
 /// </summary>
 /// <returns>
 /// The method is returning a boolean value.
 /// </returns>
  private bool ableDash()
    {
        return Physics2D.OverlapCircle(abletoDash.position, 0.2f, groundLayer);

    }

 /// <summary>
 /// The IsWalled function checks if there is a wall present at the specified position.
 /// </summary>
 /// <returns>
 /// The method is returning a boolean value.
 /// </returns>
   private bool IsWalled()
    {
    return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

    }


 private bool IsFalling()
    {
 if (IsGrounded()){
            return false;
        }


    return Physics2D.OverlapCircle(fallCheck.position, 0.6f, groundLayer);

    }

    private IEnumerator ResetJumpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
       
        anim.SetBool("airSpin",false);
    }
  /// <summary>
  /// The WallSlide function checks if the player is touching a wall and not able to dash, and if so,
  /// sets the player's velocity to slide down the wall at a specified speed.
  /// </summary>
    private void WallSlide()
    {
        if (IsWalled() && !ableDash() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y,-wallSlidingSpeed, float.MaxValue));
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y-wallSlidingSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }



    /// <summary>
    /// The Flip function flips the direction the object is facing based on the value of the horizontal
    /// input.
    /// </summary>
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
     void playLand(){
        Land.Play();

    }
    void playWalk(){
        SnowWalk.Play();

    }
    void playRun(){
        SnowRun.Play();
    }
    void playDash(){
        Dash.Play();
    }
    void playWallJump(){
        if (isWallJumping){
        Jump.Play();
        }
    }
}