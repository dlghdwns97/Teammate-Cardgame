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

    public void closeCard(float a)      // ù��° ī�尡 5�� ���� �� ������������ �����̾��� ��� ���������� �ϱ� ������ �Ű����� a�� ���� �Լ��� ����
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
            lv2GameManager.I.timerTxt.SetActive(true);     // ù��° ī�带 �������� �� timerTxt ���̰� �ϱ�
        }
        else
        {
            lv2GameManager.I.secondCard = gameObject;
            lv2GameManager.I.isMatched();
            lv2GameManager.I.timerTxt.SetActive(false);    // �ι�° ī�带 �������� �� timerTxt �����
        }

        lv2GameManager.I.FlipCounter();
    }
}
