using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLogic : MonoBehaviour
{
    public static CardLogic Instance;
    
    [Header("Prefabs")]
    public PlayerLogic Player;
    public Game game;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject reviewPrefab;
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

    [Header("ComboSystem")]
    public ComboType comboChoosed;

    [Header("Logic")]
    public bool AttackEnemy;
    public int PlayerDamage;
    public bool cardChoosed;
    public bool isChoosed;
    public bool isTutorialStart;

    [Header("Poison")]
    public List<PoisonAttack> poisonAttacks;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }


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
        if (!isTutorialStart)
        {
            GameObject cardObj = Instantiate(cardPrefab);
            cardObj.transform.SetParent(content.transform);

            RectTransform cardRect = cardObj.GetComponent<RectTransform>();
            cardRect.localScale = new Vector3(1, 1, 1);
            cardRect.localPosition = new Vector3(cardRect.transform.position.x, cardRect.transform.position.y, 0.0f);

            if (containData.Count < 5)
            {
                InitializeDatas();
            }

            int rnd = Random.Range(0, containData.Count);
            CardCell cell = cardObj.GetComponentInChildren<CardCell>();
            cell.data = containData[rnd];
            cell.InitializeCard();

            initializeCard.Add(cardObj);
            UpdateCardCells();

            initializeData.Add(containData[rnd]);
            containData.Remove(containData[rnd]);
        }
        else
        {
            for(int i = 0; i < cardDatas.Count; i++)
            {
                GameObject cardObj = Instantiate(cardPrefab);
                cardObj.transform.SetParent(content.transform);


                RectTransform cardRect = cardObj.GetComponent<RectTransform>();
                cardRect.localScale = new Vector3(1, 1, 1);
                cardRect.localPosition = new Vector3(cardRect.transform.position.x, cardRect.transform.position.y, 0.0f);              

                CardCell cell = cardObj.GetComponentInChildren<CardCell>();
                cell.data = cardDatas[i];
                cell.InitializeCard();

                initializeCard.Add(cardObj);
                UpdateCardCells();

                initializeData.Add(cardDatas[i]);
                isTutorialStart = false;
            }
        }
    }

    public void DestroyCard(GameObject card)
    {
        initializeCard.Remove(card);
        Destroy(card);
    }

    public void CardUse(CardCell card)
    {
        if (!AttackEnemy)
        {
            useCardData.Add(card.data);

            var obj = Instantiate(reviewPrefab);
            obj.transform.SetParent(reviewTransformField.transform);

            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            CardView view = obj.GetComponentInChildren<CardView>();
            view.data = card.data;
            view.InitializeCard();


            initializeData.Remove(card.data);
            DestroyCard(card.transform.parent.gameObject);

            if (card.data.Damage > 0 || card.data.Poison.damage > 0)
            {
                AttackEnemy = true;
                attackCard = card.data;
                PlayerDamage = card.data.Damage > 0 ? PlayerDamage += card.data.Damage : PlayerDamage += card.data.Poison.damage;
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

    public void InitializeCardWithData(CardData data)
    {
        GameObject cardObj = Instantiate(cardPrefab);
        cardObj.transform.SetParent(content.transform);

        RectTransform cardRect = cardObj.GetComponent<RectTransform>();
        cardRect.localScale = new Vector3(1, 1, 1);
        cardRect.localPosition = new Vector3(cardRect.transform.position.x, cardRect.transform.position.y, 0.0f);

        if (containData.Count < 5)
        {
            InitializeDatas();
        }

        CardCell cell = cardObj.GetComponentInChildren<CardCell>();
        cell.data = data;
        cell.InitializeCard();

        initializeCard.Add(cardObj);
        UpdateCardCells();

        initializeData.Add(data);
        containData.Remove(data);
    }

    public void DestroyViewCard(GameObject card)
    {
        useCardData.Remove(card.GetComponent<CardView>().data);
        useCardObj.Remove(card);
        Destroy(card);
    }

    #endregion

    #region ComboSystem
    public void ClearTurn()
    {

        foreach (var card in useCardObj)
            Destroy(card);

        useCardData.Clear();

    }


    public void ComboPlayerCheck()
    {
        if(useCardData.Count >= 2)
        {
            int countMajor = 0;
            int countMinor = 0;

            for (int i = 0; i < useCardData.Count; i++)
            {
                switch (useCardData[i].Name)
                {
                    case "Major":
                        countMajor += 1;
                        break;
                    case "Minor":
                        countMinor += 1;
                        break;
                }
            }

            if (countMajor == 2)
                StartCoroutine(CardExplotation(ComboType.NotShieldReset));
            else if (countMinor == 2)
                StartCoroutine(CardExplotation(ComboType.GetBoostForAttack));
            else
                StartCoroutine(CardExplotation(ComboType.Nothing));

        } else if(useCardData.Count >= 4)
        {
            int countMajor = 0;
            int countMinor = 0;
            int countDies = 0;
            int countBemol = 0;

            for(int i = 0; i < useCardData.Count; i++)
            {
                switch (useCardData[i].Name)
                {
                    case "Major":
                        countMajor += 1;
                        break;
                    case "Minor":
                        countMinor += 1;
                        break;
                    case "Dies":
                        countDies += 1;
                        break;
                    case "Bemol":
                        countBemol += 1;
                        break;
                }
            }

            if (countMajor >= 1 && countMinor >= 1 && countDies >= 1 && countBemol >= 1)
                StartCoroutine(CardExplotation(ComboType.PlayerGetEnemyHealth));
            else
                StartCoroutine(CardExplotation(ComboType.Nothing));

        } else
        {
            StartCoroutine(CardExplotation(ComboType.Nothing));
        }
    }

    private IEnumerator CardExplotation(ComboType combo)
    {
        yield return new WaitForSeconds(1f);
        
        if(useCardData.Count > 0)
        {
            Player.GiveHealth(useCardData[0].Health);
            Player.GiveShield(useCardData[0].Shield);

            if (useCardData[0].Damage > 0 || useCardData[0].Poison.damage > 0)
            {
                if(useCardData[0].Damage > 0)
                {
                    if (ComboType.GetBoostForAttack == combo) ChoosedEnemy[0].TakeDamage(Mathf.CeilToInt(useCardData[0].Damage * 1.5f));
                    else ChoosedEnemy[0].TakeDamage(useCardData[0].Damage);
                    ChoosedEnemy.RemoveAt(0);
                    Debug.Log("BB");
                }
                else if(useCardData[0].Poison.damage > 0)
                {
                    PoisonAttack attack = new PoisonAttack(ChoosedEnemy[0], useCardData[0].Poison.count, useCardData[0]);
                    poisonAttacks.Add(attack);
                    ChoosedEnemy.RemoveAt(0);
                    Debug.Log("QQ");
                }
            }
            Debug.Log(useCardData[0].Poison.damage);

            Player.TakeEvridikaDamage(useCardData[0].Evridika);
            if (Player.GetEvridika() <= 0)
                Player.TakeDamage(100);

            if (useCardData.Count > 0) 
            {
                useCardData.RemoveAt(0);
                Destroy(useCardObj[0]);
                useCardObj.RemoveAt(0);
            }

            if (!game.enemyLogic.EnemiesDeadCheck())
                StartCoroutine(CardExplotation(combo));
            else
                CardBaffLogic.Instance.InitializeBaff();
        } else
        {
            if (ComboType.PlayerGetEnemyHealth == combo)
                Player.GiveHealth(PlayerDamage);

            comboChoosed = combo;
            
            if (Game.Instance.enemyLogic.EnemiesDeadCheck())
                CardBaffLogic.Instance.InitializeBaff();
            else
            {
                Game.Instance.NextMotion();
                
                for(int i = 0; i < poisonAttacks.Count; i++)
                {
                    if (poisonAttacks[i].motionCount <= 0)
                        poisonAttacks.Remove(poisonAttacks[i]);
                    else
                    {
                        poisonAttacks[i].enemy.TakeDamage(poisonAttacks[i].poison.Poison.damage);
                        poisonAttacks[i].motionCount -= 1;
                    }                            
                }         
            }
        }
    } 
    #endregion
}

[System.Serializable]
public enum ComboType
{
    Nothing = 0,
    GetBoostForAttack = 1,
    NotShieldReset = 2,
    PlayerGetEnemyHealth = 3
}

[System.Serializable]
public class PoisonAttack
{
    public Enemy enemy;
    public int motionCount;
    public CardData poison;

    public PoisonAttack(Enemy _enemy, int count, CardData data)
    {
        enemy = _enemy;
        motionCount = count;
        poison = data;   
    }
}