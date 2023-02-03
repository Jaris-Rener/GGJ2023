namespace LemonBerry
{
    using UnityEngine;

    public class Interactable : MonoBehaviour
    {
        public virtual void Interact(){}

        public void OnHoveredStart(){}
        public void OnHoveredStop(){}
    }
}