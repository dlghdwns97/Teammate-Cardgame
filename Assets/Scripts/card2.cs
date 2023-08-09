using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card2 : MonoBehaviour
{
    public Animator anim;
    public AudioClip flip;
    public AudioSource audioSource;

    public void destroyCard()
    {
        Invoke("destroyCardInvoke", 0.5f);
    }

    void destroyCardInvoke()
    {
        Destroy(gameObject);
    }

    public void closeCard(float a)      // 첫번째 카드가 5초 지난 뒤 뒤집어지려면 딜레이없이 즉시 뒤집어져야 하기 때문에 매개변수 a를 갖는 함수로 수정
    {
        Invoke("closeCardInvoke", a);
    }

    void closeCardInvoke()
    {
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("front").gameObject.SetActive(false);
        anim.SetBool("IsOpen", false);
    }

    public void OpenCard()
    {
        audioSource.PlayOneShot(flip);

        anim.SetBool("IsOpen", true);

        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);

        if (lv2GameManager.I.firstCard == null)
        {
            lv2GameManager.I.firstCard = gameObject;
            lv2GameManager.I.timerTxt.SetActive(true);     // 첫번째 카드를 뒤집었을 때 timerTxt 보이게 하기
        }
        else
        {
            lv2GameManager.I.secondCard = gameObject;
            lv2GameManager.I.isMatched();
            lv2GameManager.I.timerTxt.SetActive(false);    // 두번째 카드를 뒤집었을 때 timerTxt 숨기기
        }

        lv2GameManager.I.FlipCounter();
    }
}
