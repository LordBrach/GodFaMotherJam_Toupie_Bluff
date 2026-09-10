using UnityEngine;

public class InfiniteScroller : MonoBehaviour
{
    public enum Direction { Left, Right }

    [Header("Tuiles")]
    [SerializeField] private RectTransform container;

    [Header("Config")]
    [SerializeField] private Direction direction = Direction.Left;
    [SerializeField, Min(0f)] private float speed = 50f; 

    private RectTransform[] tiles;
    private float tileWidth;
    private float totalWidth;   // largeur cumulée de toutes les tuiles = une boucle complète
    private float halfSpan;     // distance du centre à partir de laquelle une tuile est outscreen

    private void Awake()
    {
        int count = container != null ? container.childCount : 0;
        if (count == 0)
        {
            Debug.LogError("Container don't have tiles.", this);
            enabled = false;
            return;
        }

        tiles = new RectTransform[count];
        for (int i = 0; i < count; i++)
            tiles[i] = (RectTransform)container.GetChild(i);

        // Toutes les tuiles doivent faire la même largeur, donc on se base sur la 1ere
        tileWidth = tiles[0].rect.width;
        totalWidth = tileWidth * count;

        halfSpan = container.rect.width / 2f + tileWidth;
    }

    private void Update()
    {
        // unscaledDeltaTime : le fond continue de défiler
        float delta = (direction == Direction.Left ? -1f : 1f) * speed * Time.unscaledDeltaTime;

        for (int i = 0; i < tiles.Length; i++)
        {
            Vector2 pos = tiles[i].anchoredPosition;
            pos.x += delta;

            // Recycle indépendamment de la direction, si une tuile sort du container, on la replace de l'autre côté
            if (pos.x < -halfSpan) pos.x += totalWidth;
            else if (pos.x > halfSpan) pos.x -= totalWidth;

            tiles[i].anchoredPosition = pos;
        }
    }
}