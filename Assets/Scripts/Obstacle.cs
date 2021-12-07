using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class Obstacle : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    private const int Speed = 20;
    private int currentPosition;

    public Rigidbody2D obstacle;
    // Start is called before the first frame update
    void Start()
    {
        obstacle = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var position = obstacle.position;
        obstacle.MovePosition(new Vector2(position.x - (float)(Speed / (currentPosition + 0.5)), position.y));
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collision!");
        }
        else if (other.CompareTag("Respawn"))
        {
            var random = new Random();
            var line = random.Next(PlayerMovement.positionsCount);
            currentPosition = line;
            var distance = random.Next(2000, 6000 - (line + 2) * 1000);
            Debug.Log(line);
            Debug.Log(distance);
            var position = obstacle.position;
            obstacle.position = new Vector2(position.y + distance, PlayerMovement.positions[line]);
            obstacle.transform.localScale = new Vector3(PlayerMovement.Sizes[line + 1].Width, PlayerMovement.Sizes[line + 1].Height);
        }
    }
}
