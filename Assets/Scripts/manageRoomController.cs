using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class manageRoomController : MonoBehaviour
{
    public static manageRoomController MC;
    public Slider ED;
    public GameObject ED_handle;
    public Slider ED_Controller;
    public TextMeshProUGUI time;
    public GameObject stateImage;
    public bool isPlaying = true;
    public Sprite play;
    public Sprite pause;
    public GameObject notePrefab;
    public GameObject focusedNote;
    public TextMeshProUGUI num;
    public TMP_InputField dir;
    public TMP_InputField tim;
    public List<GameObject> notes = new List<GameObject>();
    public Button add;
    public Button delete;
    public Button edit;
    public Button check;
    public Button save;
    public Button reset;
    public bool editMode = true;
    private void Awake()
    {
        if (MC == null) { MC = this; }
    }
    // Start is called before the first frame update
    void Start()
    {
        ED_Controller.minValue = 0;
        ED_Controller.maxValue = AudioManager.audioManager.audioSourceBGM.clip.length - 30;
        ED.minValue = ED_Controller.value;
        ED.maxValue = ED_Controller.value + 30;
    }
    private void OnEnable()
    {
        Start();
        isPlaying = true;
        editMode = true;
        stateImage.GetComponent<Image>().sprite = play;
        ED.value = 0;
        ED_Controller.value = 0;
        dir.DeactivateInputField();
        tim.DeactivateInputField();
        editMode = true;
        edit.interactable = false;
        check.interactable = true;
        add.interactable = true;
        delete.interactable = true;
        reset.interactable = true;
        save.interactable = true;
        setNote();
    }
    private void OnDisable()
    {
        for (int i = 0; i < notes.Count; i++)
        {
            Destroy(notes[i].gameObject);
        }
        notes.Clear();
    }
    // Update is called once per frame
    void Update()
    {
        roomUpdate();
        buttonCheck();
    }
    void setNote()
    {
        for (int i = 0; i < AudioManager.audioManager.currentBGM.direction.Length; i++)
        {
            GameObject note = Instantiate(notePrefab);
            note.GetComponent<NoteForManageRoom>().setNoteForManageRoom(i, AudioManager.audioManager.currentBGM.direction[i], float.Parse(AudioManager.audioManager.currentBGM.note_timegap[i]));
            note.transform.SetParent(ED.gameObject.transform);
            note.transform.localScale =new Vector3 (1, 1, 1);
            note.transform.localEulerAngles = new Vector3(0, 0, 0);
            notes.Add(note);
        }

    }
    void roomUpdate()
    {
        if (TurnOnGame.TOG.onGame)
        {
            time.text = AudioManager.audioManager.audioSourceBGM.time.ToString("N3");
            ED.minValue = ED_Controller.value;
            ED.maxValue = ED_Controller.value + 30;
            ED.value = AudioManager.audioManager.audioSourceBGM.time;
            if ((ED.value <= ED.minValue) || (ED.value >= ED.maxValue)) { ED_handle.SetActive(false); }
            else { ED_handle.SetActive(true); }
        }
        else
        {
            time.text = "0.0";
            ED.value = 0;
        }
    }
    void buttonCheck()
    {
        if (Input.GetKeyDown(KeyCode.C) && !ball_movement.Ball.isMoving&& TurnOnGame.TOG.onGame)
        {
            if (isPlaying) { pauseMusic(); }
            else { unpauseMusic(); }
        }
    }
    public void pauseMusic()
    {
        if (notes.Find(x => x.GetComponent<NoteForManageRoom>().Time >= ED.value) != null) 
        {
            ball_movement.Ball.currentIndex = notes.Find(x => x.GetComponent<NoteForManageRoom>().Time >= ED.value).GetComponent<NoteForManageRoom>().Num;
        }
        else { ball_movement.Ball.currentIndex = notes.Count; }
        AudioManager.audioManager.audioSourceBGM.Pause(); isPlaying = false; stateImage.GetComponent<Image>().sprite = pause;
    }
    public void unpauseMusic()
    {
        AudioManager.audioManager.audioSourceBGM.UnPause(); isPlaying = true; stateImage.GetComponent<Image>().sprite = play;
        
    }
    public void noteNumChange()
    {
        if (focusedNote != null) { focusedNote.GetComponent<NoteForManageRoom>().NumChanged(); }      
    }
    public void noteDirChange()
    {
        if (focusedNote != null) { focusedNote.GetComponent<NoteForManageRoom>().DirChanged(); }
    }
    public void noteTimeChange()
    {
        if (focusedNote != null) { focusedNote.GetComponent<NoteForManageRoom>().TimeChanged(); }
    }
    public void editButton()
    {
        editMode = true;
        edit.interactable = false;
        check.interactable = true;
        add.interactable = true;
        delete.interactable = true;
        reset.interactable = true;
        save.interactable = true;
    }
    public void checkButton()
    {
        editMode = false;
        edit.interactable = true;
        check.interactable = false;
        add.interactable = false;
        delete.interactable = false;
        reset.interactable = false;
        save.interactable = false;
    }
    public void saveButton()
    {
        StreamWriter SW = new StreamWriter("Assets/UsingAsset/music/note/"+AudioManager.audioManager.currentBGM.name.ToString()+".txt", false);
        for (int i = 0; i < notes.Count; i++)
        {
            if (i + 1 != notes.Count) { SW.Write(notes[i].GetComponent<NoteForManageRoom>().Dir + "," + notes[i].GetComponent<NoteForManageRoom>().Time + "/"); }
            else { SW.Write(notes[i].GetComponent<NoteForManageRoom>().Dir + "," + notes[i].GetComponent<NoteForManageRoom>().Time); }
        }
        AudioManager.audioManager.updateNote(AudioManager.audioManager.currentBGM.name);
        SW.Close();
    }
    public void resetButton()
    {
        for (int i = 0; i < notes.Count; i++)
        {
            Destroy(notes[i].gameObject);
        }
        notes.Clear();
        block_checker.block_Checker.currentBlockIndex = 0;
        ball_movement.Ball.currentIndex = 0;
        block_checker.block_Checker.lastBlockPosX = 0;
        block_checker.block_Checker.lastBlockPosY = 0;
    }
    void sortNoteIndex(int index)
    {
        for (int i = index; i < notes.Count; i++)
        {
            notes[i].GetComponent<NoteForManageRoom>().Num = i;
        }
    }
    public void addButton(int dir=0)
    {
        GameObject note = Instantiate(notePrefab);
        int index = 0;
        if(notes.Find(x => x.GetComponent<NoteForManageRoom>().Time >= AudioManager.audioManager.audioSourceBGM.time) != null) { index = notes.Find(x => x.GetComponent<NoteForManageRoom>().Time >= AudioManager.audioManager.audioSourceBGM.time).GetComponent<NoteForManageRoom>().Num; }
        else { index = notes.Count; }
        sortNoteIndex(index + 1);
        if (dir != 0) { note.GetComponent<NoteForManageRoom>().setNoteForManageRoom(index, dir, AudioManager.audioManager.audioSourceBGM.time); }
        else
        {
            int RDindex = 0;
            int[] randomDir;
            if (index == 0)
            {
                randomDir = new int[4];
                randomDir[0] = -2;randomDir[1] = -1;randomDir[2] = 1; randomDir[3] = -2;}
            else
            {
                if (notes[index - 1].GetComponent<NoteForManageRoom>().Dir == notes[index + 1].GetComponent<NoteForManageRoom>().Dir) { randomDir = new int[3]; }
                else { randomDir = new int[2]; }
                for (int i = -2; i <= 2; i++)
                {
                    if (i == 0 || (index >= 1 && -i == notes[index - 1].GetComponent<NoteForManageRoom>().Dir) || (-i == notes[index + 1].GetComponent<NoteForManageRoom>().Dir)) { Debug.Log(i + "   pass"); }
                    else { Debug.Log("insert Num:  " + i + "   index:   " + RDindex); randomDir[RDindex] = i; RDindex++; }
                }
            }
            
            for (int i = 0; i < randomDir.Length; i++)
            {
                Debug.Log(randomDir[i]);
            }
            note.GetComponent<NoteForManageRoom>().setNoteForManageRoom(index, randomDir[Random.Range(0,randomDir.Length)], AudioManager.audioManager.audioSourceBGM.time);
        }
        notes.Insert(index, note);
        note.transform.SetParent(ED.gameObject.transform);
        note.transform.localScale = new Vector3(1, 1, 1);
        note.transform.localEulerAngles = new Vector3(0, 0, 0);
        ED_Value_controll();
    }
    public void deleteButton()
    {
        Destroy(focusedNote.gameObject);
        notes.Remove(focusedNote);
        if (focusedNote.GetComponent<NoteForManageRoom>().Num-1 != notes.Count) { sortNoteIndex(focusedNote.GetComponent<NoteForManageRoom>().Num); }
        ED_Value_controll();
    }
    public void playButton()
    {
        if (isPlaying) { pauseMusic(); }
        else { unpauseMusic(); }
    }
    public void ED_Value_controll()
    {
        if (!isPlaying)
        {
            if (ED.value <= ED.minValue || ED.value >= ED.maxValue) { ED.value = AudioManager.audioManager.audioSourceBGM.time; }
            else { AudioManager.audioManager.audioSourceBGM.time = ED.value;  }
            if (notes.Find(x => x.GetComponent<NoteForManageRoom>().Time >= ED.value) != null)
            {
                if (!ball_movement.Ball.isMoving) {
                    ball_movement.Ball.currentIndex = Mathf.Max(notes.Find(x => x.GetComponent<NoteForManageRoom>().Time >= ED.value).GetComponent<NoteForManageRoom>().Num - 1, 0);
                }
            }
        }
    }
}
