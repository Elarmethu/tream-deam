using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public CardLogic logic;
    public EnemyLogic logicEnemy;
    public Game game;

    [SerializeField] private SpriteRenderer viewModel;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject shieldObj;
    [SerializeField] private Text shieldText;

    private int shield;
    private int health;
    public bool isDead;

    /// <summary>
    /// Класс отвечающий за объект врага. Если решишь добавлять новых, а это надо будет,
    /// то в папке ресурсы лежат перфаб врага. Для создания нового типа врагов, необходимо
    /// Создать новую data, которую он инициализирует.
    /// </summary>

    public void Initialize()
    {
        viewModel.sprite = data.Model;
        health = data.Health;
        isDead = false;
    } 

    private void Dead()
    {
        isDead = true;
        data = null;
        gameObject.SetActive(false);
    }

    public int GetHealth()
    {
        return health;
    }
    public void TakeDamage(int damage)
    {
        if (shield > 0)
        {
            if (shield - damage < 0)
            {
                int difference = Mathf.Abs(shield - damage);
                if (shield - difference <= 0)
                {
                    health = 0;
                    Dead();
                }

                health -= difference;
                shield = 0;
            }
            else
            {
                shield -= damage;
            }
        }
        else
        {
            if (health - damage <= 0)
            {
                health = 0;
                Dead();
            }
            else
            {
                health -= damage;
            }
        }

        if (logicEnemy.EnemiesDeadCheck())
        {
            Game.Instance.NextLevel();
        }
    }

    public void TakeHealth(int health)
    {
        this.health += health;
        
        if (this.health > data.Health)
            this.health = data.Health;
    }

    public void TakeShield(int shield)
    {
        this.shield += shield;

        if (this.shield > data.ShieldMax)
            this.shield = data.ShieldMax;
    }

    public int GetShiled()
    {
        return shield;
    }

    private void Update()
    {
        if (data != null)
        {
            healthBar.value = Mathf.Clamp01((float)health / data.Health);
            healthText.text = string.Format("{0}/{1}", health, data.Health);

            if (shield > 0)
            {
                if (!shieldObj.activeSelf)
                    shieldObj.SetActive(true);

                shieldText.text = $"{shield}";
            } else
            {
                if(shieldObj.activeSelf)
                    shieldObj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Когда мы выбрали карту атаки, у нас появляется доступ,
    /// к выбору врага -> выбирая его мы наносим ему урон.
    /// </summary>
    private void OnMouseDown()
    {
        if (logic.AttackEnemy)
        {
            logic.AttackEnemy = false;
            logic.ChoosedEnemy.Add(this);
            gameObject.GetComponent<Renderer>().material.SetFloat("_OutlineWidth", 0.0f);
        } 
    }

    private void OnMouseEnter()
    {
        if (logic.AttackEnemy)
            gameObject.GetComponent<Renderer>().material.SetFloat("_OutlineWidth", 0.02f);
    }

    private void OnMouseExit()
    {
        if (logic.AttackEnemy)
            gameObject.GetComponent<Renderer>().material.SetFloat("_OutlineWidth", 0.0f);
    }
}
