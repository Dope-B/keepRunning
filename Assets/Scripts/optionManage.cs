using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class optionManage : MonoBehaviour
{
    public Slider music;
    public TextMeshProUGUI TM;
    public Slider effect;
    public TextMeshProUGUI TE;
    public Slider sync;
    public TextMeshProUGUI TS;
    public Slider visMode;
    public TextMeshProUGUI TV;
    public Slider blockMode;
    public TextMeshProUGUI TB;

    // Start is called before the first frame update
    void Start()
    {
        music.value = AudioManager.audioManager.maxvol;
        TM.text = music.value.ToString("N3");
        effect.value = AudioManager.audioManager.EmaxVol;
        TE.text = effect.value.ToString("N3");
        sync.value = block_checker.block_Checker.sync;
        TS.text = sync.value.ToString("N3");
        visMode.value = visualizer.vis[0].visMode;
        switch ((int)visMode.value)
        {
            case 0:
                TV.text = "White";
                break;
            case 1:
                TV.text = "Color without White";
                break;
            default:
                TV.text = "Color with White";
                break;
        }
        blockMode.value = block_checker.block_Checker.blockSummonMode;
        switch ((int)blockMode.value)
        {
            case 0:
                TB.text = "Standard";
                break;
            case 1:
                TB.text = "Random";
                break;
            case 2:
                TB.text = "Challenge";
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneManager.SM.currentsceneIndex != 2) { music.interactable = false; effect.interactable = false;sync.interactable = false;blockMode.interactable = false; }
        else { music.interactable = true; effect.interactable = true; sync.interactable = true; blockMode.interactable = true; }
    }

    public void musicVolManage()
    {
        if (sceneManager.SM.currentsceneIndex == 2)
        {
            AudioManager.audioManager.maxvol = music.value;
            AudioManager.audioManager.audioSourceBGM.volume = music.value;
            TM.text = music.value.ToString("N3");
        }
        
    }
    public void effectVolManage()
    {
        if (sceneManager.SM.currentsceneIndex == 2)
        {
            AudioManager.audioManager.EmaxVol = effect.value;
            TE.text = effect.value.ToString("N3");
        }

    }
    public void syncManage()
    {
        if (sceneManager.SM.currentsceneIndex == 2)
        {
            block_checker.block_Checker.sync = sync.value;
            TS.text = sync.value.ToString("N3");
        }

    }
    public void visManage()
    {
        if (sceneManager.SM.currentsceneIndex == 2)
        {
            visualizer.vis[0].visMode = (int)visMode.value;
            visualizer.vis[1].visMode = (int)visMode.value;
            switch ((int)visMode.value)
            {
                case 0:
                    TV.text = "White";
                    break;
                case 1:
                    TV.text = "Color without White";
                    break;
                default:
                    TV.text = "Color with White";
                    break;
            }
            visualizer.vis[0].visColorManage();
            visualizer.vis[1].visColorManage();
        }

    }
    public void blockModeManage()
    {
        if (sceneManager.SM.currentsceneIndex == 2)
        {
            block_checker.block_Checker.blockSummonMode = (int)blockMode.value;
            switch ((int)blockMode.value)
            {
                case 0:
                    TB.text = "Standard";
                    break;
                case 1:
                    TB.text = "Random";
                    break;
                case 2:
                    TB.text = "Challenge";
                    break;
                default:
                    break;
            }
        }

    }
}
