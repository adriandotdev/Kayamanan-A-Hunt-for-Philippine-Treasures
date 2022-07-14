using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * <summary> 
 *  This class is responsible for spawning the player based
 *  on the last or saved position.
 * </summary> 
 */
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
        }
        else
        {
            Destroy(gameObject);
        }
        //Destroy(gameObject);
    }

    private void OnEnable()
    {
        print("SCENE TRANSITION: Enabled");
        SceneManager.sceneLoaded += OnOutsideSceneLoaded;
        SceneManager.sceneLoaded += OnPlayerHouseLoaded;
    }

    private void OnDisable()
    {
        print("SCENE TRANSITION: Disabled");
        SceneManager.sceneLoaded -= OnOutsideSceneLoaded;
        SceneManager.sceneLoaded -= OnPlayerHouseLoaded;
    }

    public void OnPlayerHouseLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "House")
        {
            this.nameOfExit = "Building"; // The name of gameobject that is the referenced player's position.

            print("CURRENT VALUE OF FROM ENTER HOUSE: " + fromEnter);
            Vector2 position;

            /**
             * <summary>
             *  Una, ichecheck muna kung ang ni-load na profile ay bagong created o hindi.
             *  
             *  Sa else block naman, 
             *  iche-check naman kung galing sa loob o labas ng bahay ang player
             *  kung galing siya sa loob, maseset siya sa entrance ng bahay based
             *  sa given value sa variable na 'nameOfExit'. 
             *  
             *  If hindi naman siya galing sa loob o labas ng bahay,
             *  iseset lang siya sa last saved position ng player.
             *  
             *  NOTE:
             *  ang property na 'fromEnter' ay sineset ng 
             *  SceneTransition script. 
             *  
             *  TIGNAN NA LANG ANG SceneTransition script.
             * </summary>
             */
            if (DataPersistenceManager.instance.playerData.isNewlyCreated)
            {
                DataPersistenceManager.instance.playerData.isNewlyCreated = false;
                position = GameObject.Find(this.nameOfExit).transform.GetChild(0).position;
            }
            else
            {
                if (fromEnter)
                {
                    fromEnter = false;
                    position = GameObject.Find(this.nameOfExit).transform.GetChild(0).position;
                }
                else
                {
                    position = new Vector2(DataPersistenceManager.instance.playerData.xPos, DataPersistenceManager.instance.playerData.yPos);
                }
                    
            }

            this.SpawnPlayerCharacter(position);
        }
    }

    public void OnOutsideSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Outside")
        {
            print("CURRENT VALUE OF FROM ENTER OUTSIDE: " + fromEnter);
            Vector2 position;

            /**
             * <summary>
             *  Since ang default scene pag nag create ng new profile is ang 'House' scene,
             *  direct na natin ichecheck if ang outside scene ay nag load kung from entrance 
             *  ba sa loob o labas galing si player.
             *  
             *  Also the player's position sa 'Outside' scene is nasasaved din if ever na hindi
             *  siya galing sa loob ng bahay.
             * </summary>
             */
            if (fromEnter)
            {
                fromEnter = false;
                position = GameObject.Find(this.nameOfExit).transform.GetChild(0).position;
            }
            else
            {
                position = new Vector2(DataPersistenceManager.instance.playerData.xPos, DataPersistenceManager.instance.playerData.yPos);
            }
            
            this.SpawnPlayerCharacter(position);
        }
    }


    private void SpawnPlayerCharacter(Vector2 position)
    {
        GameObject player = null;

        if (DataPersistenceManager.instance.playerData.gender == "male")
        {
            player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        }
        else
        {
            player = Instantiate(Resources.Load<GameObject>("Prefabs/Female"));
        }

        player.transform.position = new Vector2(position.x, position.y);
    }
}
