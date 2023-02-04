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
            var hits = Physics.SphereCastAll(_sunOrigin.position, _radius, _sunOrigin.forward, _distance);
            if (hits.Any())
            {
                foreach (var hit in hits)
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