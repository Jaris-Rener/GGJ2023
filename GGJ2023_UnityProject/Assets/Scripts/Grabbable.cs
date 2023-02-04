namespace LemonBerry
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class Grabbable : Interactable
    {
        public override string Prompt => IsHeld ? "Drop" : "Pickup";
        protected Rigidbody Rigidbody;
        public bool CanGrab { get; set; } = true;
        public bool IsHeld { get; set; }

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