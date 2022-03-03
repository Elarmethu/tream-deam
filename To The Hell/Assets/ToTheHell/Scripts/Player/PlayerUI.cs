using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    /// <summary>
    /// ƒанный скрипт отвечает за всю статистику UI персонажа.
    /// я думаю более тут описывать нет смысла.
    /// </summary>


    [Header("Main")]
    [SerializeField] private PlayerData data;
    [SerializeField] private CardLogic logic;

    [Header("Health")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image damageImage;

    [Header("Shield")]
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private Text shieldText;

    [Header("Evridika")]
    [SerializeField] private Image evridikaImage;

    [Header("Escape Menu")]
    [SerializeField] private GameObject escapeMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Slider musicSlider;

    [Header("Dead Screen")]
    public GameObject DeadScreen;
    public GameObject LoadScreen;
    private bool pressed;

    public void Dead()
    {
        DeadScreen.SetActive(true);
        logic.ClearTurn();
    }

    public void RestartGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void MenuGame()
    {
        SceneManager.LoadScene(0);
    }

    public void CloseGame()
    {
        Application.Quit();
    }


    public void TakeDamage()
    {
        StartCoroutine(GiveDamageImage());
    }

    private IEnumerator GiveDamageImage()
    {
        for (float f = 0.05f; f <= 1.0f; f += 0.05f)
        {
            Color color = damageImage.color;
            color.a = f;
            damageImage.color = color;
            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine(DisableDamageImage());
    }

    private IEnumerator DisableDamageImage()
    {

        for(float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color color = damageImage.color;
            color.a = f;
            damageImage.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }


    public void SettingsMenuOpen()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void SettingMenuClose()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void CloseEscapeMenu()
    {
        escapeMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapeMenu.SetActive(true);
            mainMenu.SetActive(true);
            settingsMenu.SetActive(false);
        }

        healthBar.value = Mathf.Clamp01((float)data.Health / data.HealthMax);
        
        if (data.Shield > 0)
        {
            if (!shieldObject.activeSelf)
                shieldObject.SetActive(true);

            shieldText.text = $"{data.Shield}";
        }
        else
        {
            if (shieldObject.activeSelf)
                shieldObject.SetActive(false);
        }

        evridikaImage.fillAmount = Mathf.Clamp01(1.0f - ((float)data.EvridikaHealth / data.EvridikaMaxHealth));
    }
}
