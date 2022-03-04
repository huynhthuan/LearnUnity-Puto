using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextAsset TextQuestions;

    [System.Serializable]
    public class Question
    {
        public string question;
        public string[] options;
        public int answers;
    }

    [System.Serializable]
    public class QuestionList
    {
        public Question[] listQuestion;
    }

    public GameObject questionPanel;
    public GameObject mainQuestionText;
    public GameObject btnQuestionA;
    public GameObject btnQuestionB;
    public GameObject btnQuestionC;
    public GameObject btnQuestionD;
    public GameObject timeCoundown;

    [HideInInspector]
    public float timeRemaining = 16;
    [HideInInspector]
    public bool timerIsRunning = false;
    [HideInInspector]
    public bool isCheckAnswer = false;
    [HideInInspector]
    public int currentQuestion = 1;
    [HideInInspector]
    public int answerSelect = -1;

    public QuestionList myQuestion = new QuestionList();

    // Start is called before the first frame update
    void Start()
    {
        // Set question list from text
        myQuestion = JsonUtility.FromJson<QuestionList>(TextQuestions.text);
        // Set current question
        setStateQuestion(1);
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                displayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                displayTime(timeRemaining);
                checkAnswer();
            }
        }

    }

    void setStateQuestion(int indexQuestion)
    {
        Debug.Log("Set state");
        Debug.Log(indexQuestion);
        currentQuestion = indexQuestion;
        setCurrentQuestionInPanel(15 - indexQuestion);
    }

    void setCurrentQuestionInPanel(int index)
    {
        GameObject child = questionPanel.transform.GetChild(index).gameObject;
        Text txt = child.GetComponent<Text>();
        txt.color = Color.green;
        setMainQuestion(index);
        setAnswer(index);
    }

    void setMainQuestion(int index)
    {
        Text txtMain = mainQuestionText.GetComponent<Text>();
        txtMain.text = myQuestion.listQuestion[index].question;
    }

    void setAnswer(int questinIndex)
    {
        Text answerA = btnQuestionA.GetComponent<Text>();
        answerA.text = "A. " + myQuestion.listQuestion[questinIndex].options[0];
        Text answerB = btnQuestionB.GetComponent<Text>();
        answerB.text = "B. " + myQuestion.listQuestion[questinIndex].options[1];
        Text answerC = btnQuestionC.GetComponent<Text>();
        answerC.text = "C. " + myQuestion.listQuestion[questinIndex].options[2];
        Text answerD = btnQuestionD.GetComponent<Text>();
        answerD.text = "D. " + myQuestion.listQuestion[questinIndex].options[3];
    }

    void displayTime(float timeRemaining)
    {
        timeCoundown.GetComponent<Text>().text = Mathf.FloorToInt(timeRemaining % 60).ToString();
    }

    void checkAnswer()
    {
        if (myQuestion.listQuestion[currentQuestion].answers == answerSelect)
        {
            Debug.Log("Correct");
        }
        else
        {
            Debug.Log("In Correct");
            nextQuestion();
        }
    }

    void nextQuestion()
    {
        setStateQuestion(++currentQuestion);
        timeRemaining = 16;
        timerIsRunning = true;
    }
}
