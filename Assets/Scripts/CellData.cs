public enum OrganType 
{ 
    Empty, 
    // 循環與造血
    Heart, BoneMarrow, 
    // 呼吸與交換
    Lung, 
    // 消化鏈
    Stomach, SmallIntestine, LargeIntestine, Liver, Pancreas,
    // 排泄
    Kidney, Bladder,
    // 運動與控制
    Muscle, Nerve 
}
public enum VesselType { None, Artery, Vein, Capillary }    // 動脈、靜脈、微血管

[System.Serializable]
public struct CellData {
    public OrganType type;
    public float health;
    public float oxygen;    // 氧氣存量
    public float energy;    // ATP 存量
    public float pressure;  // 用於物流傳導的壓力值
    
    // 構造函數初始化
    public CellData(OrganType t) {
        type = t;
        health = 100f;
        oxygen = 0f;
        energy = 0f;
        pressure = 0f;
    }
}