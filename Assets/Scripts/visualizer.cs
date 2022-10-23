using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class visualizer : MonoBehaviour
{
    #region SingleTon
    private void Awake()
    {
        if (vis[0] == null)
        {
            vis[0] = this;
            
        }
        else if (vis[1] == null) { vis[1] = this; }
    }
    #endregion SingleTon
    public static visualizer[] vis=new visualizer[2];
    public BSound currentBGM;
    AudioManager audioManager;
    public static float[] samples = new float[512];
    public static float[] freBand = new float[64];
    public static float[] bandBuffer = new float[64];
    float[] bufferDecrease = new float[64];
    public float maxScale = 20;
    public float halfRad = 5f;
    public int sortNum;
    public int visMode = 1;
    public GameObject sampleSquarePrefab;
    GameObject[] sampleSquare = new GameObject[64];

    // Start is called before the first frame update
    private void Start()
    {
        setSquare();
        StartCoroutine(visualize());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void getSpectrum()
    {
        audioManager.audioSourceBGM.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }
    void updateSquare()
    {
        for (int i = 0; i < freBand.Length; i++)
        {
            if (sampleSquare != null)
            {
                if (sortNum == 0) { sampleSquare[i].transform.localScale = new Vector3(8f, Mathf.Clamp((bandBuffer[i] * 40 * (3 - (AudioManager.audioManager.audioSourceBGM.volume * 2))) + 10f, 10, 200), 1); }
                else { sampleSquare[i].transform.localScale = new Vector3(12f, Mathf.Clamp((bandBuffer[i] * 40 * (3 - (AudioManager.audioManager.audioSourceBGM.volume * 2))) + 10f, 10, 200), 1); }
            }
        }
    }
    void setSquare()
    {
        this.audioManager = AudioManager.audioManager;
        this.currentBGM = AudioManager.audioManager.currentBGM;
        for (int i = 0; i < freBand.Length; i++)
        {
            GameObject samplePrefab = (GameObject)Instantiate(sampleSquarePrefab);
            samplePrefab.transform.position = this.transform.position;
            samplePrefab.transform.parent = this.transform;
            samplePrefab.name = "Square" + i;
            if (sortNum == 0)
            {
                float angle = (-5.625f * i);
                this.transform.eulerAngles = new Vector3(0, 0, angle);
                samplePrefab.transform.position = transform.position + (Vector3.down * halfRad);
            }
            else if(sortNum==1)
            {
                samplePrefab.transform.eulerAngles = new Vector3(0, 0, 180);
                samplePrefab.transform.position = transform.position + new Vector3(i - 32, 0, 0) * 0.3f - new Vector3(0, 5, 0);
            }
            switch (visMode)
            {
                case 0:
                        samplePrefab.GetComponent<SpriteRenderer>().color = new Color(255/255f, 255/255f, 255/255f, 1);
                    break;
                case 1:
                        samplePrefab.GetComponent<SpriteRenderer>().color = new Color(UnityEngine.Random.Range(40, 255) / 255f, UnityEngine.Random.Range(40, 255) / 255f, UnityEngine.Random.Range(40, 255) / 255f, 1);
                    break;
                default:
                    if (UnityEngine.Random.Range(0, 100) > 70)
                    {
                        samplePrefab.GetComponent<SpriteRenderer>().color = new Color(UnityEngine.Random.Range(40, 255) / 255f, UnityEngine.Random.Range(40, 255) / 255f, UnityEngine.Random.Range(40, 255) / 255f, 1);
                    }
                    else
                    {
                        samplePrefab.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 1);
                    }
                    break;
            }
            sampleSquare[i] = samplePrefab;
        }
        this.transform.eulerAngles = new Vector3(0, 0, 0);
    }
    void setFrequencyBands()
    {
        int count = 0;
        for (int i = 0; i < freBand.Length; i++)
        {
            float average = 0;
            int sampleCount = 6 * i;
            for (int j = 0; j < 6; j++)
            {
                average += samples[sampleCount+j]*(count*((i*0.1f)+0.2f));
                count++;
            }
            average /= count;
            freBand[i] = average * 10;
        }
    }
    void setBandBuffer()
    {
        for (int i = 0; i < freBand.Length; ++i)
        {
            if (freBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = freBand[i];
                bufferDecrease[i] = 0.01f;
            }
            if(freBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.1f;
            }
        }
    }
    public void visColorManage()
    {
        for (int i = 0; i < sampleSquare.Length; i++)
        {
            switch (visMode)
            {
                case 0:
                    sampleSquare[i].GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 1);
                    break;
                case 1:
                    sampleSquare[i].GetComponent<SpriteRenderer>().color = new Color(UnityEngine.Random.Range(40, 255) / 255f, UnityEngine.Random.Range(40, 255) / 255f, UnityEngine.Random.Range(40, 255) / 255f, 1);
                    break;
                default:
                    if (UnityEngine.Random.Range(0, 100) > 40)
                    {
                        sampleSquare[i].GetComponent<SpriteRenderer>().color = new Color(UnityEngine.Random.Range(40, 255) / 255f, UnityEngine.Random.Range(40, 255) / 255f, UnityEngine.Random.Range(40, 255) / 255f, 1);
                    }
                    else
                    {
                        sampleSquare[i].GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 1);
                    }
                    break;
            }
        }
    }
    IEnumerator visualize()
    {
        getSpectrum();
        setFrequencyBands();
        setBandBuffer();
        updateSquare();
        yield return new WaitForEndOfFrame();
        StartCoroutine(visualize());
    }
}
