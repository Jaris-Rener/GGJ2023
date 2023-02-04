namespace LemonBerry
{
    using UnityEngine;

    public class Ice : MonoBehaviour, ISunReceiver
    {
        public void OnSunlightReceived()
        {
            gameObject.SetActive(false);
        }
    }
}