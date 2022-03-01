using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    public Sprite Icon;
    public string Name;
    public string Desription;

    public int Shield;
    public int Health;
    public int Damage;
    public int Evridika;
    
    public int EmountMax;
}
