using System.Collections;
using UnityEngine;

public class PlayerBlink : MonoBehaviour
{
    public float blinkDuration = 2f;   // how long blinking lasts
    public float blinkInterval = 0.1f; // on/off speed

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void StartBlink()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        float endTime = Time.time + blinkDuration;

        while (Time.time < endTime)
        {
            sr.enabled = !sr.enabled;   // toggle visibility
            yield return new WaitForSeconds(blinkInterval);
        }

        sr.enabled = true;  // end by making player visible
    }
}
