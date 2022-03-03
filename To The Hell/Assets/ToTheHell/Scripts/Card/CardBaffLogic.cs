using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBaffLogic : MonoBehaviour
{
    public static CardBaffLogic Instance;

    public EnemyLogic enemyLogic;
    public GameObject chooseObject;
    public Transform fieldContent;

    public List<CardData> datas;
    public GameObject prefab;

    public void Awake()
    {
        Instance = this;
    }

    public void InitializeBaff()
    {
        chooseObject.SetActive(true);
        
        for (int i = 0; i < datas.Count; i++)
        {
            GameObject cardObj = Instantiate(prefab);
            cardObj.transform.SetParent(fieldContent);


            CardBaff cell = cardObj.GetComponentInChildren<CardBaff>();
            cell.data = datas[i];
            cell.InitializeCard();
        }
    }


    public void ChooseBaff(CardData data)
    {
        float k = 0;
        for (int i = 0; i < enemyLogic.enemiesData.Count; i++)
            k += enemyLogic.enemiesData[i].k;

        if(data.Damage > 0)
            data.Damage += Mathf.CeilToInt(k);

        if(data.Health > 0)
            data.Health +=  Mathf.CeilToInt(k);

        if (data.Shield > 0)
            data.Shield += Mathf.CeilToInt(k);

        if (data.Evridika < 0)
            data.Evridika -= Mathf.CeilToInt(k);

        if (data.Poison.damage > 0)
            data.Poison.damage  += Mathf.CeilToInt(k);

        chooseObject.SetActive(false);
        Game.Instance.NextLevel();
    }
}
