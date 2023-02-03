namespace LemonBerry
{
    using UnityEngine;
    using UnityEngine.Events;

    public interface IGrowable
    {
        bool IsGrown { get; set; }
        void Grow();
        void UnGrow();
    }

    public class Seed : Grabbable, IGrowable
    {
        public UnityEvent OnGrown;
        public UnityEvent OnUnGrown;

        [SerializeField] private MeshRenderer _meshRenderer;
        private bool _isGrown;

        public bool IsGrown
        {
            get => _isGrown;
            set
            {
                CanGrab = !value;
                _isGrown = value;
            }
        }

        public override void OnHoveredStart()
        {
            _meshRenderer.material.color = Color.red;
        }

        public override void OnHoveredStop()
        {
            _meshRenderer.material.color = Color.white;
        }

        public void Grow()
        {
            if (IsGrown)
                return;

            IsGrown = true;
            _meshRenderer.material.color = Color.green;
            Rigidbody.isKinematic = true;
            OnGrown?.Invoke();
        }

        public void UnGrow()
        {
            if (!IsGrown)
                return;

            IsGrown = false;
            _meshRenderer.material.color = Color.white;
            Rigidbody.isKinematic = false;
            OnUnGrown?.Invoke();
        }
    }
}