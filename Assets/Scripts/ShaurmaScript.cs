using UnityEngine;
using UnityEngine.UI;

public class ShaurmaScript : MonoBehaviour
{
    public Button nextLevelbutton;
    public Color colorWhite, colorBlack;
    public Image[] shaurmas;
    public MenuScript menuScript;

    public string levelName;

    void Awake()
    {
        shaurmas = GetComponentsInChildren<Image>();
        menuScript = Camera.main.GetComponent<MenuScript>();
        if (shaurmas.Length != 0)
        {
            colorWhite = new Color(shaurmas[1].color.r, shaurmas[1].color.g, shaurmas[1].color.b, 1f);
            colorBlack = new Color(shaurmas[1].color.r, shaurmas[1].color.g, shaurmas[1].color.b, 0.3f);
        }
    }
    void Update()
    {
        switch (PlayerPrefs.GetInt(levelName))
        {
            case 3:
                nextLevelbutton.interactable = true;
                shaurmas[1].color = colorWhite;
                shaurmas[2].color = colorWhite;
                shaurmas[3].color = colorWhite;
                break;
            case 2:
                nextLevelbutton.interactable = true;
                shaurmas[1].color = colorWhite;
                shaurmas[2].color = colorWhite;
                shaurmas[3].color = colorBlack;
                break;
            case 1:
                nextLevelbutton.interactable = false;
                shaurmas[1].color = colorWhite;
                shaurmas[2].color = colorBlack;
                shaurmas[3].color = colorBlack;
                break;
            case 0:
                nextLevelbutton.interactable = false;
                shaurmas[1].color = colorBlack;
                shaurmas[2].color = colorBlack;
                shaurmas[3].color = colorBlack;
                break;
        }
    }
}