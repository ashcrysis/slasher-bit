using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float acceleration = 5f; // Adjust this to control acceleration
    public float maxSpeed = 10f; // Adjust this to control maximum speed
    public float detectionRange = 5f;
    private GameObject player;
    private bool moving;
    public Animator anim;
    public bool hit = true;
    private Rigidbody2D rb;

    void Start()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        catch (System.Exception)
        {
            throw;
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
      /* This code is responsible for controlling the enemy's behavior when the player is within the
      detection range. */
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= detectionRange)
            {
                Vector3 direction = player.transform.position - transform.position;

                if (direction.x > 0)
                {
                    transform.localScale = new Vector3(2, 2, 2); // Facing right
                }
                else if (direction.x < 0)
                {
                    transform.localScale = new Vector3(-2, 2, 2); // Facing left
                }

                // Check if the player is moving
                bool playerMoving = player.GetComponent<PlayerMovement>().moving; // Replace "PlayerMovement" with your actual player movement script

                if (distanceToPlayer <= detectionRange)
                {
                    Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);

                    // Calculate the direction and apply acceleration
                    Vector2 accelerationVector = (targetPosition - (Vector2)transform.position).normalized * acceleration;
                    rb.velocity += accelerationVector * Time.deltaTime;

                    // Limit the speed to maxSpeed
                    rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

                    // Check for movement
                    if (rb.velocity.magnitude > 0)
                    {
                        moving = true;
                        // Do something when the enemy is moving
                    }
                    else
                    {
                        moving = false;
                    }
                }
                if (!playerMoving && distanceToPlayer <= 2f)
                {
                    moving = false;
                }

                anim.SetBool("isPatrolling", moving);
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {   
      if (other.CompareTag("Player") && hit)
        {
        var col = other.gameObject.GetComponent<PlayerMovement>();
        if (col != null){    
            Debug.Log("Has touched player!" + col.name);
            anim.SetBool("isAttacking",true);
            Debug.Log("Has hit!");
            hit = false;
            col.
            StartCoroutine(ResetHitAfterDelay(2f));
        }
    }
        
    }
    private void turnAnimOff(){

         anim.SetBool("isAttacking",false);
    }
  
    IEnumerator ResetHitAfterDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            hit = true;
        }
    }
}
