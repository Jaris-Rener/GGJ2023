namespace LemonBerry
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using DG.Tweening;
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

    public class Seed : Grabbable, IGrowable, IRespawn
    {
        private Color _startColor;

        public UnityEvent OnGrown;
        public UnityEvent OnUnGrown;

        [SerializeField] private AudioClip _growSound;
        [SerializeField] private AudioClip _unGrowSound;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private bool _faceUpWhenGrown;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private int _growCost = 1;

        private readonly List<WaterDroplet> _droplets = new();

        private bool _isGrown;
        private Vector3 _startPos;

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

        private void Start()
        {
            _startColor = _meshRenderer.material.color;
            _startPos = transform.position;
        }

        public void Grow()
        {
            if (IsGrown)
                return;

            IsGrown = true;
            StartCoroutine(GrowRoutine());
        }

        public void UnGrow()
        {
            if (!IsGrown)
                return;

            IsGrown = false;
            _meshRenderer.material.color = _startColor;
            Rigidbody.isKinematic = false;
            OnUnGrown?.Invoke();

            foreach (var droplet in _droplets)
            {
                droplet.gameObject.SetActive(true);
                droplet.StartCoroutine(droplet.FollowPlayer());
                PlayerController.Instance.AddFollower(droplet);
            }

            _audioSource.PlayOneShot(_unGrowSound);
            _droplets.Clear();
        }

        public void AddWater(WaterDroplet waterDroplet)
        {
            _droplets.Add(waterDroplet);
            if (_droplets.Count >= GrowCost)
                Grow();
        }

        public override void OnHoveredStart()
        {
            _meshRenderer.material.color = Color.red;
        }

        public override void OnHoveredStop()
        {
            _meshRenderer.material.color = _startColor;
        }

        private IEnumerator GrowRoutine()
        {
            _meshRenderer.material.color = Color.green;
            Rigidbody.isKinematic = true;

            if (_faceUpWhenGrown)
                yield return transform
                    .DORotate(PlayerController.Instance.transform.eulerAngles, 0.1f)
                    .WaitForCompletion();

            _audioSource.PlayOneShot(_growSound);
            OnGrown?.Invoke();
        }

        public void OnRespawn()
        {
            transform.position = _startPos;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.isKinematic = false;
        }
    }
}