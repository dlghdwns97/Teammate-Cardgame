using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class gameManager : MonoBehaviour
{
    public Text timeText;
    public GameObject endText;
    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    public AudioClip match;
    public AudioSource audioSource;
    public AudioClip incorrect;
    public GameObject successTxt;
    public GameObject timerTxt;
    public GameObject scoreTxt;
    float time = 30.0f;
    float fiveSecond = 5.0f;         // 5초 카운트다운용 시간
    float timeLeft = 0f;             // 점수 계산용 남은 시간

    public GameObject endCanvas;
    public Text flipCountText;
    private int flipCount = 0;

    public static gameManager I;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        Time.timeScale = 1.0f;

        int[] bfour = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };

        bfour = bfour.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("Cards").transform;

            float x = (i / 4) * 1.4f - 2.1f;
            float y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(x, y, 0);

            string bfourName = "bfour" + bfour[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(bfourName);
        }

    }

    void Update()
    {
        time -= Time.deltaTime;
        timeText.text = time.ToString("N2");

        if (time <= 0f)
        {
            endText.SetActive(true);
            Time.timeScale = 0.0f;
            ShowEndCanvas();
        }
        else if (time <= 10.0f)
        {
            ChangeTimerColor();
        }

        if (timerTxt.activeInHierarchy == true)     // timerTxt SetActive 가 true 일때, 그냥 timerTxt.SetActive(true)를 사용하면 실제론 반환값이 없기 때문에 이렇게 썼습니다. 
        {
            fiveSecond -= Time.deltaTime;
            timerTxt.GetComponent<Text>().text = fiveSecond.ToString("N2");
            if (fiveSecond < 0f)
            {
                timerTxt.SetActive(false);
                firstCard.GetComponent<card>().closeCard(0f);   // 첫번째 카드 즉시 뒤집음
                firstCard = null;                               // 안해주면 첫번째 카드 고르고 5초 지난뒤에 똑같은 카드 고르면 카드 사라짐
            }
        }
        else
        {
            fiveSecond = 5.0f;                                  // timerTxt 가 사라지면 제한시간 5초로 초기화
        }
    }

    public void isMatched()
    {
        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage)
        {
            audioSource.PlayOneShot(match);

            firstCard.GetComponent<card>().destroyCard();
            secondCard.GetComponent<card>().destroyCard();

            string[] imageName = new string[] { "이홍준", "이홍준", "이홍준", "김나운", "김나운", "김나운", "진재환", "진재환" };

            for (int i = 0; i < 8; i++)
            {
                if (firstCardImage.Equals("bfour" + i))     // firstCardImage 값이 bfour0 ~ bfour7 중 어떤 것인지 확인
                {
                    successTxt.GetComponent<Text>().text = "성공! 팀원 " + imageName[i] + "입니다.";       // i값 그대로 위에 배열에서 불러옴
                }
            }

            successTxt.SetActive(true);                     // 카드 맞췄을 시 성공 텍스트 등장
            Invoke("hideSuccessTxt", 2f);                   // 2초 뒤에 숨김

            int cardsLeft = GameObject.Find("Cards").transform.childCount;
            if (cardsLeft == 2)
            {
                endText.SetActive(true);
                timeLeft = time;                            // 클리어시 남은 시간 저장
                Time.timeScale = 0.0f;
                Invoke("GameEnd", 1f);
                ShowEndCanvas();    
            }

            addTime(); //매치 성공 시 시간 추가
        }
        else
        {
            audioSource.PlayOneShot(incorrect);

            firstCard.GetComponent<card>().closeCard(0.5f);
            secondCard.GetComponent<card>().closeCard(0.5f);
            ChangeCardColor(firstCard.transform);
            ChangeCardColor(secondCard.transform);

            successTxt.GetComponent<Text>().text = "실패!";
            successTxt.SetActive(true);                     // 카드 못맞췄을 시 실패 텍스트 등장
            Invoke("hideSuccessTxt", 1f);                   // 실패 텍스트는 1초 뒤에 숨김

            if (time >= 10.0f)
            {
                ReduceTime(); //매치 실패 시 시간 감소
            }
        }

        firstCard = null;
        secondCard = null;
    }

    void ChangeTimerColor()
    {
        GameObject.Find("timeText").GetComponent<Text>().color = Color.red;
    }

    void ReduceTime() //매칭 실패 시 시간 감소, 타이머 색상 변경
    {
        time -= 2f;
        GameObject.Find("timeText").GetComponent<Text>().color = Color.gray;
        StartCoroutine("ReturnTimerColorCoroutine");
    }

    void addTime() //매칭 성공 시 시간 증가, 타이머 색상 변경
    {
        time += 2f;
        GameObject.Find("timeText").GetComponent<Text>().color = Color.yellow;
        StartCoroutine("ReturnTimerColorCoroutine");
    }

    private IEnumerator ReturnTimerColorCoroutine()//타이머 색상 원상태로 변경
    {
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("timeText").GetComponent<Text>().color = Color.black;
    }

    void ChangeCardColor(Transform cardTransform)//카드 색상 회색으로 변경
    {
        cardTransform.Find("back").GetComponent<SpriteRenderer>().color = Color.gray;

        StartCoroutine(ReturnCardColorCoroutine(cardTransform));
    }

    private IEnumerator ReturnCardColorCoroutine(Transform cardTransformRevert)//카드 색상 원상태로 변경
    {
        yield return new WaitForSeconds(1.0f);
        cardTransformRevert.Find("back").GetComponent<SpriteRenderer>().color = Color.black;
    }

    public void hideSuccessTxt()    // 성공 텍스트 숨기기용 함수
    {
        successTxt.SetActive(false);
    }

    public void FlipCounter() //시도 횟수 카운팅
    {
        flipCount++;
    }

    private void FlipCountText() //flipCount 텍스트로 변환
    {
        flipCountText.text = flipCount.ToString() + " 회";
    }

    private void ShowEndCanvas()
    {
        endCanvas.SetActive(true);
        FlipCountText(); // 종료 시 flipCount 텍스트 업데이트
        makeScore(); // 점수 업데이트
    }

    public void makeScore()
    {
        float score = (timeLeft * 100.0f) - (flipCount * 10.0f);    // 점수 계산 : 남은시간 * 100 - 뒤집은횟수 * 10
        scoreTxt.GetComponent<Text>().text = "점수 : " + score.ToString("N0");    // 계산한 점수를 소숫점 빼고 표시
    }
}
