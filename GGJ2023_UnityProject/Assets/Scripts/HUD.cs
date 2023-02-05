namespace LemonBerry
{
    using System.Collections;
    using DG.Tweening;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class HUD : Singleton<HUD>
    {
        [SerializeField] private Image _screenWipe;
        [SerializeField] private TextMeshProUGUI _promptText;
        [SerializeField] private TextMeshProUGUI _waterCountText;

        private void Start()
        {
            GameManager.Instance.OnLevelComplete += OnLevelComplete;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnLevelComplete -= OnLevelComplete;
        }

        private void OnLevelComplete()
        {
            StartCoroutine(LevelComplete());

            IEnumerator LevelComplete()
            {
                yield return TransitionIn();
                SceneManager.LoadScene("StartMenu");
                yield return new WaitForSeconds(1.5f);
                yield return TransitionOut();
            }
        }

        private void Update()
        {
            if (PlayerController.Instance == null)
                return;

            _waterCountText.text = PlayerController.Instance.Droplets.ToString();
            var interactable = PlayerController.Instance.HoveredInteractable;
            if (interactable == null)
            {
                _promptText.text = string.Empty;
                return;
            }

            _promptText.text = interactable.Prompt;
        }

        public IEnumerator TransitionIn()
        {
            yield return _screenWipe.transform.DOScale(0.0f, 1.0f).WaitForCompletion();
        }

        public IEnumerator TransitionOut()
        {
            yield return _screenWipe.transform.DOScale(3.0f, 1.0f).WaitForCompletion();
        }
    }

}