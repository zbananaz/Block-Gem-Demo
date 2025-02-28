using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridPrefab; // Prefab của mỗi ô Grid
    private ProceduralPlane plane;
    private Vector3[,] gridPositions; // Lưu vị trí từng ô trên Grid
    private float cellSize; // Kích thước của mỗi ô

    void Start()
    {
        plane = GetComponent<ProceduralPlane>();
        if (plane == null)
        {
            Debug.LogError("Không tìm thấy ProceduralPlane trên GameObject!");
            return;
        }

        cellSize = plane.width / plane.gridWidth; // Lấy `cellSize` từ ProceduralPlane
        GenerateGrid();
    }

    void GenerateGrid()
    {
        int gridWidth = plane.gridWidth;
        int gridHeight = plane.gridHeight;
        float halfWidth = plane.width / 2;
        float halfHeight = plane.height / 2;

        gridPositions = new Vector3[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                // Tính vị trí trung tâm của từng ô
                Vector3 position = new Vector3(
                    - halfWidth + x * cellSize ,
                    transform.position.y + 0.01f, // Để ô Grid nổi lên mặt phẳng
                    halfHeight - z * cellSize - cellSize
                );

                gridPositions[x, z] = position;
                Debug.Log("Cell Pos: " + "["+x + ", " + z + "]");
                // Tạo một ô Grid tại vị trí tính toán
                SpawnGridCell(position);
            }
        }
    }

    void SpawnGridCell(Vector3 position)
    {
        if (gridPrefab == null)
        {
            Debug.LogError("Chưa gán Prefab `gridPrefab` vào GridManager!");
            return;
        }

        GameObject newGridCell = Instantiate(gridPrefab, position, Quaternion.identity);
        newGridCell.transform.localScale = new Vector3(cellSize/10, 0.1f, cellSize/10); // Chỉnh kích thước Grid
    }
}
