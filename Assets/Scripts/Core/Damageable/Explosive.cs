using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour, IDamageable
{
    [SerializeField] private float _radius;
    [SerializeField] private float _peakDamage;
    [SerializeField] private AnimationCurve _falloff;
    [SerializeField] private GameObject _explosionFX;
    [SerializeField] private LayerMask _collisionLayers;
    [SerializeField] private DamageType _damageType = DamageType.Physical;

    private bool _detonated = false;

    public void TakeDamage(float damage, DamageType damageType, Vector2? worldSource, Vector3? impactPoint)
    {
        Debug.DrawRay(transform.position, 10 * Vector3.up, Color.red);

        _detonated = true;

        if (damageType != DamageType.Physical)
            return;

        Debug.DrawRay(transform.position, 10 * Vector3.up, Color.red);

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _radius, Vector3.up, _radius, _collisionLayers);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform == transform) continue;

            IDamageable damageable = hit.transform.GetComponent<IDamageable>();

            if (damageable == null || !damageable.Enabled()) continue;

            float dist = Vector3.Distance(hit.transform.position, transform.position);
            float lerp = dist / _radius;
            float damageMultiplier = _falloff.Evaluate(lerp);
            float adjustedDamage = _peakDamage * damageMultiplier;

            damageable.TakeDamage(adjustedDamage, _damageType, VectorUtils.Vec3ToVec2(transform.position), hit.point);
        }

        Instantiate(_explosionFX, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public bool Enabled() => !_detonated;
}
