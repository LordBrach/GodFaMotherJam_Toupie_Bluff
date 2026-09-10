using System.Collections;
using UnityEngine;

public class Flames : MonoBehaviour
{
    private RectTransform recttransform;
    [SerializeField] float animTime = 2.0f;
    private void Awake()
    {
        recttransform = GetComponent<RectTransform>();
    }

    public void CallFlames()
    {
        Debug.Log("Call flames");
        StartCoroutine(AnimFlames());
    }

    public IEnumerator AnimFlames()
    {
        Vector3 origin = recttransform.localScale;
        float elapsedTime = 0;
        // Wait until the time has reached the target duration.
        while (elapsedTime < animTime)
        {
            // Increment our timer.
            elapsedTime += Time.deltaTime;
            // Normalise our timer, and use it to interpolate.
            recttransform.localScale = Vector3.Lerp(origin, new Vector3(1,1,1), elapsedTime / animTime);
            yield return null;
        }
        yield return null;
    }
}
