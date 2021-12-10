using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D Player;
    public GameObject Freeze;
    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    public List<Rigidbody2D> Obstacles;

    private List<float> positions;

    public int Score;
    
    private bool isControlChanged;

    private float speed;

    private float normalSpeed;

    private float slowSpeed;

    private int currentPosition;

    private bool isMoving;

    private int movement;

    private int destination;

    private Stopwatch timer;

    private GUIStyle style;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Player = GetComponent<Rigidbody2D>();
        positions = new List<float>
        {
            -3, 0, 3
        };
        timer = new Stopwatch();
        ResetPlayer();
        style = new GUIStyle
        {
            fontSize = 24,
            normal = new GUIStyleState
            {
                textColor = Color.white
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.Elapsed.Seconds >= 5)
        {
            timer.Reset();
            ClearEffects();
        }
        
        if (isMoving)
        {
            Move();
        }
        else
        {
            var currentMovement = 0;

            if (Input.GetKeyDown(KeyCode.W))
            {
                currentMovement = 1;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                currentMovement = -1;
            }

            if (isControlChanged)
                currentMovement *= -1;

            if (currentMovement != 0 && IsDestinationCorrect(currentMovement))
            {
                movement = currentMovement;
                isMoving = true;
                destination = currentPosition + currentMovement;
            }
        }
    }
    
    public void OnGUI() {
        GUI.Label(new Rect(1000, 10, 100, 20), Score.ToString(), style);
    }

    public void ResetLevel()
    {
        ResetPlayer();
        ResetObstacles();
        Time.timeScale = 1;
    }

    public void Pause()
    {
        timer.Stop();
        Time.timeScale = 0;
        PauseScreen.SetActive(true);
    }

    public void Resume()
    {
        timer.Start();
        Time.timeScale = 1;
        PauseScreen.SetActive(false);
    }

    private void ResetObstacles()
    {
        foreach (var obstacle in Obstacles)
        {
            var position = obstacle.position;
            position = new Vector2(position.x + 40, position.y);
            obstacle.position = position;
        }
    }

    public void ResetPlayer()
    {
        Freeze.SetActive(false);
        GameOverScreen.SetActive(false);
        PauseScreen.SetActive(false);
        isControlChanged = false;
        normalSpeed = 0.12f;
        slowSpeed = 0.06f;
        speed = normalSpeed;
        isMoving = false;
        movement = 0;
        currentPosition = 1;
        Player.position = new Vector2(Player.position.x, positions[currentPosition]);
        Score = 0;
        timer.Reset();
        Time.timeScale = 1;
    }

    private bool IsDestinationCorrect(int delta)
    {
        var newPosition = currentPosition + delta;
        return 0 <= newPosition && 2 >= newPosition;
    }

    private void Move()
    {
        if (!IsOnLine())
        {
            var position = Player.position;
            Player.position = new Vector2(position.x, position.y + speed * movement);
        }
        else
        {
            movement = 0;
            isMoving = false;
        }
    }

    private bool IsOnLine()
    {
        var position = Player.position;
        
        if (Math.Abs(position.y - positions[destination]) < speed)
        {
            Player.position = new Vector2(position.x, positions[destination]);
            currentPosition = destination;
            return true;
        }
        
        return false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Snowdrift"))
        {
            timer.Restart();
            ReverseControl();
            SlowDown();
        }

        if (other.CompareTag("Deer"))
        {
            timer.Restart();
            SetDestination(2);
            ReverseControl();
        }

        if (other.CompareTag("Fir"))
        {
            timer.Restart();
            SetDestination(0);
            ReverseControl();
        }

        if (other.CompareTag("Snowman"))
        {
            GameOver();
        }

        if (other.CompareTag("Gift"))
        {
            timer.Reset();
            ClearEffects();
            IncreaseScore(3);
        }

        if (other.CompareTag("Snowball"))
        {
            timer.Reset();
            SlowDown();
        }
    }

    private void ReverseControl()
    {
        isControlChanged = true;
        timer.Start();
        Freeze.SetActive(true);
    }

    private void SlowDown()
    {
        speed = slowSpeed;
    }

    private void SetDestination(int dest)
    {
        if (currentPosition == dest)
            return;
        
        if (currentPosition < dest)
        {
            movement = 1;
        }
        else if (currentPosition > dest)
        {
            movement = -1;
        }
        
        destination = dest;
        isMoving = true;
    }

    private void ClearEffects()
    {
        isControlChanged = false;
        speed = normalSpeed;
        Freeze.SetActive(false);
    }

    private void GameOver()
    {
        timer.Stop();
        Time.timeScale = 0;
        GameOverScreen.SetActive(true);
    }

    private void IncreaseScore(int score)
    {
        Score += score;
    }
}
