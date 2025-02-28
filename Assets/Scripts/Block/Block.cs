using UnityEngine;

public class Block : MonoBehaviour {
    private void OnCollisionEnter(Collision other) {
    
    EventBroker.instance.OnBlockCollided?.Invoke(this.gameObject, other.gameObject);
    Debug.
    Log($"{this.gameObject.name} collided with {other.gameObject.name}");
        if (other.gameObject.TryGetComponent<Wall> (out Wall wall))
        {
            
        }
    }
}