using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : class
{
    public static T instance;

    private void Awake ()
    {
        if (instance != null) return;
        
        instance = this as T;
    }
}