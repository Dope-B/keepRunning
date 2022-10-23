using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockWay : MonoBehaviour
{
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        erase();
    }
    void erase()
    {
        if ((ball_movement.Ball.currentIndex == this.index+1)||!TurnOnGame.TOG.onGame) { StartCoroutine(disappearBlockWay()); }
    }
    public void blockWayStart()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.localEulerAngles = new Vector3(0, 0, 0);
        this.GetComponent<SpriteRenderer>().color = new Color(this.GetComponent<SpriteRenderer>().color.r,
                                                              this.GetComponent<SpriteRenderer>().color.g,
                                                              this.GetComponent<SpriteRenderer>().color.b, 0);
        StartCoroutine(appearBlockWay());
    }
    IEnumerator disappearBlockWay()
    {
        while (this.GetComponent<SpriteRenderer>().color.a >= 0f)
        {
            this.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.002f);
            yield return new WaitForEndOfFrame();
        }
        block_checker.block_Checker.returnObject(this.gameObject, 1);
    }
    public IEnumerator appearBlockWay()
    {
        while (this.GetComponent<SpriteRenderer>().color.a <= 0.2f)
        {
            this.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.002f);
            yield return new WaitForEndOfFrame();
        }
    }
}
