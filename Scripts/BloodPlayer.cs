using System.Collections;
using UnityEditor;
using UnityEngine;
public class BloodPlayer : MonoBehaviour
{
    public ParticleSystem blood;
    private bool bloodInstantiated = false;
    private bool hit;
    private swordController swordSlash;
    private EnemyPatrol enemy;
   
    void Start()
    {
        enemy = GetComponent<EnemyPatrol>();
    }
    void Update()
    {
       hit = enemy.hit;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
       
        if (!bloodInstantiated && collider.gameObject.tag == "Player" && hit)
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
