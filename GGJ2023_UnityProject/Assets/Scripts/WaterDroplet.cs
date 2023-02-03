namespace LemonBerry
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.AI;
    using Random = UnityEngine.Random;

    [RequireComponent(typeof(NavMeshAgent))]
    public class WaterDroplet : MonoBehaviour
    {
        [SerializeField] private GameObject _plantEffect;
        [SerializeField] private float _minFollowDistance = 1;
        [SerializeField] private float _maxFollowDistance = 2;
        private NavMeshAgent _navAgent;
        private Vector3 _offset;

        public event Action<WaterDroplet> OnEnteredSeed;

        private void Awake()
        {
            _offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            _navAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(CameraController.Instance.Camera.transform.forward, Vector3.up);
        }

        private void Start()
        {
            _followRoutine = StartCoroutine(FollowPlayer());
        }

        public IEnumerator FollowPlayer()
        {
            while (true)
            {
                _navAgent.SetDestination(PlayerController.Instance.transform.position + _offset*Random.Range(_minFollowDistance, _maxFollowDistance));
                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            }
        }

        private IGrowable _target;
        private Coroutine _followRoutine;

        public void CommandTo(IGrowable growable)
        {
            _target = growable;
            StopCoroutine(_followRoutine);
            _navAgent.SetDestination(growable.Position);
            _followRoutine = StartCoroutine(CheckForSeed());
        }

        public IEnumerator CheckForSeed()
        {
            while (_navAgent.remainingDistance > 0.1f)
            {
                yield return null;
            }

            OnArrivedAtSeed();
        }

        private void OnArrivedAtSeed()
        {
            gameObject.SetActive(false);
            OnEnteredSeed?.Invoke(this);
            _target.AddWater(this);
            Instantiate(_plantEffect, transform.position, Quaternion.identity);
        }
    }

}