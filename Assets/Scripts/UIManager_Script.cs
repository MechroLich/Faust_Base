using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Script : MonoBehaviour {
    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
