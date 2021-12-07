using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [FormerlySerializedAs("PositionsCount")] public int positionsCount = 3;
    [FormerlySerializedAs("Player")] public Rigidbody2D player;
    
    private const int CameraHeight = 1080;
    public List<float> positions;
    private int currentPosition;
    private RectTransform rectTransform;
    public Dictionary<int, Size> Sizes;

    // Start is called before the first frame update
    void Start()
    {
        if (!(Camera.main is { }))
        {
            throw new Exception("Main camera is null");
        }
        
        player = GetComponent<Rigidbody2D>();
        rectTransform = GetComponent<RectTransform>();
        CalculateAndAddPositions();
        CalculateAndAddPlayerSizes();
        currentPosition = positionsCount / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && currentPosition < positionsCount - 1)
        {
            currentPosition++;

            UpdatePlayerSize();
        }
        else if (Input.GetKeyDown(KeyCode.S) && currentPosition > 0)
        {
            currentPosition--;

            UpdatePlayerSize();
        }

        player.MovePosition(new Vector2(player.position.x, positions[currentPosition]));
    }

    private void UpdatePlayerSize()
    {
        player.transform.localScale = new Vector3(Sizes[currentPosition + 1].Width, Sizes[currentPosition + 1].Height);
    }

    private void CalculateAndAddPositions()
    {
        var lineSize = CameraHeight / positionsCount;
        positions = new List<float>();

        for (var i = 0; i < positionsCount; i++)
        {
            positions.Add(i * lineSize - CameraHeight / 2 + rectTransform.localScale.y);
        }
    }

    private void CalculateAndAddPlayerSizes()
    {
        Sizes = new Dictionary<int, Size>();
        var localScale = rectTransform.localScale;
        
        for (var i = 1; i <= positionsCount; i++)
        {
            Sizes[i] = new Size((int)(localScale.x / i * 1.5), (int)(localScale.y / i * 1.5));
        }
    }
}
