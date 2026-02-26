using UnityEngine;
using UnityEngine.UI;

public static class UIUtils
{
    private static int gridSize = 50;

    public static Image[][] InitIconGrid(RectTransform rectTransform, GameObject iconPrefab)
    {
        Rect rect = rectTransform.rect;

        int columns = (int)rect.width / gridSize;
        int rows = (int)rect.height / gridSize;

        Image[][] grid = new Image[rows][];

        for (int x = 0; x < rows; x++)
        {
            grid[x] = new Image[columns];
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
                GameObject icon = Object.Instantiate(iconPrefab, rectTransform, false);

                RectTransform iconRect = icon.GetComponent<RectTransform>();

                // Ensure proper UI setup
                iconRect.anchorMin = new Vector2(0, 1);
                iconRect.anchorMax = new Vector2(0, 1);
                iconRect.pivot = new Vector2(0, 1);
                iconRect.sizeDelta = new Vector2(gridSize/2, gridSize/2);

                iconRect.anchoredPosition = cellPosition;

                Image image = icon.GetComponent<Image>();
                image.raycastTarget = false;
                image.enabled = false; // start hidden

                grid[y][x] = image;
            }
        }

        return grid;
    }

    public static void DisplayContainerContents(Container container, RectTransform rectTransform, Vector2 mousePosition, Image[][] iconGrid, 
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
                    iconGrid[y][x].enabled = false;
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
                    Image gridImage = iconGrid[y][x];
                    gridImage.sprite = item.Item.Image;
                    gridImage.enabled = true;
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