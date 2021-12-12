#region

using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class SceneSwitchScripts : MonoBehaviour
{
    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}