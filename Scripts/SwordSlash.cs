using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    public SpriteRenderer sprite; // Drag your Particle System here in the Inspector

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate the sword GameObject to face the mouse
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Trigger the sword slash particle effect

        }
    }
}
