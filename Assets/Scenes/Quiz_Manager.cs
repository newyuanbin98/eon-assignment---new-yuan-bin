using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Quiz_Manager : MonoBehaviour
{
    [Header("Main Panel")]
    public GameObject Quiz_main_prefab;
    public RectTransform Main_content; 

    [Header("New Quiz Panel")]
    [SerializeField] InputField quiz_name;

    [Header("Quiz Details Panel")]
    public GameObject Quiz_detail_panel;
    public Text Quiz_detail_Title;
    public GameObject PreviewQuiz_obj;
    public RectTransform Detail_content;
    [SerializeField] InputField quiz_question;
    [SerializeField] InputField quiz_option_1;
    [SerializeField] InputField quiz_option_2;
    [SerializeField] InputField quiz_option_3;

    [Header("Pop up error message")]
    public GameObject pop_up_msg;
    public Text pop_up_txt;


    private int totalQuiz = 0;
    public static Quiz_Manager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Quiz_Json_Writer.Instance.ReadQuizJsonFile();
        instantiateQuiz_main();
    }
    //QUIZ MAIN PANEL FUNCTIONS----------------------------------------------------------------------------------
    public void instantiateQuiz_main()
    {
        if (Quiz_Json_Writer.Instance.myItemList.QuizData.Count > 0)
        {
            
            for (int i = 0; i < Quiz_Json_Writer.Instance.myItemList.QuizData.Count; i++) 
            {
                GameObject clone_main_prefab = Instantiate(Quiz_main_prefab, Main_content);
                clone_main_prefab.SetActive(true);
                clone_main_prefab.GetComponent<Quiz_Info>().insertInfo_main(i + 1, Quiz_Json_Writer.Instance.myItemList.QuizData[i].QuizName);
            }
        }
    }
    public void Go_btn(Button btn)
    {

    }
    public void Edit_btn(Button btn)
    {

    }
    public void Delete_btn(int btnNum)
    {
        Quiz_Json_Writer.Instance.myItemList.QuizData.RemoveAt(btnNum);
        Quiz_Json_Writer.Instance.OverwriteQuizData_JSON();
    }

    //NEW QUIZ PANEL FUNCTIONS------------------------------------------------------------------------------------
    public void Save_QuizName()
    {
        if (quiz_name.text != null && quiz_name.text.Length > 0)
        {
            Quiz_Json_Writer.Instance.newData.QuizName = quiz_name.text;
            Quiz_Json_Writer.Instance.myItemList.QuizData.Add(Quiz_Json_Writer.Instance.newData);
            Quiz_Json_Writer.Instance.OverwriteQuizData_JSON();
        }
        else
        {
            Popup_error_msg("Quiz Name cannot be empty");
        }
    }

    //QUIZ DETAILS PANEL FUNCTIONS------------------------------------------------------------------------------------
    public void Quiz_Detail_panel_Active(int quizNum)
    {
        Quiz_detail_panel.SetActive(true);
        Quiz_detail_Title.text = Quiz_Json_Writer.Instance.myItemList.QuizData[quizNum].QuizName;
        if (Quiz_Json_Writer.Instance.myItemList.QuizData[quizNum].QuizList.Count > 0)
        {
            totalQuiz = Quiz_Json_Writer.Instance.myItemList.QuizData[quizNum].QuizList.Count;
            for (int i = 0; i < Quiz_Json_Writer.Instance.myItemList.QuizData[quizNum].QuizList.Count; i++)
            {
                GameObject clone_main_prefab = Instantiate(Quiz_main_prefab, Main_content);
                clone_main_prefab.SetActive(true);
                clone_main_prefab.GetComponent<Quiz_Info>().insertInfo_main(i + 1, Quiz_Json_Writer.Instance.myItemList.QuizData[quizNum].QuizName);
            }
        }
    }
    public void Save_quiz_details()
    {
        
        //quiz_question.text
        //    quiz_option_1.text
        //    quiz_option_2.text
        //    quiz_option_3.text
    }


    //GLOBAL FUNCTIONS---------------------------------------------------------------------------------------------
    void Popup_error_msg(string msg)
    {
        pop_up_msg.SetActive(true);
        pop_up_txt.text = "Error: " + msg;
    }
}
