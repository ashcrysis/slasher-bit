using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class enemyHandler : MonoBehaviour
{
  
    public int life = 30;
    public AudioSource audio;
    public float knockbackForce = 9f;
    bool hasPlayed = false;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
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
