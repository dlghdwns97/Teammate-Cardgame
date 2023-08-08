using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
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
        anim.SetBool("IsOpen", false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("front").gameObject.SetActive(false);
    }


    public void OpenCard()
    {
        audioSource.PlayOneShot(flip);

        anim.SetBool("IsOpen", true);

        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);

        if (gameManager.I.firstCard == null)
        {
            gameManager.I.firstCard = gameObject;
            gameManager.I.timerTxt.SetActive(true);     // ù��° ī�带 �������� �� timerTxt ���̰� �ϱ�
        }
        else
        {
            gameManager.I.secondCard = gameObject;
            gameManager.I.isMatched();
            gameManager.I.timerTxt.SetActive(false);    // �ι�° ī�带 �������� �� timerTxt �����
        }

        gameManager.I.FlipCounter();
    }
}
