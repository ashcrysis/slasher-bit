using System.Collections;
using UnityEditor;
using UnityEngine;
public class Blood : MonoBehaviour
{
    public ParticleSystem blood;
    public SwordSlash player;
    private bool bloodInstantiated = false;
    private bool hit;
    private bool hasTriggered = false;

    void Update()
    {
        hit = player.hit;
       
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!bloodInstantiated && collider.gameObject.tag == "Attack" && hit)
        {
            Vector3 hitDirection = (collider.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(hitDirection.y, hitDirection.x) * Mathf.Rad2Deg;

            ParticleSystem instantiatedBlood = Instantiate(blood, collider.ClosestPoint(transform.position), Quaternion.Euler(0, 0, angle));
            instantiatedBlood.Play();
            TurnBloodyOff(0.001f, instantiatedBlood);
            bloodInstantiated = true;
            Debug.Log("Tried to stop.");
        }
    }

    private IEnumerator TurnBloodyOff(float delay, ParticleSystem blood)
    {
        yield return new WaitForSeconds(delay);
        blood.Stop();
    }

    
}
