using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SimpleFadeAnimation : MonoBehaviour
{
    [Header("Config")]
    public Image toFade;
    public float fadeMagnitude;
    public bool fadeState;
    public float fadeMax, fadeMin;

    [Header("Testing")] 
    public bool  runTest;
    public bool  randomizeTestValues;
    public int   testIterations;
    public float waitBetweenTests;
    
    private Coroutine      fadeCoroutine;
    private Color          colorOverride;
    private WaitForSeconds testWait;

    private void OnEnable()
    {
        if(toFade.color.a > fadeMin) fadeState = false;
        else fadeState = true;
        if(fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        if (runTest) StartCoroutine(TestComponent());
    }

    public void Fade()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (fadeState)
        {
            fadeCoroutine = StartCoroutine(DoFade(fadeMax));
        }
        else
        {
            fadeCoroutine = StartCoroutine(DoFade(fadeMin));
        }
    }

    private IEnumerator DoFade(float targetAlpha)
    {
        fadeState = !fadeState;
        colorOverride = toFade.color;
        
        float currentAlpha = colorOverride.a;
        
        float fadeIdentity = Mathf.Sign(targetAlpha - currentAlpha);
        float fadeStep     = Mathf.Abs(currentAlpha - targetAlpha) * fadeMagnitude;
        
        while (!Mathf.Approximately(targetAlpha, currentAlpha))
        {
            yield return null;
            currentAlpha  = Mathf.Clamp(toFade.color.a + fadeStep * fadeIdentity * Time.deltaTime, fadeMin, fadeMax);
            colorOverride = new Color(colorOverride.r, colorOverride.g, colorOverride.b, currentAlpha);
            toFade.color  = colorOverride;
        }
        fadeCoroutine = null;
    }

    private IEnumerator TestComponent()
    {
        testWait = new WaitForSeconds(waitBetweenTests);
        Debug.Log($"SimpleFadeAnimation. TestComponent started. testIterations: {testIterations}. waitBetweenTests: {waitBetweenTests}.");
        for (int i = 0; i < testIterations; i++)
        {
            if(randomizeTestValues) RandomizeTestValues();
            Fade();
            yield return testWait;
        }
        Debug.Log("SimpleFadeAnimation. TestComponent complete!");
    }

    private void RandomizeTestValues()
    {
        fadeMagnitude = Random.Range(0.5f, 2f);
        fadeMax       = Random.Range(0.75f, 1f);
        fadeMin       = Random.Range(0f, 0.25f);
        Debug.Log($"RandomizeTestValues. fadeMagnitude: {fadeMagnitude.ToString("n2")}, fadeMin: {fadeMin.ToString("n2")}, fadeMax: {fadeMax.ToString("n2")}");
    }
}