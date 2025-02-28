using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    private ProceduralPlane plane;
    public float blockSize;
    private Outline outline;
    private Rigidbody rb;
    [SerializeField] private float speed;

    void Start()
    {
        outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;

        rb = GetComponent<Rigidbody>();
        plane = GetComponent<ProceduralPlane>();
        if (plane == null)
        {
            Debug.LogError("Không tìm thấy ProceduralPlane trên GameObject!");
            return;
        }

        blockSize = plane.width / plane.gridWidth;
        gameObject.transform.localScale = new Vector3(blockSize, blockSize, blockSize) / 10;

        EventBroker.instance.OnBlockSelected.AddListener(HandleBlockSelected);
        EventBroker.instance.OnBlockDeselected.AddListener(HandleBlockDeselected);
        EventBroker.instance.OnBlockMoving.AddListener(HandleBlockMoving);
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

    private void OnCollisionEnter(Collision other)
    {

        EventBroker.instance.OnBlockCollided?.Invoke(gameObject, other.gameObject);

    }

    private Vector3 RearangeBlock()
    {
        Collider[] floorColliders = Physics.OverlapBox(gameObject.transform.position, gameObject.transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Floor"));

        if (floorColliders.Length > 0)
        {
            Vector3 avgPosition = Vector3.zero;

            // 📌 Tính trung bình vị trí của các Floor mà block đang phủ lên
            foreach (Collider floor in floorColliders)
            {
                avgPosition += floor.transform.position;
            }
            avgPosition /= floorColliders.Length; // Tính trung bình vị trí

            // 📌 Làm tròn giá trị vị trí về grid (giữ nguyên Y)
            float cellSize = 1f; // Cập nhật với kích thước cell của bạn
            float snappedX = Mathf.Round(avgPosition.x / cellSize) * cellSize;
            float snappedZ = Mathf.Round(avgPosition.z / cellSize) * cellSize;

            return new Vector3(snappedX, transform.position.y, snappedZ);
        }

        return transform.position; // Nếu không tìm thấy Floor, giữ nguyên vị trí cũ 
    }
}