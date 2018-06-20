using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishScript : MonoBehaviour
{
    public CameraScript cameraScript;
    public CarScript carScript;
    public Color colorWhite;
    public GameObject[] cars;
    public GameObject failPanel, pausePanel, shaurma3S;
    public Image[] stars;
    public Text rubCount;

    public bool isPaused = false;
    public int coinsTarger1 = 4;
    public int coinsTarger2 = 8;
    public int coinsTarger3 = 12;
    public string levelName;

    void Awake()
    {
        cars[PlayerPrefs.GetInt("car")].SetActive(true);
        cameraScript.target = cars[PlayerPrefs.GetInt("car")].transform;
        carScript = cars[PlayerPrefs.GetInt("car")].GetComponent<CarScript>();
        colorWhite = new Color(stars[0].color.r, stars[0].color.g, stars[0].color.b, 1f);
    }
    void Update()
    {
        if (carScript != null)
        {
            if (carScript.finishPanel.activeSelf)
            {
                if (rubCount != null)
                {
                    rubCount.text = (10 * carScript.coinsInt).ToString();
                }
                for (int i = 0; i < carScript.controlCar.Length; i++)
                {
                    carScript.controlCar[i].clickedIs = false;
                    carScript.controlCar[i].gameObject.SetActive(false);
                }
                if (carScript.coinsInt == coinsTarger3)
                {
                    stars[0].color = colorWhite;
                    stars[1].color = colorWhite;
                    stars[2].color = colorWhite;
                    if (shaurma3S != null)
                    {
                        shaurma3S.SetActive(true);
                    }
                    PlayerPrefs.SetInt(levelName, 3);
                    PlayerPrefs.Save();
                }
                else if (carScript.coinsInt >= coinsTarger2 && carScript.coinsInt < coinsTarger3)
                {
                    stars[0].color = colorWhite;
                    stars[1].color = colorWhite;
                    if (PlayerPrefs.GetInt(levelName) != 3)
                    {
                        PlayerPrefs.SetInt(levelName, 2);
                        PlayerPrefs.Save();
                    }
                }
                else if (carScript.coinsInt >= coinsTarger1 && carScript.coinsInt < coinsTarger2)
                {
                    stars[0].color = colorWhite;
                    if (PlayerPrefs.GetInt(levelName) != 3 && PlayerPrefs.GetInt(levelName) != 2)
                    {
                        PlayerPrefs.SetInt(levelName, 1);
                        PlayerPrefs.Save();
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && carScript.failPanel.activeSelf)
        {
            carScript.buttonSound.Play();
            PlayerPrefs.SetInt("menu", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && !carScript.finishPanel.activeSelf)
        {
            pausePanel.SetActive(true);
            carScript.accelerationSound.Stop();
            carScript.buttonSound.Play();
            Time.timeScale = 0;
            isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            pausePanel.SetActive(false);
            carScript.accelerationSound.Play();
            carScript.buttonSound.Play();
            Time.timeScale = 1;
            isPaused = false;
        }
    }
    public void OnClickRestart(int level)
    {
        carScript.buttonSound.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(level);
    }
    public void OnClickLevel()
    {
        carScript.buttonSound.Play();
        PlayerPrefs.SetInt("menu", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
    public void PauseContinue()
    {
        pausePanel.SetActive(false);
        carScript.accelerationSound.Play();
        carScript.buttonSound.Play();
        Time.timeScale = 1;
        isPaused = false;
    }
    public void PauseExit()
    {
        carScript.buttonSound.Play();
        Time.timeScale = 1;
        PlayerPrefs.SetInt("menu", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
}