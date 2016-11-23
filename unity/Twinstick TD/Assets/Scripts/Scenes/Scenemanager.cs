using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Class using UnityEngine.SceneManagement;
/// Contains functions to navigate through scenes
/// https://unity3d.com/learn/tutorials/topics/user-interface-ui/creating-main-menu
/// </summary>
public class Scenemanager : MonoBehaviour
{
    // Load next Scene
    public void LoadNextSceneIndex(int sceneindex)
    {
        SceneManager.LoadScene(sceneindex);
    }

    //Exit game
    public void Quit()
    {
        //Running in editor
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
