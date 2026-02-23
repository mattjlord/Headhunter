using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform _barrel;
    [SerializeField] private LayerMask _hitLayers;
    [SerializeField] private float _range;
    [SerializeField] private float _damage;
    public void Shoot(Vector2 forward)
    {
        Vector3 worldForward = VectorUtils.Vec2ToVec3(forward);
        bool hitSomething = Physics.Raycast(_barrel.position, worldForward * _range, out RaycastHit hit);

        // TODO: Integrate actual visuals later to replace this
        Debug.DrawRay(_barrel.position, worldForward * _range, Color.yellow, 0.5f);

        if (hitSomething)
        {
            Debug.DrawLine(_barrel.position, hit.point, Color.red, 0.5f);
            Organism organism = hit.transform.parent.gameObject.GetComponent<AIOrganism>();
            if (organism != null)
            {
                Debug.Log("Organisms vitals found");
                organism.Vitals.GetVital(VitalType.Injury).IncreaseValue(_damage);
            }
        }
    }
}
