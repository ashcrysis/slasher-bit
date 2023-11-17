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
        /* This code block is checking if the `life` variable is less than or equal to 0. If it is, it
        then checks if `hasPlayed` is false. If both conditions are true, it proceeds to destroy the
        game object that this script is attached to using `Destroy(gameObject)`. It also
        instantiates an `AudioSource` component using the `Instantiate` method and assigns it to the
        `_audio` variable. Finally, it sets `hasPlayed` to true to prevent this code block from
        executing again. */
        if (life<= 0){
            if (!hasPlayed){
           Destroy(gameObject);
           AudioSource _audio = Instantiate(audio);
           
           hasPlayed = true;
        }
        }
    }


 
}
