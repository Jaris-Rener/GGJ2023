namespace LemonBerry
{
    using UnityEngine;

    public interface IRespawn
    {
        public void OnRespawn();
    }

    public class KillBox : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var respawnable = other.GetComponent<IRespawn>();
            if (respawnable == null)
                return;

            respawnable.OnRespawn();
        }
    }

}