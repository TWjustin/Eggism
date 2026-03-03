using UnityEngine;

public class GridCell
{
    // 座標資料
    public int X { get; private set; }
    public int Y { get; private set; }

    // 物件狀態 (這格現在有什麼)
    public OrganType organType = OrganType.Empty;
    public VesselType vesselType = VesselType.None;

    // 邏輯狀態
    public bool IsInBloodRange = false; // 是否被血管供給覆蓋
    public float health = 100f;         // 細胞健康度 (0 = 壞死)

    // 分子數值 (使用剛才討論的現實化學/虛構原子結構)
    public MetabolismStats stats;

    public GridCell(int x, int y)
    {
        X = x;
        Y = y;
        stats = new MetabolismStats();
        // 初始化數值
        stats.oxygen = 0f;
        stats.glucose = 0f;
        stats.co2 = 0f;
    }
}

[System.Serializable]
public struct MetabolismStats
{
    [Header("Energy & Respiration")]
    public float oxygen;    // 氧氣 (或虛構原子 Lx)
    public float glucose;   // 葡萄糖 (或虛構原子 Gv)
    
    [Header("Waste")]
    public float co2;       // 二氧化碳
    public float urea;      // 尿素
    
    [Header("Construction")]
    public float aminoAcid; // 氨基酸 (用於修復或升級)
}