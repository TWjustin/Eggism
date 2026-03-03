using UnityEngine;

public class MetabolismSystem : MonoBehaviour
{
    public static MetabolismSystem Instance { get; private set; }

    [SerializeField] private float updateInterval = 1.0f; // 每秒代謝一次
    private float timer;

    private void Awake() => Instance = this;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            RunMetabolismTick();
            timer = 0;
        }
    }

    public void RequestUpdate() => UpdateBloodCoverage();

    // 更新血管覆蓋範圍 (原本在 GridManager 的功能)
    private void UpdateBloodCoverage()
    {
        var manager = GridManager.Instance;
        // 重置所有格子的供應狀態
        for (int x = 0; x < manager.width; x++)
            for (int y = 0; y < manager.height; y++)
                manager.Grid[x, y].IsInBloodRange = false;

        // 重新計算 (此處可優化為只掃描血管物件)
        for (int x = 0; x < manager.width; x++)
            for (int y = 0; y < manager.height; y++)
                if (manager.Grid[x, y].vesselType != VesselType.None)
                    MarkRange(new Vector2Int(x, y), 3);
    }

    private void RunMetabolismTick()
    {
        var manager = GridManager.Instance;
        for (int x = 0; x < manager.width; x++)
        {
            for (int y = 0; y < manager.height; y++)
            {
                GridCell cell = manager.Grid[x, y];
                
                // 1. 基礎邏輯：如果有血管覆蓋，回補氧氣與營養
                if (cell.IsInBloodRange) {
                    cell.stats.oxygen = Mathf.Min(100, cell.stats.oxygen + 20);
                }

                // 2. 器官消耗：如果有器官，消耗 $O_2$ 產生 $CO_2$
                if (cell.organType != OrganType.Empty) {
                    cell.stats.oxygen -= 5;
                    cell.stats.co2 += 2;
                }
            }
        }
    }

    private void MarkRange(Vector2Int center, int radius)
    {
        var manager = GridManager.Instance;
        for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                Vector2Int target = center + new Vector2Int(x, y);
                if (manager.IsValid(target)) manager.Grid[target.x, target.y].IsInBloodRange = true;
            }
    }
}