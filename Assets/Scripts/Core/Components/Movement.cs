using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed; // TODO: Integrate vitals into speed
    private Vector2 _dir;
    private Organism _organism;

    private void Start()
    {
        _dir = Vector2.zero;
    }

    public bool IsMoving { get { return _dir != Vector2.zero;} }

    private void FixedUpdate()
    {
        if (_organism == null)
            return;
        if (_dir != Vector2.zero)
            _organism.LookDirection = _dir;
        _organism.Position += (_dir * _walkSpeed);
    }

    public void Move(Organism organism, Vector2 dir)
    {
        _organism = organism;
        _dir = dir;
    }
}
