using UnityEngine;

public class UnitySingleton<T> : MonoBehaviour where T : Component
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Object.FindObjectOfType(typeof(T)) as T;
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = (T)obj.AddComponent(typeof(T));
                    obj.hideFlags = HideFlags.DontSave;
                    //obj.hideFlags = HideFlags.HideAndDontSave;
                    obj.name = typeof(T).Name;
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}