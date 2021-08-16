using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image playerHPbar;
    [SerializeField]
    Image hpBarFillimg;
    [SerializeField]
    private Button[] actionButtions;
    [SerializeField]
    private Image skill1;
    [SerializeField]
    private Image skill2;
    [SerializeField]
    private Image endImg;
    [SerializeField]
    private Image winImg;
    [SerializeField]
    private Image endBackImg;
    [SerializeField]
    private Text skill1Txt;
    [SerializeField]
    private Text skill2Txt;
    private KeyCode action1, action2, action3;

    public GameObject target;
    PlayerCtrl playerCtrl;
    private float curTime;
    private float curTime2;
    private float startTime;
    private float startTime2;
    private float Skill1coolTime = 5f;
    private float Skill2coolTime = 10f;
    private bool Skill1isEnded = true;
    private bool Skill2isEnded = true;
    private void Start()
    {
        hpBarFillimg = playerHPbar.transform.GetChild(0).GetComponent<Image>();
        action1 = KeyCode.Alpha1;
        action2 = KeyCode.Alpha2;
        action3 = KeyCode.Alpha3;

        skill1.gameObject.SetActive(false);
        skill2.gameObject.SetActive(false);
        skill1Txt.gameObject.SetActive(false);
        skill2Txt.gameObject.SetActive(false);
        playerCtrl = target.GetComponent<PlayerCtrl>();
        endBackImg.gameObject.SetActive(false);
        endImg.gameObject.SetActive(false);
        winImg.gameObject.SetActive(false);
    }

    private void Update()
    {
        playerHPbar.transform.position = target.transform.position + new Vector3(0f, 2f, 0);
        PlayerHP();
        if (Input.GetKeyDown(action1))
        {
            ActionButtonClick(0);
        }
        if(Input.GetKeyDown(action2))
        {
            ActionButtonClick(1);
        }
        if(!Skill1isEnded)
        {
            CheckCoolTime(0);
        }
        if(!Skill2isEnded)
        {
            CheckCoolTime(1);
        }

        if(Manager.Instance.IsGameOver == true)
        {
           
            StartCoroutine(FadeIn());
        }
        if(Manager.Instance.IsGameWin == true)
        {
            StartCoroutine(GameWin());
        }
    }

    public void PlayerHP()
    {
        PlayerCtrl player = target.GetComponent<PlayerCtrl>();
        hpBarFillimg.fillAmount =  (player.health/ 100f);
    }

    void ActionButtonClick(int btnIndex)
    {
        if (playerCtrl._stopSkill == false) return;

        if (btnIndex == 0)
        {
            if (Skill1isEnded)
            {
                actionButtions[btnIndex].onClick.Invoke();
                ResetCoolTime(btnIndex);
                
            }
        }
        else
        {
            if (Skill2isEnded)
            {
                actionButtions[btnIndex].onClick.Invoke();
                ResetCoolTime(btnIndex);
            }
        }
    }

    void CheckCoolTime(int skillNum)
    {
        curTime = Time.time - startTime;
        curTime2 = Time.time - startTime2;
       
        if(curTime < Skill1coolTime)
        {
            SetFillAmount(0, Skill1coolTime - curTime);
        }
        else if (!Skill1isEnded)
        {
            EndCollTime(0);
        }

        if (curTime2 < Skill2coolTime)
        {
            SetFillAmount(1, Skill2coolTime - curTime2);
        }
        else if(!Skill2isEnded)
        {
            EndCollTime(1);
        }
    }

    private void SetFillAmount(int skillNum, float value)
    {
        if(skillNum == 0)
        {
            skill1.fillAmount = value / Skill1coolTime;
            string txt = value.ToString("0.0");
            skill1Txt.text = txt;
        }
        else if(skillNum == 1)
        {
            skill2.fillAmount = value / Skill2coolTime;
            string txt = value.ToString("0.0");
            skill2Txt.text = txt;
            Debug.Log("3");
        }
    }

    private void EndCollTime(int skillNum)
    {
        if(skillNum == 0)
        {
            SetFillAmount(0, 0);
            Skill1isEnded = true;
            skill1Txt.transform.gameObject.SetActive(false);
        }
        else
        {
            SetFillAmount(1, 0);
            Skill2isEnded = true;
            skill2Txt.transform.gameObject.SetActive(false);
        }
    }

    void ResetCoolTime(int btnIndex)
    {
        if(btnIndex == 0)
        {
            skill1Txt.gameObject.SetActive(true);
            skill1.gameObject.SetActive(true);
            curTime = Skill1coolTime;
            startTime = Time.time;
            SetFillAmount(0,Skill1coolTime);
            Skill1isEnded = false;
            
        }
        else if(btnIndex == 1)
        {
            skill2Txt.gameObject.SetActive(true);
            skill2.gameObject.SetActive(true);
            curTime2 = Skill2coolTime;
            startTime2 = Time.time;
            SetFillAmount(1,Skill2coolTime);
            Skill2isEnded = false;
        }
    }

    IEnumerator FadeIn()
    {
        endBackImg.gameObject.SetActive(true);
        endImg.gameObject.SetActive(true);
        Color color = endImg.color;
        Time.timeScale = 0;
        yield return null;
    }

    IEnumerator GameWin()
    {
        winImg.gameObject.SetActive(true);
        Time.timeScale = 0;
        yield return null;
    }
}
