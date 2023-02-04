namespace LemonBerry 
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class StartMenu : MonoBehaviour 
    {
        public void PlayGame 
        {

        }

        public void QuitGame 
        {
            Application.Quit();
            print("Quitting game. Goodbye!");
        }
    }
}
