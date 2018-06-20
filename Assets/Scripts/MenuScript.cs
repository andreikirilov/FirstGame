using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public AudioSource buttonSound;
    public Button[] buttons, updateButtons;
    public Color colorWhite, colorBlack;
    public Image[] cars;
    public GameObject levelChanger, exitChanger, garagePanel, updatePanel;
    public Text[] carsText, updateText;
    public Text moneyText, moneyCount;

    public char del = 'd';
    public string[] levels;
    public string[] tuning1;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("menu"))
        {
            PlayerPrefs.SetInt("button0", 0);
            PlayerPrefs.SetInt("button1", 0);
            PlayerPrefs.SetInt("button2", 0);
            PlayerPrefs.SetInt("car", 0);
            PlayerPrefs.SetInt("level1", 0);
            PlayerPrefs.SetInt("menu", 0);
            PlayerPrefs.SetInt("money", 0);
            PlayerPrefs.SetString("tuning1", "0d0d0");
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.GetInt("menu") == 1)
        {
            levelChanger.SetActive(true);
            buttonSound.Play();
            moneyText.color = new Color(moneyText.color.r, moneyText.color.g, moneyText.color.b, 0f);
            moneyCount.color = new Color(moneyText.color.r, moneyText.color.g, moneyText.color.b, 0f);
            PlayerPrefs.SetInt("menu", 0);
            PlayerPrefs.Save();
        }
        cars[PlayerPrefs.GetInt("car")].color = Color.white;
        if (PlayerPrefs.GetInt(levels[0]) == 3)
        {
            buttons[0].interactable = true;
            carsText[0].gameObject.SetActive(false);
        }
        else
        {
            buttons[0].interactable = false;
            carsText[0].gameObject.SetActive(true);
        }
        for (int i = 0; i < updateButtons.Length; i++)
        {
            updateText[i].text = updateText[i].text.ToString().Remove(updateText[i].text.ToString().Length - 7) + 100 * (PlayerPrefs.GetInt("button" + i) + 1) + "руб.";
        }
        tuning1 = PlayerPrefs.GetString("tuning1").Split(del);
        for (int i = 0; i < tuning1.Length; i++)
        {
            if (int.Parse(tuning1[i]) == 3)
            {
                updateButtons[i].interactable = false;
            }
            else
            {
                updateButtons[i].interactable = true;
            }
        }
        colorWhite = new Color(moneyText.color.r, moneyText.color.g, moneyText.color.b, 1f);
        colorBlack = new Color(moneyText.color.r, moneyText.color.g, moneyText.color.b, 0f);

    }
    public void CarTuning1(int carPart)
    {
        buttonSound.Play();
        int updateState = int.Parse(tuning1[carPart]) + 1;
        if (updateState <= 3 && 100 * updateState <= PlayerPrefs.GetInt("money"))
        {
            if (updateState != 3)
            {
                updateText[carPart].text = updateText[carPart].text.ToString().Remove(updateText[carPart].text.ToString().Length - 7) + 100 * (updateState + 1) + "руб.";
                PlayerPrefs.SetInt("button" + carPart, updateState);
                PlayerPrefs.Save();
            }
            tuning1[carPart] = updateState.ToString();
            PlayerPrefs.SetString("tuning1", tuning1[0] + del.ToString() + tuning1[1] + del.ToString() + tuning1[2]);
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 100 * updateState);
            PlayerPrefs.Save();
        }
        if (int.Parse(tuning1[carPart]) == 3)
        {
            updateButtons[carPart].interactable = false;
        }
        for (int i = 0; i < tuning1.Length; i++)
        {
            print(tuning1[i]);
        }
    }
    void Update()
    {
        moneyCount.text = PlayerPrefs.GetInt("money").ToString();
        if (updatePanel.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            updatePanel.SetActive(false);
            buttonSound.Play();
            moneyText.color = colorWhite;
            moneyCount.color = colorWhite;
        }
        else if (garagePanel.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            garagePanel.SetActive(false);
            buttonSound.Play();
            moneyText.color = colorWhite;
            moneyCount.color = colorWhite;
        }
        else if (levelChanger.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            levelChanger.SetActive(false);
            buttonSound.Play();
            moneyText.color = colorWhite;
            moneyCount.color = colorWhite;
        }
        else if (exitChanger.activeSelf == false && Input.GetKeyDown(KeyCode.Escape))
        {
            exitChanger.SetActive(true);
            buttonSound.Play();
            moneyText.color = colorBlack;
            moneyCount.color = colorBlack;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitChanger.SetActive(false);
            buttonSound.Play();
            moneyText.color = colorWhite;
            moneyCount.color = colorWhite;
        }
    }
    public void OnClickStart()
    {
        levelChanger.SetActive(true);
        buttonSound.Play();
        moneyText.color = colorBlack;
        moneyCount.color = colorBlack;
    }
    public void OnClickExit()
    {
        exitChanger.SetActive(true);
        buttonSound.Play();
        moneyText.color = colorBlack;
        moneyCount.color = colorBlack;
    }
    public void OnClickExitYes()
    {
        buttonSound.Play();
        Application.Quit();
    }
    public void OnClickExitNo()
    {
        exitChanger.SetActive(false);
        buttonSound.Play();
    }
    public void OnClickGarage()
    {
        garagePanel.SetActive(true);
        buttonSound.Play();
    }
    public void OnClickUpdate()
    {
        updatePanel.SetActive(true);
        buttonSound.Play();
    }
    public void OnClickReset()
    {
        buttonSound.Play();
        PlayerPrefs.DeleteKey("button0");
        PlayerPrefs.DeleteKey("button1");
        PlayerPrefs.DeleteKey("button2");
        PlayerPrefs.DeleteKey("car");
        PlayerPrefs.DeleteKey("level1");
        PlayerPrefs.DeleteKey("menu");
        PlayerPrefs.DeleteKey("money");
        PlayerPrefs.DeleteKey("tuning1");
        PlayerPrefs.Save();
        Awake();
    }
    public void LevelButtons(int level)
    {
        SceneManager.LoadScene(level);
        buttonSound.Play();
    }
    public void CarChanger(int car)
    {
        buttonSound.Play();
        PlayerPrefs.SetInt("car", car);
        PlayerPrefs.Save();
        switch (car)
        {
            case 0:
                cars[car].color = new Color(cars[car].color.r, cars[car].color.g, cars[car].color.b, 1f);
                cars[car + 1].color = new Color(cars[car + 1].color.r, cars[car + 1].color.g, cars[car + 1].color.b, 0.3f);
                break;
            case 1:
                cars[car].color = new Color(cars[car].color.r, cars[car].color.g, cars[car].color.b, 1f);
                cars[car - 1].color = new Color(cars[car - 1].color.r, cars[car - 1].color.g, cars[car - 1].color.b, 0.3f);
                break;
        }
    }
}
/*public class SaveToJson
{
    public int money = 0;
    public void Init(int money)
    {
        money = this.money;
    }
}
public static class Data
{
    public static int money = 0;
    public static void Init(int money)
    {
        Data.money = money;
    }
}*/
