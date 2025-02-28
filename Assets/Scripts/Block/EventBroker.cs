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
    public UnityEvent<GameObject> OnBlockSelected;
    public UnityEvent<Vector3> OnBlockDeselected;
    public UnityEvent<Vector3> OnBlockMoving;

    public UnityEvent<float> OnSendingCellSize;
    public UnityEvent<int, int, float, float> OnSendingGridSize;
}
