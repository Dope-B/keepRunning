using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class flicker : MonoBehaviour
{
    public int sortNum;// 1-> flick 2-> hightlight
    // Start is called before the first frame update
    private float highlightBuffer=0;
    private float bufferDecrease = 0.01f;
    void Start()
    {
        if (sortNum == 1) { StartCoroutine(flick()); }
        else if (sortNum == 2) { StartCoroutine(highlight()); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator flick()
    {
        while (this.GetComponent<TextMeshProUGUI>().color.a<=1f)
        {
            this.GetComponent<TextMeshProUGUI>().color += new Color(0, 0, 0, 0.025f);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.7f);
        while (this.GetComponent<TextMeshProUGUI>().color.a >= 0f)
        {
            this.GetComponent<TextMeshProUGUI>().color -= new Color(0, 0, 0, 0.025f);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(flick());
    }
    IEnumerator highlight()
    {
        gethighlightBuffer();
        this.GetComponent<TextMeshProUGUI>().fontSize = Mathf.Min((highlightBuffer) + 20f, 22);
        yield return new WaitForEndOfFrame();
        StartCoroutine(highlight());
    }
    float gethighlightScale()
    {

        return visualizer.freBand[20];
    }
    void gethighlightBuffer()
    {
        if (gethighlightScale() > highlightBuffer)
        {
            highlightBuffer = gethighlightScale();
            bufferDecrease = 0.01f;
        }
        if (gethighlightScale() < highlightBuffer)
        {
            highlightBuffer -= bufferDecrease;
            bufferDecrease *= 1.2f;
        }
    }
}
