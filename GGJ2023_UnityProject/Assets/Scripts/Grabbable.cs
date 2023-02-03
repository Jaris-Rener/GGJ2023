namespace LemonBerry
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class Grabbable : Interactable
    {
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public override void Interact()
        {

        }

        public void Grab()
        {
            _rigidbody.isKinematic = true;
        }

        public void Release()
        {
            _rigidbody.isKinematic = false;
        }
    }
}