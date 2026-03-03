using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public bool showOxygenMap = true;

    [SerializeField] private Color gridColor = Color.gray;
    [SerializeField] private Color oxygenColor = new Color(0, 0.5f, 1f, 0.2f);

    private void OnDrawGizmos()
    {
        if (GridManager.Instance == null || GridManager.Instance.Grid == null) return;

        var manager = GridManager.Instance;
        for (int x = 0; x < manager.width; x++)
        {
            for (int y = 0; y < manager.height; y++)
            {
                GridCell cell = manager.Grid[x, y];
                Vector2 pos = manager.GetWorldPos(new Vector2Int(x, y));

                // 畫氧氣範圍 (藍色)
                if (showOxygenMap && cell.IsInBloodRange)
                {
                    Gizmos.color = oxygenColor;
                    Gizmos.DrawCube(pos, Vector3.one * manager.cellSize);
                }

                // 畫器官 (紫色) 或 血管 (紅色)
                // if (cell.organType != OrganType.Empty)
                // {
                //     Gizmos.color = Color.magenta;
                //     Gizmos.DrawCube(pos, Vector3.one * 0.8f);
                // }
                // else if (cell.vesselType != VesselType.None)
                // {
                //     Gizmos.color = Color.red;
                //     Gizmos.DrawCube(pos, new Vector3(0.9f, 0.2f, 0.1f));
                // }

                // 繪製基礎網格線
                Gizmos.color = gridColor;
                Gizmos.DrawWireCube(pos, Vector3.one * manager.cellSize);
            }
        }
    }
}