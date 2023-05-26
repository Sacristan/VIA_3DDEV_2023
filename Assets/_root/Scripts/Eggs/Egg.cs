using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Egg : MonoBehaviour
{
    [SerializeField] GameObject explosionEffect;

    public virtual void DoEffect(Player player)
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();

            if (player != null)
            {
                DoEffect(player);
            }

            Destroy(gameObject);
        }

    }
}
