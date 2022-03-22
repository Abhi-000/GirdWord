using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScreenHandler : MonoBehaviour
{
    public Text coins, gems, keys;
    public GameObject extendedSettings;
    public GameObject bonus, challenge, daily;
    void Start()
    {
        coins.text = PlayerPrefs.GetInt("Coins", 0).ToString();
        gems.text = PlayerPrefs.GetInt("Gems", 0).ToString();
        keys.text = PlayerPrefs.GetInt("Keys", 100).ToString();
    }
    public void Settings()
    {
        if(!extendedSettings.activeSelf)
        extendedSettings.SetActive(true);
        else
            extendedSettings.SetActive(false);
    }
    public void Bonus()
    {
        bonus.SetActive(true);
    }
    public void Challenge()
    {
        challenge.SetActive(true);
    }
    public void Daily()
    {
        daily.SetActive(true);
    }
    public void Back()
    {
        bonus.SetActive(false);
        challenge.SetActive(false);
        daily.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
