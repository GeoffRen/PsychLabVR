using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Controls the WSAP scene.
public class WordSentenceAssociationParadigm : MonoBehaviour {

    public GameObject debrief;

    public BehavioralTracking tracker;
    private static bool trackerInserted;

    private const float FOCUS_LENGTH = .5f;
    private const float WORD_LENGTH = 1f;
    private const string FOCUS = "+";

    private Valve.VR.EVRButtonId triggerButton =
        Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    public SteamVR_Controller.Device controller
    { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    private SteamVR_TrackedObject trackedObj;

    private static Text mText;
    private static Stack<WSAPEnum> states;
    private static bool createdSession;
    private static bool modifiable;
    private static bool running;
    private static System.Diagnostics.Stopwatch reactionTimer;

    public GameObject screen;

    private static List<WSAPAnswerData> data;

    void Awake()
    {
        createdSession = false;
    }

    // Initializes everything
    void Start()
    {
        trackerInserted = false;

        trackedObj = GetComponent<SteamVR_TrackedObject>();

        mText = screen.GetComponent<Text>();
        modifiable = false;
        running = false;

        // Creates a stack containing all of the WSAP states and then shuffles the stack.
        states = new Stack<WSAPEnum>();
        for (var WSAPState = WSAPEnum.Trial0; WSAPState != WSAPEnum.END; WSAPState = WSAPState.next())
        {
            states.Push(WSAPState);
        }

        var rnd = new System.Random();
        states = new Stack<WSAPEnum>(states.OrderBy(x => rnd.Next()));

        reactionTimer = System.Diagnostics.Stopwatch.StartNew();

        // This is so only one session is created upon a WSAP scene.
        if (!createdSession)
        {
            DatabaseInserts.CreateSession(Constants.WSAP_SCENEID);
            createdSession = true;
        }

        data = new List<WSAPAnswerData>();
    }
	
    // Controls the WSAP states.
	void Update () {

        // Starts the trials initially. Can only be activated at the beginning.
        if (controller.GetPressDown(triggerButton) && !running)
        {
            Debug.Log("STARTING TRIALS");

            running = true;
            mText.text = FOCUS;
            Invoke("SetText", FOCUS_LENGTH);
        }

        // Controls what happens when the user presses the trigger while the trials are running. Can only be activated when the sentence is displayed.
        // This will advance the state, insert the answer into the database, and display the FOCUS.
        if (controller.GetPressDown(triggerButton) && modifiable && running)
        {
            Debug.Log(states.Peek() + " FINISHED");

            modifiable = false;

            var curState = states.Pop();

            if (controller.index == SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost))
            {
                Debug.Log("User input: false in " + reactionTimer.ElapsedMilliseconds / 1000.0 + " seconds");
                Debug.Log("Word association: " + curState.GetPositive());
                data.Add(new WSAPAnswerData(curState.GetSentence(), curState.GetWord(), curState.GetPositive(), false, reactionTimer.ElapsedMilliseconds / 1000.0));
            }
            else
            {
                Debug.Log("User input: true " + reactionTimer.ElapsedMilliseconds / 1000.0 + " seconds");
                Debug.Log("Word association: " + curState.GetPositive());
                data.Add(new WSAPAnswerData(curState.GetSentence(), curState.GetWord(), curState.GetPositive(), true, reactionTimer.ElapsedMilliseconds / 1000.0));
            }
            
            if (states.Count == 0)
            {
                debrief.SetActive(true);

                if (!trackerInserted)
                {
                    trackerInserted = true;
                    InsertAllData();
                    tracker.InsertData();
                }

                return;
            }

            mText.text = FOCUS;
            Invoke("SetText", FOCUS_LENGTH);
        }
    }

    // Displays the word for WORD_LENTH time. 
    private void SetText()
    {
        mText.text = states.Peek().GetWord();
        Invoke("SetSentence", WORD_LENGTH);
    }

    // Displays the sentence and allows the user to input an answer.
    private void SetSentence()
    {
        mText.text = "Is the word related to the sentence?\n\n" + states.Peek().GetSentence() + "\n\nLeft: No\t\t\t\tRight: Yes";
        modifiable = true;

        reactionTimer.Reset();
        reactionTimer.Start();
    }

    // Represents a WSAP answer.
    private class WSAPAnswerData
    {
        private string question;
        private string word;
        private bool positive;
        private bool answer;
        private DateTime timeStamp;
        private double reactionTime;

        public WSAPAnswerData(string question, string word, bool positive, bool answer, double reactionTime)
        {
            this.question = question;
            this.word = word;
            this.positive = positive;
            this.answer = answer;
            timeStamp = DateTime.Now;
            this.reactionTime = reactionTime;
        }

        public string GetQuestion
        {
            get
            {
                return question;
            }
        }

        public string GetWord
        {
            get
            {
                return word;
            }
        }

        public bool GetPositive
        {
            get
            {
                return positive;
            }
        }

        public bool GetAnswer
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

    // Inserts all WSAPAnswerData in Data into the database.
    private void InsertAllData()
    {
        string connectionString = Constants.CONNECTION_STRING;
        Debug.Log("~~~WSAPAnswer insert open~~~");
        using (MySqlConnection dbcon = new MySqlConnection(connectionString))
        {
            dbcon.Open();

            foreach (WSAPAnswerData oneTimeData in data)
            {
                using (MySqlCommand dbcmd = dbcon.CreateCommand())
                {
                    dbcmd.CommandText = "INSERT INTO WSAPAnswer VALUES(?sessionId, ?question, ?word, ?positive, ?answer, ?timeStamp, ?reactionTime)";
                    dbcmd.Parameters.Add("?sessionId", PlayerState.SessionId);
                    dbcmd.Parameters.Add("?question", oneTimeData.GetQuestion);
                    dbcmd.Parameters.Add("?word", oneTimeData.GetWord);
                    dbcmd.Parameters.Add("?positive", oneTimeData.GetPositive);
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
