using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : class
{
    public static T instance;

    private void Awake ()
    {
        T thisT = this as T;
        if (instance == null)
        {
            instance = thisT;
        }
        else if (instance != thisT && instance != null)
        {
            Destroy(gameObject);
        }
    }
}