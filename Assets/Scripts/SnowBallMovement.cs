using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallMovement : MonoBehaviour
{
    public Rigidbody2D Snowball;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 18f;
    }

    // Update is called once per frame
    void Update()
    {
        var position = Snowball.position;
        Snowball.position = new Vector2(position.x - speed * Time.deltaTime, position.y);
    }
}
