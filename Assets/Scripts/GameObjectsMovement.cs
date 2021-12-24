#region

using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

#endregion

public class GameObjectsMovement : MonoBehaviour
{
    public Rigidbody2D GameObject;
    public Rigidbody2D Snowball;
    private float speed;
    private bool didThrowSnowball;

    private Dictionary<string, List<int>> availableLines;

    private Dictionary<string, List<float>> positions;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject = GetComponent<Rigidbody2D>();
        speed = 3f;
        availableLines = new Dictionary<string, List<int>>
        {
            { "Deer", new List<int> { 0, 1 } },
            { "Snowman", new List<int> { 0, 1, 2 } },
            { "Snowdrift", new List<int> { 0, 1, 2 } },
            { "Gift", new List<int> { 0, 1, 2 } },
            { "Fir", new List<int> { 1, 2 } }
        };
        positions = new Dictionary<string, List<float>>
        {
            { "Deer", new List<float> { -0.15f, 3.65f, 7.37f }},
            { "Snowman", new List<float> { -2.61f, -1.18f, 0.2f }},
            { "Snowdrift", new List<float> { -6.29f, -3.2f, -0.23f }},
            { "Gift", new List<float> { -3, 0.1f, 3.1f }},
            { "Fir", new List<float> { 0.1f, 1.35f, 2.6f }},
        };
        didThrowSnowball = false;
    }

    // Update is called once per frame
    private void Update()
    {
        var position = GameObject.position;
        GameObject.position = new Vector2(position.x - speed * Time.deltaTime, position.y);

        if (!didThrowSnowball && GameObject.CompareTag("Snowman") && IsObjectOnScreen()) 
            TryThrowSnowball();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Respawn") || other.CompareTag("Player") && GameObject.CompareTag("Gift"))
        {
            var random = new Random();
            int line;

            do
            {
                line = random.Next(3);
            } while (!availableLines[GameObject.tag].Contains(line));

            if (Time.timeScale < 6)
                Time.timeScale += 0.1f;
            didThrowSnowball = false;
            GameObject.transform.Translate(GetNewPosition(), 0, 0);
        }
    }

    private bool IsObjectOnScreen()
    {
        return GameObject.position.x < -7;
    }

    private static float GetNewPosition()
    {
        return 90 * (1 + (Time.timeScale - 1) / 4);
    }

    private void TryThrowSnowball()
    {
        var random = new Random();
        if (random.Next(700) < 1)
        {
            Snowball.position = GameObject.position;
            didThrowSnowball = true;
        }
    }
}