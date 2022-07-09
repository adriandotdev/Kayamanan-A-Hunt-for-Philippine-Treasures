using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInfoManager : MonoBehaviour, IDataPersistence
{
    public static PlayerInfoManager instance;
    public PlayerData playerData;

    [Header("UI")] 
    [SerializeField] private TMPro.TMP_InputField inputField; // Textfield from Character Creation scene.
    [SerializeField] private Button button;
 
    public void LoadPlayerData(PlayerData playerData)
    {
        Debug.Log("LOADED PLAYER DATA: PLAYER INFO MANAGER");
        this.playerData = playerData;
    }

    public void SavePlayerData()
    {
        Debug.Log("SAVED PLAYER DATA: PLAYER INFO MANAGER");
        this.playerData.name = this.inputField.text;
    }

    public void LoadSlotsData(Slots slots)
    {
        throw new System.NotImplementedException();
    }

    public void SaveSlotsData(ref Slots slots)
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        instance = this;
    }
}
