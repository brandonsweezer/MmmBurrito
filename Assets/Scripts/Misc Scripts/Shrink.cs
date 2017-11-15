using UnityEngine;
using System.Collections;

public class Shrink : MonoBehaviour
{
    private bool shrinking;
    private bool gone;

    // Use this for initialization
    void Awake()
    {
        shrinking = false;
        gone = false;
    }

    private void Update()
    {
        if (shrinking)
        {
            if (!gone && gameObject != GameController.instance.player)
            {
				gone = true;
				GameController.instance.objects.RemoveAt(GameController.instance.objects.IndexOf(gameObject));
            }
            transform.localScale -= Vector3.one * Time.deltaTime*0.7f;
            if(this.transform.localScale.x < 0.3f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void StartShrink()
    {
        shrinking = true;
    }
}
