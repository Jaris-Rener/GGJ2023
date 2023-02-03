namespace LemonBerry
{
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

        private Collider[] _interactColliders;
        private Rigidbody _rigidbody;
        private Vector2 _moveInputVec;

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
            var item = _interactColliders[0];

            var interactable = item.GetComponent<Interactable>();
            if (interactable == null)
                return;

            interactable.Interact();
            if (interactable is Grabbable grabbable)
            {
                grabbable.Grab();
                grabbable.transform.SetParent(_grabPoint);
            }
        }

        private void Update()
        {
            _moveInputVec = _moveInput.action.ReadValue<Vector2>();
            CheckInteractionArea();
        }

        private void CheckInteractionArea()
        {
            _interactColliders = Physics.OverlapSphere(_interactionPoint.position, _interactionRadius, _interactionLayers);
        }

        private void FixedUpdate()
        {
            var cam = CameraController.Instance.Camera;
            var forward = cam.transform.forward;
            var right = cam.transform.right;
            forward.y = right.y = 0f;
            forward.Normalize();
            right.Normalize();

            var moveDir = forward*_moveInputVec.y + right*_moveInputVec.x;
            _rigidbody.position += (moveDir*_moveSpeed*Time.deltaTime);
        }

        private void Jump(InputAction.CallbackContext obj)
        {
            _rigidbody.AddForce(Vector3.up*_jumpForce);
        }
    }
}