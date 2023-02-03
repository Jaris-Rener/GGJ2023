namespace LemonBerry
{
    using UnityEngine;

    public interface IGrowable
    {
        void Grow();
    }

    public class Seed : Grabbable, IGrowable
    {
        [SerializeField] private MeshRenderer _meshRenderer;

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
            _meshRenderer.material.color = Color.green;
        }
    }
}