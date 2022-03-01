using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesData", menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{    
    public int Health;
    public int ShieldMax;

    public EnemyCardData MotionData;

    public Sprite Model;
}
