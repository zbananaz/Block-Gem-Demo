using UnityEngine;
using UnityEngine.Events;

public class EventBroker : MonoBehaviour
{
    public static EventBroker instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public UnityEvent<GameObject, GameObject> OnBlockCollided;
    
}
