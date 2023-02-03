namespace LemonBerry
{
    using UnityEngine;

    public class Interactable : MonoBehaviour
    {
        public virtual void Interact(){}

        public virtual void OnHoveredStart(){}
        public virtual void OnHoveredStop(){}
    }
}