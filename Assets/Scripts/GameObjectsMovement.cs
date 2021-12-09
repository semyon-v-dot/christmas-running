using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class GameObjectsMovement : MonoBehaviour
{
    public Rigidbody2D GameObject;
    private float speed;

    private List<float> positions;
    // Start is called before the first frame update
    void Start()
    {
        GameObject = GetComponent<Rigidbody2D>();
        speed = 3f;
        
        positions = new List<float>
        {
            -3, 0, 3
        };

    }

    // Update is called once per frame
    void Update()
    {
        var position = GameObject.position;
        GameObject.position = new Vector2(position.x - speed * Time.deltaTime, position.y);

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Respawn"))
        {
            var random = new Random();
            var line = random.Next(3);
            
            GameObject.position = new Vector2(60, positions[line]);
        }
    }
}
