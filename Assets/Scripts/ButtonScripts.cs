#region

using UnityEngine;

#endregion

public class NewBehaviourScript : MonoBehaviour
{
    public PlayerMovement player;

    public void RestartButton()
    {
        player.ResetLevel();
    }
}