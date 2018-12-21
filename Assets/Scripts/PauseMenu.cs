using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
//my own work, intended to be expanded on when further developing the project in my own time.
public class PauseMenu : MonoBehaviour {

    public static bool isPaused;
    public GameObject pausemenu;
    public GameObject skills;

    // Use this for initialization
    void Start () {
        Resume();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("pause"))
            {
            if (isPaused)
                {
                    Resume();
                BasicController.PlayerRotLock = false;
            }
            else
                {
                    Pause();
                BasicController.PlayerRotLock = true;
                }
            }
    }

    public void Resume()
    {
        pausemenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pausemenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        EventSystem.current.SetSelectedGameObject(skills);
    }
    public void Exit()
    {
        SceneManager.LoadScene("MenuScreen");
    }
}
