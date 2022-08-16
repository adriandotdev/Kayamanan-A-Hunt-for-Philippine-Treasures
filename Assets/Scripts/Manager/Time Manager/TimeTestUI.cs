using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTestUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerDataHandler playerDataHandler = new PlayerDataHandler("Time Test");

        playerDataHandler.Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
