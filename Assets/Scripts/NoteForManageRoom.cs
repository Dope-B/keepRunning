using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteForManageRoom : MonoBehaviour
{
    bool isFocused = false;
    public int Num;
    public int Dir;
    public float Time;
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(Mathf.Lerp(-175f, 175f, Mathf.Clamp((Time - manageRoomController.MC.ED.minValue) / 30, 0, 1)), 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (manageRoomController.MC.isPlaying||manageRoomController.MC.focusedNote!=this.gameObject) { unfocused();}
        transform.localPosition = new Vector3(Mathf.Lerp(-175f, 175f, Mathf.Clamp((Time-manageRoomController.MC.ED.minValue)/30 ,0, 1)), 0, 0);
        if (isInRange()) {
            if (!GetComponent<Button>().enabled)
                GetComponent<Image>().enabled = true;
            GetComponent<Button>().enabled = true;
        }
        else 
        {
            if (GetComponent<Button>().enabled) {
                GetComponent<Image>().enabled = false;
                GetComponent<Button>().enabled = false;
            }
        }
    }
    private void OnDestroy()
    {
        unfocused();
    }
    public void setNoteForManageRoom(int Num,int Dir,float Time)
    {
        this.Num = Num;
        this.Dir = Dir;
        this.Time = Time;
    }
    public void focused()
    {
        if (!isFocused)
        {
            GetComponent<Image>().color = new Color(255/255f, 70/255f, 70/255f, 1f);
            manageRoomController.MC.dir.ActivateInputField();
            manageRoomController.MC.tim.ActivateInputField();
            manageRoomController.MC.pauseMusic();
            manageRoomController.MC.focusedNote = this.gameObject;
            manageRoomController.MC.num.text = this.Num.ToString();
            manageRoomController.MC.dir.text = this.Dir.ToString();
            manageRoomController.MC.tim.text = this.Time.ToString();
            isFocused = true;
        }
        else
        {
            unfocused();
        }
        
    }
    void unfocused()
    {
        GetComponent<Image>().color = new Color(255/255f, 255/255f, 255/255f, 1f);
        if (manageRoomController.MC.focusedNote == this.gameObject) {
            manageRoomController.MC.focusedNote = null;
            manageRoomController.MC.num.text = "Index";
            manageRoomController.MC.dir.text = null;
            manageRoomController.MC.tim.text = null;
            manageRoomController.MC.dir.DeactivateInputField();
            manageRoomController.MC.tim.DeactivateInputField();
            
        }
        isFocused = false;
    }
    public bool isInRange()
    {
        if (Time >= manageRoomController.MC.ED.minValue && Time <= manageRoomController.MC.ED.maxValue) { return true; }
        else { return false; }
    }
    public void NumChanged()
    {
        Num = int.Parse(manageRoomController.MC.num.text);
    }
    public void DirChanged()
    {
        
        if (Num != 0)
        {
            if(manageRoomController.MC.notes.Count>Num-1&& manageRoomController.MC.notes.Count!=Num)
            {
                if (Dir != manageRoomController.MC.notes[Num - 1].GetComponent<NoteForManageRoom>().Dir && Dir != manageRoomController.MC.notes[Num + 1].GetComponent<NoteForManageRoom>().Dir)
                {
                    Dir = int.Parse(manageRoomController.MC.dir.text);
                }
            }
            else if(manageRoomController.MC.notes.Count != Num)
            {
                if (Dir != manageRoomController.MC.notes[Num + 1].GetComponent<NoteForManageRoom>().Dir)
                {
                    Dir = int.Parse(manageRoomController.MC.dir.text);
                }
            }
            else if(manageRoomController.MC.notes.Count > Num - 1)
            {
                if (Dir != manageRoomController.MC.notes[Num - 1].GetComponent<NoteForManageRoom>().Dir)
                {
                    Dir = int.Parse(manageRoomController.MC.dir.text);
                }
            }
        }
        else
        {
            if(manageRoomController.MC.notes.Count != Num)
            {
                if (Dir != manageRoomController.MC.notes[Num + 1].GetComponent<NoteForManageRoom>().Dir)
                {
                    Dir = int.Parse(manageRoomController.MC.dir.text);
                }
            }
            else { Dir = int.Parse(manageRoomController.MC.dir.text); }
        }
    }
    public void TimeChanged()
    {
        Time = float.Parse(manageRoomController.MC.tim.text);
        if (manageRoomController.MC.notes.Find(x => x.GetComponent<NoteForManageRoom>().Time >= Time) != null)
        {
            if (!ball_movement.Ball.isMoving)
            {
                ball_movement.Ball.currentIndex = Mathf.Max(manageRoomController.MC.notes.Find(x => x.GetComponent<NoteForManageRoom>().Time >= Time).GetComponent<NoteForManageRoom>().Num, 0);
            }
        }
    }
}
