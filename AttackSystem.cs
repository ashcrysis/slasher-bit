using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    float lastClickTime = 0.0f;
    float timeBetweenClicks = 0.3f;
    int currentSequence = 0;
    public Animator anim;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastClickTime = Time.time;
        }
        else if (Input.GetMouseButton(0) && (Time.time - lastClickTime) >= 1.0f)
        {
            // Player is holding the mouse button for 1 second or more
            Debug.Log("Holding the mouse button for 1 second!");
        }

    }

    void PerformAttack(int sequence)
    {
        Debug.Log("Sequential Attack: " + sequence);
    }
}
