using System.Collections;
using UnityEditor;
using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    public SpriteRenderer sprite; // Drag your Particle System here in the Inspector
    public PlayerMovement playerMovement;
    public AudioSource audio;
    public bool hit;
    public int damage = 30;
    private bool canDamage = true;
    private float lasthitTime;
    void Update()
    {
        lasthitTime = playerMovement.lasthitTime;
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            audio.Play();
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
    if (hit && Time.time - lasthitTime > 0.1f){
        if (other.CompareTag("enemy")){
        
        
            var enemy = other.GetComponent<enemyHandler>();

            if (enemy != null)
            {
                // Check if the player wants to attack before dealing damage

                    Debug.Log("Slash has made " +damage +  " points of damage on " + other);
                    if (canDamage){
                    enemy.life -= damage;
                    canDamage = false;
                    StartCoroutine(OnlyDamageOnce(0.2f));
                    }
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

        private IEnumerator OnlyDamageOnce(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDamage = true;
    }

 
}