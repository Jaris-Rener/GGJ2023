namespace LemonBerry
{
    using System;
    using System.Collections;
    using Pathfinding;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class WaterDroplet : MonoBehaviour
    {
        [SerializeField] private GameObject _plantEffect;
        [SerializeField] private float _minFollowDistance = 1;
        [SerializeField] private float _maxFollowDistance = 2;

        private Transform _followTransform;
        private Vector3 _offset;
        private AIDestinationSetter _aiDestinationSetter;

        public event Action<WaterDroplet> OnEnteredSeed;

        private void Awake()
        {
            _followTransform = new GameObject("FollowTransform").transform;
            _aiDestinationSetter = GetComponent<AIDestinationSetter>();
            _aiDestinationSetter.target = _followTransform;
            _offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(CameraController.Instance.Camera.transform.forward, Vector3.up);
        }

        private void Start()
        {
            _followRoutine = StartCoroutine(FollowPlayer());
            UpdateTarget();
        }

        public void UpdateTarget()
        {
            var dest = PlayerController.Instance.transform.position + _offset*Random.Range(_minFollowDistance, _maxFollowDistance);
            _followTransform.position = dest;
        }

        public IEnumerator FollowPlayer()
        {
            while (true)
            {
                UpdateTarget();
                yield return new WaitForSeconds(Random.Range(5.5f, 7.5f));
            }
        }

        private IGrowable _target;
        private Coroutine _followRoutine;

        public void CommandTo(IGrowable growable)
        {
            _target = growable;
            StopCoroutine(_followRoutine);
            _followRoutine = StartCoroutine(CheckForSeed());
        }

        public IEnumerator CheckForSeed()
        {
            yield return null;
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