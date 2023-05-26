using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI objectiveText;
    [SerializeField] RectTransform victoryPanel;
    [SerializeField] RectTransform gameoverPanel;

    [SerializeField] Slider healthSlider;

    private void Start()
    {
        GameManager.instance.OnGameWon += OnGameWon;
        GameManager.instance.OnGameLost += OnGameLost;

        GameManager.instance.OnObjectiveCountChanged += OnObjectiveCountChanged;
        GameManager.instance.Player.OnHealthUpdated += OnPlayerHealthUpdated;
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
}
