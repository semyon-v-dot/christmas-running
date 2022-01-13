using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLoad : MonoBehaviour
{
    public Text ScoreLabel;

    public void Start()
    {
        int MaxScore = PlayerPrefs.HasKey("MaxScore") ? PlayerPrefs.GetInt("MaxScore") : 0;
        ScoreLabel.text = MaxScore.ToString();

        int charWidth = 15;

        var newWidth = ScoreLabel.text.Length * charWidth;

        RectTransform rt = ScoreLabel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(newWidth, rt.sizeDelta.y);
        rt.localPosition = new Vector2(rt.localPosition.x - ScoreLabel.text.Length * charWidth / 2, rt.localPosition.y);
    }
}