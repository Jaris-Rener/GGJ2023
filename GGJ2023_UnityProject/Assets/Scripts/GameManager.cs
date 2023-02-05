namespace LemonBerry
{
    using System;

    public class GameManager : Singleton<GameManager>
    {
        public event Action OnLevelComplete;
        public event Action OnLevelStart;

        public void EndLevel()
        {
            OnLevelComplete?.Invoke();
        }

        public void StartLevel()
        {
            OnLevelStart?.Invoke();
        }
    }
}