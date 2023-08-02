using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    public GameObject startingScreen;
    [SerializeField] move playerMove;
    public GameObject winScreen;

    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        playerMove.gameIsPaused = false;
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Application.Quit();
        playerMove.gameIsPaused = false;
        Time.timeScale = 1f;
    }
    
    public void Win()
    {
        StartCoroutine(GameOverWin());
    }

    IEnumerator GameOverWin()
    {
        yield return new WaitForSeconds(3);
        winScreen.SetActive(true);
        
        playerMove.gameIsPaused = true;
        Time.timeScale = 0f;
    }
} 
