namespace LemonBerry
{
    using UnityEngine;

    public class EndTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            PlayerController.Instance.enabled = false;
            GameManager.Instance.EndLevel();
        }
    }
}