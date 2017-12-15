using System;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.UI;

// Inserts into the database that many different scenes can make use of.
public static class DatabaseInserts
{

    // Creates a new Session and saves that SessionId in Utils.PlayerState.
    public static void CreateSession(int sceneId)
    {
        string connectionString = Constants.CONNECTION_STRING;
        Debug.Log(connectionString);
        using (MySqlConnection dbcon = new MySqlConnection(connectionString))
        {
            dbcon.Open();
            Debug.Log("Opened database for Session insert");

            // Transaction inserting a new SessionId and then immediately returning what SessionId was inserted.
            using (MySqlTransaction dbtrans = dbcon.BeginTransaction())
            {
                using (MySqlCommand dbcmd = dbcon.CreateCommand())
                {
                    dbcmd.CommandText = "INSERT INTO Session VALUES(null, ?timeStamp, ?subjectId, ?sceneId)";
                    dbcmd.Parameters.Add("?timeStamp", DateTime.Now);
                    dbcmd.Parameters.Add("?subjectId", PlayerState.SubjectId);
                    dbcmd.Parameters.Add("?sceneId", sceneId);
                    dbcmd.ExecuteNonQuery();
                    dbcmd.Parameters.Clear();
                }

                using (MySqlCommand dbcmd = dbcon.CreateCommand())
                {
                    dbcmd.CommandText = "SELECT SessionId FROM Session WHERE SessionId NOT IN (SELECT S1.SessionId FROM Session S1, Session S2 WHERE S1.SessionId < S2.SessionId)";
                    PlayerState.SessionId = (int)dbcmd.ExecuteScalar();
                    Debug.Log("SessionId: " + PlayerState.SessionId);
                }

                dbtrans.Commit();
            }
        }
    }
}
