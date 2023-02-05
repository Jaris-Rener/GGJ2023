namespace LemonBerry
{
    using System.Collections;
    using DG.Tweening;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class HUD : Singleton<HUD>
    {
        [SerializeField] private Image _screenWipe;
        [SerializeField] private TextMeshProUGUI _promptText;
        [SerializeField] private TextMeshProUGUI _waterCountText;

        private void Start()
        {
            GameManager.Instance.OnLevelComplete += OnLevelComplete;
            PlayerController.Instance.OnHoveredInteractable += UpdatePrompt;
        }

        private void OnDestroy()
        {
            PlayerController.Instance.OnHoveredInteractable -= UpdatePrompt;
            GameManager.Instance.OnLevelComplete -= OnLevelComplete;
        }

        private void OnLevelComplete()
        {
            StartCoroutine(LevelComplete());

            IEnumerator LevelComplete()
            {
                yield return _screenWipe.transform.DOScale(0.0f, 1.0f).WaitForCompletion();
                GameManager.Instance.StartLevel();
                yield return new WaitForSeconds(1.5f);
                yield return _screenWipe.transform.DOScale(3.0f, 1.0f).WaitForCompletion();
            }
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