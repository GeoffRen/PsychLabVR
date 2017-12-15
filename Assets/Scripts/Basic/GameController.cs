using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Controls the Basic scene.
public class GameController : MonoBehaviour {

    private const float COUNT_DOWN = 3f;
    private const float ACTIVATION_DISTANCE = 4f; 

    public GameObject player;
    public GameObject markNPC;
    public GameObject johnNPC;

    private Animator curAnimator;

    public GameObject prompt;
    public GameObject questions;
    public GameObject directions;

    public BehavioralTracking tracker;

    [SerializeField]
    public AudioSource[] audioSources;

    private int NPCIndex;
    private List<NPCState> NPCStates;
    private BasicQuestionsEnum questionState;
    private BasicPromptEnum promptState;

    private bool markPlay = true;
    private bool johnPlay = false;

    private int stateIndex;
    private List<string> stateController;

    private System.Diagnostics.Stopwatch reactionTimer;

    private bool running = true;
    private System.Diagnostics.Stopwatch countDownTimer;

    private List<BasicAnswerData> data;

    // Create a list of NPCStates, one for each audio source. Audio delays aren't implemented yet, standard of 1f for all. Every trigger plays on "Next".
    // Initialize the initial question and prompt states as well as the stateController. stateController is hardcoded.
    void Start()
    {
        reactionTimer = System.Diagnostics.Stopwatch.StartNew();

        curAnimator = markNPC.GetComponent<Animator>();

        NPCIndex = 0;
        NPCStates = new List<NPCState>();
        foreach (var audio in audioSources)
        {
            NPCStates.Add(new NPCState(audio, "Next", 1f));
        }

        questionState = BasicQuestionsEnum.Question0;
        promptState = BasicPromptEnum.Prompt0;

        // The sequence that the states will progress in. questionState and promptState will be changing too so this just controls whether a prompt or question will appear.
        stateIndex = 0;
        stateController = new List<string> { "Prompt", "Question", "Prompt", "Question", "Prompt", "Question", "Prompt", "Prompt", "Question", "Prompt", "Question", "Prompt", "Question" };

        Debug.Log("NPCStates size: " + NPCStates.Count);

        running = true;
        countDownTimer = new System.Diagnostics.Stopwatch();

        DatabaseInserts.CreateSession(Constants.BASIC_SCENEID);

        data = new List<BasicAnswerData>();
    }

    // If the user becomes close enough to either NPC, their trigger is set.
    void FixedUpdate()
    {
        if (markPlay)
        {
            var distance = Vector3.Distance(player.transform.position, markNPC.transform.position);
            curAnimator.SetFloat("Distance", distance);

            if (distance < ACTIVATION_DISTANCE)
            {
                Debug.Log("PLAYING ~~~MARK~~~ after " + reactionTimer.ElapsedMilliseconds / 1000.0 + " seconds");
                
                markPlay = false;

                NPCState curNPCState = NPCStates[NPCIndex];
                curNPCState.Audio.PlayDelayed(curNPCState.AudioDelay);
                NPCIndex++;

                PlayPrompt();
                stateIndex++;
            }
        }

        if (johnPlay) {
            var distance = Vector3.Distance(player.transform.position, johnNPC.transform.position);
            curAnimator.SetFloat("Distance", distance);

            if (distance < ACTIVATION_DISTANCE)
            {
                prompt.GetComponentInChildren<Text>().text = promptState.GetPrompt();
                prompt.transform.Find("Continue").gameObject.SetActive(true);
                johnPlay = false;
            }
        }
    }

    // Once all interactions are complete, display a count down timer until the next scene is played.
    void Update()
    {
        if (!running)
        {
            prompt.SetActive(true);
            prompt.gameObject.transform.Find("PromptInstructionText").gameObject.SetActive(false);
            prompt.transform.Find("Continue").gameObject.SetActive(false);
            prompt.GetComponentInChildren<Text>().text = "Starting surveys in\n" + (COUNT_DOWN - countDownTimer.ElapsedMilliseconds / 1000f);
        }
    }

    // Plays the current animation and audio associated with it an associated delay. Then advances the NPC state.
    private void PlayNPC()
    {
        NPCState curNPCState = NPCStates[NPCIndex];
        curAnimator.SetTrigger(curNPCState.AnimationTrigger);
        curNPCState.Audio.PlayDelayed(curNPCState.AudioDelay);
        NPCIndex++;
    }

    // Creates a question on the associated delay.
    private void PlayQuestion()
    {
        Debug.Log("Playing question with delay: " + questionState.GetDelay());
        Invoke("CreateQuestion", questionState.GetDelay());
    }

    // Creates a prompt on the assoicated delay.
    private void PlayPrompt()
    {
        Debug.Log("Playing prompt with delay: " + promptState.GetDelay());
        Invoke("CreatePrompt", promptState.GetDelay());
    }

    // Creates a question.
    private void CreateQuestion()
    {
        Debug.Log(questionState.GetSentence());
        Debug.Log(questionState.GetPositive());
        Debug.Log(questionState.GetNeutral());
        Debug.Log(questionState.GetNegative());
        questions.SetActive(true);
        questions.transform.Find("QuestionsText").gameObject.GetComponent<Text>().text = questionState.GetSentence();
        questions.transform.Find("Positive").GetComponentInChildren<Text>().text = questionState.GetPositive();
        questions.transform.Find("Neutral").GetComponentInChildren<Text>().text = questionState.GetNeutral();
        questions.transform.Find("Negative").GetComponentInChildren<Text>().text = questionState.GetNegative();

        reactionTimer.Reset();
        reactionTimer.Start();
    }

    // Creates a prompt.
    private void CreatePrompt()
    {
        Debug.Log(promptState.GetPrompt());
        prompt.SetActive(true);
        prompt.GetComponentInChildren<Text>().text = promptState.GetPrompt();

        // Show directions to walk to John upon control switch.
        if (promptState.Equals(BasicPromptEnum.Prompt4))
        {
            directions.SetActive(true);
            prompt.GetComponentInChildren<Text>().text = "Get closer to John";
            prompt.transform.Find("Continue").gameObject.SetActive(false);
        }
    }

    // The button press for a positive answer. It advances the question state and plays the next state.
    public void PositiveClick()
    {
        Debug.Log("Positive pressed in " + reactionTimer.ElapsedMilliseconds / 1000.0 + " seconds");
        data.Add(new BasicAnswerData(questionState.GetSentence(), questionState.GetPositive(), "Positive", reactionTimer.ElapsedMilliseconds / 1000.0));
        questions.SetActive(false);

        questionState = questionState.next();
        PlayNextState();
    }

    // The button press for a neutral answer. It advances the question state and plays the next state.
    public void NeutralClick()
    {
        Debug.Log("Neutral pressed in " + reactionTimer.ElapsedMilliseconds / 1000.0 + " seconds");
        data.Add(new BasicAnswerData(questionState.GetSentence(), questionState.GetNeutral(), "Neutral", reactionTimer.ElapsedMilliseconds / 1000.0));
        questions.SetActive(false);

        questionState = questionState.next();
        PlayNextState();
    }

    // The button press for a negative answer. It advances the question state and plays the next state.
    public void NegativeClick()
    {
        Debug.Log("Negative pressed in " + reactionTimer.ElapsedMilliseconds / 1000.0 + " seconds");
        data.Add(new BasicAnswerData(questionState.GetSentence(), questionState.GetNegative(), "Negative", reactionTimer.ElapsedMilliseconds / 1000.0));
        questions.SetActive(false);

        questionState = questionState.next();
        PlayNextState();
    }

    // The button press for a prompt. It advances the prompt state and plays the next state.
    // Prompt4 corresponds to the switch from talking to Mark to talking to John.
    public void PromptContinue()
    {
        // Only switch if the user is close to John.
        if (promptState.Equals(BasicPromptEnum.Prompt4))
        {
            directions.SetActive(false);
        }

        Debug.Log("Prompt continue pressed");
        prompt.SetActive(false);

        promptState = promptState.next();
        PlayNextState();

        // If the user hits continue at the switching point, switch control and move the menu and prompt next to John.
        if (promptState.Equals(BasicPromptEnum.Prompt4))
        {

            johnNPC.SetActive(true);

            curAnimator = johnNPC.GetComponent<Animator>();
            johnPlay = true;

            var newPos = prompt.transform.position;
            newPos.x = -1.5f;
            newPos.z = 25f;
            prompt.transform.position = newPos;

            newPos.y = questions.transform.position.y;
            questions.transform.position = newPos;
        }
    }

    // Plays the next state according to stateController. Also plays an NPC action according to the script.
    private void PlayNextState()
    {

        // After playing all states, start the next scene.
        if (stateIndex >= stateController.Count)
        {
            Invoke("NextScene", COUNT_DOWN);

            running = false;
            countDownTimer.Start();

            return;
        }

        // Deciding which state to play next.
        if (stateController[stateIndex].Equals("Prompt"))
        {
            PlayPrompt();
            if (promptState.GetDelay() != 0f)
            {
                Debug.Log("Playing NPC animation & audio");
                PlayNPC();
            }
        }
        else
        {
            PlayQuestion();
            if (questionState.GetDelay() != 0f)
            {
                Debug.Log("Playing NPC animation & audio");
                PlayNPC();
            }
        }

        stateIndex++;
    }

    // Loads the BasicEndSurvey scene.
    private void NextScene()
    {
        InsertAllData();
        tracker.InsertData();
        SceneManager.LoadScene("BasicEndSurvey", LoadSceneMode.Single);
    }

    // Inner class to represent an NPCState.
    // Should be represented as enum w/attributes but I wanted to play around with C# inner classes. 
    // Change this if there's time. Also don't even know if attributes can store an AudioSource.
    // Holds the NPC's audio, animation, and associated audio delay information.
    private class NPCState
    {
        private AudioSource audio;
        private string animationTrigger;
        private float audioDelay;

        public NPCState(AudioSource audio, string animationTrigger, float audioDelay)
        {
            this.audio = audio;
            this.animationTrigger = animationTrigger;
            this.audioDelay = audioDelay;
        }

        public AudioSource Audio
        {
            get
            {
                return audio;
            }
        }

        public string AnimationTrigger
        {
            get
            {
                return animationTrigger;
            }
        }

        public float AudioDelay
        {
            get
            {
                return audioDelay;
            }
        }
    }

    // Represents a BasicAnswer.
    private class BasicAnswerData
    {
        private string question;
        private string answer;
        private string answerType;
        private DateTime timeStamp;
        private double reactionTime;

        public BasicAnswerData(string question, string answer, string answerType, double reactionTime)
        {
            this.question = question;
            this.answer = answer;
            this.answerType = answerType;
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

        public string GetAnswer
        {
            get
            {
                return answer;
            }
        }

        public string GetAnswerType
        {
            get
            {
                return answerType;
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

    // Inserts all BasicAnswerData in Data into the database.
    private void InsertAllData()
    {
        string connectionString = Constants.CONNECTION_STRING;
        Debug.Log("~~~BasicAnswer insert open~~~");
        using (MySqlConnection dbcon = new MySqlConnection(connectionString))
        {
            dbcon.Open();

            foreach (BasicAnswerData oneTimeData in data)
            {
                using (MySqlCommand dbcmd = dbcon.CreateCommand())
                {
                    dbcmd.CommandText = "INSERT INTO BasicAnswer VALUES(?sessionId, ?question, ?answer, ?answerType, ?timeStamp, ?reactionTime)";
                    dbcmd.Parameters.Add("?sessionId", PlayerState.SessionId);
                    dbcmd.Parameters.Add("?question", oneTimeData.GetQuestion);
                    dbcmd.Parameters.Add("?answer", oneTimeData.GetAnswer);
                    dbcmd.Parameters.Add("?answerType", oneTimeData.GetAnswerType);
                    dbcmd.Parameters.Add("?timeStamp", oneTimeData.GetTimeStamp);
                    dbcmd.Parameters.Add("?reactionTime", oneTimeData.GetReactionTime);
                    dbcmd.ExecuteNonQuery();
                }
            }
        }
    }
}
