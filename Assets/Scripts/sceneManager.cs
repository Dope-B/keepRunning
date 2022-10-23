using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sceneManager : MonoBehaviour
{
    #region SingleTon
    private void Awake()
    {
        if (SM == null)
        {
            SM = this;
        }
    }
    #endregion SingleTon
    public static sceneManager SM;
    public GameObject[] scene = new GameObject[3];
    public GameObject vis;
    public GameObject backGround;
    public int currentsceneIndex = 0;
    public bool switching=false;
    public bool turnOn = true;
    float vol = 0;
    Color newC = new Color(210, 210, 210);
    // Start is called before the first frame update

    void OnEnable()
    {
            turnOn = true;
            StartCoroutine(sceneSwitch());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator sceneSwitch()
    {
        if (turnOn)
        {
            if (!musicSelect.MS.switching && !TurnOnGame.TOG.switching && !switching)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    switching = true;
                    AudioManager.audioManager.playEffect("switch");
                    Vector3 scale = scene[currentsceneIndex].transform.localScale + new Vector3(0.05f, 0.05f, 0);
                    Vector3 Initscale = scene[currentsceneIndex].transform.localScale;
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < scene.Length; j++)
                        {
                            scene[j].transform.localScale = Vector3.Lerp(scene[j].transform.localScale, scale, 0.2f);
                        }
                        yield return new WaitForEndOfFrame();
                    }
                    scene[currentsceneIndex].transform.localScale = scale;
                    if (currentsceneIndex == 2) { currentsceneIndex = 0; }
                    else { currentsceneIndex++; }
                    if (currentsceneIndex == 1)
                    {
                        newC = AudioManager.audioManager.BGMSound[musicSelect.MS.SelectedMusicIndex].backgroundColor;
                        vol = AudioManager.audioManager.audioSourceBGM.volume;
                    }
                    else if (currentsceneIndex == 2)
                    {
                        newC = new Color(210 / 255f, 210 / 255f, 210 / 255f);
                        vol = AudioManager.audioManager.audioSourceBGM.volume;
                    }
                    Vector3 angle = this.transform.eulerAngles + new Vector3(0, 0, 120);
                    for (int i = 0; i < 50; i++)
                    {
                        if (currentsceneIndex == 2 || currentsceneIndex == 1) { AudioManager.audioManager.audioSourceBGM.volume -= vol / 40; }
                        this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, angle, 0.15f);
                        if (backGround.GetComponent<Image>().color != newC)
                        {
                            backGround.GetComponent<Image>().color = new Color(Mathf.Lerp(backGround.GetComponent<Image>().color.r, newC.r, 0.1f),
                                                                               Mathf.Lerp(backGround.GetComponent<Image>().color.g, newC.g, 0.1f),
                                                                               Mathf.Lerp(backGround.GetComponent<Image>().color.b, newC.b, 0.1f));
                        }
                        yield return new WaitForEndOfFrame();
                    }
                    this.transform.eulerAngles = angle;
                    backGround.GetComponent<Image>().color = newC;
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < scene.Length; j++)
                        {
                            scene[j].transform.localScale = Vector3.Lerp(scene[j].transform.localScale, Initscale, 0.2f);
                        }
                        yield return new WaitForEndOfFrame();
                    }
                    scene[currentsceneIndex].transform.localScale = Initscale;
                    if (currentsceneIndex == 2) { AudioManager.audioManager.audioSourceBGM.volume = AudioManager.audioManager.maxvol / 2; StartCoroutine(AudioManager.audioManager.playTheme()); }
                    else if (currentsceneIndex == 1)
                    {
                        AudioManager.audioManager.audioSourceBGM.volume = AudioManager.audioManager.maxvol / 2;
                        StartCoroutine(AudioManager.audioManager.playBGM(musicSelect.MS.music[musicSelect.MS.SelectedMusicIndex].transform.Find("name").GetComponent<TextMeshProUGUI>().text));
                    }
                    switching = false;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    switching = true;
                    AudioManager.audioManager.playEffect("switch");
                    Vector3 scale = scene[currentsceneIndex].transform.localScale + new Vector3(0.05f, 0.05f, 0);
                    Vector3 Initscale = scene[currentsceneIndex].transform.localScale;
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < scene.Length; j++)
                        {
                            scene[j].transform.localScale = Vector3.Lerp(scene[j].transform.localScale, scale, 0.2f);
                        }
                        yield return new WaitForEndOfFrame();
                    }
                    scene[currentsceneIndex].transform.localScale = scale;
                    if (currentsceneIndex == 0) { currentsceneIndex = 2; newC = new Color(210 / 255f, 210 / 255f, 210 / 255f); }
                    else { currentsceneIndex--; }
                    if (currentsceneIndex == 1)
                    {
                        newC = AudioManager.audioManager.BGMSound[musicSelect.MS.SelectedMusicIndex].backgroundColor;
                        vol = AudioManager.audioManager.audioSourceBGM.volume;
                    }
                    else if (currentsceneIndex == 0)
                    {
                        newC = new Color(210 / 255f, 210 / 255f, 210 / 255f);
                        vol = AudioManager.audioManager.audioSourceBGM.volume;
                    }
                    Vector3 angle = this.transform.eulerAngles + new Vector3(0, 0, -120);
                    for (int i = 0; i < 50; i++)
                    {
                        if (currentsceneIndex == 0 || currentsceneIndex == 1) { AudioManager.audioManager.audioSourceBGM.volume -= vol / 40; }
                        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, Mathf.LerpAngle(this.transform.eulerAngles.z, angle.z, 0.15f));
                        if (backGround.GetComponent<Image>().color != newC)
                        {
                            backGround.GetComponent<Image>().color = new Color(Mathf.Lerp(backGround.GetComponent<Image>().color.r, newC.r, 0.1f),
                                                                               Mathf.Lerp(backGround.GetComponent<Image>().color.g, newC.g, 0.1f),
                                                                               Mathf.Lerp(backGround.GetComponent<Image>().color.b, newC.b, 0.1f));
                        }
                        yield return new WaitForEndOfFrame();
                    }
                    this.transform.eulerAngles = angle;
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < scene.Length; j++)
                        {
                            scene[j].transform.localScale = Vector3.Lerp(scene[j].transform.localScale, Initscale, 0.2f);
                        }
                        yield return new WaitForEndOfFrame();
                    }
                    scene[currentsceneIndex].transform.localScale = Initscale;
                    if (currentsceneIndex == 0) { AudioManager.audioManager.audioSourceBGM.volume = AudioManager.audioManager.maxvol / 2; StartCoroutine(AudioManager.audioManager.playTheme()); }
                    else if (currentsceneIndex == 1)
                    {
                        AudioManager.audioManager.audioSourceBGM.volume = AudioManager.audioManager.maxvol / 2;
                        StartCoroutine(AudioManager.audioManager.playBGM(musicSelect.MS.music[musicSelect.MS.SelectedMusicIndex].transform.Find("name").GetComponent<TextMeshProUGUI>().text));
                    }
                    backGround.GetComponent<Image>().color = newC;
                    switching = false;
                }
            }
            yield return null;
            StartCoroutine(sceneSwitch());
        }
            
    }
    /*public IEnumerator rotate(KeyCode KC1,KeyCode KC2,bool SW,GameObject[] target,int index,int lastIndex,int firstIndex,float Angle, int angleTime=50,float angleMag=0.15f,float plusScale=0.05f,int scaleTime=5,float scaleMag=0.4f )
    {
        if (sceneManager.SM.turnOn)
        {
            if (Input.GetKeyDown(KC1))
            {
                AudioManager.audioManager.playEffect("switch");
                Vector3 scale = target[index].transform.localScale + new Vector3(plusScale, plusScale, 0);
                Vector3 Initscale = target[index].transform.localScale;
                for (int i = 0; i < scaleTime; i++)
                {
                    for (int j = 0; j < target.Length; j++)
                    {
                        target[j].transform.localScale = Vector3.Lerp(target[j].transform.localScale, scale, scaleMag);
                        yield return new WaitForEndOfFrame();
                    }
                }
                target[index].transform.localScale = scale;
                if (index == lastIndex) { index = firstIndex; }
                else { index++; }
                Vector3 angle = this.transform.eulerAngles + new Vector3(0, 0, Angle);
                SW = true;
                yield return new WaitUntil(() => SW);
                SW = false;
                for (int i = 0; i < angleTime; i++)
                {
                    this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, angle, angleMag);
                    yield return new WaitForEndOfFrame();
                }
                this.transform.eulerAngles = angle;
                SW = true;
                yield return new WaitUntil(() => SW);
                SW = false;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < target.Length; j++)
                    {
                        target[j].transform.localScale = Vector3.Lerp(target[j].transform.localScale, Initscale, scaleMag);
                        yield return new WaitForEndOfFrame();
                    }
                }
                SW = true;
                target[index].transform.localScale = Initscale;
                yield return new WaitUntil(() => SW);
                SW = false;
            }
            else if (Input.GetKeyDown(KC2))
            {
                AudioManager.audioManager.playEffect("switch");
                Vector3 scale = target[index].transform.localScale + new Vector3(plusScale, plusScale, 0);
                Vector3 Initscale = target[index].transform.localScale;
                for (int i = 0; i < scaleTime; i++)
                {
                    for (int j = 0; j < target.Length; j++)
                    {
                        target[j].transform.localScale = Vector3.Lerp(target[j].transform.localScale, scale, scaleMag);
                        yield return new WaitForEndOfFrame();
                    }
                }
                target[index].transform.localScale = scale;
                if (index == firstIndex) { index = lastIndex; }
                else { index--; }
                Vector3 angle = this.transform.eulerAngles + new Vector3(0, 0, -Angle);
                SW = true;
                yield return new WaitUntil(() => SW);
                SW = false;
                for (int i = 0; i < angleTime; i++)
                {
                    this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, Mathf.LerpAngle(this.transform.eulerAngles.z, angle.z, scaleMag*0.25f));
                    yield return new WaitForEndOfFrame();
                }
                this.transform.eulerAngles = angle;
                SW = true;
                yield return new WaitUntil(() => SW);
                SW = false;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < target.Length; j++)
                    {
                        target[j].transform.localScale = Vector3.Lerp(target[j].transform.localScale, Initscale, scaleMag);
                        yield return new WaitForEndOfFrame();
                    }
                }
                SW = true;
                target[index].transform.localScale = Initscale;
                yield return new WaitUntil(() => SW);
                SW = false;
            }
            yield return null;
            StartCoroutine(rotate(KC1,KC2,SW,target,index,lastIndex,firstIndex,Angle,angleTime,angleMag,plusScale,scaleTime,scaleMag));
        }
    }*/
}
public class Scene
{
    public GameObject scene;
    public int index;
}
