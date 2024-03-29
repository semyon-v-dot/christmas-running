#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D Player;
    public GameObject Freeze;
    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    public GameObject SettingsButton;
    public List<Rigidbody2D> Obstacles;

    private List<float> positions;

    public Text ScoreLabel;
	public Text ScoreLabelFromAd;
	public Text ScoreLabelFromPause;
	public Text ScoreLabelFromGameOver;
	public Text ScoreLabelBestFromGameOver;
    public int Score;
    public int MaxScore;

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

    private YandexSDK sdk;

    // Start is called before the first frame update
    private void Start()
    {
        TryLoadScore();
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

        sdk = YandexSDK.instance;
        sdk.onRewardedAdClosed += Resume;
    }

    // Update is called once per frame
    private void Update()
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
                currentMovement = 1;
            else if (Input.GetKeyDown(KeyCode.S)) currentMovement = -1;

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

    public void OnGUI()
    {
        ScoreLabel.text = Score.ToString();
        ChangeScoreLabelRectTransform();
		
		ScoreLabelFromAd.text = ScoreLabel.text;
		ChangeScoreLabelFromAdRectTransform();
		
		ScoreLabelFromPause.text = ScoreLabel.text;
		ChangeScoreLabelFromPauseRectTransform();
		
		ScoreLabelFromGameOver.text = ScoreLabel.text;
		ChangeScoreLabelFromGameOverRectTransform();
        
		ScoreLabelBestFromGameOver.text = MaxScore.ToString();
        ChangeScoreLabelBestFromGameOverRectTransform();
    }

    public void ChangeScoreLabelRectTransform()
    {
		int charWidth = 15;
        RectTransform rt = ScoreLabel.GetComponent<RectTransform>();
        if (ScoreLabel.text.Length * charWidth > rt.sizeDelta.x)
        {
            var newWidth = ScoreLabel.text.Length * charWidth;
            rt.sizeDelta = new Vector2(newWidth, rt.sizeDelta.y);
            rt.localPosition = new Vector2(rt.localPosition.x - charWidth / 2 , rt.localPosition.y);
        }
        
    }
	public void ChangeScoreLabelFromAdRectTransform()
    {
		int charWidth = 15;
		RectTransform rt = ScoreLabelFromAd.GetComponent<RectTransform>();
        if (ScoreLabelFromAd.text.Length * charWidth > rt.sizeDelta.x)
        {
            var newWidth = ScoreLabelFromAd.text.Length * charWidth;
            rt.sizeDelta = new Vector2(newWidth, rt.sizeDelta.y);
            //rt.localPosition = new Vector2(rt.localPosition.x - charWidth / 2 , rt.localPosition.y);
        }
	}
	public void ChangeScoreLabelFromPauseRectTransform()
    {
		int charWidth = 22;
		RectTransform rt = ScoreLabelFromPause.GetComponent<RectTransform>();
        if (ScoreLabelFromPause.text.Length * charWidth > rt.sizeDelta.x)
        {
            var newWidth = ScoreLabelFromPause.text.Length * charWidth;
            rt.sizeDelta = new Vector2(newWidth, rt.sizeDelta.y);
        }
	}
	
	public void ChangeScoreLabelFromGameOverRectTransform()
    {
		int charWidth = 29;
		RectTransform rt = ScoreLabelFromGameOver.GetComponent<RectTransform>();
        if (ScoreLabelFromGameOver.text.Length * charWidth > rt.sizeDelta.x)
        {
            var newWidth = ScoreLabelFromGameOver.text.Length * charWidth;
            rt.sizeDelta = new Vector2(newWidth, rt.sizeDelta.y);
            //rt.localPosition = new Vector2(rt.localPosition.x + charWidth / 2 , rt.localPosition.y);
        }
	}
	
	public void ChangeScoreLabelBestFromGameOverRectTransform()
    {
		int charWidth = 22;
		RectTransform rt = ScoreLabelBestFromGameOver.GetComponent<RectTransform>();
        if (ScoreLabelBestFromGameOver.text.Length * charWidth > rt.sizeDelta.x)
        {
            var newWidth = ScoreLabelBestFromGameOver.text.Length * charWidth;
            rt.sizeDelta = new Vector2(newWidth, rt.sizeDelta.y);
            //rt.localPosition = new Vector2(rt.localPosition.x + charWidth / 2 , rt.localPosition.y);
        }
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
        GameOverScreen.SetActive(false);
    }

    public void Resume(int id)
    {
        Resume();
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
        SettingsButton.SetActive(true);
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
            if (isControlChanged)
            {
                GameOver();
            }
            else
            {
                timer.Restart();
                ReverseControl();
                SlowDown();
            }
        }

        if (other.CompareTag("Deer"))
        {
            if (isControlChanged)
            {
                GameOver();
            }
            else
            {
                timer.Restart();
                if (IsDestinationCorrect(1))
                    SetDestination(currentPosition + 1);
                ReverseControl();
            }
        }

        if (other.CompareTag("Fir"))
        {
            if (isControlChanged)
            {
                GameOver();
            }
            else
            {
                timer.Restart();
                if (IsDestinationCorrect(-1))
                    SetDestination(currentPosition - 1);
                ReverseControl();
            }
        }

        if (other.CompareTag("Snowman")) GameOver();

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

        movement = currentPosition < dest ? 1 : -1;

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

        if (Score > MaxScore)
        {
            MaxScore = Score;
            SaveScore();
        }

        GameOverScreen.SetActive(true);
        SettingsButton.SetActive(false);
    }

    private void IncreaseScore(int score)
    {
        Score += score;
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt("MaxScore", MaxScore);
        PlayerPrefs.Save();
    }

    private void TryLoadScore()
    {
        if (PlayerPrefs.HasKey("MaxScore")) MaxScore = PlayerPrefs.GetInt("MaxScore");
    }
}