using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSetup : MonoBehaviour
{
    public enum CategoryType { NATIONAL_HEROES, NATIONAL_SYMBOLS, PHILIPPINE_MYTHS, NATIONAL_FESTIVALS, NATIONAL_GAMES }
    [SerializeField] public string sceneToLoad;
    [SerializeField] public string previousSceneToLoad;
    [SerializeField] public CategoryType categoryName = CategoryType.NATIONAL_HEROES;
    [SerializeField] public Word[] words;
}
