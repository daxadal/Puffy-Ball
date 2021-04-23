using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public GameObject mainMenu, gameMenu, gameOverMenu;

    public GameObject fuelComponent;
    public RectTransform fuelMask;
    public TMP_Text scoreText, bestText;

    float fuelMaskWidth;

    [SerializeField] int score = 0, bestScore = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMainMenu();
        fuelMaskWidth = fuelMask.rect.width;
    }

    public void ShowFuel(bool show)
    {
        fuelComponent.SetActive(show);
    }
    public void SetFuel(float fuel)
    {
        fuelMask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fuelMaskWidth * fuel);
    }

    public void IncrementScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    public void SetMainMenu()
    {
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }
    public void SetGameMenu()
    {
        mainMenu.SetActive(false);
        gameMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        score = 0;
        scoreText.text = "Score: " + score;
    }
    public void SetGameOverMenu()
    {
        mainMenu.SetActive(false);
        gameMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        if (score > bestScore)
            bestScore = score;
        bestText.text = $"Score: {score}\nBest: {bestScore}";
    }

}
