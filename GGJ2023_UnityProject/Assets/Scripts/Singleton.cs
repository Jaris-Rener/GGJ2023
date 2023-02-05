namespace LemonBerry
{
    using UnityEngine;

    public class Singleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        public virtual bool DontDestroyOnLoad => false;
        public static T Instance { get; private set; }

        public virtual void Awake()
        {
            if (Instance == null)
                Instance = this as T;
            else
                Destroy(gameObject);

            if (DontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
    }
}