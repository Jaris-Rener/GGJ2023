namespace LemonBerry 
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class StartMenu : MonoBehaviour 
    {
        public void Start() {
            Cursor.lockState = CursorLockMode.Confined;
        }

        public void PlayGame(string _nextScene)
        {
            HUD.Instance.StartCoroutine(StartGame());
            Cursor.lockState = CursorLockMode.Locked;

            IEnumerator StartGame()
            {
                yield return HUD.Instance.TransitionIn();
                GameManager.Instance.StartLevel(_nextScene);
                yield return new WaitForSeconds(1.5f);
                yield return HUD.Instance.TransitionOut();
            }
        }

        public void QuitGame()
        {
            Application.Quit();
            print("Quitting game. Goodbye!");
        }
    }
}