using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridPrefab; // Prefab của mỗi ô Grid
    private Vector3[,] gridPositions; // Lưu vị trí từng ô trên Grid
    private float cellSize; // Kích thước của mỗi ô
    private int gridWidth;
    private int gridHeight;
    private float halfWidth;
    private float halfHeight;

    void Start()
    {
        EventBroker.instance.OnSendingCellSize.AddListener(HandleSendingCellSize);
        EventBroker.instance.OnSendingGridSize.AddListener(HandleSendingGridSize);
        GenerateGrid();
    }

    private void HandleSendingGridSize(int gWidth, int gHeight, float w, float h)
    {
        gridWidth = gWidth;
        gridHeight = gHeight;
        halfWidth = w/2;
        halfHeight = h/2;
    }

    private void HandleSendingCellSize(float arg0)
    {
        cellSize = arg0;
    }

    void GenerateGrid()
    {
        

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
