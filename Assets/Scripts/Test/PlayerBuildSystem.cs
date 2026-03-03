using UnityEngine;
using UnityEngine.InputSystem;

// 建造模式定義
public enum BuildMode { Vessel, Organ }

public class PlayerBuildSystem : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject vesselPrefab; // 拖入血管 Prefab
    [SerializeField] private GameObject organPrefab;  // 拖入臟器 Prefab

    private Camera mainCamera;

    public BuildMode currentBuildMode = BuildMode.Vessel;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 1. 偵測 Tab 鍵：統一切換建造模式
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleBuildMode();
        }

        // 2. 滑鼠左鍵：放置物件
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandlePlacement();
        }

        // 3. 滑鼠右鍵：移除物件
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            HandleRemoval();
        }
    }

    private void ToggleBuildMode()
    {
        currentBuildMode = (currentBuildMode == BuildMode.Vessel) ? BuildMode.Organ : BuildMode.Vessel;
        Debug.Log($"<color=cyan>[BuildSystem]</color> 切換至 {currentBuildMode} 模式");

        // 通知視覺系統重繪 (如果你的 GridVisualizer 有區分視圖)
        #if UNITY_EDITOR
        UnityEditor.SceneView.RepaintAll();
        #endif
    }

    private void HandlePlacement()
    {
        Vector2Int gridPos = GetMouseGridPosition();

        // 檢查座標是否合法
        if (!GridManager.Instance.IsValid(gridPos)) return;

        // 選擇 Prefab
        GameObject prefabToUse = (currentBuildMode == BuildMode.Vessel) ? vesselPrefab : organPrefab;
        if (prefabToUse == null) return;

        // 實例化並透過 GridManager 嘗試放置
        // 注意：TryPlaceObject 會自動處理「是否已被佔據」的檢查
        GameObject go = Instantiate(prefabToUse);
        GridObject gridObj = go.GetComponent<GridObject>();

        if (gridObj != null)
        {
            bool success = GridManager.Instance.TryPlaceObject(gridObj, gridPos);
            if (!success)
            {
                Destroy(go); // 放置失敗（例如該格已有東西），毀滅分身
                Debug.LogWarning($"位置 {gridPos} 已被佔用或無效");
            }
        }
    }

    private void HandleRemoval()
    {
        Vector2Int gridPos = GetMouseGridPosition();
        GridManager.Instance.RemoveObject(gridPos);
    }

    // --- 工具方法：統一滑鼠座標轉換 ---
    private Vector2Int GetMouseGridPosition()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        
        // 使用與 GridObject 一致的推算邏輯
        Vector3 origin = GridManager.Instance.transform.position;
        float size = GridManager.Instance.cellSize;
        
        Vector3 relPos = worldPos - origin;
        return new Vector2Int(
            Mathf.FloorToInt(relPos.x / size),
            Mathf.FloorToInt(relPos.y / size)
        );
    }
}