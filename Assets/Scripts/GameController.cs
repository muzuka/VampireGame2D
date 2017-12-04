using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static bool paused = false;

    public GameObject WinMenu;
    public GameObject LoseMenu;
    public GameObject PauseMenu;

	// Use this for initialization
	void Start ()
    {
        WinMenu.SetActive(false);
        LoseMenu.SetActive(false);
        PauseMenu.SetActive(false);
	}

    void OnEnable ()
    {
        EventManager.StartListening("WinGame", winGame);
        EventManager.StartListening("LoseGame", loseGame);
    }

    void OnDisable ()
    {
        EventManager.StopListening("WinGame", winGame);
        EventManager.StopListening("LoseGame", loseGame);
    }

    void Update ()
    {
        if (Input.GetButtonDown("Pause"))
        {
            PauseMenu.SetActive(!PauseMenu.activeSelf);
            if (PauseMenu.activeSelf)
            {
                PauseMenu.GetComponentInChildren<Button>().Select();
                paused = true;
                Time.timeScale = 0f;
            }
            else
            {
                paused = false;
                Time.timeScale = 1f;
            }
        }
    }

    void winGame ()
    {
        WinMenu.SetActive(true);
        Time.timeScale = 0f;
        WinMenu.GetComponentInChildren<Button>().Select();
    }

    void loseGame ()
    {
        LoseMenu.SetActive(true);
        Time.timeScale = 0f;
        LoseMenu.GetComponentInChildren<Button>().Select();
    }

    public void resume ()
    {
        PauseMenu.SetActive(false);
        paused = false;
        Time.timeScale = 1f;
    }

    public void retry ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mainMenu ()
    {
        SceneManager.LoadScene(0);
    }

    public void quit ()
    {
        Application.Quit();
    }
}
