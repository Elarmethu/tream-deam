using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLogic : MonoBehaviour
{   
    [Header("Prefabs")]
    public PlayerLogic Player;
    public Game game;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject reviewPrefab;
    [SerializeField] private GameObject additionalPerfab;
    [SerializeField] private List<CardData> cardDatas;
    [SerializeField] private List<CardData> additionalDatas;
    
    [SerializeField] private List<CardData> containData;
    [SerializeField] private List<CardData> initializeData;

    [Header("Card Info")]
    [SerializeField] private List<GameObject> initializeCard;
    [SerializeField] private List<GameObject> additionalInitializeCard;
    public List<CardCell> cardCells;

    [Header("Transform")]
    [SerializeField] private Transform content;
    [SerializeField] private Transform additionalContent;
    public GameObject additionalObject;

    [Header("ViewSystem")]
    [SerializeField] private List<GameObject> useCardObj;
    [SerializeField] private List<CardData> useCardData;
    [SerializeField] private Transform reviewTransformField;
    public CardData attackCard;
    public List<Enemy> ChoosedEnemy;


    [Header("Logic")]
    public bool AttackEnemy;
    public int PlayerDamage;
    public bool cardChoosed;
    public bool isChoosed;

    #region card
    public void InitializeDatas()
    {
        containData.Clear();

        foreach(var data in cardDatas)
        {
            for(int i = 0; i < data.EmountMax; i++)
                containData.Add(data);
        }
    }

    private void UpdateCardCells()
    {
        cardCells.Clear();
        
        for(int i = 0; i < initializeCard.Count; i++) {
            cardCells.Add(initializeCard[i].GetComponentInChildren<CardCell>());
        }
    }

    public void InitializeCard()
    {
        GameObject cardObj = Instantiate(cardPrefab);
        cardObj.transform.SetParent(content.transform);

        RectTransform cardRect = cardObj.GetComponent<RectTransform>();
        cardRect.localScale = new Vector3(1, 1, 1);
        cardRect.localPosition = new Vector3(cardRect.transform.position.x, cardRect.transform.position.y, 0.0f);

        if(containData.Count < 5)
        {
            InitializeDatas();
        }

        int rnd = Random.Range(0, containData.Count);
        CardCell cell = cardObj.GetComponentInChildren<CardCell>();
        cell.data = containData[rnd];
        cell.InitializeCard();
        
        initializeCard.Add(cardObj);
        UpdateCardCells();

        if (initializeCard.Count % 2 == 0) RotationCard(true);
        else RotationCard(false);

        initializeData.Add(containData[rnd]);
        containData.Remove(containData[rnd]);
    }

    public void DestroyCard(GameObject card)
    {
        initializeCard.Remove(card);
        
        if (initializeCard.Count % 2 == 0) RotationCard(true);
        else RotationCard(false);

        Destroy(card);
    }

    private void RotationCard(bool even)
    {
        for (int i = 0; i < initializeCard.Count; i++)
        {
            int angle = (initializeCard.Count / 2 * (10)) - (i * (10));
            if ((angle == 0 || i >= initializeCard.Count / 2) && even) angle -= 10;

            initializeCard[i].transform.rotation = Quaternion.Euler(initializeCard[i].transform.eulerAngles.x, initializeCard[i].transform.eulerAngles.y, angle);
        }
    }

    public void CardUse(CardCell card)
    {
        if (!AttackEnemy)
        {
            useCardData.Add(card.data);

            var obj = Instantiate(reviewPrefab);
            obj.transform.SetParent(reviewTransformField.transform);

            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.localPosition = new Vector3(rect.transform.position.x, rect.transform.position.y, 0.0f);

            CardView view = obj.GetComponentInChildren<CardView>();
            view.data = card.data;
            view.InitializeCard();

            initializeData.Remove(card.data);
            DestroyCard(card.transform.parent.gameObject);

            if (card.data.Damage > 0)
            {
                AttackEnemy = true;
                attackCard = card.data;
                PlayerDamage += card.data.Damage;
            }

            useCardObj.Add(obj);
        }
    }

    public void CardTransfer(bool need)
    {
        if(initializeData.Count > 0)
        {
            if(need)
                containData.AddRange(initializeData);

            while(initializeCard.Count > 0)
            {
                DestroyCard(initializeCard[0]);
            }
            initializeData.Clear();
        }
    }
    #endregion

    #region ComboSystem
    public void ClearTurn()
    {

        foreach (var card in useCardObj)
            Destroy(card);

        useCardData.Clear();

    }

    public IEnumerator CardExplotation()
    {
        yield return new WaitForSeconds(1f);
        
        if(useCardData.Count > 0)
        {
            Player.GiveHealth(useCardData[0].Health);
            Player.GiveShield(useCardData[0].Shield);

            if (useCardData[0].Damage > 0)
            {
                ChoosedEnemy[0].TakeDamage(useCardData[0].Damage);
                ChoosedEnemy.RemoveAt(0);
            }

            Player.TakeEvridikaDamage(useCardData[0].Evridika);
            if (Player.GetEvridika() <= 0)
                Player.TakeDamage(100);

            useCardData.RemoveAt(0);
            Destroy(useCardObj[0]);
            useCardObj.RemoveAt(0);

            StartCoroutine(CardExplotation());
        } else
        {
            Game.Instance.NextMotion();
        }
    }


    #endregion
}
