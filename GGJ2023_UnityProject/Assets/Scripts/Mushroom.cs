namespace LemonBerry
{
    using UnityEngine;

    public class Mushroom : MonoBehaviour
    {
        [SerializeField] private AudioClip _bounceSound;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _bounceForce = 300;

        private void OnCollisionEnter(Collision collision)
        {
            var rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb == null)
                return;

            _audioSource.PlayOneShot(_bounceSound);
            var normal = collision.contacts[0].normal;
            var dot = Vector3.Dot(Vector3.up, normal);
            if (dot < -0.5f)
                rb.AddForce(Vector3.up*_bounceForce);
        }
    }
}