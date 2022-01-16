using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusucButton : MonoBehaviour
{
    public AudioSource audioSource;
   
    public Image soundBotton;
    public Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        //image = GetComponent<Image>();
        if (PlayerPrefs.HasKey("MusicToggle"))
        {
            if(PlayerPrefs.GetString("MusicToggle") == "on")
            {
                soundBotton.GetComponent<Toggle>().isOn = true;
                ToggleMusic(true);
            }
            else
            { 
                soundBotton.GetComponent<Toggle>().isOn = false;
                ToggleMusic(false);
            }
        }
    }
    
    public void ToggleMusic(bool enabled)
    {
        if (enabled)
        {
            audioSource.Play();
            PlayerPrefs.SetString("MusicToggle", "on");
            PlayerPrefs.Save();
            soundBotton.GetComponent<Image>().sprite = sprites[0];
        }
        else
        {
            audioSource.Stop();
            PlayerPrefs.SetString("MusicToggle", "of");
            PlayerPrefs.Save();
            soundBotton.GetComponent<Image>().sprite = sprites[1];
        }
    }
}
