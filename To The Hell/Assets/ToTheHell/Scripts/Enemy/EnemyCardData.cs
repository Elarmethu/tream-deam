using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCardData", menuName = "Data/EnemyCardDatas")]
public class EnemyCardData : ScriptableObject
{
    public int Shield;
    public int Damage;
    public int Health;
}
