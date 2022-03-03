using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public static Game Instance;

    [SerializeField] public PlayerLogic Player;
    [SerializeField] public EnemyLogic enemyLogic;
    [SerializeField] public CardLogic cardLogic;
    public int sceneIndex;
    public AudioSource source;
    public AudioSource mouse;

    public bool playerMotion;
    public bool isTutorial;

    [SerializeField] private List<GameObject> tutorials;

    public void NextLevel()
    {
        cardLogic.ClearTurn();
        cardLogic.CardTransfer(false);
        cardLogic.cardChoosed = false;
        Player.ResetHealth();
        Player.ResetEvridika();
        Player.ResetSheild();

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }

    public void EndMotionPlayer()
    {
        if (playerMotion && !cardLogic.AttackEnemy)
            cardLogic.ComboPlayerCheck();
    }

    public void NextMotion()
    {
        enemyLogic.PlayerDamage = cardLogic.PlayerDamage;
        playerMotion = false;
        Player.canMotion = false;
        
        if (!enemyLogic.EnemiesDeadCheck())
            enemyLogic.EnemyMotion();

        cardLogic.PlayerDamage = 0;
    }

    public void EndMotionMonster()
    {
        if (!playerMotion)
        {
            if (enemyLogic.EnemiesDeadCheck())
                CardBaffLogic.Instance.InitializeBaff();

            playerMotion = true;
            Player.canMotion = true;
            cardLogic.CardTransfer(true);
            
            if(ComboType.NotShieldReset != CardLogic.Instance.comboChoosed)
                Player.ResetSheild();

            for (int i = 0; i < 5; i++)
            {
               cardLogic.InitializeCard();
            }
        }

    }
    private void Awake() // Вместо хода
    {
        if(Instance == null)
            Instance = this;
        else if(Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }

        //Player Initialize
        Player.ResetHealth();
        Player.ResetEvridika();
        Player.ResetSheild();
        
        cardLogic.InitializeDatas();
        playerMotion = true;
        Player.canMotion = true;

        if (!isTutorial)
        {
            for (int i = 0; i < 5; i++)
            {
                cardLogic.InitializeCard();
            }
        } else
        {
            cardLogic.isTutorialStart = true;
            cardLogic.InitializeCard();
        }

        //Enemy initialize
        enemyLogic.InitializeEnemies();

    }
    private void Update()
    {
        if (cardLogic.cardChoosed)
            NextLevel();

        if (Input.GetMouseButton(0))
            mouse.Play();

    }

    public void NextTutorial()
    {
        int num = 0;
        for(int i = 0; i < tutorials.Count; i++)
        {
            if (tutorials[i].activeSelf) {
                num = i;
                break;
            }
        }

        if (num + 1 < tutorials.Count)
        {
            tutorials[num + 1].SetActive(true);
            tutorials[num].SetActive(false);
        }

        if(num + 1 >= tutorials.Count)
        {
            tutorials[num].SetActive(false);
            isTutorial = false;
        }

    }
}
