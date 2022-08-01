using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FisherYatesTest : MonoBehaviour
{
    [SerializeField] char[] characters;

    // Start is called before the first frame update
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //characters = new char[6] { 'a', 'b', 'c', 'd', 'e', 'f' };
            //Shuffle(characters);
            //print();
            Test();
        }
    }

    public void Test()
    {
        List<string> names = new List<string>();

        names.Add("Isabela");
        names.Add("Koron");
        names.Add("Heloya");

        Print(names);
        names.Insert(0, names[1]);
        Print(names);
    }

    public void Print(List<string> l)
    {
        foreach(string name in l)
        {
            print(name);
        }
        print("DONE");
    }
    //public char[] Shuffle(char[] toShuffle)
    //{
    //    char[] newArray = toShuffle;
    //    System.Random random = new System.Random();

    //    for (int i = newArray.Length - 1; i > 0; i--)
    //    {
    //        int randomNum = random.Next(newArray.Length - 1);

    //        char temp = newArray[i];
    //        newArray[i] = newArray[randomNum];
    //        newArray[randomNum] = temp;
    //    }

    //    return newArray;
    //}

    //public char[] Shuffle2(char[] toShuffle)
    //{
    //    char[] newArray = toShuffle;

    //    System.Random random = new System.Random();

    //    for (int i = 0; i < newArray.Length; i++)
    //    {
    //        int randomNum = random.Next(i, newArray.Length);

    //        char temp = newArray[i];
    //        newArray[i] = newArray[randomNum];
    //        newArray[randomNum] = temp;
    //    }
    //    return newArray;
    //}

    //public void print()
    //{
    //    string charactersToPrint = "";

    //    foreach (char c in characters)
    //    {
    //        charactersToPrint += c + " ";
    //    }

    //    print(charactersToPrint);
    //}
}
