using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TurnOnGame : MonoBehaviour
{
    public static TurnOnGame TOG;
    public GameObject gameScene;
    public GameObject backGroundImage;
    public GameObject manageRoom;
    public GameObject Fblock;
    GameObject music;
    IEnumerator CturnState;
    public bool switching=false;
    public bool onGame = false;
    public bool manageMode = false;
    private void Awake()
    {
        if(TOG == null){ TOG = this; manageMode = false; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void TS()
    {
        if (sceneManager.SM.currentsceneIndex == 1 && !sceneManager.SM.switching && !ball_movement.Ball.isMoving && !switching && !musicSelect.MS.switching && !manageMode)
        {
            if (CturnState != null) { StopCoroutine(CturnState); }
            CturnState = TurnState();
            StartCoroutine(CturnState);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { TS(); }
        else if (Input.GetKeyDown(KeyCode.X)) { StartCoroutine(TurnState_ManageRoom()); }
    }
    void getMusic()
    {
        music = musicSelect.MS.music[musicSelect.MS.SelectedMusicIndex];
    }

    public IEnumerator TurnState()
    {
        
            if (sceneManager.SM.turnOn)
            {
                switching = true;
                sceneManager.SM.turnOn = false;
                sceneManager.SM.enabled = false;
                Vector3 Initscale = gameScene.transform.localScale;
                Vector3 angleForGameScene = this.transform.eulerAngles + new Vector3(0, 0, 90);
                Vector3 angleForVis = sceneManager.SM.vis.transform.eulerAngles + new Vector3(0, 0, 180f);
                ball_movement.Ball.incAlpha();
                float vol = AudioManager.audioManager.audioSourceBGM.volume;
                gameScene.transform.localScale = gameScene.transform.localScale + new Vector3(0.05f, 0.05f, 0);
                Fblock.GetComponent<block>().incAlpha();
                getMusic();
                for (int i = 0; i < 10; i++)
                {
                    music.transform.localScale = Vector3.Lerp(music.transform.localScale, new Vector3(0.7f,0.7f,1), 0.2f);
                    yield return new WaitForEndOfFrame();
                }
                backGroundImage.GetComponent<Image>().sprite= AudioManager.audioManager.BGMSound[musicSelect.MS.SelectedMusicIndex].albumCover;
                AudioManager.audioManager.playEffect("switch");
                
                for (int i = 0; i < 80; i++)
                {
                    AudioManager.audioManager.audioSourceBGM.volume -= vol / 60;
                    this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, angleForGameScene, 0.1f);
                    backGroundImage.GetComponent<Image>().color += new Color(0, 0, 0, 0.002f);
                    sceneManager.SM.vis.transform.eulerAngles = new Vector3(sceneManager.SM.vis.transform.eulerAngles.x,
                                                                            sceneManager.SM.vis.transform.eulerAngles.y,
                                                                            -Mathf.LerpAngle(-sceneManager.SM.vis.transform.eulerAngles.z, angleForVis.z, 0.15f));
                    yield return new WaitForEndOfFrame();
                }
                this.transform.eulerAngles = angleForGameScene;
                sceneManager.SM.vis.transform.eulerAngles = angleForVis;
                for (int i = 0; i < 5; i++)
                {
                    gameScene.transform.localScale = Vector3.Lerp(gameScene.transform.localScale, Initscale, 0.4f);
                    yield return new WaitForEndOfFrame();
                }
                gameScene.transform.localScale = Initscale;
                this.GetComponentInChildren<ball_movement>().enabled = true;
                ball_movement.Ball.rePos();
                
                this.GetComponentInChildren<block_checker>().enabled = true;
                this.GetComponentInChildren<block>().enabled = true;
                onGame = true;
                yield return new WaitUntil(() => AudioManager.audioManager.audioSourceBGM.volume <= 0);
                switching = false;
                yield return new WaitForSeconds(3f);
                AudioManager.audioManager.audioSourceBGM.volume = AudioManager.audioManager.maxvol;
                StartCoroutine(AudioManager.audioManager.playBGM(musicSelect.MS.music[musicSelect.MS.SelectedMusicIndex].transform.Find("name").GetComponent<TextMeshProUGUI>().text));
                
            }
            else
            {
                switching = true;
                onGame = false;
                sceneManager.SM.enabled = true;
                sceneManager.SM.turnOn = true;
                Vector3 scale = gameScene.transform.localScale + new Vector3(0.05f, 0.05f, 0);
                Vector3 Initscale = gameScene.transform.localScale;
                Vector3 angleForGameScene = this.transform.eulerAngles + new Vector3(0, 0, -90);
                Vector3 angleForVis = sceneManager.SM.vis.transform.eulerAngles - new Vector3(0, 0, 180);
                float vol = AudioManager.audioManager.audioSourceBGM.volume;
                ball_movement.Ball.transform.SetParent(this.transform.Find("GameScene"));
                Fblock.transform.SetParent(this.transform.Find("GameScene"));
                for (int i = 0; i < 8; i++)
                {
                    gameScene.transform.localScale = Vector3.Lerp(gameScene.transform.localScale, scale, 0.3f);
                    yield return new WaitForEndOfFrame();
                }
                while (backGroundImage.GetComponent<Image>().color.a >= 0f)
                {
                    backGroundImage.GetComponent<Image>().color -= new Color(0, 0, 0, 0.01f);
                    yield return new WaitForEndOfFrame();
                }
                backGroundImage.GetComponent<Image>().sprite = null;
                gameScene.transform.localScale = scale;
                AudioManager.audioManager.playEffect("switch");
                for (int i = 0; i < 80; i++)
                {
                    AudioManager.audioManager.audioSourceBGM.volume -= vol / 60;
                    this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, angleForGameScene, 0.05f);
                    if (ball_movement.Ball.GetComponent<SpriteRenderer>().color.a > 0){ ball_movement.Ball.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.1f); }
                    sceneManager.SM.vis.transform.eulerAngles = new Vector3(sceneManager.SM.vis.transform.eulerAngles.x,
                                                                            sceneManager.SM.vis.transform.eulerAngles.y,
                                                                            Mathf.LerpAngle(sceneManager.SM.vis.transform.eulerAngles.z, angleForVis.z, 0.08f));
                    yield return new WaitForEndOfFrame();
                }
                for (int i = 0; i < 16; i++)
                {
                    music.transform.localScale = Vector3.Lerp(music.transform.localScale, new Vector3(1, 1, 1), 0.2f);
                    yield return new WaitForEndOfFrame();
                }
                this.transform.eulerAngles = angleForGameScene;
                sceneManager.SM.vis.transform.eulerAngles = angleForVis;
                gameScene.transform.localScale = Initscale;
                cameraMove.cam.transform.position = new Vector3(0.5f, 0.5f, -2);
                ball_movement.Ball.score = 0;
                ball_movement.Ball.scoreText.text = "Score: " + ball_movement.Ball.score.ToString();
                this.GetComponentInChildren<block>().enabled = false;
                this.GetComponentInChildren<ball_movement>().enabled = false;
                this.GetComponentInChildren<block_checker>().enabled = false;
                Fblock.transform.localPosition = new Vector3(-10, -10, 0);
                ball_movement.Ball.transform.localPosition = new Vector3(0, 0, 0);
                yield return new WaitUntil(() => AudioManager.audioManager.audioSourceBGM.volume <= 0);
                AudioManager.audioManager.audioSourceBGM.volume = AudioManager.audioManager.maxvol/2;
                StartCoroutine(AudioManager.audioManager.playBGM(musicSelect.MS.music[musicSelect.MS.SelectedMusicIndex].transform.Find("name").GetComponent<TextMeshProUGUI>().text));
                switching = false;
            }
        yield return null;
    }
    public IEnumerator TurnState_ManageRoom()
    {
        if (sceneManager.SM.currentsceneIndex == 1 && !sceneManager.SM.switching && !ball_movement.Ball.isMoving && !switching&&!musicSelect.MS.switching)
        {
            if (sceneManager.SM.turnOn)
            {
                switching = true;
                manageMode = true;
                sceneManager.SM.turnOn = false;
                sceneManager.SM.enabled = false;
                Vector3 Initscale = gameScene.transform.localScale;
                Vector3 angleForGameScene = this.transform.eulerAngles + new Vector3(0, 0, 90);
                Vector3 angleForVis = sceneManager.SM.vis.transform.eulerAngles + new Vector3(0, 0, 180f);
                ball_movement.Ball.incAlpha();
                float vol = AudioManager.audioManager.audioSourceBGM.volume;
                gameScene.transform.localScale = gameScene.transform.localScale + new Vector3(0.05f, 0.05f, 0);
                Fblock.GetComponent<block>().incAlpha();
                manageRoom.SetActive(true);
                getMusic();
                for (int i = 0; i < 10; i++)
                {
                    music.transform.localScale = Vector3.Lerp(music.transform.localScale, new Vector3(0.7f, 0.7f, 1), 0.2f);
                    yield return new WaitForEndOfFrame();
                }
                backGroundImage.GetComponent<Image>().sprite = AudioManager.audioManager.BGMSound[musicSelect.MS.SelectedMusicIndex].albumCover;
                AudioManager.audioManager.playEffect("switch");

                for (int i = 0; i < 80; i++)
                {
                    AudioManager.audioManager.audioSourceBGM.volume -= vol / 60;
                    this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, angleForGameScene, 0.1f);
                    backGroundImage.GetComponent<Image>().color += new Color(0, 0, 0, 0.002f);
                    sceneManager.SM.vis.transform.eulerAngles = new Vector3(sceneManager.SM.vis.transform.eulerAngles.x,
                                                                            sceneManager.SM.vis.transform.eulerAngles.y,
                                                                            -Mathf.LerpAngle(-sceneManager.SM.vis.transform.eulerAngles.z, angleForVis.z, 0.15f));
                    yield return new WaitForEndOfFrame();
                }
                this.transform.eulerAngles = angleForGameScene;
                sceneManager.SM.vis.transform.eulerAngles = angleForVis;
                for (int i = 0; i < 5; i++)
                {
                    gameScene.transform.localScale = Vector3.Lerp(gameScene.transform.localScale, Initscale, 0.4f);
                    yield return new WaitForEndOfFrame();
                }
                gameScene.transform.localScale = Initscale;
                yield return new WaitUntil(() => AudioManager.audioManager.audioSourceBGM.volume <= 0);
                this.GetComponentInChildren<ball_movement>().enabled = true;
                ball_movement.Ball.rePos();
                this.GetComponentInChildren<block_checker>().enabled = true;
                this.GetComponentInChildren<block>().enabled = true;
                switching = false;
                onGame = true;
                StartCoroutine(AudioManager.audioManager.playBGM(musicSelect.MS.music[musicSelect.MS.SelectedMusicIndex].transform.Find("name").GetComponent<TextMeshProUGUI>().text));
                AudioManager.audioManager.audioSourceBGM.volume = AudioManager.audioManager.maxvol;
            }
            else
            {
                if (manageMode)
                {
                    switching = true;
                    manageMode = false;
                    onGame = false;
                    sceneManager.SM.enabled = true;
                    sceneManager.SM.turnOn = true;
                    Vector3 scale = gameScene.transform.localScale + new Vector3(0.05f, 0.05f, 0);
                    Vector3 Initscale = gameScene.transform.localScale;
                    Vector3 angleForGameScene = this.transform.eulerAngles + new Vector3(0, 0, -90);
                    Vector3 angleForVis = sceneManager.SM.vis.transform.eulerAngles - new Vector3(0, 0, 180);
                    float vol = AudioManager.audioManager.audioSourceBGM.volume;
                    ball_movement.Ball.transform.SetParent(this.transform.Find("GameScene"));
                    Fblock.transform.SetParent(this.transform.Find("GameScene"));
                    for (int i = 0; i < 8; i++)
                    {
                        gameScene.transform.localScale = Vector3.Lerp(gameScene.transform.localScale, scale, 0.3f);
                        yield return new WaitForEndOfFrame();
                    }
                    while (backGroundImage.GetComponent<Image>().color.a >= 0f)
                    {
                        backGroundImage.GetComponent<Image>().color -= new Color(0, 0, 0, 0.01f);
                        yield return new WaitForEndOfFrame();
                    }
                    backGroundImage.GetComponent<Image>().sprite = null;
                    gameScene.transform.localScale = scale;
                    AudioManager.audioManager.playEffect("switch");
                    for (int i = 0; i < 80; i++)
                    {
                        AudioManager.audioManager.audioSourceBGM.volume -= vol / 60;
                        this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, angleForGameScene, 0.05f);
                        if (ball_movement.Ball.GetComponent<SpriteRenderer>().color.a > 0) { ball_movement.Ball.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.1f); }
                        sceneManager.SM.vis.transform.eulerAngles = new Vector3(sceneManager.SM.vis.transform.eulerAngles.x,
                                                                                sceneManager.SM.vis.transform.eulerAngles.y,
                                                                                Mathf.LerpAngle(sceneManager.SM.vis.transform.eulerAngles.z, angleForVis.z, 0.08f));
                        yield return new WaitForEndOfFrame();
                    }
                    for (int i = 0; i < 16; i++)
                    {
                        music.transform.localScale = Vector3.Lerp(music.transform.localScale, new Vector3(1, 1, 1), 0.2f);
                        yield return new WaitForEndOfFrame();
                    }
                    this.transform.eulerAngles = angleForGameScene;
                    sceneManager.SM.vis.transform.eulerAngles = angleForVis;
                    gameScene.transform.localScale = Initscale;
                    cameraMove.cam.transform.position = new Vector3(0.5f, 0.5f, -2);
                    this.GetComponentInChildren<block>().enabled = false;
                    this.GetComponentInChildren<ball_movement>().enabled = false;
                    this.GetComponentInChildren<block_checker>().enabled = false;
                    Fblock.transform.localPosition = new Vector3(-10, -10, 0);
                    ball_movement.Ball.transform.localPosition = new Vector3(0, 0, 0);
                    manageRoom.SetActive(false);
                    yield return new WaitUntil(() => AudioManager.audioManager.audioSourceBGM.volume <= 0);
                    StartCoroutine(AudioManager.audioManager.playBGM(musicSelect.MS.music[musicSelect.MS.SelectedMusicIndex].transform.Find("name").GetComponent<TextMeshProUGUI>().text));
                    AudioManager.audioManager.audioSourceBGM.volume = AudioManager.audioManager.maxvol / 2;
                    switching = false;
                }
            }
        }
        yield return null;
    }
}
