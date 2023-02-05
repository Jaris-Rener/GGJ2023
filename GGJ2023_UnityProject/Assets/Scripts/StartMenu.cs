namespace LemonBerry 
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class StartMenu : MonoBehaviour 
    {
        //[SerializeField] private string _nextScene = "";

        public void PlayGame(string _nextScene)
        {
            StartCoroutine(StartGame());

            IEnumerator StartGame()
            {
                yield return HUD.Instance.TransitionIn();
                yield return new WaitForSeconds(1.5f);
                GameManager.Instance.StartLevel(_nextScene);
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