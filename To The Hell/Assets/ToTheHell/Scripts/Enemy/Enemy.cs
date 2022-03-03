using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
    [SerializeField] private Button chooseButton;

    private int shield;
    private int health;
    public bool isDead;

    /// <summary>
    /// ����� ���������� �� ������ �����. ���� ������ ��������� �����, � ��� ���� �����,
    /// �� � ����� ������� ����� ������ �����. ��� �������� ������ ���� ������, ����������
    /// ������� ����� data, ������� �� ��������������.
    /// </summary>

    public void Initialize()
    {
        viewModel.sprite = data.Model;
        health = data.Health;
        isDead = false;
        chooseButton.onClick.AddListener(() => ChooseEnemy());

        UpdateHealth();
        UpdateShield();
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
                if (health - difference <= 0)
                {

                    health = 0;
                    Dead();
                }

                if (CardLogic.Instance.comboChoosed == ComboType.NotShieldReset)
                    CardLogic.Instance.comboChoosed = ComboType.Nothing;

                health -= difference;
                shield = 0;
                UpdateHealth(); // UI UPDATE
                UpdateShield(); // UI UPDATE
            }
            else
            {
                shield -= damage;
                UpdateShield(); // UI UPDATE
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
                UpdateHealth(); // UI UPDATE
            }
        }  
    }

    public void TakeHealth(int health)
    {
        this.health += health;

        if (this.health > data.Health)
            this.health = data.Health;

        UpdateHealth();
    }

    public void TakeShield(int shield)
    {
        this.shield += shield;

        if (this.shield > data.ShieldMax)
        {
            this.shield = data.ShieldMax;    
        }

        UpdateShield();
    }

    public int GetShiled()
    {
        return shield;
    }

    private void UpdateHealth()
    {
        healthBar.value = Mathf.Clamp01((float)health / data.Health);
        healthText.text = string.Format("{0}/{1}", health, data.Health);
    }

    private void UpdateShield()
    {
        if (shield > 0)
        {
            if (!shieldObj.activeSelf)
                shieldObj.SetActive(true);

            shieldText.text = $"{shield}";
        }
        else
        {
            if (shieldObj.activeSelf)
                shieldObj.SetActive(false);
        }
    }


    /// <summary>
    /// ����� �� ������� ����� �����, � ��� ���������� ������,
    /// � ������ ����� -> ������� ��� �� ������� ��� ����.
    /// </summary>



    public void ChooseEnemy()
    {
        if (logic.AttackEnemy)
        {
            logic.AttackEnemy = false;
            logic.ChoosedEnemy.Add(this);
        }
    }
}
