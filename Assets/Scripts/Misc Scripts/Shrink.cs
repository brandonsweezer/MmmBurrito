using UnityEngine;
using System.Collections;

public class Shrink : MonoBehaviour
{
    private bool shrinking;

    // Use this for initialization
    void Awake()
    {
        shrinking = false;
    }

    private void Update()
    {
        if (shrinking)
        {
            transform.localScale -= Vector3.one * Time.deltaTime*0.7f;
            if(this.transform.localScale.x < 0.3f)
            {
                GameController.instance.objects.RemoveAt(GameController.instance.objects.IndexOf(gameObject));
                Destroy(gameObject);
            }
        }
    }

    public void StartShrink()
    {
        shrinking = true;
    }
}
