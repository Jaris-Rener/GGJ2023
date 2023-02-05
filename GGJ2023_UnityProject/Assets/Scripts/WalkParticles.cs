namespace LemonBerry
{
    using UnityEngine;

    public class WalkParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _left;
        [SerializeField] private ParticleSystem _right;

        public void EmitLeft()
        {
            if (PlayerController.Instance.IsGrounded)
                _left.Emit(5);
        }

        public void EmitRight()
        {
            if (PlayerController.Instance.IsGrounded)
                _right.Emit(5);
        }
    }
}