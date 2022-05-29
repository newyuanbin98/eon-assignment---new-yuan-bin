using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Quiz_Manager : MonoBehaviour
{
    [Header("Main Panel")]
    public GameObject Quiz_main_prefab;
    public RectTransform Main_content;

    [Header("New Quiz Panel")]
    public GameObject New_quiz_panel;
    [SerializeField] InputField quiz_name;

    [Header("Quiz Details Panel")]
    public GameObject Quiz_detail_panel;
    public Text Quiz_detail_Title;
    public GameObject PreviewQuiz_obj;
    public GameObject Quiz_detail_prefab;
    public RectTransform Detail_content;

    [Header("Quiz Question Panel")]
    public GameObject Question_detail_panel;
    [SerializeField] InputField quiz_question;
    [SerializeField] InputField quiz_option_1;
    [SerializeField] InputField quiz_option_2;
    [SerializeField] InputField quiz_option_3;
    public Button[] pick_answer_btn;
    private int answer = 0;
    public Button Save_Button;

    [Header("Quiz Preview Panel")]
    public Quiz_Preview_script QuizPreview_script;
    public GameObject Quiz_Preview_panel;

    [Header("Pop up error message")]
    public GameObject pop_up_msg;
    public Text pop_up_txt;
    [Header("Pop up confirmation message")]
    public GameObject pop_up_confirm_msg;
    public Text pop_up_confirm_txt;
    public Button yes_btn;

    public int activeQuiz = 0;
    private int activeQuestion = 0;
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
        instantiateQuiz_main();
    }
    //QUIZ MAIN PANEL FUNCTIONS-------------------------------------------------------------------------------------
    public void instantiateQuiz_main()
    {
        Quiz_Json_Writer.Instance.CheckFileExist();
        refreshUI();
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
    public void Edit_btn(int type, int btnNum)
    {
        Save_Button.onClick.RemoveAllListeners();
        if (type == 0) 
        {
            activeQuiz = btnNum;
            Quiz_Detail_panel_Active();
            Save_Button.onClick.AddListener(() => { addNewItemData(quiz_question.text, quiz_option_1.text, quiz_option_2.text, quiz_option_3.text, answer); });
        }
        else
        {
            activeQuestion = btnNum;
            Question_detail_panel.SetActive(true);
            insert_Edit_quiz_details(btnNum);
            Save_Button.onClick.AddListener(() => { editExistingQuizDetails(activeQuestion, quiz_question.text, quiz_option_1.text, quiz_option_2.text, quiz_option_3.text, answer); });
        }
    }
    public void Delete_btn(int type, int btnNum)
    {
        yes_btn.onClick.RemoveAllListeners();
        if (type == 0)
            Popup_error_confirm_msg("Confirm delete Quiz?");
        else
            Popup_error_confirm_msg("Confirm delete Question?");
        yes_btn.onClick.AddListener(() => { ConfirmDeleteQuiz(type, btnNum); });
    }
    void ConfirmDeleteQuiz(int t, int b)
    {
        if (t == 0)
        {
            Quiz_Json_Writer.Instance.myItemList.QuizData.RemoveAt(b);
            Quiz_Json_Writer.Instance.OverwriteQuizData_JSON();
        }   
        else
        {
            Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList.RemoveAt(b);
            Quiz_Json_Writer.Instance.OverwriteQuizData_JSON();
            Quiz_Detail_panel_Active();
        }
        pop_up_confirm_msg.SetActive(false);
        //yes_btn.onClick.RemoveAllListeners();
    }

    //NEW QUIZ PANEL FUNCTIONS---------------------------------------------------------------------------------------
    public void Save_QuizName()
    {
        if (quiz_name.text != null && quiz_name.text.Length > 0)
        {
            Quiz_Json_Writer.Instance.newData.QuizName = quiz_name.text;
            Quiz_Json_Writer.Instance.myItemList.QuizData.Add(Quiz_Json_Writer.Instance.newData);
            Quiz_Json_Writer.Instance.OverwriteQuizData_JSON();
            activeQuiz = Quiz_Json_Writer.Instance.myItemList.QuizData.Count - 1;
            New_quiz_panel.SetActive(false);
            Quiz_Detail_panel_Active();
        }
        else
        {
            Popup_error_msg("Quiz Name cannot be empty");
        }
    }

    //QUIZ DETAILS PANEL FUNCTIONS-----------------------------------------------------------------------------------
    public void Quiz_Detail_panel_Active()
    {
        refreshUI();
        Quiz_detail_panel.SetActive(true);
        Quiz_detail_Title.text = Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizName;
        if (Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList.Count > 0)
        {
            PreviewQuiz_obj.SetActive(true);
            for (int i = 0; i < Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList.Count; i++)
            {
                GameObject clone_main_prefab = Instantiate(Quiz_detail_prefab, Detail_content);
                clone_main_prefab.SetActive(true);
                clone_main_prefab.GetComponent<Quiz_Info>().insertInfo_quizDetails(i + 1, Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList[i].question);
            }
        }
        else
            PreviewQuiz_obj.SetActive(false);
    }

    //QUESTION DETAILS PANEL FUNCTIONS-------------------------------------------------------------------------------
    public void Pick_Answer_toggle(Button btn)
    {
        answer = int.Parse(btn.name);
        btn.image.color = new Color(1, 1, 1, 1);
        Debug.Log("answer picked = " + answer);
        for (int i = 0; i < pick_answer_btn.Length; i++)
        {
            if (pick_answer_btn[i].name != answer.ToString())
                pick_answer_btn[i].image.color = new Color(1, 1, 1, 0);
        }
    }
    public void insert_Edit_quiz_details(int btn)
    {
        quiz_question.text = Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList[btn].question;
        quiz_option_1.text = Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList[btn].option_1;
        quiz_option_2.text = Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList[btn].option_2;
        quiz_option_3.text = Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList[btn].option_3;
        answer = Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList[btn].answer;
        for (int i = 0; i < pick_answer_btn.Length; i++)
        {
            pick_answer_btn[i].image.color = new Color(255, 255, 255, 0);
        }
        pick_answer_btn[answer - 1].image.color = new Color(255, 255, 255, 1);
    }
    public void editExistingQuizDetails(int quizListNum, string q, string o1, string o2, string o3, int an)
    {
        if (q == "" || o1 == "" || o2 == "" || o3 == "" || an == 0)
        {
            Popup_error_msg("Questionaire Incomplete");
            return;
        }
        else
        {
            Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList[quizListNum].question = q;
            Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList[quizListNum].option_1 = o1;
            Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList[quizListNum].option_2 = o2;
            Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList[quizListNum].option_3 = o3;
            Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList[quizListNum].answer = an;
            Quiz_Json_Writer.Instance.OverwriteQuizData_JSON();
            Question_detail_panel.SetActive(false);
            Quiz_Detail_panel_Active();
        }
    }
    public void Save_quiz_details()
    {
        addNewItemData(quiz_question.text, quiz_option_1.text, quiz_option_2.text, quiz_option_3.text, answer);
        editExistingQuizDetails(activeQuestion, quiz_question.text, quiz_option_1.text, quiz_option_2.text, quiz_option_3.text, answer);
    }

    void addNewItemData(string q, string o1, string o2, string o3, int an)
    {
        if (q == "" || o1 == "" || o2 == "" || o3 == "" || an == 0)
        {
            Popup_error_msg("Questionaire Incomplete");
            return;
        }
        else
        {
            Quiz_Json_Writer.Instance.newItem.question = q;
            Quiz_Json_Writer.Instance.newItem.option_1 = o1;
            Quiz_Json_Writer.Instance.newItem.option_2 = o2;
            Quiz_Json_Writer.Instance.newItem.option_3 = o3;
            Quiz_Json_Writer.Instance.newItem.answer = an;
            Quiz_Json_Writer.Instance.myItemList.QuizData[activeQuiz].QuizList.Add(Quiz_Json_Writer.Instance.newItem);
            Quiz_Json_Writer.Instance.OverwriteQuizData_JSON();
            Question_detail_panel.SetActive(false);
            Quiz_Detail_panel_Active();
        }
    }

    //QUIZ PREVIEW PANEL FUNCTIONS-----------------------------------------------------------------------------------
    public void Preview_Selected_Quiz()
    {
        Quiz_Preview_panel.SetActive(true);
        QuizPreview_script.InsertQuestionData();
    }

    //GLOBAL FUNCTIONS-----------------------------------------------------------------------------------------------
    public void Popup_error_msg(string msg)
    {
        pop_up_msg.SetActive(true);
        pop_up_txt.text = "Error: " + msg;
    }
    public void Popup_error_confirm_msg(string msg)
    {
        pop_up_confirm_msg.SetActive(true);
        pop_up_confirm_txt.text = msg;
    }

    public void refreshUI()
    {
        quiz_name.text = "";
        Quiz_detail_Title.text = "";
        quiz_question.text = "";
        quiz_option_1.text = "";
        quiz_option_2.text = "";
        quiz_option_3.text = "";
        answer = 0;
        pop_up_txt.text = "";
        pop_up_confirm_txt.text = "";
        foreach (Transform child in Main_content)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in Detail_content)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < pick_answer_btn.Length; i++)
        {
            pick_answer_btn[i].image.color = new Color(255, 255, 255, 0);
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
