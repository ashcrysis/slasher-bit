using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public int speed;
    public float detectionRange = 5f; // Adjust this to your desired detection range
    private GameObject player;

    void Start()
    {
        // You might want to find the player in the Start method instead of OnTriggerStay2D
        try
        {
            player = GameObject.FindGameObjectWithTag("Player");    
        }
        catch (System.Exception)
        {
            
            throw;
        }
        
    }

    void Update()
    {
        if (player != null)
        {
            // Check if the player is within the detection range
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= detectionRange)
            {
                // Calculate the direction to the player
                Vector3 direction = player.transform.position - transform.position;

                // Flip the frame based on the direction
                if (direction.x > 0)
                {
                    transform.localScale = new Vector3(-2, 2, 2); // Facing right
                }
                else if (direction.x < 0)
                {
                    transform.localScale = new Vector3(2, 2, 2); // Facing left
                }

                // Move towards the player
                Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
    }
}
