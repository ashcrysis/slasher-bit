using System.Collections;
using UnityEditor;
using UnityEngine;
public class Blood : MonoBehaviour
{
    public ParticleSystem blood;
    public SwordSlash player;
    private bool bloodInstantiated = false;
    private bool hit;

    void Update()
    {
        hit = player.hit;
        if (bloodInstantiated)
        {
            // Uncomment the line below for resetting bloodInstantiated
            // Invoke("ResetBlood", 0.1f);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!bloodInstantiated && collider.gameObject.tag == "Attack" && hit)
        {
            Vector3 hitDirection = (collider.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(hitDirection.y, hitDirection.x) * Mathf.Rad2Deg;

            ParticleSystem instantiatedBlood = Instantiate(blood, collider.ClosestPoint(transform.position), Quaternion.Euler(0, 0, angle));
            instantiatedBlood.Play();
            Debug.Log("Playing");
            TurnBloodyOff(0.001f, instantiatedBlood);
            Debug.Log("Used turnbloody");
            bloodInstantiated = true;
        }
    }

    private IEnumerator TurnBloodyOff(float delay, ParticleSystem blood)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Tried to stop");
        blood.Stop();
        Debug.Log("Blood stopped");
    }

    void ResetBlood()
    {
        bloodInstantiated = false;
    }
}
