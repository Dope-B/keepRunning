using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class cameraMove : MonoBehaviour
{
    static public cameraMove cam;
    public GameObject ball;
    public float speed = 3.5f;
    public Vector3 targetPos;
    // Start is called before the first frame update
    private void Awake()
    {
        if (cam == null) { cam = this;DontDestroyOnLoad(this.gameObject); }
        else { Destroy(this.gameObject); }
    }
    void Start()
    {
        if (TurnOnGame.TOG.onGame) { this.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y, this.transform.position.z); }

    }

    // Update is called once per frame
    void Update()
    {
        if (TurnOnGame.TOG.onGame)
        {
            targetPos = new Vector3(ball.transform.position.x, ball.transform.position.y, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, targetPos, speed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
    }
    public IEnumerator camShake(float duration,float mag)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-0.1f, 0.1f) * mag;
            float y = Random.Range(-0.1f, 0.1f) * mag;
            transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
