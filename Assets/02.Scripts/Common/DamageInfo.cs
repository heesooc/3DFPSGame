using UnityEngine;

public enum DamageType
{
    Normal,          // 0 
    Critical,        // 1 // 결정적인
}

public struct DamageInfo
{
    public DamageType DamageType;  // 0: 일반, 1: 크리티컬 
    public int Amount;      // 데미지량
    public Vector3 Position;
    public Vector3 Normal;

    public DamageInfo(DamageType damageType, int amount)
    {
        this.DamageType = damageType;
        this.Amount = amount;
        this.Position = Vector3.zero;
        this.Normal = Vector3.zero;
    }
}
