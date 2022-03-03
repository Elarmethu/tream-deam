using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    public Sprite Icon;
    public int Level;
    public string Name;

    public int Shield;
    public int Health;
    public int Damage;
    public Poison Poison;
    public int Evridika;
    
    
    public int EmountMax;
}

[System.Serializable]
public struct Poison
{
    public int count;
    public int damage;
}