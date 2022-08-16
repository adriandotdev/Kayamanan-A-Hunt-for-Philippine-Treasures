using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI m_TimeUI;

    // Start is called before the first frame update
    void OnEnable()
    {
        TimeManager.OnHourChanged += UpdateTimeUI;
    }

    // Update is called once per frame
    void OnDisable()
    {
        TimeManager.OnHourChanged -= UpdateTimeUI;
    }

    public void UpdateTimeUI ()
    {
        string isDaytime = TimeManager.instance.playerData.playerTime.m_IsDaytime ? "AM" : "PM";

        this.m_TimeUI.text = $"{TimeManager.instance.playerData.playerTime.m_ActualHourInRealLife} { isDaytime }";
    }
}
