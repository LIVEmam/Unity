using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int totalpoint;
    public int stagepoint;
    public int stageIndex;
    public int Hp = 3;
    public Player player;
    public GameObject[] stages;

    public Image[] UIHp;
    public Text UIPoint;
    public Text UIStage;
    public GameObject Restart;

    void Update()
    {
        UIPoint.text = (totalpoint + stagepoint).ToString();
    }

    public void NextStage()
    {
        //change stage
        //if (stageIndex < stages.Length - 1)
        //{
        //    stages[stageIndex].SetActive(false);
        //    stageIndex++;
        //    stages[stageIndex].SetActive(true);
        //    PlayerReposition();

        //    UIStage.text = "Stage" + (stageIndex + 1);
        //}
        //else
        //{
        //    //gaem clear
        //    //player contol lock
        //    Time.timeScale = 0;

        //    //Restart Button UI
        //    Restart.SetActive(true);
        //    Text btnText = Restart.GetComponentInChildren<Text>();
        //    btnText.text = "Clear!\nRetry?";
        //}

        SceneManager.LoadScene("Scenes/stage02");


        //Calculate Point
        totalpoint += stagepoint;
        stagepoint = 0;
    }

    public void HpDown()
    {
        if (Hp > 1)
        {
            Hp--;
            UIHp[0].color = new Color(1, 1, 1, 0.4f);
        }
        else
        {
            //player die effect
            player.OnDie();

            //Result UI

            //Retey Button UI
            Restart.SetActive(true);

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Hp > 1)
            {
                PlayerReposition();
            }

            //hp down
            HpDown();
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, -1);
        player.VelocityZero();
    }

    public void ReStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
