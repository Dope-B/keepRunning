using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ball_movement : MonoBehaviour
{
    public static ball_movement Ball;
    public GameObject comboText;
    public TextMeshProUGUI scoreText;
    Vector3 dest;
    Vector3 depart;
    RaycastHit2D ray;
    BoxCollider2D box;
    IEnumerator comboCourtine;
    int combo = 0;
    public int score = 0;
    public bool isMoving=false;
    public int currentIndex = 0;
    private void Awake()
    {
        if (Ball == null) { Ball = this;}
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnDisable()
    {
        comboText.GetComponent<TextMeshProUGUI>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0);
    }

    public void rePos()
    {
        this.gameObject.transform.SetParent(null);
        this.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.localEulerAngles = new Vector3(0, 0, 0);
        comboCourtine = printComboText();
        
        currentIndex = 0;
        combo = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isMoving&&moveable())
        {
            if (!TurnOnGame.TOG.manageMode)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow)) { if (onBoxCheck(1)) { StartCoroutine(ball_move(1)); } else { StartCoroutine(ball_shake(1)); } }
                else if (Input.GetKeyDown(KeyCode.DownArrow)) { if (onBoxCheck(-1)) { StartCoroutine(ball_move(-1)); } else { StartCoroutine(ball_shake(-1)); } }
                else if (Input.GetKeyDown(KeyCode.LeftArrow)) { if (onBoxCheck(-2)) { StartCoroutine(ball_move(2)); } else { StartCoroutine(ball_shake(2)); } }
                else if (Input.GetKeyDown(KeyCode.RightArrow)) { if (onBoxCheck(2)) { StartCoroutine(ball_move(-2)); } else { StartCoroutine(ball_shake(-2)); } }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow)) { summonBlockForManageMode(1);manageRoomController.MC.addButton(1); StartCoroutine(ball_move(1)); }
                else if (Input.GetKeyDown(KeyCode.DownArrow)) { summonBlockForManageMode(-1); manageRoomController.MC.addButton(-1); StartCoroutine(ball_move(-1));}
                else if (Input.GetKeyDown(KeyCode.LeftArrow)) { summonBlockForManageMode(2); manageRoomController.MC.addButton(2); StartCoroutine(ball_move(2));}
                else if (Input.GetKeyDown(KeyCode.RightArrow)) { summonBlockForManageMode(-2); manageRoomController.MC.addButton(-2); StartCoroutine(ball_move(-2));}
            }
        }
    }
    
    private bool moveable()
    {
        if (TurnOnGame.TOG.manageMode)
        {
            if(sceneManager.SM.switching || musicSelect.MS.switching || TurnOnGame.TOG.switching || !manageRoomController.MC.isPlaying || !manageRoomController.MC.editMode) { return false; }
            else { return true; }
        }
        else
        {
            if (sceneManager.SM.switching || musicSelect.MS.switching || TurnOnGame.TOG.switching) { return false; }
            else { return true; }
        }
    }
    public IEnumerator ball_move(int direction)
    {
        depart = transform.position;
        isMoving = true;
        if (!TurnOnGame.TOG.manageMode)
        {
            block_checker.block_Checker.updateMapWay();
            block_checker.block_Checker.timingCheck();
        }
        switch (direction)
        {
            case 1:   
            case -1:
                dest = new Vector3(transform.position.x, transform.position.y + direction);
                break;
            case 2:
            case -2:
                dest = new Vector3(transform.position.x - (direction * 0.5f), transform.position.y);
                break;
        }
        for(int i=0;i<=10;i++)
        {
            transform.position = Vector3.Lerp(this.transform.position, dest, 0.3f);
            yield return new WaitForEndOfFrame();
        }
        transform.position = dest;
        isMoving = false;
    }
    IEnumerator ball_shake(int direction)
    {
        depart = transform.position;
        isMoving = true;
        switch (direction)
        {
            case 1:
            case -1:
                dest = new Vector3(transform.position.x, transform.position.y + (direction*0.1f));
                break;
            case 2:
            case -2:
                dest = new Vector3(transform.position.x - (direction * 0.05f), transform.position.y);
                break;
        }
        for (int i = 0; i < 6; i++)
        {
            if (i < 3) { transform.position = Vector3.Lerp(this.transform.position, dest, 0.4f); }
            else { transform.position = Vector3.Lerp(this.transform.position, depart, 0.4f); }
            yield return new WaitForEndOfFrame();
        }
        this.transform.position = depart;
        isMoving = false;
    }
    IEnumerator printComboText()
    {
        combo++;
        if (combo > 1)
        {
            comboText.GetComponent<TextMeshProUGUI>().text = "combo " + combo;
            comboText.GetComponent<TextMeshProUGUI>().fontSize = 18;
            comboText.GetComponent<TextMeshProUGUI>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 1f);
            for (int i = 0; i < 5; i++)
            {
                comboText.GetComponent<TextMeshProUGUI>().fontSize += 0.2f;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 10; i++)
            {
                comboText.GetComponent<TextMeshProUGUI>().color -= new Color(0, 0, 0, 0.1f);
                yield return new WaitForEndOfFrame();
            }
            comboText.GetComponent<TextMeshProUGUI>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0f);
            comboText.GetComponent<TextMeshProUGUI>().fontSize -= 1f;
        }
    }
    public void incAlpha()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
    }
    void printCombo()
    {
        StopCoroutine(comboCourtine);
        comboCourtine = printComboText();
        StartCoroutine(comboCourtine);
    }
    public bool onBoxCheck(int direction)
    {
        switch (direction)
        {
            case 1:
                ray = Physics2D.Raycast(transform.position, Vector2.up, 0.9f, 1 << 8);
                //Debug.DrawRay(transform.position,Vector2.up*0.9f,Color.red,0.05f);
                if (ray&&currentIndex==ray.collider.gameObject.GetComponent<block>().index)
                {
                    box = ray.collider.gameObject.GetComponent<BoxCollider2D>();
                    if (Mathf.Abs(ray.point.x - (ray.collider.gameObject.transform.position.x + 0.5f)) <= 0.05f) { 
                        //Debug.Log("cooool"); 
                        ray.collider.gameObject.GetComponent<block>().effect_on(1);
                        AudioManager.audioManager.playEffect("cool");
                        printCombo();
                        printScoreText(500*combo);
                        StartCoroutine(cameraMove.cam.camShake(0.2f, 0.3f));
                    }
                    else if(Mathf.Abs(ray.point.x - (ray.collider.gameObject.transform.position.x + 0.5f)) > 0.05f &&
                            Mathf.Abs(ray.point.x - (ray.collider.gameObject.transform.position.x + 0.5f)) <= 0.1f) {
                        //Debug.Log("goooood");
                        printCombo();
                        printScoreText(200*combo);
                        AudioManager.audioManager.playEffect("good");
                        ray.collider.gameObject.GetComponent<block>().effect_on(3);
                    }
                    else { 
                        //Debug.Log("ummmmmm");
                        combo = 0;
                        printScoreText(100);
                        AudioManager.audioManager.playEffect("ummm");
                        ray.collider.gameObject.GetComponent<block>().effect_on(5);
                    }
                    ray.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    ray.collider.gameObject.GetComponent<block>().circle_on = true;
                    //Debug.Log("pointX:  "+ray.point.x+"    "+"posX:   "+(ray.collider.gameObject.transform.position.x + 0.5f)+"    "+"result:  "+Mathf.Abs(ray.point.x - (ray.collider.gameObject.transform.position.x + 0.5f)));
                    currentIndex++;
                    return true;
                }
                else { /*Debug.Log("miss");*/ return false; }
            case -1:
                ray = Physics2D.Raycast(transform.position, Vector2.down, 0.9f, 1 << 8);
                //Debug.DrawRay(transform.position, Vector2.down * 0.9f, Color.red, 0.05f);
                if (ray && currentIndex == ray.collider.gameObject.GetComponent<block>().index)
                {
                    box = ray.collider.gameObject.GetComponent<BoxCollider2D>();
                    if (Mathf.Abs(ray.point.x - (ray.collider.gameObject.transform.position.x + 0.5f)) <= 0.05f) { 
                        //Debug.Log("cooool"); 
                        ray.collider.gameObject.GetComponent<block>().effect_on(1);
                        AudioManager.audioManager.playEffect("cool");
                        printCombo();
                        printScoreText(500 * combo);
                        StartCoroutine(cameraMove.cam.camShake(0.2f, 0.3f));
                    }
                    else if (Mathf.Abs(ray.point.x - (ray.collider.gameObject.transform.position.x + 0.5f)) > 0.05f &&
                            Mathf.Abs(ray.point.x - (ray.collider.gameObject.transform.position.x + 0.5f)) <= 0.1f) {
                        //Debug.Log("goooood");
                        AudioManager.audioManager.playEffect("good");
                        printCombo();
                        printScoreText(200 * combo);
                        ray.collider.gameObject.GetComponent<block>().effect_on(3); 
                    }
                    else {
                        //Debug.Log("ummmmmm"); 
                        ray.collider.gameObject.GetComponent<block>().effect_on(5);
                        combo = 0;
                        printScoreText(100);
                        AudioManager.audioManager.playEffect("ummm");
                    }
                    ray.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    ray.collider.gameObject.GetComponent<block>().circle_on = true;
                    //Debug.Log("pointX:  " + ray.point.x + "    " + "posX:   " + (ray.collider.gameObject.transform.position.x + 0.5f) + "    " + "result:  " + Mathf.Abs(ray.point.x - (ray.collider.gameObject.transform.position.x + 0.5f)));
                    currentIndex++;
                    return true;
                }
                else { /*Debug.Log("miss");*/ return false; }
            case 2:
                ray = Physics2D.Raycast(transform.position, Vector2.right, 0.9f, 1 << 8);
                //Debug.DrawRay(transform.position, Vector2.right * 0.9f, Color.red, 0.05f);
                if (ray && currentIndex == ray.collider.gameObject.GetComponent<block>().index)
                {
                    box = ray.collider.gameObject.GetComponent<BoxCollider2D>();
                    if (Mathf.Abs(ray.point.y - (ray.collider.gameObject.transform.position.y + 0.5f)) <= 0.05f)
                    { 
                        //Debug.Log("cooool");
                        ray.collider.gameObject.GetComponent<block>().effect_on(1);
                        AudioManager.audioManager.playEffect("cool");
                        printCombo();
                        printScoreText(500 * combo);
                        StartCoroutine(cameraMove.cam.camShake(0.2f, 0.3f));
                    }
                    else if (Mathf.Abs(ray.point.y - (ray.collider.gameObject.transform.position.y + 0.5f)) > 0.05f &&
                            Mathf.Abs(ray.point.y - (ray.collider.gameObject.transform.position.y + 0.5f)) <= 0.1f)
                    { 
                        //Debug.Log("goooood");
                        AudioManager.audioManager.playEffect("good");
                        printCombo();
                        printScoreText(200 * combo);
                        ray.collider.gameObject.GetComponent<block>().effect_on(3);
                    }
                    else
                    {
                        //Debug.Log("ummmmmm");
                        AudioManager.audioManager.playEffect("ummm");
                        combo = 0;
                        printScoreText(100);
                        ray.collider.gameObject.GetComponent<block>().effect_on(5);
                    }
                    ray.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    ray.collider.gameObject.GetComponent<block>().circle_on = true;
                    //Debug.Log("pointY:  " + ray.point.y + "    " + "posY:   " + (ray.collider.gameObject.transform.position.y + 0.5f) + "    " + "result:  " + Mathf.Abs(ray.point.y - (ray.collider.gameObject.transform.position.y + 0.5f)));
                    currentIndex++;
                    return true;
                }
                else { /*Debug.Log("miss");*/ return false; }
            case -2:
                ray = Physics2D.Raycast(transform.position, Vector2.left, 0.9f, 1 << 8);
                //Debug.DrawRay(transform.position, Vector2.left * 0.9f, Color.red, 0.1f);
                if (ray && currentIndex == ray.collider.gameObject.GetComponent<block>().index)
                {
                    box = ray.collider.gameObject.GetComponent<BoxCollider2D>();
                    if (Mathf.Abs(ray.point.y - (ray.collider.gameObject.transform.position.y + 0.5f)) <= 0.05f) { 
                        //Debug.Log("cooool"); 
                        ray.collider.gameObject.GetComponent<block>().effect_on(1);
                        AudioManager.audioManager.playEffect("cool");
                        printCombo();
                        printScoreText(500 * combo);
                        StartCoroutine(cameraMove.cam.camShake(0.2f, 0.3f));
                    }
                    else if (Mathf.Abs(ray.point.y - (ray.collider.gameObject.transform.position.y + 0.5f)) > 0.05f &&
                            Mathf.Abs(ray.point.y - (ray.collider.gameObject.transform.position.y + 0.5f)) <= 0.1f) {
                        //Debug.Log("goooood");
                        printCombo();
                        printScoreText(200 * combo);
                        ray.collider.gameObject.GetComponent<block>().effect_on(3);
                        AudioManager.audioManager.playEffect("good");
                    }
                    else { 
                        //Debug.Log("ummmmmm");
                        AudioManager.audioManager.playEffect("ummm");
                        combo = 0;
                        printScoreText(100);
                        ray.collider.gameObject.GetComponent<block>().effect_on(5); 
                    }
                    ray.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    ray.collider.gameObject.GetComponent<block>().circle_on = true;
                    //Debug.Log("pointY:  " + ray.point.y + "    " + "posY:   " + (ray.collider.gameObject.transform.position.y + 0.5f) + "    " + "result:  " + Mathf.Abs(ray.point.y - (ray.collider.gameObject.transform.position.y + 0.5f)));
                    currentIndex++;
                    return true;
                }
                else { /*Debug.Log("miss");*/ return false; }
        }
        return false;
    }
    void summonBlockForManageMode(int dir) 
    {
        currentIndex++;
        block_checker.block_Checker.summonBlockForEditMode(dir);
        
    }
    void printScoreText(int num)
    {
        score += num;
        scoreText.text = "Score: " + score.ToString();
    }
}
