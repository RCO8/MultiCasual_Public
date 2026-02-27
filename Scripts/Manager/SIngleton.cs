using UnityEngine;
using Unity.VisualScripting;

public class SIngleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _componentInstance;

    public static T Instance
    {
        get
        {
            if (_componentInstance == null)
            {
                _componentInstance = (T)FindObjectOfType(typeof(T));

                if (_componentInstance == null)
                {
                    GameObject _gameObject = new GameObject();
                    _gameObject.name = typeof(T).ToString();
                    _componentInstance = _gameObject.GetOrAddComponent<T>();
                    DontDestroyOnLoad(_componentInstance);
                }
            }
            return _componentInstance;
        }
    }

    protected void Awake()
    {
        if(_componentInstance == null)
            DontDestroyOnLoad(gameObject);
        else if(_componentInstance != this)
            Destroy(gameObject);
    }
}
