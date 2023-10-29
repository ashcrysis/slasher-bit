using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightFlick : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEngine.Rendering.Universal.Light2D light;
    private bool lightflick = true;
    public float flick1;
    public float flick2;
    // Update is called once per frame
    void Update()
    {
        if (lightflick){
        light.enabled = false;
        lightflick = false;
        StartCoroutine(flick(flick1,flick2));
        }
    }
     private IEnumerator flick(float delay,float delay2)
    {
        yield return new WaitForSeconds(delay);
        light.enabled = true;
         yield return new WaitForSeconds(delay2);
        lightflick = true;
    }
}
