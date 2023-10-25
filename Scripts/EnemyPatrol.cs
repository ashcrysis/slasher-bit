using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public int speed;
    public float detectionRange = 5f; // Adjust this to your desired detection range
    private GameObject player;
    private Vector3 previousPosition;
    private bool moving;
    public Animator anim;
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
        previousPosition = transform.position;
    }

    void Update()
    {
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

                Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                // Check for movement
                if (transform.position != previousPosition)
                {
                    Debug.Log("Enemy is moving!");
                    moving = true;
                    // Do something when the enemy is moving
                }else{
                    moving = false;
                }

                // Update previous position
               
                anim.SetBool("isPatrolling",moving);
            }
        }
    }
}
