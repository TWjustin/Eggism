using UnityEngine;

public class GridObject : MonoBehaviour
{
    [Header("Identity Settings")]
    public BuildMode buildMode;   // 區分 Organ 或 Vessel 層
    public OrganType organType;   // 如果是器官，具體類型
    public VesselType vesselType; // 如果是血管，具體類型

    [Header("Status")]
    [SerializeField] private Vector2Int gridPosition;
    [SerializeField] private bool isPlaced = false;

    // 取得屬性的封裝
    public Vector2Int GridPos => gridPosition;
    public bool IsPlaced => isPlaced;

    private void Start()
    {
        // 如果場景中預放了這個物件，嘗試自動註冊到 GridManager
        if (!isPlaced && GridManager.Instance != null)
        {
            // 根據目前 transform.position 推算網格座標
            Vector2Int pos = InferGridPosition();
            if (GridManager.Instance.IsValid(pos))
            {
                // 嘗試佔據該格子並寫入數據
                GridManager.Instance.TryPlaceObject(this, pos);
            }
        }
    }

    /// <summary>
    /// 設定網格位置並將世界座標對齊網格中心
    /// </summary>
    public void SetGridPosition(Vector2Int newGridPos)
    {
        gridPosition = newGridPos;
        isPlaced = true;
        
        // 使用重構後的 GridManager 取得世界座標
        transform.position = GridManager.Instance.GetWorldPos(gridPosition);
    }

    /// <summary>
    /// 從目前世界座標位置嘗試放置（用於建築系統）
    /// </summary>
    public bool PlaceAtCurrentPosition()
    {
        if (isPlaced) return false;
        return GridManager.Instance.TryPlaceObject(this, InferGridPosition());
    }

    /// <summary>
    /// 將自己從網格資料庫中移除
    /// </summary>
    public void RemoveFromGrid()
    {
        if (!isPlaced) return;
        GridManager.Instance.RemoveObject(gridPosition);
        isPlaced = false;
    }

    // --- 內部私有工具 ---
    private Vector2Int InferGridPosition()
    {
        // 取得相對於 GridOrigin 的座標並換算成 Index
        Vector3 origin = GridManager.Instance.transform.position;
        float size = GridManager.Instance.cellSize;
        
        Vector3 relPos = transform.position - origin;
        return new Vector2Int(
            Mathf.FloorToInt(relPos.x / size),
            Mathf.FloorToInt(relPos.y / size)
        );
    }
}