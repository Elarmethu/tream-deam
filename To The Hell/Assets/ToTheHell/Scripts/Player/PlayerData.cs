using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player")]
public class PlayerData : ScriptableObject
{
    /// <summary>
    /// Объект, хранящий всю информацию о состояние персонажа.
    /// Его нынешнее здоровье и ману (Helth, Mana), а также количество брони.
    /// </summary>


    public int HealthMax;
    public int EvridikaMaxHealth;

    public int Health;
    public int EvridikaHealth;

    public int Shield;

}
