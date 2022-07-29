using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    // Inventory
    public RectTransform inventoryPanel;

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
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnPlaySceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnPlaySceneLoaded;
    }

    void OnPlaySceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside")
            && DataPersistenceManager.instance.playerData.isTutorialDone)
        {
            this.inventoryPanel = GameObject.Find("Inventory Panel").GetComponent<RectTransform>();
            this.DisplayInventoryItems();
        }
    }

    public void DisplayInventoryItems()
    {
        List<Item> inventory = DataPersistenceManager.instance.playerData.inventory.items;

        for (int i = 0; i < inventory.Count; i++)
        {
            // first child is the image
            // second child is the quantity text
            Transform slot = this.inventoryPanel.GetChild(i);

            slot.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Collectibles/Items/" + inventory[i].itemName);
            slot.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = inventory[i].quantity.ToString();
        }
    }
}
