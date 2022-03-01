using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private PlayerData player;
    [SerializeField] private PlayerUI playerUI;
    public bool canMotion;

    /// <summary>
    /// Класс отвечающий за логику персонажа, к нему мы обращаемся,
    /// чтобы не работать на прямую с объектом игрока.
    /// 
    /// TakeDamage - урон, который проходит учитывает и щит.
    /// </summary>
    /// <param name="PlayerLogic"></param>

    public void TakeDamage(int damage)
    {
        playerUI.TakeDamage();
        if(player.Shield > 0)
        {
            if(player.Shield - damage < 0)
            {
                int difference = Mathf.Abs(player.Shield - damage);
                if(player.Health - difference <= 0)
                {
                    player.Health = 0;
                    playerUI.Dead();
                }

                player.Health -= difference;
                player.Shield = 0;
            }
            else
            {
                player.Shield -= damage;
            }
        }
        else
        {
            if(player.Health - damage <= 0)
            {
                player.Health = 0;
                playerUI.Dead();
            }
            else
            {
                player.Health -= damage;
            }
        }
    }


    #region Health
    public int GetHealth()
    {
        return player.Health;
    }
    public void ResetHealth()
    {
        player.Health = player.HealthMax;
    }
    public void GiveHealth(int health)
    {
        player.Health += health;

        if (player.Health > player.HealthMax)
            player.Health = player.HealthMax;
    }
    #endregion

    #region Shield
    public void GiveShield(int shield)
    {
        player.Shield += shield;
    }
    
    public void ResetSheild()
    {
        player.Shield = 0;
    }

    #endregion

    #region Evridika
    public void TakeEvridikaDamage(int damage)
    {
        player.EvridikaHealth -= damage;

        if (player.EvridikaHealth > player.EvridikaMaxHealth)
            player.EvridikaHealth = player.EvridikaMaxHealth;
    }

    public void ResetEvridika()
    {
        player.EvridikaHealth = player.EvridikaMaxHealth;
    }

    public int GetEvridika()
    {
        return player.EvridikaHealth;
    }

    #endregion

}
