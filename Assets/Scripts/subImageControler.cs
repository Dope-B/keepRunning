using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class subImageControler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Alphatrans());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Alphatrans()
    {
        float min = Random.Range(0, 0.15f);
        float max = Random.Range(0.25f, 0.35f);
        while (this.GetComponent<Image>().color.a<=max)
        {
            this.GetComponent<Image>().color += new Color(0, 0, 0, 0.001f);
            yield return new WaitForEndOfFrame();
        }
        while (this.GetComponent<Image>().color.a >= min)
        {
            this.GetComponent<Image>().color -= new Color(0, 0, 0, 0.001f);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(Alphatrans());
    }
}
