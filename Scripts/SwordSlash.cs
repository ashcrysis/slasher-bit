using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    public SpriteRenderer sprite; // Drag your Particle System here in the Inspector
    public PlayerMovement PlayerMovement;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - transform.position;
            direction.Normalize();

            // Get the player's scale to determine the direction they are facing
            float playerScaleX = PlayerMovement.transform.localScale.x;

            // Calculate the angle based on the direction and adjust for player's facing direction
            float angle = Mathf.Atan2(direction.y * playerScaleX, direction.x * playerScaleX) * Mathf.Rad2Deg;

            // Clamp the angle between 0 and 180 degrees
            

            // Rotate the sword GameObject to face the mouse
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Trigger the sword slash particle effect

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerMovement.isAttacking)
        {
            Debug.Log("Slash has entered in collision with " + other);
        }
    }
}
