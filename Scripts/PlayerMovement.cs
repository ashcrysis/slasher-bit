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


    public float hitForce = 10f;
    public float hitCooldown = 1f;
    public float lasthitTime;
    public Animator animSlash;
    public AudioSource Jump;
    public AudioSource Land;
    public bool canHit ;
    Animator anim;
    private void Start(){
            origSpeed = speed;
            anim = GetComponent<Animator>();
     
    
    }

    private void Update()
    {
        
        horizontal = Input.GetAxisRaw("Horizontal");    
        var moving = horizontal!= 0 ? true : false;
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

        if (Input.GetKeyDown(KeyCode.LeftControl) && ableDash() && canDash)
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

        
        
        if (ableDash()){
            jumpCount= 1;
            Debug.Log("jumpCount now is one " + jumpCount);
        }

        else{
               anim.SetBool("isJumping",false);
        }


        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f){
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        


       if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time - lasthitTime > hitCooldown){
                lasthitTime = Time.time;
                
                anim.SetBool("isAttacking",Input.GetButtonDown("Fire1"));
                animSlash.SetBool("isAttacking",Input.GetButtonDown("Fire1"));
                canHit=true;
            }
        else{
            anim.SetBool("isAttacking",false);
            canHit=false;
            animSlash.SetBool("isAttacking",false);
        }

        WallSlide();
        WallJump();
        if (cutscene){
            speed = 2f;
        }
}

        if (!isWallJumping && canHit==false){
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


 private void WallJump()
    {
        if (isWallSliding)
        {
            
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
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

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

    }
  private bool ableDash()
    {
        return Physics2D.OverlapCircle(abletoDash.position, 0.2f, groundLayer);

    }

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
    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
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
}