namespace LemonBerry
{
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    public class CameraController : Singleton<CameraController>
    {
        public Camera Camera => _camera;
        private Camera _camera;

        public override void Awake()
        {
            base.Awake();
            _camera = GetComponent<Camera>();
        }
    }
}