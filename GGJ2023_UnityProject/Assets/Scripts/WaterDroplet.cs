namespace LemonBerry
{
    using System;
    using System.Collections;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class WaterDroplet : MonoBehaviour
    {
        [SerializeField] private GameObject _plantEffect;
        private Vector3 _offset;

        private IGrowable _target;

        private void Update()
        {
            if (_target != null && Vector3.Distance(transform.position, _target.Position) > 5)
            {
                transform.position = _target.Position;
            }

            // transform.rotation = Quaternion.LookRotation(CameraController.Instance.Camera.transform.forward, Vector3.up);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var growable = collision.transform.GetComponent<IGrowable>();
            if (growable == null)
                return;

            if (growable == _target)
            {
                OnArrivedAtSeed(_target);
            }
        }

        public event Action<WaterDroplet> OnEnteredSeed;

        public IEnumerator FollowPlayer()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(0.3f, 1f));
            }
        }

        public void CommandTo(IGrowable growable)
        {
            growable.PendingWater++;
            _target = growable;
            OnArrivedAtSeed(_target);
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