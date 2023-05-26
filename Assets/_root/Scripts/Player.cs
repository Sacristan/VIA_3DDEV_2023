using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
public class Player : MonoBehaviour
{
    public System.Action<int> OnHealthUpdated;
    public const int MaxHealth = 100;
    int _health;
    public int Health
    {
        get => _health;
        private set
        {
            _health = value;
            OnHealthUpdated?.Invoke(_health);
        }
    }

    public float MovementDir { get; private set; } = 1;

    IEnumerator Start()
    {
        GameManager.instance.OnGameWon += GameWon;
        yield return null;
        Health = MaxHealth;
    }

    void GameLost()
    {
        GameManager.instance.GameLost();
        HandleGameEnded();
    }

    void GameWon()
    {
        HandleGameEnded();
    }

    void HandleGameEnded()
    {
        GetComponent<CharacterController>().enabled = false;
        GetComponent<FirstPersonController>().enabled = false;
        GetComponent<PlayerInput>().enabled = false;
    }

    public void DoExplosionEffect()
    {
        Debug.Log(nameof(DoExplosionEffect));
        Health -= 25;

        if (Health <= 0)
        {
            HandleGameEnded();
            GameManager.instance.GameLost();
        }
    }

    public void DoIntoxicatedEffect()
    {
        Debug.Log(nameof(DoIntoxicatedEffect));
        StartCoroutine(IntoxicatedRoutine());
    }

    IEnumerator IntoxicatedRoutine()
    {
        MovementDir = -1;
        yield return new WaitForSeconds(5f);
        MovementDir = 1;
    }
}
