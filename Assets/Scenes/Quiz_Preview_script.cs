using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quiz_Preview_script : MonoBehaviour
{
    [Header("Quiz Data")]
    public Text QuizNum_txt;
    public Text Quiz_Question_txt;
    public Text Quiz_Option1_txt;
    public Text Quiz_Option2_txt;
    public Text Quiz_Option3_txt;
    int quiz_answer;

    public Text Next_btn_text;
    public Button[] pick_answer_btn;
    int pick;
    int quizCount = -1;

    [Header("Quiz Results Panel")]
    public GameObject Quiz_Result_Panel;
    public Text Result_txt;

    int ActiveQuiz;
    int Correct_Count = 0;

    public void InsertQuestionData()
    {
        quizCount++;
        ActiveQuiz = Quiz_Manager.Instance.activeQuiz;
        if (quizCount + 1 == Quiz_Json_Writer.Instance.myItemList.QuizData[ActiveQuiz].QuizList.Count) 
            Next_btn_text.text = "Submit";
        if (quizCount < Quiz_Json_Writer.Instance.myItemList.QuizData[ActiveQuiz].QuizList.Count)//if quiz still hav more questions
        {
            QuizNum_txt.text = "Question (" + (quizCount + 1) + "/" + Quiz_Json_Writer.Instance.myItemList.QuizData[ActiveQuiz].QuizList.Count + ")";
            Quiz_Question_txt.text = Quiz_Json_Writer.Instance.myItemList.QuizData[ActiveQuiz].QuizList[quizCount].question;
            Quiz_Option1_txt.text = Quiz_Json_Writer.Instance.myItemList.QuizData[ActiveQuiz].QuizList[quizCount].option_1;
            Quiz_Option2_txt.text = Quiz_Json_Writer.Instance.myItemList.QuizData[ActiveQuiz].QuizList[quizCount].option_2;
            Quiz_Option3_txt.text = Quiz_Json_Writer.Instance.myItemList.QuizData[ActiveQuiz].QuizList[quizCount].option_3;
            quiz_answer = Quiz_Json_Writer.Instance.myItemList.QuizData[ActiveQuiz].QuizList[quizCount].answer;
        }
        else//finished all questions
        {
            Quiz_Manager.Instance.Quiz_Preview_panel.SetActive(false);
            Show_Results();
        }
    }
    
    public void Pick_Answer_toggle(Button btn)
    {
        pick = int.Parse(btn.name);
        btn.image.color = new Color(0.5f, 0.5f, 0.5f);
        Debug.Log("answer picked = " + pick);
        for (int i = 0; i < pick_answer_btn.Length; i++)
        {
            if (pick_answer_btn[i].name != pick.ToString())
                pick_answer_btn[i].image.color = new Color(1, 1, 1);
        }
    }

    public void Next_Button()
    {
        if (pick == 0)
            Quiz_Manager.Instance.Popup_error_msg("Please retry after picking an answer."); 
        if (pick == quiz_answer)
            Correct_Count++;

        for (int i = 0; i < pick_answer_btn.Length; i++)
        {
            pick_answer_btn[i].image.color = new Color(1, 1, 1);
        }

        InsertQuestionData();
    }

    public void Show_Results()
    {
        Quiz_Result_Panel.SetActive(true);
        Result_txt.text = "Your Score: " + Correct_Count + "/" + Quiz_Json_Writer.Instance.myItemList.QuizData[ActiveQuiz].QuizList.Count;
    }
    public void reset()
    {
        pick = 0;
        Next_btn_text.text = "Next";
        quizCount = -1;
        Correct_Count = 0;
        for (int i = 0; i < pick_answer_btn.Length; i++)
        {
            pick_answer_btn[i].image.color = new Color(1, 1, 1);
        }
    }
}
