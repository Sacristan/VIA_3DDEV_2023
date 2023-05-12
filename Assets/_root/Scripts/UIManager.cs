using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI objectiveText;
    [SerializeField] RectTransform victoryPanel;

    private void Start()
    {
        GameManager.instance.OnGameWon += OnGameWon;
        GameManager.instance.OnObjectiveCountChanged += OnObjectiveCountChanged;
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasReleasedThisFrame)
        {
            LoadMenu();
        }
    }

    bool isLoadingMenu = false;
    void LoadMenu()
    {
        if (isLoadingMenu) return;
        
        isLoadingMenu = true;
        SceneManager.LoadScene(0);
    }

    private void OnGameWon()
    {
        victoryPanel.gameObject.SetActive(true);
    }

    void OnObjectiveCountChanged(int count)
    {
        objectiveText.text = $"Chickens left: {count}";
    }

}
