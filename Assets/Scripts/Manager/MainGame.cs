using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    public PlayerData playerData;
    public string regionName;
    public string categoryName;
    public System.Object[] shuffled;
    public List<bool> correctAnswers;
    /**
     * <summary>
     *  Ang function na ito ay i-seset niya ang score ng category
     *  based sa current region.
     * </summary>
     */
    public void SetRegionCategoriesScores(int noOfCorrectAnswers)
    {
        // Get all the regions.
        foreach (RegionData regionData in playerData.regionsData)
        {
            if (regionData.regionName == this.regionName.ToUpper())
            {
                foreach (Category category in regionData.categories)
                {
                    if (category.categoryName == this.categoryName)
                    {
                        if (noOfCorrectAnswers > category.highestScore)
                        {
                            category.highestScore = noOfCorrectAnswers;
                        }
                    }
                }
            }
        }
    }

    private int CountNoOfStarsToShow(int noOfCorrectAnswers)
    {
        int passingScore = this.shuffled.Length / 2 + 1;

        if (noOfCorrectAnswers == this.shuffled.Length)
        {
            return 3;
        }
        else if (noOfCorrectAnswers >= passingScore)
        {
            return 2;
        }

        return 1;
    }


    public void ShowStars(int noOfCorrectAnswers)
    {
        if (noOfCorrectAnswers == 0)
        {
            return;
        }

        int noOfStars = this.CountNoOfStarsToShow(noOfCorrectAnswers);

        foreach (RegionData regionData in this.playerData.regionsData)
        {
            if (regionData.regionName.ToUpper() == this.regionName.ToUpper())
            {
                foreach (Category category in regionData.categories)
                {
                    if (category.categoryName.ToUpper() == this.categoryName.ToUpper())
                    {
                        if (category.noOfStars < noOfStars)
                            category.noOfStars = noOfStars;
                    }
                }
            }
        }
    }

    public void CheckIfNextRegionIsReadyToOpen()
    {
        int regionNumber = 0;

        foreach (RegionData regionData in this.playerData.regionsData)
        {
            if (regionData.regionName.ToUpper() == this.regionName.ToUpper())
            {
                regionNumber = regionData.regionNumber;

                foreach (Category category in regionData.categories)
                {
                    if (category.noOfStars < 2)
                    {
                        return;
                    }
                }
            }
        }

        if (regionNumber < this.playerData.regionsData.Count)
        {
            print("TEST : REGION IS OPEN: " + (regionNumber + 1));
            this.playerData.regionsData[regionNumber].isOpen = true;
        }
    }

    public bool AllCategoriesCompleted()
    {
        foreach (RegionData regionData in this.playerData.regionsData)
        {
            if (regionData.regionName.ToUpper() == this.regionName.ToUpper())
            {
                foreach (Category category in regionData.categories)
                {
                    if (category.noOfStars < 3)
                        return false;
                }
            }
        }
        return true;
    }

    public bool CheckIfRegionCollectiblesIsCollected()
    {
        foreach (Collectible collectible in playerData.notebook.collectibles)
        {
            if (collectible.regionName.ToUpper() == this.regionName.ToUpper())
            {
                if (!collectible.isCollected)
                    return false;
            }
        }
        return true;
    }

    public void CollectAllRewards()
    {
        if (AllCategoriesCompleted() != true)
            return;

        if (!CheckIfRegionCollectiblesIsCollected())
            SoundManager.instance.PlaySound("Unlock Item");

        foreach (Collectible collectible in playerData.notebook.collectibles)
        {
            if (collectible.regionName.ToUpper() == this.regionName.ToUpper())
            {
                if (collectible.isCollected)
                    return;

                collectible.isCollected = true;
            }
        }
        SceneManager.LoadSceneAsync("Collectibles", LoadSceneMode.Additive);

    }

    public int CountCorrectAnswers()
    {
        int count = 0;

        foreach (bool isCorrect in this.correctAnswers)
        {
            if (isCorrect) count++;
        }

        return count;
    }
}
