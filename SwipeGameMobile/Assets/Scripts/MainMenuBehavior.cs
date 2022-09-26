using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    ///<summary>
    ///Will load a new scene upon bieng called
    ///</summary>
    ///<param name="levelName">The name of the level we want to go </param>
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
