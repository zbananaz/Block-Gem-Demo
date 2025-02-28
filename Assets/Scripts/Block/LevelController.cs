using System;
using UnityEngine;

public class LevelController : MonoBehaviour {
    void Start()
    {
        EventBroker.instance.OnBlockCollided.AddListener(HandleBlockCollided);
    }

    private void HandleBlockCollided(GameObject block, GameObject gate)
    {
        Debug.Log($"{block.name} collided with {gate.name}");
    }
}