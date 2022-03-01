using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player")]
public class PlayerData : ScriptableObject
{
    /// <summary>
    /// ������, �������� ��� ���������� � ��������� ���������.
    /// ��� �������� �������� � ���� (Helth, Mana), � ����� ���������� �����.
    /// </summary>


    public int HealthMax;
    public int EvridikaMaxHealth;

    public int Health;
    public int EvridikaHealth;

    public int Shield;

}
