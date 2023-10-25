using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class enemyHandler : MonoBehaviour
{
  
    public int life = 30;
    public AudioSource audio;
    bool hasPlayed = false;
    void Update()
    {
        if (life<= 0){
            if (!hasPlayed){
           Destroy(gameObject);
           AudioSource _audio = Instantiate(audio);
           
           hasPlayed = true;
        }
        }
    }
 
}
