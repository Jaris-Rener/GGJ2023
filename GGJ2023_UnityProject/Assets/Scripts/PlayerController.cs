namespace LemonBerry
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.InputSystem;

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _grabPoint;

        [SerializeField] private Transform _interactionPoint;
        [SerializeField] private float _interactionRadius = 1;
        [SerializeField] private LayerMask _interactionLayers;

        [SerializeField] private InputActionReference _jumpInput;
        [SerializeField] private InputActionReference _moveInput;
        [SerializeField] private InputActionReference _interactInput;
        [SerializeField] private float _jumpForce = 100;
        [SerializeField] private float _moveSpeed = 5;
        [SerializeField] private float _rotSpeed = 15;

        private readonly List<Interactable> _hoveredInteractables = new();
        private Rigidbody _rigidbody;
        private Vector2 _moveInputVec;
        private Vector3 _moveDir;

        private Grabbable _heldObject;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _jumpInput.action.performed += Jump;
            _interactInput.action.performed += Interact;
        }

        private void OnDestroy()
        {
            _jumpInput.action.performed -= Jump;
            _interactInput.action.performed -= Interact;
        }

        private void Interact(InputAction.CallbackContext obj)
        {
            if (_heldObject != null)
            {
                ReleaseHeldObject();
                return;
            }

            var interactable = _hoveredInteractables[0];
            if (interactable == null)
                return;

            interactable.Interact();
            if (interactable is Grabbable grabbable)
            {
                grabbable.Grab();
                Physics.IgnoreCollision(
                    interactable.GetComponent<Collider>(),
                    GetComponent<Collider>(),
                    ignore: true);

                grabbable.transform.SetParent(_grabPoint);
                grabbable.transform.localPosition = Vector3.zero;
                _heldObject = grabbable;
            }
        }

        private void ReleaseHeldObject()
        {
            _heldObject.Release();
            _heldObject.transform.SetParent(null);

            Physics.IgnoreCollision(
                _heldObject.GetComponent<Collider>(),
                GetComponent<Collider>(),
                ignore: false);

            _heldObject = null;
        }

        private void Update()
        {
            _moveInputVec = _moveInput.action.ReadValue<Vector2>();
            PlayerLook();
            CheckInteractionArea();
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

        private void Jump(InputAction.CallbackContext obj)
        {
            _rigidbody.AddForce(Vector3.up*_jumpForce);
        }
    }
}