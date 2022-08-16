using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;

    public static int ActualHourInRealLife { get; private set; }
    public static float NoOfSecondsPerTwoAndHalfMinutes { get; private set; }

    public static bool IsDaytime { get; private set; }

    public bool IsAllEstablishmentsOpen;

    public PlayerData playerData;

    public TilemapRenderer test;

    public Gradient color;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        /** 
         <summary>
            24 hrs in real life. In this game, 1 hour is equivalent to 1 day.
            So 30 mins for daytime and 30 mins for night.

            The 30 mins is divided by 12 so equals ito sa 2.5 mins for kada oras in real time.

            All Establishments are open 8AM to 5PM
            Mystery Doors are open 5PM to 8PM
        </summary>
         */

        PlayerDataHandler playerDataHandler = new PlayerDataHandler("Time");

        try
        {
            playerData = playerDataHandler.Load();
        }
        catch (System.Exception e) { }
        // All code above is for testing purposes only.

        if (playerData == null)
        {
            playerDataHandler.Save(new PlayerData());
            playerData = playerDataHandler.Load();
            print("TIME DATA IS NULL");
        }
        else
        {
            print("TIME DATA IS NOT NULL");
        }

        ActualHourInRealLife = playerData.playerTime.m_ActualHourInRealLife;
        NoOfSecondsPerTwoAndHalfMinutes = .5f; // MUST BE 150
        IsDaytime = playerData.playerTime.m_IsDaytime;
        IsAllEstablishmentsOpen = playerData.playerTime.m_IsAllEstablishmentsOpen;
        OnHourChanged?.Invoke();
    }

    void Update()
    {
        NoOfSecondsPerTwoAndHalfMinutes -= Time.deltaTime;

        if (NoOfSecondsPerTwoAndHalfMinutes <= 0)
        {
            NoOfSecondsPerTwoAndHalfMinutes = .5f;
            this.playerData.playerTime.m_ActualHourInRealLife++;

            if (this.playerData.playerTime.m_ActualHourInRealLife == 24)
            {
                this.playerData.playerTime.m_IsDaytime = true;
            }
            else if (this.playerData.playerTime.m_ActualHourInRealLife >= 13) // for testing only, it must be 12
            {
                this.playerData.playerTime.m_IsDaytime = false;
            }
            else
            {
                this.playerData.playerTime.m_IsDaytime = true;
            }

            OnHourChanged?.Invoke();

            InterpolateColor(this.playerData.playerTime.m_ActualHourInRealLife);

            if (this.playerData.playerTime.m_ActualHourInRealLife >= 24)
            {
                this.playerData.playerTime.m_ActualHourInRealLife = 0;
            }

            if (this.playerData.playerTime.m_ActualHourInRealLife >= 5 && this.playerData.playerTime.m_IsDaytime == false)
            {
                print("Establishments All Closed");
            }

            if (this.playerData.playerTime.m_ActualHourInRealLife >= 8 && this.playerData.playerTime.m_IsDaytime == true)
            {
                print("Establishments All Open");
            }
        }
    }

    void InterpolateColor(int hourTime)
    {
        /** <summary>
         *  7 pm to 4am -> Color black
         *  5 am to 5pm -> Color
         *  5pm to 7pm -> Color
         * </summary> */
        Color haponColor;
        ColorUtility.TryParseHtmlString("#D2B082", out haponColor);

        Color madalingArawColor;
        ColorUtility.TryParseHtmlString("#828282", out madalingArawColor);

        if (hourTime <= 5)
        {
            test.GetComponent<Tilemap>().color = Color.Lerp(madalingArawColor, Color.white, (hourTime / 24f));
        }

        else if (hourTime >= 5 && hourTime <= 17)
        {
            test.GetComponent<Tilemap>().color = Color.Lerp(Color.white, haponColor, (hourTime / 24f));
        }
    }
}
