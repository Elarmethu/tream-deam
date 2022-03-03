using UnityEngine;

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
                NextLevel();

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

        for(int i = 0; i < 5; i++)
        {
            cardLogic.InitializeCard();
        }

        //Enemy initialize
        enemyLogic.InitializeEnemies();

    }
    private void Update()
    {
        if (cardLogic.cardChoosed)
            NextLevel();

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("HIT");
            Player.TakeDamage(5);
        }

        if (Input.GetMouseButton(0))
            mouse.Play();

    }
}
