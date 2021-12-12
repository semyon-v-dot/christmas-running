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

    private List<float> positions;

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
        positions = new List<float>
        {
            -3, 0, 3
        };
        didThrowSnowball = false;
    }

    // Update is called once per frame
    private void Update()
    {
        var position = GameObject.position;
        GameObject.position = new Vector2(position.x - speed * Time.deltaTime, position.y);

        if (!didThrowSnowball && GameObject.CompareTag("Snowman") && IsObjectOnScreen()) TryThrowSnowball();
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

            if (Time.timeScale < 3)
                Time.timeScale += 0.05f;
            didThrowSnowball = false;
            GameObject.position = new Vector2(GetNewPosition(), positions[line]);
        }
    }

    private bool IsObjectOnScreen()
    {
        return GameObject.position.x < 9;
    }

    private static float GetNewPosition()
    {
        return 60 * (1 + (Time.timeScale - 1) / 2);
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