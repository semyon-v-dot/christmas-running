using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Obstacle : MonoBehaviour
{
    private int speed = 10;
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
        obstacle.MovePosition(new Vector2(position.x - speed, position.y));
    }
}
