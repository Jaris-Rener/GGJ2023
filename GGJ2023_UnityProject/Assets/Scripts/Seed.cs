namespace LemonBerry
{
    using UnityEngine;

    public class Seed : Grabbable
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
    }
}