using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event System.Action OnGameWon;
    public static GameManager instance;

    List<Collectable> collectables = new List<Collectable>();

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        collectables = new List<Collectable>(
            FindObjectsOfType<Collectable>()
        );
    }

    public void OnCollectableCollected(Collectable collectable)
    {
        collectables.Remove(collectable);

        Debug.Log($"REMAINING {collectables.Count} Collectables to WIN");

        if (collectables.Count <= 0)
        {
            OnGameWon?.Invoke();
            Debug.Log("VICTORY");

            Invoke(nameof(RestartGame), 2f);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
