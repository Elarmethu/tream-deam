using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject loadScreen;

    public void NewGame()
    {
        StartCoroutine(LoadScene());
    }
    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadScene()
    {
        loadScreen.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!mainMenu.activeSelf)
            {
                mainMenu.SetActive(true);
                settingsMenu.SetActive(false);
            }
        }
    }
}
