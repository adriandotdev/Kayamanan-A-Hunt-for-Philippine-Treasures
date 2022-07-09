using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;
    public string nameOfExit;
    public bool fromEnter = false;

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
            this.nameOfExit = "Building";

            Vector2 position;

            if (DataPersistenceManager.instance.playerData.isNewlyCreated)
            {
                DataPersistenceManager.instance.playerData.isNewlyCreated = false;
                position = GameObject.Find(this.nameOfExit).transform.GetChild(0).position;
            }
            else
            {
                if (fromEnter)
                {
                    position = GameObject.Find(this.nameOfExit).transform.GetChild(0).position;
                    fromEnter = false;
                }
                else
                    position = new Vector2(DataPersistenceManager.instance.playerData.xPos, DataPersistenceManager.instance.playerData.yPos);
            }

            GameObject player = null;

            if (DataPersistenceManager.instance.playerData.gender == "male")
            {
                player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            }
            else
            {
                player = Instantiate(Resources.Load<GameObject>("Prefabs/Female"));
            }
            player.transform.position = new Vector2(position.x, position.y - .5f);
        }
    }

    public void OnOutsideSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside"))
        {
            Vector2 position;

            if (fromEnter)
            {
                position = GameObject.Find(this.nameOfExit).transform.GetChild(0).position;
                fromEnter = false;
            }
            else
                position = new Vector2(DataPersistenceManager.instance.playerData.xPos, DataPersistenceManager.instance.playerData.yPos);

            GameObject player = null;

            if (DataPersistenceManager.instance.playerData.gender == "male")
            {
                print("MALE FROM SceneTransitionManager");
                player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            }
            else
            {
                print("FEMALE FROM SceneTransitionManager");
                player = Instantiate(Resources.Load<GameObject>("Prefabs/Female"));
            }
            player.transform.position = new Vector2(position.x, position.y - .5f);
        }
    }
}
