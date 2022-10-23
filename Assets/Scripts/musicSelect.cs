using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class musicSelect : MonoBehaviour
{
    public static musicSelect MS;
    public GameObject[] music;
    GameObject backGround;
    public int SelectedMusicIndex = 0;
    public bool switching = false;
    private void Awake()
    {
        if (MS == null) { MS = this;}
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < music.Length; i++)
        {
            music[i] = transform.Find("Music " + "(" + i + ")").gameObject;
            float angle = (360/music.Length) * i;
            this.transform.eulerAngles = new Vector3(0, 0, angle);
            music[i].transform.localEulerAngles = -new Vector3(0, 0, angle);
            music[i].transform.position = this.transform.position + (Vector3.down * 70);
        }
        //this.transform.Rotate(new Vector3(0, 0, -90));
        this.transform.localEulerAngles = new Vector3(0, 0, 0);
        backGround = sceneManager.SM.backGround;
        banish();
        StartCoroutine(musicSwitch());
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    IEnumerator musicSwitch()
    {
        if (sceneManager.SM.turnOn&&sceneManager.SM.currentsceneIndex==1&&!sceneManager.SM.switching&&!TurnOnGame.TOG.switching && !ball_movement.Ball.isMoving)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                switching = true;
                AudioManager.audioManager.playEffect("switch");
                Vector3 scale = music[SelectedMusicIndex].transform.localScale + new Vector3(0.05f, 0.05f, 0);
                Vector3 Initscale = music[SelectedMusicIndex].transform.localScale;
                Vector3 angle = this.transform.eulerAngles + new Vector3(0, 0, (360 / music.Length));
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < music.Length; j++)
                    {
                        music[j].transform.localScale = Vector3.Lerp(music[j].transform.localScale, scale, 0.2f);
                    }
                    yield return new WaitForEndOfFrame();
                }
                music[SelectedMusicIndex].transform.localScale = scale;
                if (SelectedMusicIndex == music.Length-1) { music[0].SetActive(true);SelectedMusicIndex = 0; }
                else { music[SelectedMusicIndex + 1].SetActive(true);SelectedMusicIndex += 1; }
                Color newC = AudioManager.audioManager.BGMSound[SelectedMusicIndex].backgroundColor;
                float vol = AudioManager.audioManager.audioSourceBGM.volume;
                for (int i = 0; i < 40; i++)
                {
                    AudioManager.audioManager.audioSourceBGM.volume -= vol / 30;
                    this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, angle, 0.15f);
                    backGround.GetComponent<Image>().color = new Color(Mathf.Lerp(backGround.GetComponent<Image>().color.r, newC.r, 0.1f),
                                                                       Mathf.Lerp(backGround.GetComponent<Image>().color.g, newC.g, 0.1f),
                                                                       Mathf.Lerp(backGround.GetComponent<Image>().color.b, newC.b, 0.1f));
                    yield return new WaitForEndOfFrame();
                }
                this.transform.eulerAngles = angle;
                backGround.GetComponent<Image>().color = newC;
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < music.Length; j++)
                    {
                        music[j].transform.localScale = Vector3.Lerp(music[j].transform.localScale, Initscale, 0.2f);
                    }
                    yield return new WaitForEndOfFrame();
                }
                AudioManager.audioManager.audioSourceBGM.volume = AudioManager.audioManager.maxvol / 2;
                StartCoroutine(AudioManager.audioManager.playBGM(music[SelectedMusicIndex].transform.Find("name").GetComponent<TextMeshProUGUI>().text));
                music[SelectedMusicIndex].transform.localScale = Initscale;
                banish();
                switching = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                switching = true;
                AudioManager.audioManager.playEffect("switch");
                Vector3 scale = music[SelectedMusicIndex].transform.localScale + new Vector3(0.05f, 0.05f, 0);
                Vector3 Initscale = music[SelectedMusicIndex].transform.localScale;
                Vector3 angle = this.transform.eulerAngles + new Vector3(0, 0, -(360 / music.Length));
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < music.Length; j++)
                    {
                        music[j].transform.localScale = Vector3.Lerp(music[j].transform.localScale, scale, 0.2f);
                    }
                    yield return new WaitForEndOfFrame();
                }
                music[SelectedMusicIndex].transform.localScale = scale;
                if (SelectedMusicIndex == 0) { music[music.Length-1].SetActive(true); SelectedMusicIndex = music.Length - 1; }
                else { music[SelectedMusicIndex - 1].SetActive(true); SelectedMusicIndex -= 1; }
                Color newC = AudioManager.audioManager.BGMSound[SelectedMusicIndex].backgroundColor;
                float vol = AudioManager.audioManager.audioSourceBGM.volume;
                for (int i = 0; i < 40; i++)
                {
                    AudioManager.audioManager.audioSourceBGM.volume -= vol / 30;
                    this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, Mathf.LerpAngle(this.transform.eulerAngles.z, angle.z, 0.15f));
                    backGround.GetComponent<Image>().color = new Color(Mathf.Lerp(backGround.GetComponent<Image>().color.r, newC.r, 0.1f),
                                                                       Mathf.Lerp(backGround.GetComponent<Image>().color.g, newC.g, 0.1f),
                                                                       Mathf.Lerp(backGround.GetComponent<Image>().color.b, newC.b, 0.1f));
                    yield return new WaitForEndOfFrame();
                }
                this.transform.eulerAngles = angle;
                backGround.GetComponent<Image>().color = newC;
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < music.Length; j++)
                    {
                        music[j].transform.localScale = Vector3.Lerp(music[j].transform.localScale, Initscale, 0.2f);
                    }
                    yield return new WaitForEndOfFrame();
                }
                AudioManager.audioManager.audioSourceBGM.volume = AudioManager.audioManager.maxvol / 2;
                StartCoroutine(AudioManager.audioManager.playBGM(music[SelectedMusicIndex].transform.Find("name").GetComponent<TextMeshProUGUI>().text));
                music[SelectedMusicIndex].transform.localScale = Initscale;                
                banish();
                switching = false;
            }
            else { }
        }
        yield return null;
        StartCoroutine(musicSwitch());
    }
    void banish()
    {
        for (int i = 0; i < music.Length; i++) { if (i != SelectedMusicIndex) { music[i].SetActive(false); } }
    }
}
