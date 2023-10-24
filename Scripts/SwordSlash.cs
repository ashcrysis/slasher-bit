using System.Collections;
using UnityEditor;
using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    public SpriteRenderer sprite; // Drag your Particle System here in the Inspector
    public PlayerMovement playerMovement;
    
    public bool hit;
    public int damage = 30;
    private float lasthitTime;
    void Update()
    {
        lasthitTime = playerMovement.lasthitTime;
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - transform.position;
            direction.Normalize();

            // Get the player's scale to determine the direction they are facing
            float playerScaleX = playerMovement.transform.localScale.x;

            // Calculate the angle based on the direction and adjust for player's facing direction
            float angle = Mathf.Atan2(direction.y * playerScaleX, direction.x * playerScaleX) * Mathf.Rad2Deg;

            // Rotate the sword GameObject to face the mouse
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            hit = true;              
                
            StartCoroutine(ResetHitAfterDelay(0.2f));
            
            
        }
    }
    
 private void OnTriggerStay2D(Collider2D other)
{  
    Debug.Log(hit && Time.time - lasthitTime > 0.1f);
    if (hit && Time.time - lasthitTime > 0.1f){
        Debug.Log("Test ONE: hit = true and time > coodown passed");
        if (other.CompareTag("enemy")){
        Debug.Log("Test TWO: other.tag == enemy");
        Debug.Log("Condition met");
        
        
            var enemy = other.GetComponent<enemyHandler>();

            if (enemy != null)
            {
                // Check if the player wants to attack before dealing damage
               
                    Debug.Log("Slash has made damage on " + other);

                    enemy.life -= damage;
                
            }
        }
    }
}

    private IEnumerator ResetHitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Hit reseted");
        hit = false;
    }

 
}