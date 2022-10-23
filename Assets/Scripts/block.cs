using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour
{
    // Start is called before the first frame update
    public int index;
    private int pos_x;
    private int pos_y;
    int direction;
    float speed;
    public bool circle_on=false;
    public float timeForCheck;
    public bool remove_on=false;
    bool block_on = false;
    public GameObject effect1;
    public GameObject effect2;
    public GameObject effect3;
    public GameObject effect4;
    public GameObject effect5;
    void Start()
    {

    }
    private void OnEnable()
    {
        this.gameObject.transform.SetParent(null);
        remove_on = false;
        if (index == -1) { transform.position = new Vector3(1, 1, 0); }
    }
    public void blockStart()
    {
            if (ball_movement.Ball.currentIndex == index) { this.GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 0f); }
            else { this.GetComponent<SpriteRenderer>().color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 0f); }
            StartCoroutine(blockOn());
            StartCoroutine(block_move());
    }
    // Update is called once per frame
    void Update()
    {
        if (!remove_on&&((removeCheck()||circleOnCheck())||!TurnOnGame.TOG.onGame)) 
        { 
            remove_on = true;
            StartCoroutine(removeBlock()); 
        }
        if (circleOnCheck()) {
            if (TurnOnGame.TOG.onGame) { TurnOnGame.TOG.StartCoroutine(TurnOnGame.TOG.TurnState()); }
        }
        indexCheck();
        if (TurnOnGame.TOG.manageMode) { if (manageRoomController.MC.editMode) { removeCheckFormanageRoom(); } }

    }
    IEnumerator block_move()// 1-> UP -1-> DOWN 2-> LEFT -2-> RIGHT
    {
        switch (direction)
        {
            case 1:
                transform.position -= Vector3.up * direction * speed * Time.deltaTime;
                //transform.position = new Vector2(this.transform.position.x, Mathf.Max(pos_y, transform.position.y));
                break;
            case -1:
                transform.position -= Vector3.up * direction * speed * Time.deltaTime;
                //transform.position = new Vector2(this.transform.position.x, Mathf.Min(pos_y, transform.position.y));
                break;
            case 2:
                transform.position -= Vector3.left * direction * (speed * 0.5f) * Time.deltaTime;
                //transform.position = new Vector2(Mathf.Min(pos_x, transform.position.x), this.transform.position.y);
                break;
            case -2:
                transform.position -= Vector3.left * direction * (speed * 0.5f) * Time.deltaTime;
                //transform.position = new Vector2(Mathf.Max(pos_x, transform.position.x), this.transform.position.y);
                break;
            default:
                break;
        }
        yield return new WaitForEndOfFrame();
        if (!circle_on) { StartCoroutine(block_move()); }
        else { StartCoroutine(fixBlockPos());}
    }
    IEnumerator fixBlockPos()
    {
        for(int i = 0; i < 10; i++)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector2(pos_x, pos_y), 0.2f);
            yield return new WaitForEndOfFrame();
        }
        this.transform.position = new Vector2(pos_x, pos_y);
    }
    public void set_block(int direction,float speed,int pos_x,int pos_y, int index)
    {
        this.direction = direction;
        this.speed = speed;
        this.pos_x = pos_x;
        this.pos_y = pos_y;
        this.index = index;
        this.circle_on = false;
        this.remove_on = false;
        this.block_on = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.localEulerAngles = new Vector3(0, 0, 0);
    }
    bool circleOnCheck()
    {
        switch (direction)
        {
            case 1:
                if (pos_x == transform.position.x && pos_y > transform.position.y+1.5f) {return true; }
                else { return false; }
            case -1:
                if (pos_x == transform.position.x && pos_y < transform.position.y-1.5f) {return true; }
                else {return false; }
            case 2:
                if (pos_x < transform.position.x-1.5f && pos_y == transform.position.y) {return true; }
                else {return false; }
            case -2:
                if (pos_x > transform.position.x+1.5f && pos_y == transform.position.y) {return true; }
                else { return false; }
        }
        return false;
    }
    bool removeCheck()
    {
        if (ball_movement.Ball.currentIndex - index > 4&& index != -1){ return true; }
        else if (index == -1 && ball_movement.Ball.currentIndex >= 5) { return true; }
        return false;
    }
    void removeCheckFormanageRoom()
    {
        if(!manageRoomController.MC.isPlaying && ball_movement.Ball.currentIndex <= index-1&&!remove_on)
        {
            //Debug.Log("Fball pos:  x:" + ball_movement.Ball.transform.position.x + "     y:    " + ball_movement.Ball.transform.position.y);
                switch (direction)
                {
                    case 1:
                    block_checker.block_Checker.lastBlockPosY += -1;
                    ball_movement.Ball.transform.position += new Vector3(0, -1);
                    break;
                    case -1:
                    block_checker.block_Checker.lastBlockPosY += 1;
                    ball_movement.Ball.transform.position += new Vector3(0, 1);
                    break;
                    case 2:
                    block_checker.block_Checker.lastBlockPosX += 1;
                    ball_movement.Ball.transform.position += new Vector3(1, 0);
                    break;
                    case -2:
                    block_checker.block_Checker.lastBlockPosX += -1;
                    ball_movement.Ball.transform.position += new Vector3(-1, 0);
                    break;
                }
            //Debug.Log("dir:   " + direction + "     index:     " + index);
            //Debug.Log("Lball pos:  x:" + ball_movement.Ball.transform.position.x + "     y:    " + ball_movement.Ball.transform.position.y);
            block_checker.block_Checker.currentBlockIndex--;
            remove_on = true;
            if (index >= 5) { block_checker.block_Checker.setBlockForEditMode(index, pos_x, pos_y); }
            StartCoroutine(removeBlock());
        }
    }
    IEnumerator removeBlock()
    {
        while (this.GetComponent<SpriteRenderer>().color.a >= 0f)
        {
            this.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.08f);
            yield return new WaitForEndOfFrame();
        }
        if (index != -1) { block_checker.block_Checker.returnObject(this.gameObject, 2); }
        else { remove_on = false; }
    }
    IEnumerator blockOn()
    {
        if (ball_movement.Ball.currentIndex == index)
        {
            while (this.GetComponent<SpriteRenderer>().color.a <= 0.9f)
            {
                this.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.09f);
                yield return new WaitForEndOfFrame();
            }
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            
        }
        else if (!circle_on)
        {
            while (this.GetComponent<SpriteRenderer>().color.a <= 0.5f)
            {
                this.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.05f);
                yield return new WaitForEndOfFrame();
            }
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
        block_on = true;
    }
    public void effect_on(int num)
    {
        Vector2 pos = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y + 0.5f);
        switch (num)
        {
            case 1:
                Instantiate(effect1, pos, transform.rotation);
                break;
            case 2:
                Instantiate(effect2, pos, transform.rotation);
                break;
            case 3:
                Instantiate(effect3, pos, transform.rotation);
                break;
            case 4:
                Instantiate(effect4, pos, transform.rotation);
                break;
            default:
                Instantiate(effect5, pos, transform.rotation);
                break;
        }
    }
    void indexCheck()
    {
        if (block_on&&TurnOnGame.TOG.onGame){
            if (ball_movement.Ball.currentIndex == index){
                this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                this.GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 0.9f);
                this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
        }  
    }
    public void incAlpha()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
    }
    public void simpleRemove()
    {
        if (index != -1) { block_checker.block_Checker.returnObject(this.gameObject, 2); }
        else { remove_on = false; }
    }
}
