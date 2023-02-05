namespace LemonBerry 
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class StartMenu : MonoBehaviour 
    {
        //[SerializeField] private string _nextScene = "";

        public void PlayGame(string _nextScene)
        {
            SceneManager.LoadScene(_nextScene, LoadSceneMode.Single);
        }

        public void QuitGame()
        {
            Application.Quit();
            print("Quitting game. Goodbye!");
        }
    }
}