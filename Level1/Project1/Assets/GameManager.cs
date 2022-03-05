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
    public GameObject popupLose;
    public GameObject popupWin;

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
        newGame();
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
        setCurrentQuestionInPanel(14 - indexQuestion);
    }

    void setCurrentQuestionInPanel(int index)
    {
        GameObject child = questionPanel.transform.GetChild(index).gameObject;
        Text txt = child.GetComponent<Text>();
        txt.color = Color.green;
        setMainQuestion();
        setAnswer();
    }

    void resetQuestionHighLight()
    {
        foreach (Transform child in questionPanel.transform)
        {
            child.GetComponent<Text>().color = new Color(154, 150, 150);
        }
    }

    void setMainQuestion()
    {
        Text txtMain = mainQuestionText.GetComponent<Text>();
        txtMain.text = myQuestion.listQuestion[currentQuestion].question;
    }

    void setAnswer()
    {
        Text answerA = btnQuestionA.GetComponent<Text>();
        answerA.text = "A. " + myQuestion.listQuestion[currentQuestion].options[0];
        Text answerB = btnQuestionB.GetComponent<Text>();
        answerB.text = "B. " + myQuestion.listQuestion[currentQuestion].options[1];
        Text answerC = btnQuestionC.GetComponent<Text>();
        answerC.text = "C. " + myQuestion.listQuestion[currentQuestion].options[2];
        Text answerD = btnQuestionD.GetComponent<Text>();
        answerD.text = "D. " + myQuestion.listQuestion[currentQuestion].options[3];
    }

    void displayTime(float timeRemaining)
    {
        timeCoundown.GetComponent<Text>().text = Mathf.FloorToInt(timeRemaining % 60).ToString();
    }

    void checkAnswer()
    {
        Debug.Log("My answer " + answerSelect);

        if (myQuestion.listQuestion[currentQuestion].answers == answerSelect)
        {
            Debug.Log("Correct");
            showCorrectAnswer();
            nextQuestion();
        }
        else
        {
            Debug.Log("In Correct");
            showCorrectAnswer();
            enablePopupLose();
        }
    }

    void nextQuestion()
    {
        if (currentQuestion + 1 > 14)
        {
            enablePopupWin();
        }
        else
        {
            answerSelect = -1;
            setStateQuestion(++currentQuestion);
            timeRemaining = 16;
            timerIsRunning = true;
            enableInteractable();
        }
    }

    public void onClickAnswer(int answerIndex)
    {
        answerSelect = answerIndex;
    }

    void showCorrectAnswer()
    {
        switch (myQuestion.listQuestion[currentQuestion].answers)
        {
            case 0:
                btnQuestionA.transform.parent.GetComponent<Button>().interactable = false;
                break;
            case 1:
                btnQuestionB.transform.parent.GetComponent<Button>().interactable = false;
                break;
            case 2:
                btnQuestionC.transform.parent.GetComponent<Button>().interactable = false;
                break;
            case 3:
                btnQuestionD.transform.parent.GetComponent<Button>().interactable = false;
                break;
        }
    }

    void enableInteractable()
    {
        btnQuestionA.transform.parent.GetComponent<Button>().interactable = true;
        btnQuestionB.transform.parent.GetComponent<Button>().interactable = true;
        btnQuestionC.transform.parent.GetComponent<Button>().interactable = true;
        btnQuestionD.transform.parent.GetComponent<Button>().interactable = true;
    }

    public void newGame()
    {
        answerSelect = -1;
        timeRemaining = 16;
        timerIsRunning = true;
        enableInteractable();
        disablePopupLose();
        disablePopupWin();
        resetQuestionHighLight();
        setStateQuestion(0);
    }

    void disablePopupLose()
    {
        popupLose.SetActive(false);
    }

    void enablePopupLose()
    {
        popupLose.SetActive(true);
    }
    void disablePopupWin()
    {
        popupWin.SetActive(false);
    }

    void enablePopupWin()
    {
        popupWin.SetActive(true);
    }
}
