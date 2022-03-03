using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    public Sprite Icon;
    public int Level;
    public string Name;

    public int s_Shield;
    public int Shield;

    public int s_Health;
    public int Health;

    public int s_Damage;
    public int Damage;

    public Poison s_Poison;
    public Poison Poison;

    public int s_Evridika;
    public int Evridika;

    public int EmountMax;
}

[System.Serializable]
public struct Poison
{
    public int count;
    public int damage;
}