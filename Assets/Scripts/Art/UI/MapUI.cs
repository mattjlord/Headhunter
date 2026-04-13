using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField] private GameObject _map;
    [SerializeField] private GameObject _playerPos;
    [SerializeField] private GameObject _homePos;
    [SerializeField] private PlayerOrganism _playerOrganism;
    [SerializeField] private PlayerShelter _playerShelter;

    private bool _enabled;

    public bool Enabled
    {
        set
        {
            _map.SetActive(value);
            _enabled = value;
        }
        get => _enabled;
    }

    private void Update()
    {
        RectTransform playerRect = _playerPos.GetComponent<RectTransform>();
        playerRect.anchoredPosition = WorldToMapPos(_playerOrganism.Position);

        RectTransform homeRect = _homePos.GetComponent<RectTransform>();
        homeRect.anchoredPosition = WorldToMapPos(_playerShelter.Position);
    }

    public Vector2 WorldToMapPos(Vector2 worldPos)
    {
        RectTransform mapRect = _map.GetComponent<RectTransform>();
        float width = mapRect.rect.width;
        return worldPos / 1500 * (width) / 2;
    }
}
