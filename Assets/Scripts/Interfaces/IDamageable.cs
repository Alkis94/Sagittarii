using UnityEngine;

public interface IDamageable
{
    void ApplyDamage(int damage, DamageSource damageSource, DamageType damageType = DamageType.normal);
}

