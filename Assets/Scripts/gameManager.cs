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
    float time = 30.0f;

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
                if (firstCardImage.Equals("bfour" + i))
                {
                    successTxt.GetComponent<Text>().text = "성공! 팀원 " + imageName[i] + "입니다.";
                }
            }

            successTxt.SetActive(true);
            Invoke("hideSuccessTxt", 2f);

            int cardsLeft = GameObject.Find("Cards").transform.childCount;
            if (cardsLeft == 2)
            {
                endText.SetActive(true);
                Time.timeScale = 0.0f;
                Invoke("GameEnd", 1f);
                ShowEndCanvas();    
            }

            addTime(); //매치 성공 시 시간 추가
        }
        else
        {
            audioSource.PlayOneShot(incorrect);

            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
            ChangeCardColor(firstCard.transform);
            ChangeCardColor(secondCard.transform);

            successTxt.GetComponent<Text>().text = "실패!";
            successTxt.SetActive(true);
            Invoke("hideSuccessTxt", 1f);

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

    public void hideSuccessTxt()
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
    }
}
