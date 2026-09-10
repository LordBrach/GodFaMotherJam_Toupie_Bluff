using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] GameObject targetPoint;
    [SerializeField] Sprite rest;
    [SerializeField] Sprite attack;
    [SerializeField] private float attackLength = 0.25f;

    private Transform transform;
    private SpriteRenderer spriteRenderer;
    private IEnumerator coroutine;
    private Vector3 savedPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        savedPosition = transform.position;

    }
    public void CallAttack()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            spriteRenderer.sprite = rest;
            transform.position = savedPosition;
        }
        coroutine = Anim();
        StartCoroutine(coroutine);
    }

    private IEnumerator Anim()
    {
        yield return new WaitForSeconds(0.02f);
        spriteRenderer.sprite = attack;
        transform.position = targetPoint.transform.position;
        yield return new WaitForSeconds(attackLength);
        spriteRenderer.sprite = rest;
        transform.position = savedPosition;
        yield return null;
    }
}
