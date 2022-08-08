using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadHouseScene : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("House");        
    }
}
