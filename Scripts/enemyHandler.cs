using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHandler : MonoBehaviour
{
    public int life = 30;
    // Start is called before the first frame update
  
    // Update is called once per frame
    void Update()
    {
        if (life<= 0){

            Destroy(gameObject);
        }
    }
}
