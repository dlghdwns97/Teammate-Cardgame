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
    public Animator back;
    public Text bestscore;
    public Text endbestscoreTxt;
    float time = 30.0f;
    float fiveSecond = 5.0f;         // 5�� ī��Ʈ�ٿ�� �ð�
    float timeLeft = 0f;             // ���� ���� ���� �ð�

    public GameObject endCanvas;
    public Text flipCountText;
    private int flipCount = 0;

    public int rows = 4; // ��
    public int cols = 4; // ��


    public static gameManager I;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        Time.timeScale = 1.0f;

        int cardCount = rows * cols; // ī�� �� ����
        int[] bfour = new int[cardCount]; 

        for (int i = 0; i < cardCount; i++)
        {
            bfour[i] = i / 2; // 0���� (cardCount / 2 - 1)���� �ݺ��Ǵ� �� �Ҵ�, 4x4�� �� {0,0,1,1 ... 7,7}
        }

        bfour = bfour.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < cardCount; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("Cards").transform;

            float x = (i / cols) * 1.4f - 2.1f;
            float y = (i % cols) * 1.4f - (cols - 1.0f);
            newCard.transform.position = new Vector3(x, y, 0);

            string bfourName = "bfour" + bfour[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(bfourName);
        }

        back.Play("card_back_fade");

        int level = cols - 2;
        bestscore.text = PlayerPrefs.GetFloat("bestScore" + level).ToString("N0");
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

        if (timerTxt.activeInHierarchy == true)     // timerTxt SetActive �� true �϶�, �׳� timerTxt.SetActive(true)�� ����ϸ� ������ ��ȯ���� ���� ������ �̷��� ����ϴ�. 
        {
            fiveSecond -= Time.deltaTime;
            timerTxt.GetComponent<Text>().text = fiveSecond.ToString("N2");
            if (fiveSecond < 0f)
            {
                timerTxt.SetActive(false);
                firstCard.GetComponent<card>().closeCard(0f);   // ù��° ī�� ��� ������
                firstCard = null;                               // �����ָ� ù��° ī�� ���� 5�� �����ڿ� �Ȱ��� ī�� ���� ī�� �����
            }
        }
        else
        {
            fiveSecond = 5.0f;                                  // timerTxt �� ������� ���ѽð� 5�ʷ� �ʱ�ȭ
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

            string[] imageName = new string[] { "��ȫ��", "��ȫ��", "��ȫ��", "�質��", "�質��", "�質��", "��ź��", "��ź��", "��ź��", "��ź��" };

            for (int i = 0; i < 10; i++)
            {
                if (firstCardImage.Equals("bfour" + i))     // firstCardImage ���� bfour0 ~ bfour9 �� � ������ Ȯ��
                {
                    successTxt.GetComponent<Text>().text = "����! ���� " + imageName[i] + "�Դϴ�.";       // i�� �״�� ���� �迭���� �ҷ���
                }
            }

            successTxt.SetActive(true);                     // ī�� ������ �� ���� �ؽ�Ʈ ����
            Invoke("hideSuccessTxt", 2f);                   // 2�� �ڿ� ����

            int cardsLeft = GameObject.Find("Cards").transform.childCount;
            if (cardsLeft == 2)
            {
                timeLeft = time;                            // Ŭ����� ���� �ð� ����
                Time.timeScale = 0.0f;
                Invoke("GameEnd", 1f);
                int level = cols - 2;
                PlayerPrefs.SetInt("levelAt", level + 2);
                ShowEndCanvas();    
            }

            addTime(); //��ġ ���� �� �ð� �߰�
        }
        else
        {
            audioSource.PlayOneShot(incorrect);

            firstCard.GetComponent<card>().closeCard(0.5f);
            secondCard.GetComponent<card>().closeCard(0.5f);
            ChangeCardColor(firstCard.transform);
            ChangeCardColor(secondCard.transform);

            successTxt.GetComponent<Text>().text = "����!";
            successTxt.SetActive(true);                     // ī�� �������� �� ���� �ؽ�Ʈ ����
            Invoke("hideSuccessTxt", 1f);                   // ���� �ؽ�Ʈ�� 1�� �ڿ� ����

            if (time >= 10.0f)
            {
                ReduceTime(); //��ġ ���� �� �ð� ����
            }
        }

        firstCard = null;
        secondCard = null;
    }

    void ChangeTimerColor()
    {
        GameObject.Find("timeText").GetComponent<Text>().color = Color.red;
    }

    void ReduceTime() //��Ī ���� �� �ð� ����, Ÿ�̸� ���� ����
    {
        time -= 2f;
        GameObject.Find("timeText").GetComponent<Text>().color = Color.gray;
        StartCoroutine("ReturnTimerColorCoroutine");
    }

    void addTime() //��Ī ���� �� �ð� ����, Ÿ�̸� ���� ����
    {
        time += 2f;
        GameObject.Find("timeText").GetComponent<Text>().color = Color.yellow;
        StartCoroutine("ReturnTimerColorCoroutine");
    }

    private IEnumerator ReturnTimerColorCoroutine()//Ÿ�̸� ���� �����·� ����
    {
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("timeText").GetComponent<Text>().color = Color.black;
    }

    void ChangeCardColor(Transform cardTransform)//ī�� ���� ȸ������ ����
    {
        cardTransform.Find("back").GetComponent<SpriteRenderer>().color = Color.gray;

        StartCoroutine(ReturnCardColorCoroutine(cardTransform));
    }

    private IEnumerator ReturnCardColorCoroutine(Transform cardTransformRevert)//ī�� ���� �����·� ����
    {
        yield return new WaitForSeconds(1.0f);
        cardTransformRevert.Find("back").GetComponent<SpriteRenderer>().color = Color.black;
    }

    public void hideSuccessTxt()    // ���� �ؽ�Ʈ ������ �Լ�
    {
        successTxt.SetActive(false);
    }

    public void FlipCounter() //�õ� Ƚ�� ī����
    {
        flipCount++;
    }

    private void FlipCountText() //flipCount �ؽ�Ʈ�� ��ȯ
    {
        int matchCount = flipCount / 2 + 1;
        flipCountText.text = matchCount.ToString() + " ȸ";
    }

    private void ShowEndCanvas()
    {
        endCanvas.SetActive(true);
        FlipCountText(); // ���� �� flipCount �ؽ�Ʈ ������Ʈ
        makeScore(); // ���� ������Ʈ
    }

    public void makeScore()
    {
        float score = (timeLeft * 100.0f) - (flipCount * 10.0f); // ���� ��� : �����ð� * 100 - ������Ƚ�� * 10
        if (score < 0.0f)
        {
            score = 0.0f;
        }

        scoreTxt.GetComponent<Text>().text = "���� : " + score.ToString("N0");    // ����� ������ �Ҽ��� ���� ǥ��

        int level = cols - 2;

        if (PlayerPrefs.HasKey("bestScore" + level) == false)
        {
            PlayerPrefs.SetFloat("bestScore" + level, score);
        }
        else
        {
            if (PlayerPrefs.GetFloat("bestScore" + level) < score)
            {
                PlayerPrefs.SetFloat("bestScore" + level, score);
            }
        }

        endbestscoreTxt.text = "�ְ����� : " + PlayerPrefs.GetFloat("bestScore" + level).ToString("N0");
    }
}
