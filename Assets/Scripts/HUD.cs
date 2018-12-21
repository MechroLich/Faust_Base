using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//all my own code, but I am confident it could have been done better
public class HUD : MonoBehaviour {

    public GameObject win;
    public GameObject gameover;
    public GameObject goverscreen;
    public GameObject winscreen;

    public Slider Healthbar;
    public Slider Distortionbar;

    public static int kills = 0;
    public int maxhealth;
    public static int curhealth;
    public int maxdistortion;
    public int curdistortion;
    public Text cash;
    public static int cashval = 0;

    // Use this for initialization
    void Start() {

        gameover.SetActive(false);
        win.SetActive(false);

        cashval = 0;
        maxhealth = 500;
        curhealth = maxhealth;
        maxdistortion = 100;
        curdistortion = maxdistortion;
        cash.text = "Cash Money " + cashval;
    }

    // Update is called once per frame
    void Update() {
        Maintenance();
        Healthbar.value = curhealth;
        Distortionbar.value = curdistortion;
        cash.text = "Cash Money " + cashval;
    }

    void Maintenance()
    {
        if (curhealth > maxhealth)
        {
            curhealth = maxhealth;
        }
        else if (curhealth <= 0)
        {
            if (gameover.activeSelf)
            {
                
            }
            else
            {
                Gameover();
            }
        }
        if (curdistortion > maxdistortion)
        {
            curdistortion = maxdistortion;
        }
        else if (curdistortion <= 0)
        {
            curdistortion = 0;
        }
        if (cashval>=5)
        {
            if (win.activeSelf)
            {

            }
            else
            {
                Win();
            }
        }
    }

    private void Win()
    {

        win.SetActive(true);
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(winscreen);
    }
    private void Gameover()
    {
        gameover.SetActive(true);
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(goverscreen);
    }
    public void Continue()
    {
        //set up to load next scene, current is placeholder
        win.SetActive(false);
        gameover.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Retry()
    {
        win.SetActive(false);
        gameover.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        SceneManager.LoadScene("MenuScreen");
    }

    public void TakeDamage(int damage)
    {
        curhealth = curhealth - damage;
    }
}
