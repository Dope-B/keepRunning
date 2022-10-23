using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class block_checker : MonoBehaviour
{
    static public block_checker block_Checker; 
    public GameObject blockWayPrefab;
    public GameObject blockPrefab;
    Queue<GameObject> blockWay = new Queue<GameObject>();
    Queue<GameObject> blocka = new Queue<GameObject>();
    const int blockCount = 6;
    int blockWayCount = 10;
    public int currentBlockIndex = 0;
    public int lastBlockPosX=0;
    int lastBlockWayPosX = 0;
    public int lastBlockPosY=0;
    int lastBlockWayPosY = 0;
    int[] direction;
    float bs;// block speed
    float[] timeGap;
    public float timeForCheck = 0;
    public float sync = 0;
    public int blockSummonMode = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        if (block_Checker == null) { block_Checker = this;}
        else { }
    }
    private void OnEnable()
    {
        rePos();
        setSong();
        if (!TurnOnGame.TOG.manageMode){ AudioManager.audioManager.audioSourceBGM.time = 0f; startChecker();}
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnOnGame.TOG.manageMode) {
            if (manageRoomController.MC.isPlaying) { manageModePlay(); }
            else { fillBlockForEditmode(); }
        }
    }
    private void blockInit()
    {
        for(int i = 0; i < blockWayCount; i++) { blockWay.Enqueue(creatObject(blockWayPrefab));}
        for(int i = 0; i < blockCount; i++) { blocka.Enqueue(creatObject(blockPrefab)); }
    }
    private GameObject creatObject(GameObject prefab)
    {
        GameObject Object = Instantiate(prefab);
        Object.SetActive(false);
        Object.transform.SetParent(this.gameObject.transform);
        return Object;
    }
    public GameObject getObject(int sort_number)// 1-> blockWayPrefab 2-> blockPrefab 
    {
        GameObject Object=null;
        switch (sort_number)
        {
            case 1:
                if (blockWay.Count > 0) { Object = this.blockWay.Dequeue(); }
                else { Object = creatObject(blockWayPrefab); }
                break;
            case 2:
                if (blocka.Count > 0) { Object = this.blocka.Dequeue(); }
                else { Object = creatObject(blockPrefab); }
                break;
        }
        Object.SetActive(true);
        Object.transform.SetParent(null);
        return Object;
    }
    public void returnObject(GameObject prefab,int sort_number)// 1-> blockWayPrefab 2-> blockPrefab 
    {
        prefab.gameObject.SetActive(false);
        prefab.transform.SetParent(this.transform);
        switch (sort_number)
        {
            case 1: this.blockWay.Enqueue(prefab); break;
            case 2: this.blocka.Enqueue(prefab); break;
        }
    }
    public void setSong()
    {
        direction = new int[AudioManager.audioManager.currentBGM.direction.Length];
        timeGap = new float[AudioManager.audioManager.currentBGM.timeGap.Length];
        for (int i = 0; i < AudioManager.audioManager.currentBGM.direction.Length; i++)
        {
            direction[i] = AudioManager.audioManager.currentBGM.direction[i];
        }
        for (int i = 0; i < AudioManager.audioManager.currentBGM.timeGap.Length; i++)
        {
            timeGap[i] = AudioManager.audioManager.currentBGM.timeGap[i];
        }
    }
    IEnumerator summonBlock()
    {
        GameObject block = getObject(2);
        int randomPos;
        float reachTime=0;
        block.GetComponent<block>().timeForCheck = AudioManager.audioManager.audioSourceBGM.time + sync;
        reachTime = timeGap[currentBlockIndex] - (AudioManager.audioManager.audioSourceBGM.time + sync);
        if (ball_movement.Ball.currentIndex == 0) { reachTime += 3f; }
        //Debug.Log("timeGap==>   " + timeGap[currentBlockIndex] + "    audioTime==>   " + AudioManager.audioManager.audioSourceBGM.time);
        //Debug.Log("reach==>   "+reachTime);
        //Debug.Log(ball_movement.Ball.currentIndex+"  "+currentBlockIndex + "  " + reachTime);
        switch (direction[currentBlockIndex])// set block 
        {
            case 1:
                randomPos = Random.Range(-1, -5);
                bs = (Mathf.Abs(randomPos) / reachTime); //Debug.Log("bs:  "+bs);
                lastBlockPosY += 1;
                block.transform.position = new Vector2(lastBlockPosX + randomPos, lastBlockPosY);
                block.GetComponent<block>().set_block(2, bs, lastBlockPosX, lastBlockPosY,currentBlockIndex);      // UP
                block.GetComponent<block>().blockStart();
                break;
            case -1:
                randomPos = Random.Range(1, 5);
                bs = (Mathf.Abs(randomPos) / reachTime); //Debug.Log("bs:  " + bs);
                lastBlockPosY -= 1;
                block.transform.position = new Vector2(lastBlockPosX + randomPos, lastBlockPosY);
                block.GetComponent<block>().set_block(-2, bs, lastBlockPosX, lastBlockPosY, currentBlockIndex);
                block.GetComponent<block>().blockStart();
                break;                                                                               // DOWN
            case 2:
                randomPos = Random.Range(1,5);
                bs = (Mathf.Abs(randomPos) / reachTime); //Debug.Log("bs:  " + bs);
                lastBlockPosX -= 1;
                block.transform.position = new Vector2(lastBlockPosX, lastBlockPosY + randomPos);
                block.GetComponent<block>().set_block(1, bs, lastBlockPosX, lastBlockPosY, currentBlockIndex);      //LEFT
                block.GetComponent<block>().blockStart();
                break;
            case -2:
                randomPos = Random.Range(-1, -5);
                bs = (Mathf.Abs(randomPos) / reachTime); //Debug.Log("bs:  "+bs);
                lastBlockPosX += 1;
                block.transform.position = new Vector2(lastBlockPosX, lastBlockPosY + randomPos);
                block.GetComponent<block>().set_block(-1, bs, lastBlockPosX, lastBlockPosY, currentBlockIndex);
                block.GetComponent<block>().blockStart();
                break;                                                                             // RIGHT
        }
        yield return null;
    }
    public void timingCheck()
    {
        switch (blockSummonMode)
        {
            case 0:
                if (ball_movement.Ball.currentIndex == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if (currentBlockIndex < direction.Length)
                {
                    StartCoroutine(summonBlock());
                    currentBlockIndex++;
                }
            }
        }
        else {
            if (currentBlockIndex < direction.Length)
            {
                StartCoroutine(summonBlock());
                currentBlockIndex++;
            }
        }
                break;
            case 1:
                if (currentBlockIndex-ball_movement.Ball.currentIndex<=2)
                {
                    for (int i = 0; i < Random.Range(1,3); i++)
                    {
                        if (currentBlockIndex < direction.Length)
                        {
                            StartCoroutine(summonBlock());
                            currentBlockIndex++;
                        }
                    }
                }
                break;
            case 2:
                if (ball_movement.Ball.currentIndex == 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (currentBlockIndex < direction.Length)
                        {
                            StartCoroutine(summonBlock());
                            currentBlockIndex++;
                        }
                    }
                }
                else
                {
                    if (currentBlockIndex < direction.Length)
                    {
                        StartCoroutine(summonBlock());
                        currentBlockIndex++;
                    }
                }
                break;
            default:
                break;
        }
  
    }
    void drawMapWay()
    {
        GameObject blockWay = getObject(1);
        blockWay.GetComponent<blockWay>().blockWayStart();
        switch (direction[ball_movement.Ball.currentIndex + 4])
            {
                case 1:
                    lastBlockWayPosY += 1;
                    break;
                case 2:
                    lastBlockWayPosX -= 1;
                    break;
                case -1:
                    lastBlockWayPosY -= 1;
                    break;
                case -2:
                    lastBlockWayPosX += 1;
                    break;
            }
        blockWay.transform.position = new Vector2(lastBlockWayPosX, lastBlockWayPosY);
        blockWay.transform.localEulerAngles = new Vector3(0, 0, 0);
        blockWay.GetComponent<blockWay>().index = ball_movement.Ball.currentIndex + 4;
    }

    public void updateMapWay()
    {
        if (ball_movement.Ball.currentIndex != 0)
        {
            if(ball_movement.Ball.currentIndex + 4< direction.Length) { drawMapWay(); }
            
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject blockWay = getObject(1);
                blockWay.GetComponent<blockWay>().blockWayStart();
                switch (direction[i])
                {
                    case 1:
                        lastBlockWayPosY += 1;
                        break;
                    case 2:
                        lastBlockWayPosX -= 1;
                        break;
                    case -1:
                        lastBlockWayPosY -= 1;
                        break;
                    case -2:
                        lastBlockWayPosX += 1;
                        break;
                }
                blockWay.transform.position = new Vector2(lastBlockWayPosX, lastBlockWayPosY);
                blockWay.GetComponent<blockWay>().index = i;
            }
        }
    }
    public void startChecker()
    {
        blockInit();
        updateMapWay();
        timingCheck();
    }
    public void summonBlockForEditMode(int dir)
    {
        GameObject block = getObject(2);
        currentBlockIndex++;
        switch (dir)
        {
            case 1:
                lastBlockPosY += 1;
                block.GetComponent<block>().set_block(1, 0, lastBlockPosX, lastBlockPosY, currentBlockIndex);
                break;
            case -1:
                lastBlockPosY -= 1;
                block.GetComponent<block>().set_block(-1, 0, lastBlockPosX, lastBlockPosY, currentBlockIndex);
                break;
            case 2:
                lastBlockPosX -= 1;
                block.GetComponent<block>().set_block(2, 0, lastBlockPosX, lastBlockPosY, currentBlockIndex);
                break;
            case -2:
                lastBlockPosX += 1;
                block.GetComponent<block>().set_block(-2, 0, lastBlockPosX, lastBlockPosY, currentBlockIndex);
                break;
        }
        block.transform.position = new Vector2(lastBlockPosX, lastBlockPosY);
        block.GetComponent<block>().circle_on = true;
        block.GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 0.9f);
    }
    public void setBlockForEditMode(int index,int posX,int posY)
    {
        if (index == 5) { TurnOnGame.TOG.Fblock.GetComponent<block>().incAlpha(); return; }
        GameObject block = getObject(2);
        int posx = posX;
        int posy = posY;
        //Debug.Log("Findex:  " + (index) + "     Fdir:   " + manageRoomController.MC.notes[index-1].GetComponent<NoteForManageRoom>().Dir);
        //Debug.Log("before    Fx:  " + posx + "     Fy:   " + posy);
            for (int i = 1; i < 6; i++)
            {
                    switch (manageRoomController.MC.notes[index - i].GetComponent<NoteForManageRoom>().Dir)
                    {
                        case 1:
                            posy -= 1;
                        break;
                        case -1:
                            posy += 1;
                            break;
                        case 2:
                            posx += 1;
                            break;
                        case -2:
                            posx -= 1;
                            break;
                    }
                //Debug.Log("index:  " + (index - i) + "     dir:   " + manageRoomController.MC.notes[index-1 - i].GetComponent<NoteForManageRoom>().Dir);
                //Debug.Log("after    x:  " + posx + "     y:   " + posy);
            }
            block.GetComponent<block>().set_block(manageRoomController.MC.notes[index-6].GetComponent<NoteForManageRoom>().Dir, 0, posx, posy, index-5);
            //Debug.Log("Lx:  " + posx + "     Ly:   " + posy);
        block.transform.position = new Vector2(posx, posy);
        block.GetComponent<block>().circle_on = true;
        block.GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 0.9f);
    }
    void fillBlockForEditmode()
    {
        while (currentBlockIndex < ball_movement.Ball.currentIndex)
        {
            currentBlockIndex++;
            GameObject block = getObject(2);
            block.transform.position = new Vector2(lastBlockPosX, lastBlockPosY);
            switch (manageRoomController.MC.notes[currentBlockIndex-1].GetComponent<NoteForManageRoom>().Dir)
            {
                case 1:
                    lastBlockPosY += 1;
                    ball_movement.Ball.transform.position += new Vector3(0, 1);
                    break;
                case -1:
                    lastBlockPosY -= 1;
                    ball_movement.Ball.transform.position += new Vector3(0, -1);
                    break;
                case 2:
                    lastBlockPosX -= 1;
                    ball_movement.Ball.transform.position += new Vector3(-1, 0);
                    break;
                case -2:
                    lastBlockPosX += 1;
                    ball_movement.Ball.transform.position += new Vector3(1, 0);
                    break;
            }
            block.GetComponent<block>().set_block(manageRoomController.MC.notes[currentBlockIndex-1].GetComponent<NoteForManageRoom>().Dir, 0, lastBlockPosX, lastBlockPosY, currentBlockIndex);
            block.transform.position = new Vector2(lastBlockPosX, lastBlockPosY);
            block.GetComponent<block>().circle_on = true;
            block.GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 0.9f);
        }
    }
    void manageModePlay()
    {
        if (currentBlockIndex < manageRoomController.MC.notes.Count)
        {
            if (AudioManager.audioManager.audioSourceBGM.time >= manageRoomController.MC.notes[currentBlockIndex].GetComponent<NoteForManageRoom>().Time+sync)
            {
                //Debug.Log("AUD Time:  " + AudioManager.audioManager.audioSourceBGM.time + "   noteTime:   " + manageRoomController.MC.notes[currentBlockIndex].GetComponent<NoteForManageRoom>().Time+"    gap:   "+ (AudioManager.audioManager.audioSourceBGM.time - manageRoomController.MC.notes[currentBlockIndex].GetComponent<NoteForManageRoom>().Time));
                summonBlockForEditMode(manageRoomController.MC.notes[currentBlockIndex].GetComponent<NoteForManageRoom>().Dir);
                StartCoroutine(ball_movement.Ball.ball_move(manageRoomController.MC.notes[ball_movement.Ball.currentIndex].GetComponent<NoteForManageRoom>().Dir));
                ball_movement.Ball.currentIndex++;
            }
        }
        
    }
    void rePos()
    {
        lastBlockPosX = 0;
        lastBlockPosY = 0;
        lastBlockWayPosX = 0;
        lastBlockWayPosY = 0;
        currentBlockIndex = 0;
    }
    void refreshMapForEditmode()
    {
        lastBlockPosX = 0;
        lastBlockPosY = 0;
        for (int i = 0; i <currentBlockIndex ; i++)
        {
            summonBlockForEditMode(manageRoomController.MC.notes[i].GetComponent<NoteForManageRoom>().Dir);
        }
    }
}
