using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Collectable : MonoBehaviour
{
    enum ChickenState
    {
        None,
        Patrolling,
        Idle
    }

    [SerializeField] float closeEnoughDistance = 1f;
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
        yield return new WaitForSeconds(Random.Range(3f, 10f));
        CurrentState = ChickenState.Patrolling;
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