namespace LemonBerry
{
    using System;
    using UnityEngine.SceneManagement;

    public class GameManager : Singleton<GameManager>
    {
        public override bool DontDestroyOnLoad => true;

        public event Action OnLevelComplete;
        public event Action OnLevelStart;

        public void EndLevel()
        {
            OnLevelComplete?.Invoke();
        }

        public void StartLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            OnLevelStart?.Invoke();
        }
    }
}