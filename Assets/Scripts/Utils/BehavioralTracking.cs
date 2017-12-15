using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using UnityEngine;

// Tracks what the user is looking at, where the user is, and where the user's controllers are for every second.
public class BehavioralTracking : MonoBehaviour {

    // Debugging on will turn off BehavioralTracking
    private bool debug = false;

    // Script is attached to the eyes so only a reference to the controllers is needed. 
    // Attached to the eyes so that raycasting can be done easily.
    public GameObject leftController;
    public GameObject rightController;

    private List<AllBehavioralData> data;

    // Inserts behavioral data every second.
	void Start () {
        if (!debug)
        {
            Debug.Log("Starting behavioral tracking");
            data = new List<AllBehavioralData>();
            InvokeRepeating("BehavioralTrackingAccumulator", 1f, 1f);
        }
	}
	
    // Draws a ray replicating the eye tracking.
	void FixedUpdate () {
        if (!debug)
        {
            Debug.DrawRay(transform.position, transform.forward * 25f, Color.green, .05f);
        }
    }

    // Adds tracking data to data.
    private void BehavioralTrackingAccumulator()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            data.Add(new AllBehavioralData(gameObject, leftController, rightController, hit.collider.gameObject));
            Debug.Log("Tracked data WITH vision: " + hit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("Tracked data WITHOUT vision");
            data.Add(new AllBehavioralData(gameObject, leftController, rightController));
        }
    }

    // Inserts all BehavioralData in data into the database.
    public void InsertData()
    {
        string connectionString = Constants.CONNECTION_STRING;
        Debug.Log("~~~Behavioral tracking insert open~~~");
        using (MySqlConnection dbcon = new MySqlConnection(connectionString))
        {
            dbcon.Open();

            foreach (AllBehavioralData oneTimeData in data)
            {
                using (MySqlCommand dbcmd = dbcon.CreateCommand())
                {
                    dbcmd.CommandText = "INSERT INTO Location VALUES(?sessionId, ?timeStamp, ?xPos, ?yPos, ?zPos)";
                    dbcmd.Parameters.Add("?sessionId", PlayerState.SessionId);
                    dbcmd.Parameters.Add("?timeStamp", oneTimeData.GetLocation.GetTimeStamp);
                    dbcmd.Parameters.Add("?xPos", oneTimeData.GetLocation.GetxPos);
                    dbcmd.Parameters.Add("?yPos", oneTimeData.GetLocation.GetyPos);
                    dbcmd.Parameters.Add("?zPos", oneTimeData.GetLocation.GetzPos);
                    dbcmd.ExecuteNonQuery();
                }

                using (MySqlCommand dbcmd = dbcon.CreateCommand())
                {
                    dbcmd.CommandText = "INSERT INTO LeftControllerLocation VALUES(?sessionId, ?timeStamp, ?xPos, ?yPos, ?zPos)";
                    dbcmd.Parameters.Add("?sessionId", PlayerState.SessionId);
                    dbcmd.Parameters.Add("?timeStamp", oneTimeData.GetLeftController.GetTimeStamp);
                    dbcmd.Parameters.Add("?xPos", oneTimeData.GetLeftController.GetxPos);
                    dbcmd.Parameters.Add("?yPos", oneTimeData.GetLeftController.GetyPos);
                    dbcmd.Parameters.Add("?zPos", oneTimeData.GetLeftController.GetzPos);
                    dbcmd.ExecuteNonQuery();
                    dbcmd.Parameters.Clear();
                }

                using (MySqlCommand dbcmd = dbcon.CreateCommand())
                {
                    dbcmd.CommandText = "INSERT INTO RightControllerLocation VALUES(?sessionId, ?timeStamp, ?xPos, ?yPos, ?zPos)";
                    dbcmd.Parameters.Add("?sessionId", PlayerState.SessionId);
                    dbcmd.Parameters.Add("?timeStamp", oneTimeData.GetRightController.GetTimeStamp);
                    dbcmd.Parameters.Add("?xPos", oneTimeData.GetRightController.GetxPos);
                    dbcmd.Parameters.Add("?yPos", oneTimeData.GetRightController.GetyPos);
                    dbcmd.Parameters.Add("?zPos", oneTimeData.GetRightController.GetzPos);
                    dbcmd.ExecuteNonQuery();
                    dbcmd.Parameters.Clear();
                }

                if (oneTimeData.GetVision != null)
                {
                    using (MySqlCommand dbcmd = dbcon.CreateCommand())
                    {
                        dbcmd.CommandText = "INSERT INTO Vision VALUES(?sessionId, ?timeStamp, ?object, ?xPos, ?yPos, ?zPos)";
                        dbcmd.Parameters.Add("?sessionId", PlayerState.SessionId);
                        dbcmd.Parameters.Add("?timeStamp", oneTimeData.GetVision.GetTimeStamp);
                        dbcmd.Parameters.Add("?object", oneTimeData.GetVision.GetVisionObj);
                        dbcmd.Parameters.Add("?xPos", oneTimeData.GetVision.GetxPos);
                        dbcmd.Parameters.Add("?yPos", oneTimeData.GetVision.GetyPos);
                        dbcmd.Parameters.Add("?zPos", oneTimeData.GetVision.GetzPos);
                        dbcmd.ExecuteNonQuery();
                        dbcmd.Parameters.Clear();
                    }
                }
            }
        }
    }

    // Represents all tracked data per time step.
    private class AllBehavioralData
    {
        private BehavioralData location;
        private BehavioralData leftController;
        private BehavioralData rightController;
        private BehavioralData vision;

        public AllBehavioralData(GameObject location, GameObject leftController, GameObject rightController, GameObject vision = null)
        {
            this.location = new BehavioralData(location);
            this.leftController = new BehavioralData(leftController);
            this.rightController = new BehavioralData(rightController);
            this.vision = vision ? new BehavioralData(vision, true) : null;
        }

        public BehavioralData GetLocation
        {
            get
            {
                return location;
            }
        }

        public BehavioralData GetLeftController
        {
            get
            {
                return leftController;
            }
        }

        public BehavioralData GetRightController
        {
            get
            {
                return rightController;
            }
        }

        public BehavioralData GetVision
        {
            get
            {
                return vision;
            }
        }
    }

    // Represents one type of behavioral data at a time step.
    private class BehavioralData
    {
        private DateTime timeStamp;
        private float xPos;
        private float yPos;
        private float zPos;
        private string visionObjName;

        public BehavioralData(GameObject trackedObj, bool isVision = false)
        {
            timeStamp = DateTime.Now;
            xPos = trackedObj.transform.position.x;
            yPos = trackedObj.transform.position.y;
            zPos = trackedObj.transform.position.z;
            visionObjName = isVision ? trackedObj.name : null;
        }

        public DateTime GetTimeStamp
        {
            get
            {
                return timeStamp;
            }
        }

        public float GetxPos
        {
            get
            {
                return xPos;
            }
        }

        public float GetyPos
        {
            get
            {
                return yPos;
            }
        }

        public float GetzPos
        {
            get
            {
                return zPos;
            }
        }

        public string GetVisionObj
        {
            get
            {
                return visionObjName;
            }
        }
    }
}
