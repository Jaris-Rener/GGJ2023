namespace LemonBerry 
{
    using UnityEngine;

    public class StartMenu : MonoBehaviour 
    {
        public void PlayGame()
        {

        }

        public void QuitGame()
        {
            Application.Quit();
            print("Quitting game. Goodbye!");
        }
    }
}