using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [FormerlySerializedAs("PositionsCount")] public int positionsCount = 3;
    public Rigidbody2D Player;
    
    private const int CameraHeight = 1080;
    private List<float> positions;
    private int currentPosition;
    private readonly Dictionary<KeyCode, int> controls = new Dictionary<KeyCode, int>();
    private RectTransform rectTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!(Camera.main is { }))
        {
            throw new Exception("Main camera is null");
        }

        Player = GetComponent<Rigidbody2D>();

        SetControls();
        
        rectTransform = GetComponent<RectTransform>();
        
        CalculateAndAddPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (var key in controls.Keys.Where(Input.GetKeyDown))
            {
                if (controls[key] > 0 && currentPosition < positionsCount - 1 
                    || controls[key] < 0 && currentPosition > 0)
                {
                    currentPosition += controls[key];
                }
            }
        }

        Player.MovePosition(new Vector2(Player.position.x, positions[currentPosition]));
    }
    
    private void SetControls()
    {
        switch (Player.name)
        {
            case "Player2":
                controls[KeyCode.DownArrow] = -1;
                controls[KeyCode.UpArrow] = 1;
                break;
            case "Player1":
                controls[KeyCode.S] = -1;
                controls[KeyCode.W] = 1;
                break;
        }
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
}
