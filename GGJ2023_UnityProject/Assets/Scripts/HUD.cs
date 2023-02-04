namespace LemonBerry
{
    using System;
    using TMPro;
    using UnityEngine;

    public class HUD : Singleton<HUD>
    {
        [SerializeField] private TextMeshProUGUI _promptText;
        [SerializeField] private TextMeshProUGUI _waterCountText;

        private void Start()
        {
            PlayerController.Instance.OnHoveredInteractable += UpdatePrompt;
        }

        private void OnDestroy()
        {
            PlayerController.Instance.OnHoveredInteractable -= UpdatePrompt;
        }

        private void Update()
        {
            _waterCountText.text = PlayerController.Instance.Droplets.ToString();
            if (_promptInteractable == null)
            {
                _promptText.text = string.Empty;
                return;
            }

            _promptText.text = _promptInteractable.Prompt;
        }

        private Interactable _promptInteractable;
        private void UpdatePrompt(Interactable obj)
        {
            _promptInteractable = obj;
        }
    }

}