using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private Shooting _shooting;
    [SerializeField] private TMP_Text _bulletCounter;

    private void Update()
    {
        _bulletCounter.text = _shooting.Bullets.ToString();
    }
}
