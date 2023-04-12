# 리듬게임 keepRunning

#### 사용 기술: Unity(2020.2.2f1)

#### 제작 기간: 2020.07.10~2020.09.20

#### 예시영상
[![Video Label](http://img.youtube.com/vi/CN8t0P0eB0k/0.jpg)](https://youtu.be/CN8t0P0eB0k)

#### 설명
- 플레이 가능한 음악에 대한 정보는 BSound 클래스에 저장된다.
  - 제목
  - 가수
  - 오디오 클립
  - 앨범 커버
  - 배경색
  - 노트에 대한 정보
<p>
<img width="50%" src="https://user-images.githubusercontent.com/33209821/230932201-2a52ad6f-8746-4b62-90bd-5e59f6ad5298.png"/>
<img width="50%" src="https://user-images.githubusercontent.com/33209821/230932205-f2a15f9b-e7e0-4ce0-b04b-d7c12143eebe.png"/>
</p>

- 게임 시작 시 AudioManager클래스에서 플레이 가능 노래들을 배열로 저장한다.

```C#
for (int i = 0; i < BGMSound.Length; i++) { if (BGMSound[i].note != null) BGMSound[i].getNote(); }
```
- 노트 정보는 텍스트 파일에 저장되며 (노트방향1,노트간격1/노트방향2,노트간격2...) 처럼 저장된다.

```C#
    public void getNote()
    {
        SR = new StringReader(note.text);// 텍스트 파일 읽음
        source = SR.ReadLine();
        note_direction = new string[source.Split('/').Length];// /단위로 나눈 후 그 갯수 만큼 동적 할당
        note_timegap = new string[source.Split('/').Length];
        for (int i = 0; i < source.Split('/').Length; i++)
        {
            note_direction[i] = source.Split('/')[i].Split(',')[0];// ,를 기준으로 나눈 문자열의 첫번째 요소를 노트 방향에 저장
            note_timegap[i] = source.Split('/')[i].Split(',')[1];// 마지막 요소를 노트 간격에 저장
        }
        direction = new int[note_direction.Length];
        timeGap = new float[note_timegap.Length];
        for (int i = 0; i < note_direction.Length; i++)
        {
            direction[i] = int.Parse(note_direction[i]);// int로 형변환 후 저장(위 반복문이랑 합치기 가능)
            timeGap[i] = float.Parse(note_timegap[i]);// float로 형변환 후 저장
        }
    }
```

- 게임의 화면구성은 총 3개로 씬전환 없는 연출을 위해 각 화면은 120도 간격으로 원 형태로 배치돼있다.(타이틀 - 플레이 - 옵션)

<img width="50%" src="https://user-images.githubusercontent.com/33209821/230932194-7094192b-2aaf-4c40-ab25-015fe193a3e7.png"/>

- 비주얼라이저는 기본적으로 일자형으로 백그라운드에 배치되면 플레이 시 원형 비주얼라이저로 바뀐다.
```C#
      if (sortNum == 0)// 원형
            {
                float angle = (-5.625f * i);// 360/64=5.625
                this.transform.eulerAngles = new Vector3(0, 0, angle);
                samplePrefab.transform.position = transform.position + (Vector3.down * halfRad);
            }
            else if(sortNum==1)// 막대형
            {
                samplePrefab.transform.eulerAngles = new Vector3(0, 0, 180);
                samplePrefab.transform.position = transform.position + new Vector3(i - 32, 0, 0) * 0.3f - new Vector3(0, 5, 0);
            }

```

- 비주얼라이저는 퓨리에 변환을 사용하여 구현하였다.
```C#
public static visualizer[] vis=new visualizer[2];// 원형, 막대형
public static float[] samples = new float[512];// 최초로 스팩트럼 저장
public static float[] freBand = new float[64];// 512채널을 64개로 압축 
public static float[] bandBuffer = new float[64];// 부드러운 연출을 위한 버퍼
void getSpectrum()
    {
        audioManager.audioSourceBGM.GetSpectrumData(samples, 0, FFTWindow.Blackman);// 오디오에 대한 스팩트럼을 sample배열에 저장
    }
void setFrequencyBands()// 512개 채널을 64개 채널로 압축
    {
        int count = 0;
        for (int i = 0; i < freBand.Length; i++)
        {
            float average = 0;
            int sampleCount = 6 * i;
            for (int j = 0; j < 6; j++)
            {
                average += samples[sampleCount+j]*(count*((i*0.1f)+0.2f));// 한 채널당 6개의 오리지널 채널의 평균값을 가짐
                count++;
            }
            average /= count;
            freBand[i] = average * 10;
        }
    }
    
    void setBandBuffer()// 부드러운 비주얼라이저 연출을 위한 버퍼(감소 시 천천히 감소)
    {
        for (int i = 0; i < freBand.Length; ++i)
        {
            if (freBand[i] > bandBuffer[i])// 버퍼에 값이 실제 값보다 작을 경우
            {
                bandBuffer[i] = freBand[i];// 바로 상승
                bufferDecrease[i] = 0.01f;// 한 프레임마다 각 진폭에 해당하는 막대들의 최초 감소량
            }
            if(freBand[i] < bandBuffer[i])// 버퍼에 값이 실제 값보다 클 경우
            {
                bandBuffer[i] -= bufferDecrease[i];// 천천히 감소
                bufferDecrease[i] *= 1.1f;// 매 프레임마다 감소량이 1.1배로 커짐
            }
        }
    }
```
- 비주얼라이저 막대의 색은 옵션에서 선택할 수 있다.
  - 흰색
  - 흰색을 제외한 랜덤
  - 흰색을 포함한 랜덤
  
- 

- 특정 주파수의 진폭에 따라 타이틀 글자의 크기가 바뀐다.
```C#
 IEnumerator highlight()
    {
        gethighlightBuffer();// 특정 주파수를 저장
        this.GetComponent<TextMeshProUGUI>().fontSize = Mathf.Min((highlightBuffer) + 20f, 22);// 최대 크기가 22이고 크기가 가변적
        yield return new WaitForEndOfFrame();
        StartCoroutine(highlight());// 반복
    }

```
- 최초 게임 시작 시 전용 BGM이 나오고 플레이 화면으로 넘어갈 시 해당 음악이 BGM으로 바뀐다. 옵션 화면으로 넘어갈 시 다시 전용 BGM이 나온다.
- 화면 이동은 위아래 방향키로 가능하고 부드러운 연출을 위해 Mathf.LerpAngle()함수를 사용했다.
- 화면 이동은 턴테이블 같은 연출을 위해 다음과 같은 순서로 이루어진다.
  - 화면 확대(Vector3.Lerp() 함수를 사용하여 localScale 변화)
  - 화면 회전(Mathf.LerpAngle()함수하여 eulerAngles 변화)
  - 화면 축소(Vector3.Lerp() 함수를 사용하여 localScale 변화)
- 화면 전환 중에는 키 입력을 안 받는다.
- 플레이 화면으로 전환 시 플레이리스트가 원형으로 배치되고 키보드 좌우입력으로 리스트 바꾸기가 가능하다.
- 리스트 전환 시 BGM이 해당 리스트 노래로 바뀐다.
- 리스트 전환 연출은 화면전환 연출과 동일하다.
- 플레이 화면에서 스페이스바를 누른다면 게임이 시작된다.
- 가운데 점은 키보드 상하좌우 입력이 가능하며 생성되는 블럭에 맞게 타이밍을 맞춰서 플레이한다.
- 키보드 입력 시 레이캐스트를 사용해 가려는 방향에 블럭의 판정 콜라이더가 없다면 이동하지 않고 흔들린다.(ball_movement-> ball_shake(int direction))
- 블럭는 판정 콜라이더가 있고 더 정확하게 맞추면 더 높은 점수를 받는다.
```C#
ray = Physics2D.Raycast(transform.position, Vector2.up, 0.9f, 1 << 8);
//Debug.DrawRay(transform.position,Vector2.up*0.9f,Color.red,0.05f);
if (ray&&currentIndex==ray.collider.gameObject.GetComponent<block>().index)
{
    box = ray.collider.gameObject.GetComponent<BoxCollider2D>();
     if (Mathf.Abs(ray.point.x - (ray.collider.gameObject.transform.position.x + 0.5f)) <= 0.05f) { // 정확도 체크, perfect
           ray.collider.gameObject.GetComponent<block>().effect_on(1);// 이펙트 출력
           AudioManager.audioManager.playEffect("cool");// 효과음 출력
           printCombo();// 콤보 텍스트 출력
           printScoreText(500*combo);// 점수 업데이트
           StartCoroutine(cameraMove.cam.camShake(0.2f, 0.3f));// 화면 흔들림 효과
          }
     else if(Mathf.Abs(ray.point.x - (ray.collider.gameObject.transform.position.x + 0.5f)) > 0.05f &&
            Mathf.Abs(ray.point.x - (ray.collider.gameObject.transform.position.x + 0.5f)) <= 0.1f) {
              // good
             }
      else { // bad
            combo = 0;// 콤보 초기화
            printScoreText(100);
            AudioManager.audioManager.playEffect("ummm");
            ray.collider.gameObject.GetComponent<block>().effect_on(5);
            }
       ray.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;// 점이 레이캐스트 중 위치한 점을 인식하면 안되기에 콜라이더 비활성화

```

- 블럭은 오브젝트 풀링을 사용하여 최적화를 했다.
```C#
private void blockInit()// 최초로 큐에 할당
    {
        for(int i = 0; i < blockWayCount; i++) { blockWay.Enqueue(creatObject(blockWayPrefab));}// 특정 개수만큼 큐에 할당
        for(int i = 0; i < blockCount; i++) { blocka.Enqueue(creatObject(blockPrefab)); }
    }

 public GameObject getObject(int sort_number)// 1-> blockWayPrefab(플레이어가 맞추어야 할 블럭의 예상 위치) 2-> blockPrefab(실제 움직이는 블럭)
    {
        GameObject Object=null;
        switch (sort_number)
        {
            case 1:
                if (blockWay.Count > 0) { Object = this.blockWay.Dequeue(); }// 큐가 비어있지 않다면
                else { Object = creatObject(blockWayPrefab); }// 큐가 비어있다면
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
    
public void returnObject(GameObject prefab,int sort_number)// 1-> blockWayPrefab 2-> blockPrefab/ 큐에 반환
    {
        prefab.gameObject.SetActive(false);
        prefab.transform.SetParent(this.transform);
        switch (sort_number)
        {
            case 1: this.blockWay.Enqueue(prefab); break;
            case 2: this.blocka.Enqueue(prefab); break;
        }
    }

```
- 블럭의 방향은 정해져있지만 위치는 랜덤으로 정해진다.
```C#
block.GetComponent<block>().timeForCheck = AudioManager.audioManager.audioSourceBGM.time + sync;// 현재 플레이중인 음악의 경과 시간+ 싱크
reachTime = timeGap[currentBlockIndex] - (AudioManager.audioManager.audioSourceBGM.time + sync);// 블럭이 올바른 타이밍에 도달해야 하는 이동 시간
if (ball_movement.Ball.currentIndex == 0) { reachTime += 3f; }// 첫 블럭은 플레이어에게 3초의 준비시간을 준다.
.
.
.
bs = (Mathf.Abs(randomPos) / reachTime); // 블럭의 속도는 블럭의 생성 위치(거리)/이동 시간(시간)
```
- 블럭은 큐에 있는 갯수만 출력되며 게임 진행 중 기존 블럭은 페이드 아웃 연출로 큐에 반환된다. 
- 블럭 가이드라인은 전방 4개의 블럭의 위치를 알려준다.(block_checker-> updateMapWay())
```C#
GameObject blockWay = getObject(1);// 오브젝트 풀링
blockWay.GetComponent<blockWay>().blockWayStart();
switch (direction[i])// 다음 블럭 방향에 따라 이전 블럭 위치에서 위치 조정
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
blockWay.transform.position = new Vector2(lastBlockWayPosX, lastBlockWayPosY);// 가이드라인 위치 설정
blockWay.GetComponent<blockWay>().index = i;// 인덱스 증가
```

- 플레이 시 백그라운드와 앨범커버의 투명도는 가변적으로 바뀐다.
```C#
float min = Random.Range(0, 0.15f);// 투명도 최소값
float max = Random.Range(0.25f, 0.35f);/ 투명도 최대값
while (this.GetComponent<Image>().color.a<=max)// 투명도 증가
  {
     this.GetComponent<Image>().color += new Color(0, 0, 0, 0.001f);
     yield return new WaitForEndOfFrame();
   }
while (this.GetComponent<Image>().color.a >= min)// 투명도 감소
   {
      this.GetComponent<Image>().color -= new Color(0, 0, 0, 0.001f);
      yield return new WaitForEndOfFrame();
   }
   StartCoroutine(Alphatrans());

```

- 플레이 화면이 아닐 시 플레이 리스트들은 비활성화 된다.
- 옵션 화면에서 선택 할 수 있는 옵션은 다음과 같다.
  - BGM 소리 크기
  - 이펙트 소리 크기(화면 전환 소리, 노트 입력 소리 등등)
  - 싱크
  - 난이도
  - 비주얼라이저 막대 색깔
<img width="50%" src="https://user-images.githubusercontent.com/33209821/230932203-f651741b-30ed-4736-96c1-680554dfe8a3.png"/>
  
- 노트를 만들 수 있는 매니지먼트 룸은 다음과 같은 기능을 가졌다.(미완성)
  -  노트 추가 버튼
  -  노트 생성 시간, 방향 조정 텍스트 필드
  -  노트 분포도를 나타내는 슬라이드 바
  -  노트 삭제 버튼
  -  노트 리셋 버튼
  -  저장 버튼
  - 노래 일시정지 및 재생
  
<img width="50%" src="https://user-images.githubusercontent.com/33209821/230932207-fbd0d00f-0189-4547-818f-ff49a6b522a2.png"/>
  
- 매니지먼트 룸에서 음악 실행 중 키보드 입력을 하면 노트가 생성된다.
- 슬라이드 바에 나온 노트들은 드래그가 가능하다.
- 음악 재생 상태또한 드래그로 조절할 수 있으면 블럭 상태도 자동으로 조절된다.
- 매니지먼트 모드에서의 블럭 움직임은 다음과 같은 함수로 이루어진다.
  - setBlockForEditMode(): 매니지먼트 상태에서도 블럭의 갯수는 제한적이기 때문에 블럭의 갯수를 제한적으로 출력
  - fillBlockForEditmode(): 실행 시 블럭의 생성 위치에 따라 점 위치 설정
  - manageModePlay(): 매니지먼트 모드에서 음악 실행 시 실행



#### 피드백
 - 씬 전환에 수직동기화가 안 되어 컴푸터 성능에 따라 속도가 차이남
 - 씬 전환 함수가 비효율적
 - 매니지먼트 기능에 버그가 있어 미완성
 - 노래 추가가 어려움
 - 스테이지 클리어 시 되돌아가기 기능 구현 필요
