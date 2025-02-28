using System;
using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float blockSize;
    private Outline outline;
    private Rigidbody rb;
    [SerializeField] private float speed;
    private float width;
    private int gridWidth;
    private float cellSize;

    void Start()
    {
        EventBroker.instance.OnSendingGridSize.AddListener(HandleSendingGridSize);
        EventBroker.instance.OnSendingCellSize.AddListener(HandleSendingCellSize);
        outline = gameObject.GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
        }
        outline.enabled = false;

        rb = GetComponent<Rigidbody>();
        blockSize = width / gridWidth;
        gameObject.transform.localScale = new Vector3(blockSize, blockSize, blockSize);

        EventBroker.instance.OnBlockSelected.AddListener(HandleBlockSelected);
        EventBroker.instance.OnBlockDeselected.AddListener(HandleBlockDeselected);
        EventBroker.instance.OnBlockMoving.AddListener(HandleBlockMoving);
    }

    private void HandleSendingCellSize(float arg0)
    {
        cellSize = arg0;
    }

    private void HandleSendingGridSize(int arg0, int arg1, float arg2, float arg3)
    {
        width = arg3;
        gridWidth = arg0;
    }

    private void HandleBlockSelected(GameObject blockSelected)
    {
        outline.enabled = true;
        outline.OutlineColor = Color.white;
        outline.OutlineWidth = 10.0f;
    }

    private void HandleBlockMoving(Vector3 targetPos)
    {
        rb.linearVelocity = (targetPos - transform.position).normalized * speed;
    }

    private void HandleBlockDeselected(Vector3 targetPos)
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
        rb.linearVelocity = Vector3.zero;
        RearangeBlock();

    }

    public Vector3 RearangeBlock()
    {   
        Collider[] floorColliders = Physics.OverlapBox
        (
            gameObject.transform.position, 
            gameObject.transform.localScale / 2, 
            Quaternion.identity, LayerMask.GetMask("Floor")
        );

        if (floorColliders.Length > 0)
        {
            Vector3 avgPosition = Vector3.zero;

            //Tính trung bình vị trí của các Floor mà block đang phủ lên
            foreach (Collider floor in floorColliders)
            {
                avgPosition += floor.transform.position;
            }
            avgPosition /= floorColliders.Length; // Tính trung bình vị trí

            //Làm tròn giá trị vị trí về grid (giữ nguyên Y)
            float snappedX = Mathf.Round(avgPosition.x / cellSize) * cellSize;
            float snappedZ = Mathf.Round(avgPosition.z / cellSize) * cellSize;

            return new Vector3(snappedX, transform.position.y, snappedZ);
        }

        return transform.position; // Nếu không tìm thấy Floor, giữ nguyên vị trí cũ 
    }
}