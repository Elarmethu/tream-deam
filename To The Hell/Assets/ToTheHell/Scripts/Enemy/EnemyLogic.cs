using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private Game gameLogic;
    [SerializeField] private PlayerLogic Player;
    [SerializeField] private List<EnemyData> enemiesData;
    [SerializeField] private List<Enemy> enemiesObject;
    [SerializeField] private List<Enemy> initializedEnemyObject;

    private List<Enemy> aliveEnemy = new List<Enemy>();
    public int PlayerDamage;
    public float Coefficient;
    private bool win;

    /// <summary>
    /// Получаем список врагов на карте. Враги в начале раунда не инициализированны,
    /// подругзка приходит рандомно. Мы получает дату из нашего списка и инициализируем.
    /// </summary>
    public void InitializeEnemies()
    {
        initializedEnemyObject.Clear();

        for(int i = 0; i < enemiesObject.Count; i++)
        {
            int rndData = Random.Range(0, enemiesData.Count);
            enemiesObject[i].data = enemiesData[rndData];
            enemiesObject[i].Initialize();
            initializedEnemyObject.Add(enemiesObject[i]);
        }

        foreach(var enemy in enemiesObject)
        {
            if (enemy.data == null)
                enemy.gameObject.SetActive(false);
            else
                enemy.gameObject.SetActive(true);
        }
    }

    public bool EnemiesDeadCheck()
    {
        win = true;
        for(int i = 0; i < initializedEnemyObject.Count; i++)
        {
            if (!initializedEnemyObject[i].isDead)
            {
                win = false;
                break;
            }
        }
        return win;
    }

    public void EnemyMotion()
    {
        if(aliveEnemy.Count != 0)
            aliveEnemy.Clear();

        for(int i = 0; i < initializedEnemyObject.Count; i++)
        {
            if (!initializedEnemyObject[i].isDead)
                aliveEnemy.Add(initializedEnemyObject[i]);
        }

        int rnd = Random.Range(0, aliveEnemy.Count);
        StartCoroutine(Motion(aliveEnemy[rnd]));
    }

    private IEnumerator Motion(Enemy enemy)
    {
        yield return new WaitForSeconds(0.5f);

        var data = enemy.data;
        float healthRatio = (float)(enemy.GetHealth() - Player.GetHealth()) / (enemy.GetHealth() + Player.GetHealth());
        float logic = PlayerDamage == 0 ? Coefficient * healthRatio : Coefficient * PlayerDamage * healthRatio;


        if (enemy.GetHealth() * 1.5f < Player.GetHealth())
            logic = Random.Range(-2, 2);
        
        if (logic > 0)
        {
            int rndAttack = Random.Range(0, 100);
            if(rndAttack <= 50)
            {
                Player.TakeDamage(data.MotionData.Damage);
                Debug.Log("Enemy Attack!");
            } else if(rndAttack <= 70 && rndAttack >= 50)
            {
                enemy.TakeHealth(data.MotionData.Health);
                Debug.Log("Enemy Health!");
            } else if(rndAttack >= 70 && rndAttack <= 100 && enemy.GetShiled() != enemy.data.ShieldMax)
            {
                enemy.TakeShield(data.MotionData.Shield);
                Debug.Log("Enemy get shield!");
            }
            else
            {
                rndAttack = Random.Range(0, 50);
                if(rndAttack <= 30)
                {
                    Player.TakeDamage(data.MotionData.Damage);
                    Debug.Log("Enemy Attack!");
                } else
                {
                    enemy.TakeHealth(data.MotionData.Health);
                    Debug.Log("Enemy Health!");
                }
            }
        }
        else
        {
            if (enemy.GetHealth() == enemy.data.Health)
            {
                int rndAttack = Random.Range(0, 100);
                if (rndAttack <= 95)
                {
                    Player.TakeDamage(data.MotionData.Damage);
                    Debug.Log("Enemy Attack!");
                }
                else if(enemy.GetShiled() != enemy.data.ShieldMax)
                {
                    enemy.TakeShield(data.MotionData.Shield);
                    Debug.Log("Enemy get shield!");
                } else
                {
                    Player.TakeDamage(data.MotionData.Damage);
                    Debug.Log("Enemy Attack!");
                }
                             
            }
            else
            {
                int rndAttack = Random.Range(0, 100);
                if (rndAttack <= 50)
                {
                    Player.TakeDamage(data.MotionData.Damage);
                    Debug.Log("Enemy Attack!");
                }
                else if (rndAttack <= 90 && rndAttack >= 50)
                {
                    enemy.TakeHealth(data.MotionData.Health);
                    Debug.Log("Enemy Health!");
                }
                else if (rndAttack >= 70 && rndAttack <= 100 && enemy.GetShiled() != enemy.data.ShieldMax)
                {
                    enemy.TakeShield(data.MotionData.Shield);
                    Debug.Log("Enemy get shield!");
                } else
                {
                    rndAttack = Random.Range(0, 50);
                    if (rndAttack <= 30)
                    {
                        Player.TakeDamage(data.MotionData.Damage);
                        Debug.Log("Enemy Attack!");
                    }
                    else
                    {
                        enemy.TakeHealth(data.MotionData.Health);
                        Debug.Log("Enemy Health!");
                    }
                }
            }
        }
        
        gameLogic.EndMotionMonster();
    }

}
