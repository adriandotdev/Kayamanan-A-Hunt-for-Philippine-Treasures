using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CategoryHandler : MonoBehaviour
{
    public GameObject[] categories;

    private void OnEnable()
    {
        string regionName = EventSystem.current.currentSelectedGameObject.name.ToUpper();

        foreach (GameObject category in categories)
        {
            this.GetRegionCategories(regionName, category.name, category);
        }       
    }

    public void GetRegionCategories(string regionName, string categoryName, GameObject categoryButton)
    {
        foreach (RegionData regionData in DataPersistenceManager.instance.playerData.regionsData)
        {
            // regionName is already in uppercase, while regionData.regionName is not.
            if (regionData.regionName.ToUpper() == regionName)
            {
                foreach(Category category in regionData.categories)
                {
                    if (category.categoryName.ToUpper() == categoryName.ToUpper())
                    {
                        for (int i = 0; i < category.noOfStars; i++)
                        {
                            categoryButton.transform.GetChild(0).transform.GetChild(i).GetComponent<SpriteRenderer>().sprite 
                                = Resources.Load<Sprite>("UI ELEMENTS/Fill Star");
                        }
                    }
                }
            }
        }
    }
}
