using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _characterMovement;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    private bool detected;

    private IEnumerator _currentState;
    [SerializeField] private Transform player;
    private Vector3 _defaultLocation;
    private float _nextIdleActionTime = 0;
    private float _destinationReachedThreshold = 5;
    [SerializeField] private float _idleMoveRadius = 10;
    [SerializeField] private float _idleTimeMin = 5;
    [SerializeField] private float _idleTimeMax = 10;
    [SerializeField] private float _attackRangeMax = 5;
    [SerializeField] private float _attackRangeMin = 0;
    [SerializeField] private float _chaseSpeed = 5;
    [SerializeField] private float _wanderSpeed = 1;

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").transform;
        _defaultLocation = gameObject.transform.position;
        _wanderSpeed = _navMeshAgent.speed;
    }

    private void Start()
    {
        ChangeState(Wander());
    }


    private void ChangeState(IEnumerator newState)
    {
        if (_currentState != null) StopCoroutine(_currentState);

        _currentState = newState;
        StartCoroutine(_currentState);
    }
    private IEnumerator Wander()
    {
        _navMeshAgent.speed = _wanderSpeed;
        Vector3 destination = gameObject.transform.position;


        while (true)
        {
            float curTime = Time.time;
            //print(_nextIdleActionTime - Time.time + " || " + destination);
            if (_nextIdleActionTime < curTime)
            {
                //calculate idle wander location
                _nextIdleActionTime = Random.Range(_idleTimeMin, _idleTimeMax) + Time.time;
                Vector3 randPos = (new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)) * _idleMoveRadius);
                destination = _defaultLocation + randPos;
            }

            //move to idle wander location
            if (Vector3.Distance(destination, gameObject.transform.position) < _destinationReachedThreshold && _characterMovement != null)
            {
                if (_navMeshAgent.isOnNavMesh)
                {
                    _navMeshAgent.ResetPath();
                }
            }
            else if (_characterMovement != null)
            {
                if (_navMeshAgent.isOnNavMesh)
                {
                    _navMeshAgent.SetDestination(destination);
                }
            }

            if (detected)
            {
                //TODO:Playdetect anim and Wait seconds
                ChangeState(Chase());
            }

            yield return null;
        }
    }

    private IEnumerator Chase()
    {
        _navMeshAgent.speed = _chaseSpeed;
        while (true) 
        { 
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) < _attackRangeMax && _characterMovement != null)
            {

                if (Vector3.Distance(player.transform.position, gameObject.transform.position) < _attackRangeMin && _navMeshAgent.isOnNavMesh)
                {
                    print("Running");
                    _navMeshAgent.SetDestination(gameObject.transform.position - (player.transform.position - gameObject.transform.position).normalized * Random.Range(_attackRangeMin + 0.5f,_attackRangeMax - 0.5f));
                }
                else if (_navMeshAgent.isOnNavMesh)
                {
                    _navMeshAgent.ResetPath();
                }
            }
            else if (_characterMovement != null)
            {
                if (_navMeshAgent.isOnNavMesh)
                {
                    _navMeshAgent.SetDestination(player.transform.position);
                }
            }

            yield return null;
        }
    }

    public void Detected(bool _true)
    {
        detected = _true;
        print("Player detected, " + _true);
        if (!detected) ChangeState(Wander());
    }

    private Vector3 _Pdestination = Vector3.zero;
    public void Paused()
    {
        if (_navMeshAgent.isOnNavMesh)
        {
            _Pdestination = _navMeshAgent.destination;
            _navMeshAgent.ResetPath();
        }
        StopAllCoroutines();
    }
    public void Play()
    {
        if (_navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.SetDestination(_Pdestination);
        }
        ChangeState(_currentState);
    }
}
