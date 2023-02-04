namespace LemonBerry
{
    using System.Collections.Generic;
    using System.Linq;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.InputSystem;

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : Singleton<PlayerController>, IRespawn
    {
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _grabSound;
        [SerializeField] private AudioClip _dropSound;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _grabPoint;

        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private Transform _interactionPoint;
        [SerializeField] private float _interactionRadius = 1;
        [SerializeField] private LayerMask _interactionLayers;
        [SerializeField] private LayerMask _groundLayers;

        [SerializeField] private InputActionReference _jumpInput;
        [SerializeField] private InputActionReference _moveInput;
        [SerializeField] private InputActionReference _interactInput;
        [SerializeField] private InputActionReference _addWaterInput;
        [SerializeField] private InputActionReference _removeWaterInput;
        [SerializeField] private float _jumpForce = 100;
        [SerializeField] private float _moveSpeed = 5;
        [SerializeField] private float _rotSpeed = 15;

        [SerializeField] private AudioSource _walkingAudioSource;
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private List<WaterDroplet> _followers;

        private readonly List<Interactable> _hoveredInteractables = new();
        private bool _grounded;

        private Grabbable _heldObject;
        private Vector3 _moveDir;
        private Vector2 _moveInputVec;
        private Rigidbody _rigidbody;
        private Vector3 _startPos;

        private void Start()
        {
            _startPos = transform.position;
            _rigidbody = GetComponent<Rigidbody>();
            _jumpInput.action.performed += Jump;
            _interactInput.action.performed += Interact;
            _addWaterInput.action.performed += AddWater;
            _removeWaterInput.action.performed += RemoveWater;

            foreach (var water in _followers)
            {
                water.OnEnteredSeed += RemoveFollower;
            }
        }

        private void Update()
        {
            _moveInputVec = _moveInput.action.ReadValue<Vector2>();
            _animator.SetBool("Walking", _moveInputVec != Vector2.zero);
            PlayerLook();
            GroundedCheck();
            CheckInteractionArea();

            if (_moveDir == Vector3.zero || !_grounded)
            {
                _walkingAudioSource.Stop();
            }
            else if (_walkingAudioSource.isPlaying == false)
            {
                _walkingAudioSource.Play();
            }
        }

        private void FixedUpdate()
        {
            var cam = CameraController.Instance.Camera;
            var forward = cam.transform.forward;
            var right = cam.transform.right;
            forward.y = right.y = 0f;
            forward.Normalize();
            right.Normalize();

            _moveDir = forward*_moveInputVec.y + right*_moveInputVec.x;
            _rigidbody.position += _moveDir*_moveSpeed*Time.deltaTime;
        }

        private void OnDestroy()
        {
            _jumpInput.action.performed -= Jump;
            _interactInput.action.performed -= Interact;
            _addWaterInput.action.performed -= AddWater;
            _removeWaterInput.action.performed -= RemoveWater;
        }

        private void OnGUI()
        {
            foreach (var seed in FindObjectsOfType<Seed>())
            {
                GUILayout.Label($"{seed.name}: {seed.GrowCost - seed.RemainingGrowCost}/{seed.GrowCost}");
            }
        }

        private void WalkStop(InputAction.CallbackContext obj)
        {
            _audioSource.Stop();
        }

        private void WalkStart(InputAction.CallbackContext obj)
        {
            _audioSource.Play();
        }

        private void RemoveFollower(WaterDroplet obj)
        {
            obj.OnEnteredSeed -= RemoveFollower;
            _followers.Remove(obj);
        }

        private void AddWater(InputAction.CallbackContext obj)
        {
            if (_heldObject != null)
                return;

            var interactable = _hoveredInteractables.FirstOrDefault();
            if (interactable == null)
                return;

            if (interactable is IGrowable growable)
            {
                if (interactable is Grabbable { IsHeld: true })
                    return;

                for (int i = 0; i < Mathf.Min(growable.RemainingGrowCost, _followers.Count); i++)
                {
                    _followers[i].CommandTo(growable);
                }
            }
        }

        private void RemoveWater(InputAction.CallbackContext obj)
        {
            if (_heldObject != null)
                return;

            var interactable = _hoveredInteractables.FirstOrDefault();
            if (interactable == null)
                return;

            if (interactable is IGrowable growable)
            {
                growable.UnGrow();
            }
        }

        private void Interact(InputAction.CallbackContext obj)
        {
            if (_heldObject != null)
            {
                ReleaseHeldObject();
                return;
            }

            var interactable = _hoveredInteractables.FirstOrDefault();
            if (interactable == null)
                return;

            interactable.Interact();
            if (interactable is Grabbable grabbable)
            {
                if (!grabbable.CanGrab)
                    return;

                grabbable.Grab();
                Physics.IgnoreCollision(
                    interactable.GetComponent<Collider>(),
                    GetComponent<Collider>(),
                    ignore: true);

                _audioSource.PlayOneShot(_grabSound);
                grabbable.transform.SetParent(_grabPoint);
                grabbable.transform.DOKill();
                grabbable.transform.DOLocalMove(Vector3.zero, 0.1f);
                grabbable.transform.DOLocalRotate(new Vector3(90, 0, 0), 0.1f);
                _heldObject = grabbable;
            }
        }

        private void ReleaseHeldObject()
        {
            if (_heldObject == null)
                return;

            _audioSource.PlayOneShot(_dropSound);
            _heldObject.Release();
            _heldObject.transform.SetParent(null);

            Physics.IgnoreCollision(
                _heldObject.GetComponent<Collider>(),
                GetComponent<Collider>(),
                ignore: false);

            _heldObject = null;
        }

        private void GroundedCheck()
        {
            _grounded = Physics.Raycast(
                _groundCheckPoint.position,
                Vector3.down,
                maxDistance: 0.2f,
                _groundLayers.value);
        }

        private void PlayerLook()
        {
            if (_moveDir == Vector3.zero)
                return;

            var rot = Quaternion.LookRotation(_moveDir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.smoothDeltaTime*_rotSpeed);
        }

        private void CheckInteractionArea()
        {
            if (_heldObject != null)
                return;

            var colliders = Physics
                .OverlapSphere(_interactionPoint.position, _interactionRadius, _interactionLayers)
                .ToList();

            foreach (var obj in colliders)
            {
                var interactable = obj.GetComponent<Interactable>();
                if (interactable == null)
                    continue;

                if (!_hoveredInteractables.Contains(interactable))
                {
                    _hoveredInteractables.Add(interactable);
                    interactable.OnHoveredStart();
                }
            }

            for (var i = _hoveredInteractables.Count - 1; i >= 0; i--)
            {
                var interactable = _hoveredInteractables[i];
                var col = interactable.GetComponent<Collider>();
                if (!colliders.Contains(col))
                {
                    _hoveredInteractables.Remove(interactable);
                    interactable.OnHoveredStop();
                }
            }
        }

        private void Jump(InputAction.CallbackContext obj)
        {
            if (!_grounded)
                return;

            _audioSource.PlayOneShot(_jumpSound, 1);
            _rigidbody.AddForce(Vector3.up*_jumpForce);
        }

        public void AddFollower(WaterDroplet droplet)
        {
            _followers.Add(droplet);
        }

        public void OnRespawn()
        {
            ReleaseHeldObject();
            transform.position = _startPos;
        }
    }
}