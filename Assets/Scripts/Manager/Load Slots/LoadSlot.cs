using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadSlot : MonoBehaviour
{
    [SerializeField] private GameObject content; // Container of all the slots.
    [SerializeField] private GameObject saveSlot; // Save slot to render or to put inside of 'content'.

    private void OnEnable()
    {
        Slots slots = DataPersistenceManager.instance.GetAllSlots();
        PlayerDataHandler playerDataHandler = null;

        for (int i = 0; i < slots.ids.Count; i++)
        {
            GameObject gameObject = Instantiate(saveSlot, content.transform);

            playerDataHandler = new PlayerDataHandler(slots.ids[i]);

            PlayerData playerData = playerDataHandler.Load();

            gameObject.GetComponent<Button>().onClick.AddListener(() => 
            {
                DataPersistenceManager.instance.playerData = playerData;
            });

            gameObject.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = playerData.name;
            gameObject.AddComponent<SlotScript>();
        }

        //if (content.transform.childCount > 0)
        //{
        //    content.transform.GetChild(0).GetComponent<Button>().Select();

        //    playerDataHandler = new PlayerDataHandler(slots.ids[0]);

        //    PlayerData playerData = playerDataHandler.Load();

        //    DataPersistenceManager.instance.playerData = playerData;
        //}
            
         //gameObject.GetComponent<Button>().Select();
    }  
}

public class SlotScript : MonoBehaviour
{
    private void OnDisable()
    {
        Destroy(gameObject);
    }
}