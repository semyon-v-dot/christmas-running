using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public PlayerMovement player;
    
    public void RestartButton()
    {
        player.ResetPlayer(); 
    }
}
