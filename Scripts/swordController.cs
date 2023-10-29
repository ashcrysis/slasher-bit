using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordController : MonoBehaviour
{
    
    public float lasthitTime;
    public Animator anim;
    public Animator animSlash;
    public bool canHit ;
    public float hitCooldown = 0.5f;
    public PlayerMovement playerMovement;
    
    public AudioSource audio;
    public bool hit;
    public int damage = 30;
    private bool canDamage = true;
    private bool isCutscene = false;
    private float knockbackForce = 9f;
  
    // Update is called once per frame
    void Update()
    {
    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("hold_hit_walking") &&!anim.GetCurrentAnimatorStateInfo(0).IsName("hold_hit"))
        {
            canDamage = true;
            canHit = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time - lasthitTime > hitCooldown && canDamage && !anim.GetCurrentAnimatorStateInfo(0).IsName("roll")){
                    lasthitTime = Time.time;
                    
                    anim.SetBool("isAttacking",Input.GetButtonDown("Fire1"));
                    animSlash.SetBool("isAttacking",Input.GetButtonDown("Fire1"));
                    canHit=true;
                }
            else{
                anim.SetBool("isAttacking",false);
                animSlash.SetBool("isAttacking",false);
                canHit = false;
            }
       
        isCutscene = playerMovement.cutscene;

        if (!isCutscene){
        if (canHit && canDamage) // Check for left mouse button click
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
                
            StartCoroutine(ResetHitAfterDelay(0.1f));
            
            
            }
        }   
    }
        private IEnumerator ResetHitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hit = false;
    }

  void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("enemy") && hit)
        {
            enemyHandler enemy = other.GetComponent<enemyHandler>();

            if (enemy != null)
            {
                if (canDamage)
                    {
                        
                    enemy.life -= damage;
                    canDamage = false;
                    Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                    if (enemyRb != null)
                    {
                        Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                        enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                        Debug.Log("Applied knockback");
                       
                    }
                    Debug.Log(damage + " points ");
                    hit = false;
                    }
            }
        }
    }

}
