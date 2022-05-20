using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class LightBlink : MonoBehaviour
{
    [SerializeField] private new Light2D light;
    [SerializeField] private float prefferedMin;
    [SerializeField] private float prefferedMax;

    private IEnumerator Expand ()
    {
        while (light.pointLightOuterRadius < prefferedMax)
        {
            light.pointLightOuterRadius += Time.deltaTime;
            light.pointLightOuterRadius = Mathf.Clamp(light.pointLightOuterRadius, prefferedMin, prefferedMax);
            yield return null;
        }

        StartCoroutine(Shrink());
        yield break;
    }

    private IEnumerator Shrink ()
    {
        while (light.pointLightOuterRadius > prefferedMin)
        {
            light.pointLightOuterRadius -= Time.deltaTime;
            light.pointLightOuterRadius = Mathf.Clamp(light.pointLightOuterRadius, prefferedMin, prefferedMax);
            yield return null;
        }

        StartCoroutine(Expand());
        yield break;
    }
}
