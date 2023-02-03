namespace LemonBerry
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class Grabbable : Interactable
    {
        public bool CanGrab { get; set; } = true;
        public bool IsHeld { get; set; }

        protected Rigidbody Rigidbody;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public override void Interact()
        {

        }

        public void Grab()
        {
            IsHeld = true;
            Rigidbody.isKinematic = true;
        }

        public void Release()
        {
            IsHeld = false;
            Rigidbody.isKinematic = false;
        }
    }
}