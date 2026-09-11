using UnityEngine;

public class BGLoop : MonoBehaviour
{
    public enum HorizontalDirection { Left, Right }
    public enum VerticalDirection { Up, Down }

    [Header("Tuile")]
    [SerializeField] private RectTransform tilePrefab;

    [SerializeField] private RectTransform container;

    [Header("Config")]
    [SerializeField] private HorizontalDirection horizontal = HorizontalDirection.Left;
    [SerializeField] private VerticalDirection vertical = VerticalDirection.Down;
    [SerializeField, Min(0f)] private float speed = 50f;

    private RectTransform[] tiles;
    private float tileWidth, tileHeight;
    private float totalWidth, totalHeight; // largeur et hauteur de la grille complète (tuiles + marge)
    private float halfSpanX, halfSpanY; // distance du centre à partir de laquelle une tuile est hors écran
    [SerializeField, Min(0f)] private float SlowDownValue = 0.2f;
    public bool SlowDownEnabled = false;

    private void Awake() => BuildGrid();

    private void BuildGrid()
    {
        if (tilePrefab == null || container == null)
        {
            Debug.LogError("Tileprab or container is missing.", this);
            enabled = false;
            return;
        }

        tileWidth = tilePrefab.rect.width;
        tileHeight = tilePrefab.rect.height;

        // +2 tuiles de marge (1 de chaque côté) : une tuile ne se recycle qu'une fois
        int cols = Mathf.CeilToInt(container.rect.width / tileWidth) + 2;
        int rows = Mathf.CeilToInt(container.rect.height / tileHeight) + 2;

        totalWidth = cols * tileWidth;
        totalHeight = rows * tileHeight;
        halfSpanX = container.rect.width / 2f + tileWidth;
        halfSpanY = container.rect.height / 2f + tileHeight;

        tiles = new RectTransform[cols * rows];

        // Grille centrée sur (0,0), comme le container (pivot 0.5/0.5).
        float startX = -(cols - 1) * tileWidth / 2f;
        float startY = -(rows - 1) * tileHeight / 2f;

        int index = 0;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                RectTransform tile = Instantiate(tilePrefab, container);
                tile.anchoredPosition = new Vector2(startX + x * tileWidth, startY + y * tileHeight);
                tiles[index++] = tile;
            }
        }
    }

    private void Update()
    {
        // unscaledDeltaTime : le fond continue de défiler même en pause
        float y = 1.0f;
        if (SlowDownEnabled)
            y = SlowDownValue;

        float dt = Time.unscaledDeltaTime;
        float dx = (horizontal == HorizontalDirection.Left ? -1f : 1f) * speed * dt * SlowDownValue;
        float dy = (vertical == VerticalDirection.Down ? -1f : 1f) * speed * dt * SlowDownValue;

        for (int i = 0; i < tiles.Length; i++)
        {
            Vector2 pos = tiles[i].anchoredPosition;
            pos.x += dx;
            pos.y += dy;

            // Recycle les tuiles qui sortent de la zone visible, en les repositionnant de l'autre côté
            if (pos.x < -halfSpanX) pos.x += totalWidth;
            else if (pos.x > halfSpanX) pos.x -= totalWidth;

            if (pos.y < -halfSpanY) pos.y += totalHeight;
            else if (pos.y > halfSpanY) pos.y -= totalHeight;

            tiles[i].anchoredPosition = pos;
        }
    }
}