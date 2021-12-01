using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private int speed = 10;
    public Rigidbody2D Obsctacle;
    // Start is called before the first frame update
    void Start()
    {
        Obsctacle = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var position = Obsctacle.position;
        Obsctacle.MovePosition(new Vector2(position.x - speed, position.y));
    }
}
