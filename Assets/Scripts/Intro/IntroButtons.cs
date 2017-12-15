using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Controls the Intro scene.
public class IntroButtons : MonoBehaviour {

    private const float COUNT_DOWN = 3f;

    public GameObject consentMenu;
    public GameObject demographicMenu;

    [SerializeField]
    public Dropdown[] dropDownMenus;

    private bool running = true;
    private System.Diagnostics.Stopwatch countDownTimer;

    // Initializes everything.
    void Start()
    {
        running = true;
        countDownTimer = new System.Diagnostics.Stopwatch();
    }

    // After the scene finishes, display a count down until the next scene starts.
    void Update()
    {
        if (!running)
        {
            demographicMenu.GetComponentInChildren<Text>().text = "Starting interaction paradigm in\n" + (COUNT_DOWN - countDownTimer.ElapsedMilliseconds / 1000f);
        }
    }

    // The Consent button press, advances to the Demographic survey.
    public void Consent()
    {
        consentMenu.SetActive(false);
        demographicMenu.SetActive(true);
    }

    // The Demographic survey button press, creates a new Subject according to the survey data and loads the next scene in COUNT_DOWN time.
    public void Complete()
    {
        if (running)
        {
            Debug.Log("~~~Complete~~~");
            foreach (Dropdown curDropDown in dropDownMenus)
            {
                Debug.Log(curDropDown.options[curDropDown.value].text);
            }

            CreateUser();

            Invoke("NextScene", COUNT_DOWN);

            running = false;
            countDownTimer.Start();
        }
    }

    // Loads the Basic - LS scene.
    private void NextScene()
    {
        SceneManager.LoadScene("Basic - LS", LoadSceneMode.Single);
    }

    // Creates a new Subject and saves that SubjectId in Utils.PlayerState.
    private void CreateUser()
    {
        string connectionString = Constants.CONNECTION_STRING;
        Debug.Log(connectionString);
        using (MySqlConnection dbcon = new MySqlConnection(connectionString))
        {
            dbcon.Open();
            Debug.Log("Opened database for Subject insert");

            // Transaction inserting a new SubjectId and then immediately returning what SubjectId was inserted.
            using (MySqlTransaction dbtrans = dbcon.BeginTransaction())
            {
                using (MySqlCommand dbcmd = dbcon.CreateCommand())
                {
                    dbcmd.CommandText = "INSERT INTO Subject VALUES(null, ?age, ?ethnicity, ?education, ?marital, ?employment)";
                    dbcmd.Parameters.Add("?age", dropDownMenus[0].options[dropDownMenus[0].value].text);
                    dbcmd.Parameters.Add("?ethnicity", dropDownMenus[1].options[dropDownMenus[1].value].text);
                    dbcmd.Parameters.Add("?education", dropDownMenus[2].options[dropDownMenus[2].value].text);
                    dbcmd.Parameters.Add("?marital", dropDownMenus[3].options[dropDownMenus[3].value].text);
                    dbcmd.Parameters.Add("?employment", dropDownMenus[4].options[dropDownMenus[4].value].text);
                    dbcmd.ExecuteNonQuery();
                    dbcmd.Parameters.Clear();
                }

                using (MySqlCommand dbcmd = dbcon.CreateCommand())
                {
                    dbcmd.CommandText = "SELECT SubjectId FROM Subject WHERE SubjectId NOT IN (SELECT S1.SubjectId FROM Subject S1, Subject S2 WHERE S1.SubjectId < S2.SubjectId)";
                    PlayerState.SubjectId = (int)dbcmd.ExecuteScalar();
                    Debug.Log("SubjectId: " + PlayerState.SubjectId);
                }

                dbtrans.Commit();
            }
        }
    }
}
