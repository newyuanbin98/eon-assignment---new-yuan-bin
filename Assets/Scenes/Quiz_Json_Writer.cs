using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Quiz_Json_Writer : MonoBehaviour
{
    public TextAsset textJSON;
    public static Quiz_Json_Writer Instance { get; private set; }
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
        ReadQuizJsonFile();
    }

    [Header("Important string")]
    private string FULL_PATH_TEXT_FILE = "";
    private string FULL_NAME_TEXT_FILE = "QuizData.json";
    private string LOADED_JSON_TEXT = "";
    private bool _isFileFound = false;

    public string NEW_JSON_TEXT = "";

    [Serializable]
    public class QuizDataList
    {
        public List<QuizData> QuizData;
    }
    [Serializable]
    public class QuizData
    {
        public string QuizName;
        public List<QuizItem> QuizList;
    }

    [Serializable]
    public class QuizItem
    {
        public string question;
        public string option_1;
        public string option_2;
        public string option_3;
        public int answer;
    }

    public QuizDataList myItemList = new QuizDataList();
    public QuizData newData = new QuizData();
    public QuizItem newItem = new QuizItem();
    public void ReadQuizJsonFile()
    {
        Debug.Log("read quiz data json file");
        textJSON = Resources.Load("QuizData") as TextAsset;
        myItemList = JsonUtility.FromJson<QuizDataList>(textJSON.ToString());
        
        NEW_JSON_TEXT = JsonUtility.ToJson(myItemList).ToString();
        Debug.Log("QuizData.json = " + NEW_JSON_TEXT);
    }
    public void OverwriteQuizData_JSON()
    {
        //#if UNITY_IOS || UNITY_ANDROID
        //        FULL_PATH_TEXT_FILE = Path.Combine(Application.persistentDataPath, FULL_NAME_TEXT_FILE);
        //#else
        //        FULL_PATH_TEXT_FILE = Path.Combine(Application.streamingAssetsPath, FULL_NAME_TEXT_FILE);
        //#endif
        //File.WriteAllText(FULL_PATH_TEXT_FILE + FULL_NAME_TEXT_FILE, JSON_reader.myItemList.QuizItems.ToString());
        //Debug.Log("new quiz data = ");
        //JsonUtility.FromJsonOverwrite(NEW_JSON_TEXT, this);
        NEW_JSON_TEXT = JsonUtility.ToJson(myItemList).ToString();
        Debug.Log("new json text = " + NEW_JSON_TEXT);
        File.WriteAllText(Application.dataPath + "/Resources/QuizData.json", NEW_JSON_TEXT);
        ReadQuizJsonFile();
    }

//    public void CheckFileExist()
//    {
//#if UNITY_IOS || UNITY_ANDROID
//        FULL_PATH_TEXT_FILE = Path.Combine(Application.persistentDataPath, FULL_NAME_TEXT_FILE);
//#else
//        FULL_PATH_TEXT_FILE = Path.Combine(Application.streamingAssetsPath, FULL_NAME_TEXT_FILE);
//#endif
//        if (!File.Exists(FULL_PATH_TEXT_FILE))
//        {
//            CopyFileFromResources();
//        }
//        else
//        {
//            LoadFileContents();
//        }
//        myItemList = JsonUtility.FromJson<QuizData>(LOADED_JSON_TEXT);
//    }
//    private void LoadFileContents()
//    {
//        LOADED_JSON_TEXT = File.ReadAllText(FULL_PATH_TEXT_FILE);
//        _isFileFound = true;
//        //Debug.Log("loaded json text =" + LOADED_JSON_TEXT);
//    }
//    private void CopyFileFromResources()
//    {
//        TextAsset myFile = Resources.Load("QuizData") as TextAsset;
//        //Debug.Log("Copying file from resources : " + myFile.text);
//        if (myFile == null)
//        {
//            //Debug.LogError("Make sure file QuizData is in resource folder");
//            return;
//        }
//        LOADED_JSON_TEXT = myFile.ToString();
//        File.WriteAllText(FULL_PATH_TEXT_FILE, LOADED_JSON_TEXT);
//        StartCoroutine(WaitCreationFile());
//    }
//    IEnumerator WaitCreationFile()
//    {
//        FileInfo myFile = new FileInfo(FULL_NAME_TEXT_FILE);
//        float timeOut = 0.0f;

//        while (timeOut < 5.0f && !IsFileFinishCreate(myFile))
//        {
//            timeOut += Time.deltaTime;
//            yield return null;
//        }
//    }

//    private bool IsFileFinishCreate(FileInfo file)
//    {
//        FileStream stream = null;
//        try
//        {
//            stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
//        }
//        catch (IOException)
//        {
//            _isFileFound = true;
//            Debug.Log("Quiz Data file successfully created");
//            return true;
//        }
//        finally
//        {
//            if (stream != null)
//            {
//                stream.Close();
//            }
//        }

//        _isFileFound = false;
//        return false;
//    }
}