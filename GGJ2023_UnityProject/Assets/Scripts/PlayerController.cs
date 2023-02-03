namespace LemonBerry
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputActionReference _jumpInput;
        [SerializeField] private InputActionReference _moveInput;
        [SerializeField] private float _jumpForce = 100;
        [SerializeField] private float _moveSpeed = 5;
        private Rigidbody _rigidbody;
        private Vector2 _moveInputVec;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _jumpInput.action.performed += Jump;
        }

        private void Update()
        {
            _moveInputVec = _moveInput.action.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            var cam = CameraController.Instance.Camera;
            var forward = cam.transform.forward;
            var right = cam.transform.right;
            forward.y = right.y = 0f;
            forward.Normalize();
            right.Normalize();

            var moveDir = forward * _moveInputVec.y + right * _moveInputVec.x;
            _rigidbody.MovePosition(moveDir * _moveSpeed * Time.deltaTime);
        }

        private void OnDestroy()
        {
            _jumpInput.action.performed -= Jump;
        }

        private void Jump(InputAction.CallbackContext obj)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce);
        }
    }
}