using UnityEngine;

public enum DamageType
{
    Physical,
    Toxic,
    Fire
}

public interface IDamageable
{
    public void TakeDamage(float damage, DamageType damageType, Vector2? worldSource, Vector3? impactPoint);
}