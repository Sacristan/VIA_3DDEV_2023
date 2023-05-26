using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event System.Action OnGameWon;
    public event System.Action OnGameLost;
    public event System.Action<int> OnObjectiveCountChanged;
    public static GameManager instance;
    List<Collectable> collectables = new List<Collectable>();

    Player _player;
    public Player Player
    {
        get
        {
            if (_player == null) _player = FindObjectOfType<Player>();
            return _player;
        }

    }

    void Awake()
    {
        if (instance == null) instance = this;
    }

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        collectables = new List<Collectable>(
            FindObjectsOfType<Collectable>()
        );
        RecalculateObjectives();

        Game.ShowCursor(false);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasReleasedThisFrame)
        {
            LoadMenu();
        }
    }

    public void OnCollectableCollected(Collectable collectable)
    {
        collectables.Remove(collectable);
        RecalculateObjectives();

        Debug.Log($"REMAINING {collectables.Count} Collectables to WIN");

        if (collectables.Count <= 0)
        {
            OnGameWon?.Invoke();
            Debug.Log("VICTORY");

            Invoke(nameof(RestartGame), 2f);
        }
    }

    public void GameLost()
    {
        OnGameLost?.Invoke();
        Invoke(nameof(RestartGame), 2f);
    }

    void RecalculateObjectives()
    {
        OnObjectiveCountChanged?.Invoke(collectables.Count);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    bool isLoadingMenu = false;
    void LoadMenu()
    {
        if (isLoadingMenu) return;

        isLoadingMenu = true;
        SceneManager.LoadScene(0);
    }
}