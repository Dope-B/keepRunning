using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;
    #region SingleTon
    private void Awake()
    {
        if (audioManager == null)
        {
            DontDestroyOnLoad(this.gameObject);
            audioManager = this;
            audioSourceBGM = this.gameObject.AddComponent<AudioSource>();
            StartCoroutine(playTheme());
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion SingleTon
    public AudioSource[] audioSourceEffect;
    public AudioSource audioSourceBGM;
    public ESound[] effectSound;
    public BSound[] BGMSound;
    public BSound currentBGM=null;
    public AudioClip theme;
    public string[] playSoundName;
    public float maxvol = 1f;
    public float EmaxVol = 1f;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < BGMSound.Length; i++) { if (BGMSound[i].note != null) BGMSound[i].getNote(); }
        audioSourceEffect = new AudioSource[effectSound.Length+4];
        playSoundName = new string[audioSourceEffect.Length];
        for (int i = 0; i < effectSound.Length+4; i++)
        {
            audioSourceEffect[i] = this.gameObject.AddComponent<AudioSource>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator playTheme()
    {
        audioSourceBGM.clip = theme;
        audioSourceBGM.volume = 0.5f;
        audioSourceBGM.Play();
        while (audioSourceBGM.volume < 0.5f)
        {
            audioSourceBGM.volume += 0.025f;
            yield return new WaitForEndOfFrame();
        }
        audioSourceBGM.volume = 0.5f;
    }
    public void playEffect(string name)
    {
        for (int i = 0; i < effectSound.Length; i++)
        {
            if (name == effectSound[i].name)
            {
                for (int j = 0; j < audioSourceEffect.Length; j++)
                {
                    if (!audioSourceEffect[j].isPlaying)
                    {
                        audioSourceEffect[j].volume = EmaxVol;
                        playSoundName[j] = effectSound[i].name;
                        audioSourceEffect[j].clip = effectSound[i].clip;
                        audioSourceEffect[j].Play();
                        return;
                    }
                }
                Debug.Log("no more AudioSource");
                return;
            }
        }
        Debug.Log("there's no soundName");
    }

    public void stopBGM()
    {
        audioSourceBGM.Stop();
    }
    public void stopAllEffect()
    {
        for (int i = 0; i<audioSourceEffect.Length; i++)
        {
            audioSourceEffect[i].Stop();
        }
    }
    public void stopEffect(string name)
    {
        for (int i = 0; i < audioSourceEffect.Length; i++)
        {
            if (name == playSoundName[i])
            {
                audioSourceEffect[i].Stop();
                break;
            }
        }
        Debug.Log("there's no soundName");
    }
    public void setEffectVol(float vol)
    {
        for (int i = 0; i < audioSourceEffect.Length; i++)
        {
            audioSourceEffect[i].volume = vol;
        }
    }
    public void setBGMVol(float vol) { audioSourceBGM.volume = vol; }

    public IEnumerator playBGM(string name, float time = 0)
    {
        audioManager.audioSourceBGM.time = 0;
        for (int i = 0; i < BGMSound.Length; i++)
        {
            if (name == BGMSound[i].name)
            {
                audioSourceBGM.clip = BGMSound[i].clip;
                currentBGM = BGMSound[i];
                if (time != 0) { yield return new WaitForSeconds(time); }
                audioSourceBGM.Play();
                
                yield return null;
                break;
            }
        }
    }
    public void updateNote(string name)
    {
        for (int i = 0; i < BGMSound.Length; i++) { if (name == BGMSound[i].name) { BGMSound[i].getNote(); currentBGM = BGMSound[i]; } }
    }
}
[System.Serializable]
public class ESound
{
    public string name;
    public AudioClip clip;
}
[System.Serializable]
public class BSound
{
    public string name;
    public string artist;
    public AudioClip clip;
    public int[] direction;
    public float[] timeGap;
    public Sprite albumCover;
    public Color backgroundColor;
    public TextAsset note;
    StringReader SR;
    string source;
    string[] note_direction;
    public string[] note_timegap;
    public void getNote()
    {
        SR = new StringReader(note.text);
        source = SR.ReadLine();
        note_direction = new string[source.Split('/').Length];
        note_timegap = new string[source.Split('/').Length];
        for (int i = 0; i < source.Split('/').Length; i++)
        {
            note_direction[i] = source.Split('/')[i].Split(',')[0];
            note_timegap[i] = source.Split('/')[i].Split(',')[1];
        }
        direction = new int[note_direction.Length];
        timeGap = new float[note_timegap.Length];
        for (int i = 0; i < note_direction.Length; i++)
        {
            direction[i] = int.Parse(note_direction[i]);
            timeGap[i] = float.Parse(note_timegap[i]);
        }

    }
}
