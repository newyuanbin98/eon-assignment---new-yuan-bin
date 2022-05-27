using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Quiz_Info : MonoBehaviour
{
    public Text num;
    public Text title;
    public Text questions;
    public Text[] options;
    public int answer;

    int btnNum;
    public void insertInfo_main(int num, string title)
    {
        btnNum = num;
        this.num.text = num + ".";
        this.title.text = title;
    }

    public void insertInfo_quizDetails()
    {

    }

    public void GoButton()
    {
        //Quiz_Manager.Instance.Go_btn();
    }
    public void EditButton()
    {

    }
    public void DeleteButton()
    {
        Quiz_Manager.Instance.Delete_btn(btnNum - 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
