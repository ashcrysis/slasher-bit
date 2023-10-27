using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNewAnimTest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform abletoDash;
    [SerializeField] private Transform fallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private Animator anim;
    float horizontal;
    // Update is called once per frame
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");    
        
        anim.SetFloat("isMoving",horizontal);
        anim.SetFloat("isGrounded",IsGrounded() ? 1f:-1f);
        }
     private bool IsGrounded()
    {

        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

    }
}
