using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    public PlayerData playerData;
    public string regionName;
    public string previousSceneToLoad;
    public string categoryName;
    public System.Object[] shuffled;
    public List<bool> correctAnswers;

    /**
     * <summary>
     *  Ang function na ito ay i-seset niya ang score ng category
     *  based sa current region.
     * </summary>
     */
    public void SetRegionHighestScore(int noOfCorrectAnswers)
    {
        // Get all the regions.
        foreach (RegionData regionData in playerData.regionsData)
        {
            if (regionData.regionName.ToUpper() == this.regionName.ToUpper())
            {
                if (noOfCorrectAnswers > regionData.highestScore)
                    regionData.highestScore = noOfCorrectAnswers;
            }
        }
    }

    /** <summary>
     *  Ang function na ito ang magbibilang kung ilan ang stars na 
     *  ipapakita sa score panel scene.
     * </summary> */
    public int CountNoOfStarsToShow(int noOfCorrectAnswers)
    {
        int passingScore = this.shuffled.Length / 2 + 1;

        if (noOfCorrectAnswers != this.shuffled.Length)
        {
            print("COMPLETED QUEST RESET");

            if (QuestManager.instance != null)
                QuestManager.instance.ResetAllCompletedQuests(this.regionName);
            else
                print("Quest Manager is NULL");
        }

        if (noOfCorrectAnswers == this.shuffled.Length)
        {
            return 3;
        }
        else if (noOfCorrectAnswers >= passingScore)
        {
            return 2;
        }
        else if (noOfCorrectAnswers >= 1)
            return 1;

        return 0;
    }

    //public void ShowStars(int noOfCorrectAnswers)
    //{
    //    if (noOfCorrectAnswers == 0)
    //    {
    //        return;
    //    }

    //    int noOfStars = this.CountNoOfStarsToShow(noOfCorrectAnswers);

    //    foreach (RegionData regionData in this.playerData.regionsData)
    //    {
    //        if (regionData.regionName.ToUpper() == this.regionName.ToUpper())
    //        {
    //            foreach (Category category in regionData.categories)
    //            {
    //                if (category.categoryName.ToUpper() == this.categoryName.ToUpper())
    //                {
    //                    if (category.noOfStars < noOfStars)
    //                        category.noOfStars = noOfStars;
    //                }
    //            }
    //        }
    //    }
    //}

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
                regionData.noOfStars = noOfStars;
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

                if (regionData.noOfStars != 3)
                {
                    return;
                }
            }
        }

        if (regionNumber < this.playerData.regionsData.Count)
        {
            //print("TEST : REGION IS OPEN: " + (regionNumber + 1));
            this.playerData.regionsData[regionNumber].isOpen = true;
        }
    }

    //public void CheckIfNextRegionIsReadyToOpen()
    //{
    //    int regionNumber = 0;

    //    foreach (RegionData regionData in this.playerData.regionsData)
    //    {
    //        if (regionData.regionName.ToUpper() == this.regionName.ToUpper())
    //        {
    //            regionNumber = regionData.regionNumber;

    //            foreach (Category category in regionData.categories)
    //            {
    //                if (category.noOfStars < 2)
    //                {
    //                    return;
    //                }
    //            }
    //        }
    //    }

    //    if (regionNumber < this.playerData.regionsData.Count)
    //    {
    //        print("TEST : REGION IS OPEN: " + (regionNumber + 1));
    //        this.playerData.regionsData[regionNumber].isOpen = true;
    //    }
    //}

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
        foreach (RegionData rd in this.playerData.regionsData)
        {
            if (rd.regionName.ToUpper() == this.regionName.ToUpper())
            {
                if (rd.noOfStars != 3) return;
            }
        }

        //if (!CheckIfRegionCollectiblesIsCollected())
        //    SoundManager.instance.PlaySound("Unlock Item");

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

    //public void CollectAllRewards()
    //{
    //    if (AllCategoriesCompleted() != true)
    //        return;

    //    if (!CheckIfRegionCollectiblesIsCollected())
    //        SoundManager.instance.PlaySound("Unlock Item");

    //    foreach (Collectible collectible in playerData.notebook.collectibles)
    //    {
    //        if (collectible.regionName.ToUpper() == this.regionName.ToUpper())
    //        {
    //            if (collectible.isCollected)
    //                return;

    //            collectible.isCollected = true;
    //        }
    //    }
    //    SceneManager.LoadSceneAsync("Collectibles", LoadSceneMode.Additive);

    //}

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
