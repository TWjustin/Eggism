# 2D Grid System 使用說明

## 概述
這是一個簡單但功能完整的 2D 網格系統，適用於精靈遊戲（如塔防、策略遊戲）。

## 核心組件

### 1. GridManager
- **功能**：管理整個網格系統的核心類
- **特性**：
  - 單例模式，場景中自動創建
  - 追蹤所有被佔據的格子
  - 提供座標轉換功能（世界座標 ↔ 網格座標）
  - 在 Scene 視圖中可視化網格（使用 Gizmos）

- **主要方法**：
  ```csharp
  // 取得網格座標對應的世界座標
  Vector2 worldPos = GridManager.Instance.GetWorldPosition(gridX, gridY);
  
  // 取得世界座標對應的網格座標
  Vector2Int gridPos = GridManager.Instance.GetGridPosition(worldPosition);
  
  // 檢查座標是否在網格範圍內
  bool isValid = GridManager.Instance.IsValidGridPosition(gridPos);
  
  // 檢查格子是否被佔據
  bool occupied = GridManager.Instance.IsCellOccupied(gridPos);
  
  // 放置物體
  bool success = GridManager.Instance.TryPlaceObject(gridObject, gridPos);
  
  // 移動物體
  bool success = GridManager.Instance.TryMoveObject(fromPos, toPos);
  
  // 移除物體
  GridManager.Instance.RemoveObject(gridPos);
  
  // 取得指定位置的物體
  GridObject obj = GridManager.Instance.GetObjectAt(gridPos);
  ```

- **可調整的參數**：
  - `Grid Width`: 網格寬度（格子數）
  - `Grid Height`: 網格高度（格子數）
  - `Cell Size`: 單個格子的大小
  - `Grid Origin`: 網格的左下角世界座標
  - `Show Grid`: 是否顯示網格（Scene 視圖）
  - `Grid Color`: 網格線顏色
  - `Occupied Color`: 被佔據格子的顏色

### 2. GridObject
- **功能**：代表可以放在網格上的遊戲物體
- **使用方法**：
  1. 在 GameObject 上添加此組件
  2. 物體會自動被 GridManager 追蹤

- **主要方法**：
  ```csharp
  // 放置到當前位置
  gridObject.PlaceAtCurrentPosition();
  
  // 移動到新網格座標
  gridObject.TryMoveTo(new Vector2Int(3, 5));
  
  // 移動到新世界座標
  gridObject.TryMoveTo(new Vector2(10, 5));
  
  // 從網格移除
  gridObject.RemoveFromGrid();
  
  // 取得當前網格座標
  Vector2Int pos = gridObject.GetGridPosition();
  
  // 檢查是否已放置
  if (gridObject.IsPlaced) { ... }
  ```

## 快速開始

### 步驟 1：設置 GridManager
1. 在場景中創建一個空 GameObject，命名為 `GridManager`
2. 添加 `GridManager` 組件
3. 在 Inspector 中調整網格參數：
   - Grid Width: 10
   - Grid Height: 10
   - Cell Size: 1
   - Grid Origin: (0, 0)

### 步驟 2：創建可放置的物體
1. 創建一個 Sprite GameObject（例如一個正方形精靈）
2. 添加 `GridObject` 組件
3. 放置在網格上，使用代碼：
   ```csharp
   GridObject gridObj = GetComponent<GridObject>();
   gridObj.TryMoveTo(new Vector2Int(5, 5));
   ```

### 步驟 3：檢查網格狀態
在 Scene 視圖中，你會看到：
- 白色網格線：網格佈局
- 紅色邊框：被佔據的格子

## 範例代碼

```csharp
// 在滑鼠點擊位置放置物體
void OnMouseClick(Vector2 mouseWorldPos)
{
    Vector2Int gridPos = GridManager.Instance.GetGridPosition(mouseWorldPos);
    GridObject newObject = Instantiate(prefab).GetComponent<GridObject>();
    GridManager.Instance.TryPlaceObject(newObject, gridPos);
}

// 移動已有物體
void MoveObject(GridObject obj, Vector2Int newPos)
{
    if (obj.TryMoveTo(newPos))
    {
        Debug.Log("移動成功");
    }
    else
    {
        Debug.Log("目標位置被佔據或超出範圍");
    }
}
```

## 注意事項
- 格子座標從 (0, 0) 開始，即左下角
- 世界座標對應到格子的左下角
- 放置物體時不會自動設置精靈大小為 1x1，需要手動調整
- 同一時刻一個格子只能被一個物體佔據
