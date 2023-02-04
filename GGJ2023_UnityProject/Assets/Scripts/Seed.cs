namespace LemonBerry
{
    using System.Collections;
    using System.Collections.Generic;
    using DG.Tweening;
    using Unity.AI.Navigation;
    using UnityEngine;
    using UnityEngine.Events;

    public interface IGrowable
    {
        public Vector3 Position { get; }
        public int GrowCost { get; }
        bool IsGrown { get; set; }
        int RemainingGrowCost { get; }
        void Grow();
        void UnGrow();
        void AddWater(WaterDroplet waterDroplet);
    }

    public class Seed : Grabbable, IGrowable
    {
        public UnityEvent OnGrown;
        public UnityEvent OnUnGrown;

        [SerializeField] private bool _faceUpWhenGrown;
        [SerializeField] private NavMeshSurface _navSurface;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private int _growCost = 1;

        private bool _isGrown;

        public Vector3 Position => transform.position;
        public int GrowCost => _growCost;
        public bool IsGrown
        {
            get => _isGrown;
            set
            {
                CanGrab = !value;
                _isGrown = value;
            }
        }

        public int RemainingGrowCost => GrowCost - _droplets.Count;

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
            StartCoroutine(GrowRoutine());
        }

        private IEnumerator GrowRoutine()
        {
            _meshRenderer.material.color = Color.green;
            Rigidbody.isKinematic = true;
            _navSurface.BuildNavMesh();

            if (_faceUpWhenGrown)
                yield return transform
                    .DORotate(Vector3.zero, 0.1f)
                    .WaitForCompletion();

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

            foreach (var droplet in _droplets)
            {
                droplet.gameObject.SetActive(true);
                droplet.StartCoroutine(droplet.FollowPlayer());
                PlayerController.Instance.AddFollower(droplet);
            }
        }

        private readonly List<WaterDroplet> _droplets = new();
        public void AddWater(WaterDroplet waterDroplet)
        {
            _droplets.Add(waterDroplet);
            if (_droplets.Count >= GrowCost)
                Grow();
        }
    }
}