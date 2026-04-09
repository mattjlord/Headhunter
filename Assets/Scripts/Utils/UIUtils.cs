using UnityEngine;
using UnityEngine.UI;

public static class UIUtils
{
    private static int gridSize = 50;

    public static InventoryIconUI[][] InitIconGrid(RectTransform rectTransform, GameObject iconPrefab)
    {
        Rect rect = rectTransform.rect;

        int columns = (int)rect.width / gridSize;
        int rows = (int)rect.height / gridSize;

        InventoryIconUI[][] grid = new InventoryIconUI[rows][];

        for (int x = 0; x < rows; x++)
        {
            grid[x] = new InventoryIconUI[columns];
        }

        Vector2 start = new Vector2(rect.xMin, rect.yMax);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector2 cellPosition = new Vector2(
                    start.x + x * gridSize,
                    start.y - y * gridSize
                );

                // Instantiate prefab
                GameObject instance = Object.Instantiate(iconPrefab, rectTransform, false);

                RectTransform iconRect = instance.GetComponent<RectTransform>();

                // Ensure proper UI setup
                iconRect.anchorMin = new Vector2(0, 1);
                iconRect.anchorMax = new Vector2(0, 1);
                iconRect.pivot = new Vector2(0, 1);
                iconRect.sizeDelta = new Vector2(gridSize/2, gridSize/2);

                iconRect.anchoredPosition = cellPosition;

                Image image = instance.GetComponent<Image>();
                image.raycastTarget = false;
                image.enabled = false; // start hidden

                InventoryIconUI icon = instance.GetComponent<InventoryIconUI>();

                grid[y][x] = icon;
            }
        }

        return grid;
    }

    public static void DisplayContainerContents(Container container, RectTransform rectTransform, Vector2 mousePosition, InventoryIconUI[][] iconGrid, 
                                                out InventoryItemInstance? currentItem, out Vector2? currentItemPos)
    {
        currentItem = null;
        currentItemPos = null;

        Rect rect = rectTransform.rect;

        int columns = (int)rect.width / gridSize;
        int rows = (int)rect.height / gridSize;

        Vector2 start = new Vector2(rect.xMin, rect.yMax);

        int itemIdx = 0;
        int lastIdx = container.Items.Count;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (itemIdx >= lastIdx)
                {
                    iconGrid[y][x].Enabled = false;
                    continue;
                }

                InventoryItemInstance item = container.Items[itemIdx];

                Vector2 cellPosition = new Vector2(
                    start.x + x * gridSize,
                    start.y - y * gridSize
                );

                if (PointInRect(mousePosition, cellPosition, cellPosition + new Vector2(gridSize, -gridSize)))
                {
                    currentItem = item;
                    currentItemPos = cellPosition;
                }

                if (iconGrid != null)
                {
                    InventoryIconUI icon = iconGrid[y][x];
                    icon.Item = item;
                }

                itemIdx++;
            }
        }
    }

    public static bool PointInRect(Vector2 point, Vector2 min, Vector2 max)
    {
        return (point.x >= min.x && point.x <= max.x) && (point.y <= min.y && point.y >= max.y);
    }
}