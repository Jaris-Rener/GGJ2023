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
        private AIDestinationSetter _aiDestinationSetter;
        private AIPath _aiPath;
        private Coroutine _followRoutine;

        private Transform _followTransform;
        private Vector3 _offset;

        private IGrowable _target;

        private void Awake()
        {
            _aiPath = GetComponent<AIPath>();
            _followTransform = new GameObject("FollowTransform").transform;
            _aiDestinationSetter = GetComponent<AIDestinationSetter>();
            _aiDestinationSetter.target = _followTransform;
            _offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        }

        private void Start()
        {
            _followRoutine = StartCoroutine(FollowPlayer());
            UpdateTarget();
        }

        private void Update()
        {
            if (_target != null && Vector3.Distance(transform.position, _target.Position) > 5)
            {
                transform.position = _target.Position;
            }

            transform.rotation = Quaternion.LookRotation(CameraController.Instance.Camera.transform.forward, Vector3.up);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var growable = collision.transform.GetComponent<IGrowable>();
            if (growable == null)
                return;

            if (growable == _target)
            {
                StopCoroutine(_followRoutine);
                OnArrivedAtSeed(_target);
            }
        }

        public event Action<WaterDroplet> OnEnteredSeed;

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
                yield return new WaitForSeconds(Random.Range(0.3f, 1f));
            }
        }

        public void CommandTo(IGrowable growable)
        {
            growable.PendingWater++;
            _target = growable;
            OnArrivedAtSeed(_target);
            StopCoroutine(_followRoutine);
            _followTransform.position = growable.Position;
            // _followRoutine = StartCoroutine(CheckForSeed());
        }

        public IEnumerator CheckForSeed()
        {
            yield return null;
            while (_aiPath.remainingDistance > 0.5f)
            {
                yield return null;
            }

            // OnArrivedAtSeed();
        }

        private void OnArrivedAtSeed(IGrowable growable)
        {
            growable.PendingWater--;
            gameObject.SetActive(false);
            OnEnteredSeed?.Invoke(this);
            _target.AddWater(this);
            Instantiate(_plantEffect, transform.position, Quaternion.identity);
        }
    }

}