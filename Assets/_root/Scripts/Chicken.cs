using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : Collectable
{
    enum ChickenState
    {
        None = 0,
        Patrolling = 1,
        Idle = 2
    }

    [SerializeField] float closeEnoughDistance = 1f;
    [SerializeField] float minIdleWaitTime = 3f;
    [SerializeField] float maxIdleWaitTime = 10f;

    [Header("Laying")]
    [SerializeField] Transform layLocation;
    [SerializeField] Egg[] layableEggs;

    Animator _animator;
    NavMeshAgent _navMeshAgent;

    ChickenState chickenState = ChickenState.None;

    ChickenState CurrentState
    {
        get => chickenState;
        set
        {
            if (value != chickenState)
            {
                chickenState = value;
                UpdateState();
            }
        }
    }

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetFloat("animationOffset", Random.value);
        CurrentState = ChickenState.Patrolling;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.OnCollectableCollected(this);
            Destroy(gameObject);
        }
    }

    void UpdateState()
    {
        // Debug.Log($"Chicken {gameObject.name} state: {CurrentState}");

        _animator.SetBool("isWalking", CurrentState == ChickenState.Patrolling);

        switch (CurrentState)
        {
            case ChickenState.Patrolling:
                StartCoroutine(PatrollingRoutine());
                break;
            case ChickenState.Idle:
                StartCoroutine(IdleRoutine());
                break;
        }
    }

    IEnumerator PatrollingRoutine()
    {
        bool success = RandomPoint(transform.position, 10f, out Vector3 walkToPoint);
        _navMeshAgent.SetDestination(walkToPoint);
        yield return new WaitUntil(() => HasReachedTarget());
        CurrentState = ChickenState.Idle;
    }

    IEnumerator IdleRoutine()
    {
        LayAnEgg();
        yield return new WaitForSeconds(Random.Range(minIdleWaitTime, maxIdleWaitTime));
        CurrentState = ChickenState.Patrolling;
    }

    void LayAnEgg()
    {
        if (layableEggs == null || layableEggs.Length == 0) Debug.LogError($"{nameof(Chicken)} there is nothing to lay", gameObject);

        int eggIndex = Random.Range(0, layableEggs.Length);
        Egg eggPrefab = layableEggs[eggIndex];

        Instantiate(eggPrefab, layLocation.position, Quaternion.Euler(Vector3.up * Random.Range(0, 360)));
    }

    bool HasReachedTarget()
    {
        return Vector3.Distance(transform.position, _navMeshAgent.destination) < closeEnoughDistance;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}