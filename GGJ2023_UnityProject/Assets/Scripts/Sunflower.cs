namespace LemonBerry
{
    using System.Linq;
    using UnityEngine;


    public class Sunflower : MonoBehaviour
    {
        [SerializeField] private Transform _sunOrigin;
        [SerializeField] private float _radius = 1;
        [SerializeField] private float _distance = 10;

        private void Update()
        {
            var raycastCount = 8;
            for (int i = 0; i < raycastCount; i++)
            {
                const float radius = 0.35f;
                var origin = _sunOrigin.TransformPoint(new Vector3(Mathf.Cos(i), Mathf.Sin(i), 0)*radius);
                var ray = new Ray(origin, _sunOrigin.forward);
                Debug.DrawRay(origin, _sunOrigin.forward, Color.red);
                if (Physics.Raycast(ray, out var hit, maxDistance: _distance))
                {
                    var sunReceiver = hit.transform.GetComponent<ISunReceiver>();
                    if (sunReceiver == null)
                        return;

                    sunReceiver.OnSunlightReceived();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_sunOrigin.position, _radius);
            Gizmos.DrawLine(_sunOrigin.position, _sunOrigin.position + _sunOrigin.forward*_distance);
        }
    }
}