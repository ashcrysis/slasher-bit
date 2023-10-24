using System.Collections;
using UnityEditor;
using UnityEngine;
public class Blood : MonoBehaviour
{
    public ParticleSystem blood;
    private bool bloodInstantiated = false;
    private bool hit;
    private SwordSlash swordSlash;

   
    void Start()
    {
         GameObject slashObject = GameObject.Find("Slash");
 if (slashObject != null)
        {
            // Obtendo o componente SwordSlash do objeto Slash
            swordSlash = slashObject.GetComponent<SwordSlash>();

            if (swordSlash == null)
            {
                Debug.LogError("SwordSlash component not found on the Slash object!");
            }
        }
        else
        {
            Debug.LogError("Slash object not found in the scene!");
        }

        
    }
    void Update()
    {
        hit = swordSlash.hit;
       
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
            StartCoroutine(deleteBlood(0.3f,instantiatedBlood));
            StartCoroutine(RestartBlood(0.1f));
            bloodInstantiated = true;
            
        }
    }

    private IEnumerator TurnBloodyOff(float delay, ParticleSystem blood)
    {
        yield return new WaitForSeconds(delay);
        blood.Stop();
    }
  private IEnumerator RestartBlood(float delay)
    {
        yield return new WaitForSeconds(delay);
        bloodInstantiated = false;
        
    }
     private IEnumerator deleteBlood(float delay,ParticleSystem blood)
    {
        yield return new WaitForSeconds(delay);
       Destroy(blood);
    }
}
