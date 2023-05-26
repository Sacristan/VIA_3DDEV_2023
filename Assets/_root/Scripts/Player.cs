using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
public class Player : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.OnGameWon += GameWon;
    }

    void GameWon()
    {
        GetComponent<CharacterController>().enabled = false;
        GetComponent<FirstPersonController>().enabled = false;
        GetComponent<PlayerInput>().enabled = false;
    }

    public void DoExplosionEffect()
    {
        Debug.Log(nameof(DoExplosionEffect));
    }

    public void DoIntoxicatedEffect()
    {
        Debug.Log(nameof(DoIntoxicatedEffect));
    }
}
