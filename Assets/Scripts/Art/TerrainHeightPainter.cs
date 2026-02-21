using UnityEngine;

public class TerrainHeightPainter : MonoBehaviour
{
    public Terrain terrain;

    public float minEditHeight = 1f;        // Protects ground zone (0 area)
    public float heightThreshold = 20f;     // Where new texture starts
    public int targetLayerIndex = 1;        // Rock / snow layer index

    [ContextMenu("Apply Height Texture")]
    public void ApplyHeightTexture()
    {
        TerrainData data = terrain.terrainData;

        int alphaWidth = data.alphamapWidth;
        int alphaHeight = data.alphamapHeight;
        int layerCount = data.alphamapLayers;

        float[,,] alphamaps = data.GetAlphamaps(0, 0, alphaWidth, alphaHeight);

        for (int y = 0; y < alphaHeight; y++)
        {
            for (int x = 0; x < alphaWidth; x++)
            {
                float normX = (float)x / (alphaWidth - 1);
                float normY = (float)y / (alphaHeight - 1);

                float height = data.GetInterpolatedHeight(normX, normY);
                float worldHeight = terrain.transform.position.y + height;

                // Do NOT touch navigable ground
                if (worldHeight <= minEditHeight)
                    continue;

                // Only modify above threshold
                if (worldHeight > heightThreshold)
                {
                    // Clear all layers
                    for (int i = 0; i < layerCount; i++)
                        alphamaps[y, x, i] = 0f;

                    alphamaps[y, x, targetLayerIndex] = 1f;
                }
            }
        }

        data.SetAlphamaps(0, 0, alphamaps);
    }
}