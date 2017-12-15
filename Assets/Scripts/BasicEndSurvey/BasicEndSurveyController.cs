using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//TODO: Create button listeners via script instead of inspector.
// Controls the survey scene.
public class BasicEndSurveyController : MonoBehaviour {

    private const float COUNT_DOWN = 3f;

    public GameObject readAloud;
    public GameObject SPIN;
    public GameObject DASS;
    public Text question;
    public Text instructions;

    public BehavioralTracking tracker;

    private SPINEnum SPINState;
    private DASSEnum DASSState;

    private bool running = true;
    private bool readAloudRunning = true;
    private System.Diagnostics.Stopwatch reactionTimer;
    private System.Diagnostics.Stopwatch countDownTimer;

    private List<SurveyAnswerData> data;

    // Initializes everything.
    void Start () {
        SPINState = SPINEnum.Question0;
        DASSState = DASSEnum.Question0;

        DatabaseInserts.CreateSession(Constants.SURVEYS_SCENEID);

        reactionTimer = System.Diagnostics.Stopwatch.StartNew();

        data = new List<SurveyAnswerData>();
    }

    // Once all surveys are finished, display a count down until the next scene starts.
    void Update()
    {
        if (!running)
        {
            question.text = "Finished surveys, starting WSAP in " + (COUNT_DOWN - countDownTimer.ElapsedMilliseconds / 1000f);
        }
    }

    // Represents a ReadAloud button click. On click, inserts the associated answer into the database and starts the SPIN survey.
    public void ReadAloudClick()
    {
        if (!readAloudRunning)
        {
            return;
        }

        Debug.Log("Yes");
        Debug.Log(reactionTimer.ElapsedMilliseconds / 1000.0 + " seconds");

        data.Add(new SurveyAnswerData(Constants.SPOKE_ALOUD_TABLE, question.text, "True", reactionTimer.ElapsedMilliseconds / 1000.0));
        readAloudRunning = false;

        question.text = "~~~Starting SPIN Survey~~~";
        Invoke("StartSPIN", 3f);
    }

    // Represents a ReadAloud button click. On click, inserts the associated answer into the database and starts the SPIN survey.
    public void MixOfBothClick()
    {
        if (!readAloudRunning)
        {
            return;
        }

        Debug.Log("Mix");
        Debug.Log(reactionTimer.ElapsedMilliseconds / 1000.0 + " seconds");

        data.Add(new SurveyAnswerData(Constants.SPOKE_ALOUD_TABLE, question.text, "Mix", reactionTimer.ElapsedMilliseconds / 1000.0));
        readAloudRunning = false;

        question.text = "~~~Starting SPIN Survey~~~";
        Invoke("StartSPIN", 3f);
    }

    // Represents a ReadAloud button click. On click, inserts the associated answer into the database and starts the SPIN survey.
    public void DidntReadAloudClick()
    {
        if (!readAloudRunning)
        {
            return;
        }

        Debug.Log("No");
        Debug.Log(reactionTimer.ElapsedMilliseconds / 1000.0 + " seconds");

        data.Add(new SurveyAnswerData(Constants.SPOKE_ALOUD_TABLE, question.text, "False", reactionTimer.ElapsedMilliseconds / 1000.0));
        readAloudRunning = false;

        question.text = "~~~Starting SPIN Survey~~~";
        Invoke("StartSPIN", 3f);
    }

    // Starts the SPIN survey.
    private void StartSPIN()
    {
        instructions.gameObject.SetActive(true);
        instructions.text = "INSTRUCTIONS: Rate how much each statement applies to you over the past week";

        question.text = SPINState.GetQuestion();
        SPIN.SetActive(true);

        readAloud.SetActive(false);

        reactionTimer.Reset();
        reactionTimer.Start();
    }

    // Represents a SPIN button click. On click, inserts the associated answer into the database and calls SPINClick().
    public void SPINNotAtAll()
    {
        if (SPINState.Equals(SPINEnum.END))
        {
            return;
        }

        Debug.Log("Not at all");
        data.Add(new SurveyAnswerData(Constants.SPIN_TABLE, question.text, "Not at all", reactionTimer.ElapsedMilliseconds / 1000.0));

        SPINClick();
    }

    // Represents a SPIN button click. On click, inserts the associated answer into the database and calls SPINClick().
    public void SPINALittleBit()
    {
        if (SPINState.Equals(SPINEnum.END))
        {
            return;
        }

        Debug.Log("A little bit");
        data.Add(new SurveyAnswerData(Constants.SPIN_TABLE, question.text, "A little bit", reactionTimer.ElapsedMilliseconds / 1000.0));

        SPINClick();
    }

    // Represents a SPIN button click. On click, inserts the associated answer into the database and calls SPINClick().
    public void SPINSomewhat()
    {
        if (SPINState.Equals(SPINEnum.END))
        {
            return;
        }

        Debug.Log("Somewhat");
        data.Add(new SurveyAnswerData(Constants.SPIN_TABLE, question.text, "Somewhat", reactionTimer.ElapsedMilliseconds / 1000.0));

        SPINClick();
    }

    // Represents a SPIN button click. On click, inserts the associated answer into the database and calls SPINClick().
    public void SPINVeryMuch()
    {
        if (SPINState.Equals(SPINEnum.END))
        {
            return;
        }

        Debug.Log("Very Much");
        data.Add(new SurveyAnswerData(Constants.SPIN_TABLE, question.text, "Very Much", reactionTimer.ElapsedMilliseconds / 1000.0));

        SPINClick();
    }

    // Represents a SPIN button click. On click, inserts the associated answer into the database and calls SPINClick().
    public void SPINExtremely()
    {
        if (SPINState.Equals(SPINEnum.END))
        {
            return;
        }

        Debug.Log("Extremely");
        data.Add(new SurveyAnswerData(Constants.SPIN_TABLE, question.text, "Extremely", reactionTimer.ElapsedMilliseconds / 1000.0));

        SPINClick();
    }

    // Move through the SPIN survey until the end. Then switch to the DASS survey.
    private void SPINClick()
    {
        Debug.Log(reactionTimer.ElapsedMilliseconds / 1000.0 + " seconds");

        SPINState = SPINState.next();

        if (!SPINState.Equals(SPINEnum.END))
        {
            question.text = SPINState.GetQuestion();
            Debug.Log(SPINState.GetQuestion());

            reactionTimer.Reset();
            reactionTimer.Start();
        }
        else
        {
            question.text = "~~~Starting DASS Survey~~~";
            Invoke("StartDASS", 3f);
        }
    }

    // Switches control from SPIN to DASS.
    private void StartDASS()
    {
        instructions.text = @"INSTRUCTIONS: Please read each statement and choose the answer which indicates how much the statement applied to you over the past week. 
There are no right or wrong answers. Do not spend too much time on any statement.";

        SPIN.SetActive(false);
        DASS.SetActive(true);

        question.text = DASSState.GetQuestion();

        countDownTimer = new System.Diagnostics.Stopwatch();

        reactionTimer.Reset();
        reactionTimer.Start();
    }

    // Represents a DASS button click. On click, inserts the associated answer into the database and calls DASSClick().
    public void DASSNotAtAll()
    {
        if (DASSState.Equals(DASSEnum.END))
        {
            return;
        }

        Debug.Log("Did not apply at all");
        data.Add(new SurveyAnswerData(Constants.DASS_TABLE, question.text, "Did not apply at all", reactionTimer.ElapsedMilliseconds / 1000.0));

        DASSClick();
    }

    // Represents a DASS button click. On click, inserts the associated answer into the database and calls DASSClick().
    public void DASSSomeDegree()
    {
        if (DASSState.Equals(DASSEnum.END))
        {
            return;
        }

        Debug.Log("Applied to me to some degree, or some of the time");
        data.Add(new SurveyAnswerData(Constants.DASS_TABLE, question.text, "Applied to me to some degree, or some of the time", reactionTimer.ElapsedMilliseconds / 1000.0));

        DASSClick();
    }

    // Represents a DASS button click. On click, inserts the associated answer into the database and calls DASSClick().
    public void DASSSomeTimes()
    {
        if (DASSState.Equals(DASSEnum.END))
        {
            return;
        }

        Debug.Log("Applied to me a considerable degree, or a good part of the time");
        data.Add(new SurveyAnswerData(Constants.DASS_TABLE, question.text, "Applied to me a considerable degree, or a good part of the time", reactionTimer.ElapsedMilliseconds / 1000.0));

        DASSClick();
    }

    // Represents a DASS button click. On click, inserts the associated answer into the database and calls DASSClick().
    public void DASSVeryMuch()
    {
        if (DASSState.Equals(DASSEnum.END))
        {
            return;
        }

        Debug.Log("Applied to me very much, or most of the time");
        data.Add(new SurveyAnswerData(Constants.DASS_TABLE, question.text, "Applied to me very much, or most of the time", reactionTimer.ElapsedMilliseconds / 1000.0));

        DASSClick();
    }

    // Move through the DASS survey until the end. Then start WSAP.
    private void DASSClick()
    {
        Debug.Log(reactionTimer.ElapsedMilliseconds / 1000.0 + " seconds");

        DASSState = DASSState.next();

        if (!DASSState.Equals(DASSEnum.END))
        {
            question.text = DASSState.GetQuestion();
            Debug.Log(DASSState.GetQuestion());

            reactionTimer.Reset();
            reactionTimer.Start();
        }
        else
        {
            Invoke("NextScene", COUNT_DOWN);

            running = false;
            countDownTimer.Start();
        }
    }

    // Loads the WSAP scene.
    private void NextScene()
    {
        InsertAllData();
        tracker.InsertData();
        SceneManager.LoadScene("WSAP", LoadSceneMode.Single);
    }

    // Represents a survey answer.
    private class SurveyAnswerData
    {
        private string table;
        private string question;
        private string answer;
        private DateTime timeStamp;
        private double reactionTime;

        public SurveyAnswerData(string table, string question, string answer, double reactionTime)
        {
            this.table = table;
            this.question = question;
            this.answer = answer;
            timeStamp = DateTime.Now;
            this.reactionTime = reactionTime;
        }

        public string GetTable
        {
            get
            {
                return table;
            }
        }

        public string GetQuestion
        {
            get
            {
                return question;
            }
        }

        public string GetAnswer
        {
            get
            {
                return answer;
            }
        }

        public DateTime GetTimeStamp
        {
            get
            {
                return timeStamp;
            }
        }

        public double GetReactionTime
        {
            get
            {
                return reactionTime;
            }
        }
    }

    // Inserts all SurveyAnswerData in Data into the database.
    private void InsertAllData()
    {
        string connectionString = Constants.CONNECTION_STRING;
        Debug.Log("~~~Survey answer insert open~~~");
        using (MySqlConnection dbcon = new MySqlConnection(connectionString))
        {
            dbcon.Open();

            foreach (SurveyAnswerData oneTimeData in data)
            {
                using (MySqlCommand dbcmd = dbcon.CreateCommand())
                {
                    dbcmd.CommandText = "INSERT INTO " + oneTimeData.GetTable + " VALUES(?sessionId, ?question, ?answer, ?timeStamp, ?reactionTime)";
                    dbcmd.Parameters.Add("?sessionId", PlayerState.SessionId);
                    dbcmd.Parameters.Add("?question", oneTimeData.GetQuestion);
                    dbcmd.Parameters.Add("?answer", oneTimeData.GetAnswer);
                    dbcmd.Parameters.Add("?timeStamp", oneTimeData.GetTimeStamp);
                    dbcmd.Parameters.Add("?reactionTime", oneTimeData.GetReactionTime);
                    dbcmd.ExecuteNonQuery();
                    dbcmd.Parameters.Clear();
                }
            }
        }
    }
}
