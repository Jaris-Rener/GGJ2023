namespace LemonBerry
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class WaterDroplet : MonoBehaviour
    {
        [SerializeField] private GameObject _plantEffect;
        private Vector3 _offset;
        private IGrowable _target;

        public void CommandTo(IGrowable growable)
        {
            growable.PendingWater++;
            _target = growable;
            StartCoroutine(MoveTo(_target.Transform, () => OnArrivedAtSeed(_target)));
        }

        private void Update()
        {
            var newRotation = CameraController.Instance.Camera.transform.eulerAngles;
            newRotation.x = 0;
            newRotation.z = 0;
            transform.eulerAngles = newRotation;
        }

        private void OnArrivedAtSeed(IGrowable growable)
        {
            growable.PendingWater--;
            gameObject.SetActive(false);
            _target.AddWater(this);
            Instantiate(_plantEffect, transform.position, Quaternion.identity);
        }

        public IEnumerator MoveTo(Transform target, Action onArrival = null)
        {
            transform.SetParent(null);
            while (Vector3.Distance(target.position, transform.position) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, Time.smoothDeltaTime*10f);
                yield return null;
            }

            transform.position = target.position;
            onArrival?.Invoke();
        }
    }

}