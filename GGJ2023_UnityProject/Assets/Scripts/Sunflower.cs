namespace LemonBerry
{
    using UnityEngine;


    public class Sunflower : MonoBehaviour
    {
        [SerializeField] private Transform _sunOrigin;
        [SerializeField] private float _radius = 1;
        [SerializeField] private float _distance = 10;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_sunOrigin.position, _radius);
            Gizmos.DrawLine(_sunOrigin.position, _sunOrigin.position + _sunOrigin.forward*_distance);
        }

        private void Update()
        {
            if (Physics.SphereCast(_sunOrigin.position, _radius, _sunOrigin.forward, out var hit, _distance))
            {
                var sunReceiver = hit.transform.GetComponent<ISunReceiver>();
                if (sunReceiver == null)
                    return;

                sunReceiver.OnSunlightReceived();
            }
        }
    }
}