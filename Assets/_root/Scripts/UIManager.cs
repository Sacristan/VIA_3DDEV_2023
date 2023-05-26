using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static readonly string[] TutorialTexts = new string[]
    {
        "<color=green>COLLECT</color> ALL ESCAPED CHICKENS!",
        "AVOID THEM <color=red>EGGS</color> - THEY DANGEROUS!",
        "<color=green>WASD</color> TO MOVE!\n <color=green>MOUSE</color> TO LOOK AROUND!"
    };
    const string TutorialPrefsKey = "TimesTutorialSeen";
    [SerializeField] TMPro.TextMeshProUGUI objectiveText;
    [SerializeField] RectTransform victoryPanel;
    [SerializeField] RectTransform gameoverPanel;

    [SerializeField] Slider healthSlider;
    [SerializeField] TMPro.TextMeshProUGUI tutorialText;

    private void Start()
    {
        GameManager.instance.OnGameWon += OnGameWon;
        GameManager.instance.OnGameLost += OnGameLost;

        GameManager.instance.OnObjectiveCountChanged += OnObjectiveCountChanged;
        GameManager.instance.Player.OnHealthUpdated += OnPlayerHealthUpdated;
        HandleTutorial();
    }

    private void OnGameWon()
    {
        victoryPanel.gameObject.SetActive(true);
    }

    private void OnGameLost()
    {
        gameoverPanel.gameObject.SetActive(true);
    }

    void OnObjectiveCountChanged(int count)
    {
        objectiveText.text = $"Chickens left: {count}";
    }

    void OnPlayerHealthUpdated(int health)
    {
        healthSlider.value = Mathf.InverseLerp(0, Player.MaxHealth, health);
    }

    public int TimesTutorialSeen
    {
        get
        {
            int timesTutorialSeen = 0;

            if (PlayerPrefs.HasKey(TutorialPrefsKey))
            {
                timesTutorialSeen = PlayerPrefs.GetInt(TutorialPrefsKey);
            }

            return timesTutorialSeen;
        }
    }

    void HandleTutorial()
    {
        if (TimesTutorialSeen < 3)
        {
            StartCoroutine(TutorialRoutine());
        }
    }

    IEnumerator TutorialRoutine()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < TutorialTexts.Length; i++)
        {
            ShowTutorialText(i);
            yield return new WaitForSeconds(3f);
        }

        HideTutorialText();

        PlayerPrefs.SetInt(TutorialPrefsKey, TimesTutorialSeen + 1);

        void ShowTutorialText(int index)
        {
            string txt = TutorialTexts[index];
            tutorialText.text = txt;
        }

        void HideTutorialText()
        {
            tutorialText.text = string.Empty;
        }
    }
}
