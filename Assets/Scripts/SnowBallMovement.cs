#region

using UnityEngine;

#endregion

public class SnowBallMovement : MonoBehaviour
{
    public Rigidbody2D Snowball;

    private float speed;

    // Start is called before the first frame update
    private void Start()
    {
        speed = 12f;
    }

    // Update is called once per frame
    private void Update()
    {
        var position = Snowball.position;
        Snowball.position = new Vector2(position.x - speed * Time.deltaTime, position.y);
    }
}