using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    private StateMachine brain;

    private Animator _animator;
    [SerializeField]
    private Text stateNote;

    private NavMeshAgent _agent;

    private Player _player;

    private bool playerIsNear;

    private bool withinAttackRange;

    private float changeMind;

    private float attackTimer;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        brain = GetComponent<StateMachine>();
        playerIsNear = false;
        withinAttackRange = false;
        brain.PushState(Idle, OnIdleEnter, OnIdleExit);
    }

    // Update is called once per frame
    void Update()
    {
        playerIsNear = Vector3.Distance(transform.position, _player.transform.position) < 5;
        withinAttackRange = Vector3.Distance(transform.position, _player.transform.position) < 1;
    }

    void OnIdleEnter()
    {
        stateNote.text = "Idle";
        _agent.ResetPath();
    }

    void Idle()    
    {
        changeMind -= Time.deltaTime;
        if (playerIsNear)
        {
            brain.PushState(Chase, OnChaseEnter, OnChaseExit);
        }
        else if (changeMind <= 0)
        {
            brain.PushState(Wander, OnWanderEnter, OnWanderExit);
            changeMind = Random.Range(4, 10);
        }
    }

    void OnIdleExit()
    {
        
    }

    void Chase()
    {
        _agent.SetDestination(_player.transform.position);
        if (Vector3.Distance(transform.position, _player.transform.position) > 5.5f)
        {
            brain.PopState();
            brain.PushState(Idle, OnIdleEnter, OnIdleExit);
        }

        if (withinAttackRange)
        {
            brain.PushState(Attack, OnAttackEnter, null);
        }
    }

    void OnChaseEnter()
    {
        _animator.SetBool("Chase", true);
        stateNote.text = "Chase";
    }

    void OnChaseExit()
    {
        _animator.SetBool("Chase", false);
    }

    void Wander()
    {
        
        if (_agent.remainingDistance <= .25f)
        {
            _agent.ResetPath();
            brain.PushState(Idle, OnIdleEnter, OnIdleExit);
        }

        if (playerIsNear)
        {
            brain.PushState(Chase, OnChaseEnter, OnChaseExit);
        }
    }

    void OnWanderEnter()
    {
        stateNote.text = "Wander";
        _animator.SetBool("Chase", true);
        Vector3 wanderDirection = (Random.onUnitSphere * 4f) + transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(wanderDirection, out navMeshHit, 3f, NavMesh.AllAreas);
        Vector3 destination = navMeshHit.position;
        _agent.SetDestination(destination);
    }
    void OnWanderExit()
    {
        _animator.SetBool("Chase", false);
    }

    void Attack()
    {
        attackTimer -= Time.deltaTime;
        if (!withinAttackRange)
        {
            brain.PopState();
        } else if (attackTimer <= 0)
        {
            _animator.SetTrigger("Attack");
            _player.Hurt(2, 1f);
            attackTimer = 2f;
        }
    }

    void OnAttackEnter()
    {
        _agent.ResetPath();
        stateNote.text = "Attack";
    }
}
