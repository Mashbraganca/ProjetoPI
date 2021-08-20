using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactersData", menuName ="Smashing World/Caracters Data", order = 1)]
public class CharactersData : ScriptableObject
{
    public string characterName;

    public int characterHP;

    public int characterMP;

    public GameObject characterModel;

}
