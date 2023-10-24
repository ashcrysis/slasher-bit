using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 4f;
    private float jumpingPower = 12f;
    public float speedVeloc = 15f;
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

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform abletoDash;
    [SerializeField] private Transform fallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;


    public float hitForce = 10f;
    public float hitCooldown = 0.3f;
    public float lasthitTime;
    public Animator animSlash;
    Animator anim;
    private void Start(){
            origSpeed = speed;
            anim = GetComponent<Animator>();
     
    
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");    
        var moving = horizontal!= 0 ? true : false;
        
        anim.SetBool("isMoving",moving);
        anim.SetBool("isGrounded",IsGrounded());
        anim.SetBool("fallCheck",IsFalling());
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


        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            anim.SetBool("isJumping",true);
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
            }
        else{
            anim.SetBool("isAttacking",false);
            animSlash.SetBool("isAttacking",false);
        }

        WallSlide();
        WallJump();


        if (!isWallJumping){
        Flip();
        }
            // Check for Shift key input to run
        speed = Input.GetButton("Fire3") ? speedVeloc : origSpeed;
        }
        
    



    private void FixedUpdate()
    {
      if (isDashing)
        {
            return;
        }
        if (!isWallJumping ){
                
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
        Debug.Log("Executing");
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