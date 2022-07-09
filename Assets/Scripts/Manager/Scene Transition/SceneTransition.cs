using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public string nameOfExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
        
            // Check if not null 
            if (SceneTransitionManager.instance != null)
            {
                SceneTransitionManager.instance.nameOfExit = this.nameOfExit;
                SceneTransitionManager.instance.fromEnter = true;
            }
            SceneManager.LoadScene(this.sceneToLoad);
        }
    }
}
