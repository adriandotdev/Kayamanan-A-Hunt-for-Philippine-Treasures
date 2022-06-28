using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;
    public string nameOfExit;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnOutsideSceneLoaded;
        SceneManager.sceneLoaded += OnPlayerHouseLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnOutsideSceneLoaded;
        SceneManager.sceneLoaded -= OnPlayerHouseLoaded;
    }

    public void OnPlayerHouseLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House"))
        {
            print("FROM SCENE TRANSITION MANAGER : PLAYER HOUSE LOADED");
            this.nameOfExit = "Building";

            Vector2 position = GameObject.Find(this.nameOfExit).transform.GetChild(0).position;

            GameObject player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            player.transform.position = new Vector2(position.x, position.y - .5f);
        }
    }

    public void OnOutsideSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside"))
        {
            // Get the 
            Vector2 position = GameObject.Find(this.nameOfExit).transform.GetChild(0).position;

            GameObject player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            player.transform.position = new Vector2(position.x, position.y - .5f);
            player.GetComponent<SpriteRenderer>().sprite = player.GetComponent<Character>().up;
        }
    }
}
