using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform _barrel;
    [SerializeField] private LayerMask _hitLayers;
    [SerializeField] private float _range;
    [SerializeField] private float _damage;
    [SerializeField] private float _firerate;
    [SerializeField] private GameObject _muzzleFlashVFX;
    [SerializeField] private int _bullets;

    private float _lastFireTime;

    public int Bullets { get => _bullets; set => _bullets = value; }

    private void Start()
    {
        _lastFireTime = Time.time;
    }
    public void Shoot(Vector2 forward)
    {
        if (_bullets == 0)
            return;

        if (Time.time <= _lastFireTime + _firerate)
            return;

        _lastFireTime = Time.time;
        _bullets -= 1;

        Vector3 worldForward = VectorUtils.Vec2ToVec3(forward);
        bool hitSomething = Physics.Raycast(_barrel.position, worldForward * _range, out RaycastHit hit);

        if (_muzzleFlashVFX != null)
        {
            GameObject flash = Instantiate(_muzzleFlashVFX, _barrel.position, _barrel.rotation);
            flash.GetComponent<Stim_Gunshot>().Fire();
            Destroy(flash, 1f);
        }

        if (hitSomething)
        {
            Debug.DrawLine(_barrel.position, hit.point, Color.red, 0.5f);

            /*Transform parent = hit.transform.parent;
            if (parent == null) return;
            Organism organism = parent.gameObject.GetComponent<AIOrganism>();
            if (organism != null)
            {
                organism.Vitals.GetVital(VitalType.Injury).IncreaseValue(_damage);
            }*/

            IDamageable damageable = hit.transform.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
                damageable.TakeDamage(_damage, DamageType.Physical, VectorUtils.Vec3ToVec2(_barrel.position), hit.point);
        }
    }
}
