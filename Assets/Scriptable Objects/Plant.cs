using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Plant")]
public class Plant : ScriptableObject
{
    public string cropName;
    public Sprite[] plantStages; // sprites
    public float timePerStage;
}
