using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameManager gm;
    public bool isPaused = false;

    public void Awake()
    {
        pauseMenu.SetActive(isPaused);   
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.restarting) return;
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);
            
        }

    }

    public void Resume() {
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    public void Restart()
    {
        isPaused = false;
        FindObjectOfType<GameManager>().gameOver = true;
        pauseMenu.SetActive(isPaused);
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
