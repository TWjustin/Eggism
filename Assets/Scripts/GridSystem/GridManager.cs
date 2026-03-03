using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Settings")]
    public int width = 20;
    public int height = 20;
    public float cellSize = 1f;

    public GridCell[,] Grid { get; private set; }
    private Dictionary<Vector2Int, GridObject> occupiedCells = new Dictionary<Vector2Int, GridObject>();

    public static GridManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        Grid = new GridCell[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                Grid[x, y] = new GridCell(x, y);
    }

    public bool TryPlaceObject(GridObject obj, Vector2Int pos)
    {
        if (!IsValid(pos) || occupiedCells.ContainsKey(pos)) return false;

        occupiedCells[pos] = obj;
        obj.SetGridPosition(pos);

        // 更新資料層
        // 根據 BuildSystem 傳進來的模式來決定寫入哪一數據層
        if (obj.buildMode == BuildMode.Vessel)
        {
            Grid[pos.x, pos.y].vesselType = obj.vesselType;
            // 只有蓋血管才需要觸發範圍更新
            MetabolismSystem.Instance.RequestUpdate(); 
        }
        else
        {
            Grid[pos.x, pos.y].organType = obj.organType;
        }

        // 通知代謝系統更新範圍
        MetabolismSystem.Instance.RequestUpdate();
        return true;
    }

    public void RemoveObject(Vector2Int pos)
    {
        if (occupiedCells.TryGetValue(pos, out GridObject obj))
        {
            if (obj.buildMode == BuildMode.Vessel) Grid[pos.x, pos.y].vesselType = VesselType.None;
            else Grid[pos.x, pos.y].organType = OrganType.Empty;

            Destroy(obj.gameObject);
            occupiedCells.Remove(pos);
            MetabolismSystem.Instance.RequestUpdate();
        }
    }

    public bool IsValid(Vector2Int p) => p.x >= 0 && p.x < width && p.y >= 0 && p.y < height;
    public Vector2 GetWorldPos(Vector2Int p) => (Vector2)transform.position + new Vector2(p.x * cellSize + cellSize/2, p.y * cellSize + cellSize/2);
}