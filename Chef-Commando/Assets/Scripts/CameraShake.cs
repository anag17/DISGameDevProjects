using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public float shakeAmount;
    public float shakeDuration;

    bool isRunning = false;

    // Many thanks to Adrian Lopez for posting this snippet on his blog: http://gamedesigntheory.blogspot.dk/2010/09/controlling-aspect-ratio-in-unity.html
    void Start() {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f) {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        } else // add pillarbox
          {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

    public void ShakeCamera(float amount, float duration) {
        shakeAmount = amount;
        shakeDuration = duration;

        if (!isRunning) StartCoroutine(Shake());
    }

    IEnumerator Shake() {
        isRunning = true;

        while (shakeDuration > 0) {
            Vector3 rotationAmount = Random.insideUnitSphere * shakeAmount;
            rotationAmount.z = 0;
            
            shakeDuration -= Time.deltaTime;
            transform.localRotation = transform.localRotation = transform.localRotation * Quaternion.Euler(rotationAmount);

            yield return null;
        }

        transform.localRotation = Quaternion.Euler(0.0f, transform.localRotation.eulerAngles.y, 0.0f);
        isRunning = false;
    }
}
