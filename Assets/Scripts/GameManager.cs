using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isPaused = false;
    public PhysControllerFP playerController;
    public GameObject player;
    public GameObject playerCamera;
    public GameObject enemy;

    public GameObject objectiveScreen;
    public static GameManager instance;
    public List<GameObject> spawnPoints = new List<GameObject>();
    

    void Awake()
    {
        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        playerController.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (isPaused == true)
        {
            PauseGame();
        }

        if (isPaused == false)
        {
            ResumeGame();
        }

    }

    void PauseGame()
    {
        playerController.enabled = false;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void ResumeGame()
    {
        playerController.enabled = true;
        Time.timeScale = 1f;
        Cursor.visible = false;

    }

    public void GameOver()
    {
        
        //need to hide the available hud
        DisplayGameOver();
        isPaused = true;
    }

    public void DisplayGameOver()
    {
       // player.GetComponent<DamageOverlay>().TurnOn();
    }

    public void Respawn()
    {
        Debug.Log("Hello");
        //teleport ai back to start location
        enemy.transform.position = spawnPoints[1].transform.position;

        //move player to respawn point
        player.transform.position = spawnPoints[0].transform.position;
        
        //Reset
        
        Reset();
    }

    public void Win()
    {
        isPaused = true;
        Debug.Log("Win");
        SceneManager.LoadSceneAsync("Escaped");
    }

    public void ReturnToMenu()
    {

    }

    public void ToggleObjective()
    {
        if (objectiveScreen.activeSelf)
        {
            objectiveScreen.SetActive(false);
        }
        else
        {
            objectiveScreen.SetActive(true);
        }
    }

    private void Reset()
    {
        isPaused = false;

        //Reset AI
        //if (enemy)
        //{
        //    enemy.GetComponent<EnemyAI>().ResetAI();
        //}
        

        //Reset Dmg Overlay
        //if(player)
        //{
        //    player.GetComponent<DamageOverlay>().Reset();
        //}
        
    }
}
