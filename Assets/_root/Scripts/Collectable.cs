using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] public float movementHeight = 1f;
    [SerializeField] public float speed = 1f;

    float originalY;
    float animOffset;
    void Start()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value, 1f);

        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        renderer.material.color = randomColor;

        originalY = transform.position.y;
        animOffset = Random.value;
    }

    // void Update()
    // {
    //     transform.position = new Vector3(
    //         transform.position.x,
    //         originalY + Mathf.PingPong((Time.realtimeSinceStartup + animOffset) * speed, movementHeight),
    //         transform.position.z
    //     );
    // }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.OnCollectableCollected(this);
            Destroy(gameObject);
        }
    }
}
