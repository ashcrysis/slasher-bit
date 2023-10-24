using System.Collections;
using UnityEngine;

public class Dash : MonoBehaviour
{

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private bool untouchable;

    public Animator anim;
    public Rigidbody2D rb;

    private void Update()
    {
        if (isDashing)
        {   
            untouchable = true;
            anim.SetBool("isDashing",true);
            return;
        }else{
            anim.SetBool("isDashing",false);
            untouchable = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && canDash)
        {
            StartCoroutine(Dashh());
        }

    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

    }

    private IEnumerator Dashh()
    {
        Debug.Log("Executing");
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }
}