using UnityEngine;

public static class UIUtils
{
    private static int gridSize = 50;

    public static void DisplayContainer(Container container, RectTransform rectTransform, Vector2 mousePosition, out InventoryItem? currentItem, out Vector2? currentItemPos)
    {
        currentItem = null;
        currentItemPos = null;

        Rect rect = rectTransform.rect;

        int columns = (int)rect.width / gridSize;
        int rows = (int)rect.height / gridSize;

        Vector2 start = new Vector2(rect.xMin, rect.yMax);

        int itemIdx = 0;
        int lastIdx = container.Items.Count - 1;

        for (int y = 0; y < rows; y++)
        {
            if (itemIdx == lastIdx)
                break;
            for (int x = 0; x < columns; x++)
            {
                if (itemIdx == lastIdx)
                    break;

                InventoryItem item = container.Items[itemIdx];

                Vector2 cellPosition = new Vector2(
                    start.x + x * gridSize,
                    start.y - y * gridSize
                );

                if (PointInRect(mousePosition, cellPosition, cellPosition + new Vector2(gridSize, -gridSize)))
                {
                    currentItem = item;
                    currentItemPos = cellPosition;
                }

                // TODO: Image display

                itemIdx++;
            }
        }
    }

    public static bool PointInRect(Vector2 point, Vector2 min, Vector2 max)
    {
        return (point.x >= min.x && point.x <= max.x) && (point.y <= min.y && point.y >= max.y);
    }
}