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

        int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };

        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("Cards").transform;

            float x = (i / 4) * 1.4f - 2.1f;
            float y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(x, y, 0);

            string rtanName = "rtan" + rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
        }

    }

    void Update()
    {
        time -= Time.deltaTime;
        timeText.text = time.ToString("N2");

        if(time <= 0f)
        {
            endText.SetActive(true);
            Time.timeScale = 0.0f;
            ShowEndCanvas();
        }
        else if(time <= 10.0f)
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

            int cardsLeft = GameObject.Find("Cards").transform.childCount;
            if (cardsLeft == 2)
            {
                endText.SetActive(true);
                Time.timeScale = 0.0f;
                Invoke("GameEnd", 1f);
                ShowEndCanvas();
            }
        }
        else
        {
            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
            ChangeCardColor(firstCard.transform);
            ChangeCardColor(secondCard.transform);
            //time -= 3f; //��ġ ���� �� �ð� ����
        }

        firstCard = null;
        secondCard = null;
    }

    void ChangeTimerColor()
    {
        GameObject.Find("timeText").GetComponent<Text>().color = Color.red;
    }

    void ChangeCardColor(Transform cardTransform) //ī�� ���� ȸ������ ����
    {
        cardTransform.Find("back").GetComponent<SpriteRenderer>().color = Color.gray;

        StartCoroutine(ReturnCardColorCoroutine(cardTransform));
    }

    private IEnumerator ReturnCardColorCoroutine(Transform cardTransformRevert) //ī�� ���� �����·� ����
    {
        yield return new WaitForSeconds(1.0f);
        cardTransformRevert.Find("back").GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void FlipCounter() //�õ� Ƚ�� ī����
    {
        flipCount++;
    }

    private void FlipCountText() //flipCount �ؽ�Ʈ�� ��ȯ
    {
        flipCountText.text = flipCount.ToString() + " ȸ";
    }

    private void ShowEndCanvas()
    {
        endCanvas.SetActive(true);
        FlipCountText(); // ���� �� flipCount �ؽ�Ʈ ������Ʈ
    }
}
