using UnityEngine;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] private RectTransform pointeur;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetupScoreBar()
    {
        if (pointeur != null)
        {
            pointeur.anchoredPosition = new Vector2 (0f, 0f);
        }
    }

    public void UpdateScoreBar(int currentScore)
    {
        pointeur.anchoredPosition = new Vector2(currentScore * 5, 0f);
    }

}
