using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Quiz_Info : MonoBehaviour
{
    public int Type;
    public Text num;
    public Text title;
    public Text question;
    public int answer;

    int btnNum;
    public void insertInfo_main(int num, string title)
    {
        btnNum = num;
        this.num.text = num + ".";
        this.title.text = title;
    }

    public void insertInfo_quizDetails(int num, string question)
    {
        btnNum = num;
        this.num.text = num + ".";
        this.question.text = question;
    }

    public void GoButton()
    {
        Quiz_Manager.Instance.Go_btn(Type, btnNum - 1);
    }
    public void EditButton()
    {
        Quiz_Manager.Instance.Edit_btn(Type, btnNum - 1);
    }
    public void DeleteButton()
    {
        Quiz_Manager.Instance.Delete_btn(Type, btnNum - 1);
    }
}
