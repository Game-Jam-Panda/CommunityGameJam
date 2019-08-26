using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionFlicker : MonoBehaviour
{
    public int flickerTimes;
    public float flickerTime;
    public float flickerFrequency;
    public int materialIndex;
    public new Light light;

    public bool playOnAwake;

    static string EmissiveColor = "_EmissionColor";
    new Renderer renderer;
    Color originalColor;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.materials[materialIndex].GetColor(EmissiveColor);

        if (playOnAwake)
            Flicker();
    }

    public void Flicker()
    {
        StartCoroutine(DoFlicker());
    }

    private IEnumerator DoFlicker()
    {
        while (true)
        {
            float elapsed = 0;
            while (elapsed < flickerFrequency)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            // flicker light
            for (int i = 0; i < flickerTimes; i++)
            {
                // shut light
                renderer.materials[materialIndex].SetColor(EmissiveColor, Color.black);
                if (light != null)
                    light.enabled = false;

                float elapsedShut = 0;
                while (elapsedShut < flickerTime)
                {
                    elapsedShut += Time.deltaTime;
                    yield return null;
                }

                // turn on light
                renderer.materials[materialIndex].SetColor(EmissiveColor, originalColor);
                if (light != null)
                    light.enabled = true;

                elapsedShut = 0;
                while (elapsedShut < flickerTime)
                {
                    elapsedShut += Time.deltaTime;
                    yield return null;
                }

                yield return null;
            }

            yield return null;
        }
    }
}
