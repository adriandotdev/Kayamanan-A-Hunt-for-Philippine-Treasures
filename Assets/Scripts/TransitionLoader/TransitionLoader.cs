using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionLoader : MonoBehaviour
{
    public static TransitionLoader instance;
    public Animator animator;
    public float transitionSpeed = .20f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    public void StartAnimation(string sceneToLoad)
    {
        StartCoroutine(TransitionToScene(sceneToLoad));
    }

    IEnumerator TransitionToScene(string sceneToLoad)
    {
        animator.SetTrigger("Transition");

        yield return new WaitForSeconds(this.transitionSpeed);

        SceneManager.LoadScene(sceneToLoad);
    }
}
