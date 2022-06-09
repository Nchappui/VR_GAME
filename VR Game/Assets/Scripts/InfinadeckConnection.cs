using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InfinadeckConnection : MonoBehaviour
{

    public string m_InfinadeckServerIPAddress = "localhost"; //"192.168.21.101";
    public int m_InfinadeckServerPort = 2500;
    public bool m_EnableConnection;

    private Thread clientThread;
    private TcpClient clientSocket;
    public SharedMemory memory;
    
    public long xCenter = 0L;
    public long yCenter = 0L;
    public long xTreadmillPos = 0L;
    public long yTreadmillPos = 0L;


    // Start is called before the first frame update
    void Start()
    {
        /* 1) Create shared memory
         * 2) Start client in its own thread
         * 3) Offer functions to set and retrieve data
         */

        //memory = new SharedMemory();
        if (m_EnableConnection)
        {
            clientThread = new Thread(ExchangeData);
            clientThread.Start();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (m_EnableConnection && clientThread == null)
        {
            clientThread = new Thread(ExchangeData);
            clientThread.Start();
        }
    }

    /**
     * Returns the treadmill's filtered speeds [XSpeed, YSpeed]
     */
    public float[] getFilteredSpeeds()
    {
        float[] ret = { memory.filteredXSpeed, memory.filteredYSpeed };
        return ret;
    }

    /**
     * In m/s, max 1.1
     */
    public void setSpeed(float x, float y)
    {
        memory.setX = x;
        memory.setY = y;
    }

    public float[] getRawSpeeds()
    {
        float[] ret = { memory.rawXSpeed, memory.rawYSpeed };
        return ret;
    }

    public long[] getDistancesFromCenter()
    {
        long[] ret = { memory.XdistanceFromCentre, memory.YdistanceFromCentre };
        return ret;
    }

    private void ResetThread()
    {
        clientThread.Join();
        clientThread = null;
        m_EnableConnection = false;
    }

    private void ExchangeData()
    {
        bool noError = true;
        while (m_EnableConnection)
        {
            try
            {
                clientSocket = new TcpClient(m_InfinadeckServerIPAddress, m_InfinadeckServerPort);
                noError = true;

                while (noError)
                {
                    noError = SendSpeeds(clientSocket.Client);
                    noError = noError && RecieveData(clientSocket.Client);
                    Thread.Sleep(9);
                }
            }
            catch (Exception e)
            {
                print("Could not connect to " + clientSocket.ToString());
            }
            Debug.Log("Connection to Server closed");
            clientSocket.Close();
        }
        Invoke("ResetThread", 0.0f);
    }

    private bool SendSpeeds(Socket s)
    {
        string toSend = string.Format("{0,6:+00.00;-00.00;+00.00}{1,6:+00.00;-00.00;+00.00}", memory.setX, memory.setY);
        byte[] buffer = Encoding.ASCII.GetBytes(toSend);

        try
        {
            s.Send(buffer);
            return true;
        }
        catch(Exception e)
        {
            print(e);
            return false;
        }

    }

    private bool RecieveData(Socket s)
    {
        byte[] buffer = new byte[1024];

        try
        {
            s.Receive(buffer);
            string recieved = Encoding.ASCII.GetString(buffer);
            memory.filteredXSpeed = float.Parse(recieved.Substring(0, 5));
            memory.filteredYSpeed = float.Parse(recieved.Substring(5, 5));

            //bugfix 'Right' button going left by commenting the following 2 lines
            //memory.filteredYSpeed = float.Parse(recieved.Substring(10, 5));
            //memory.filteredYSpeed = float.Parse(recieved.Substring(15, 5));

            xTreadmillPos = long.Parse(recieved.Substring(20, 11));
            yTreadmillPos = long.Parse(recieved.Substring(31, 11));

            memory.XdistanceFromCentre = xTreadmillPos - xCenter;//m_centerPosition[0];
            memory.YdistanceFromCentre = yTreadmillPos - yCenter;// m_centerPosition[1];
            return true;
        }
        catch (Exception e)
        {
            print(e);
            return false;
        }
    }

    public void reset_origin()
    {
        xCenter = xTreadmillPos;
        yCenter = yTreadmillPos;
    }


    [Serializable]
    public class SharedMemory
    {
        [Range(-1.1f, 1.1f)]
        // Commands to send to Infinadeck
        public float setX = 0;

        [Range(-1.1f, 1.1f)]
        public float setY = 0;

        // Commands to retrieve Infinadeck data
        public float filteredXSpeed = 0;

        public float filteredYSpeed = 0;

        public float rawXSpeed = 0;

        public float rawYSpeed = 0;

        public long XdistanceFromCentre = 0;

        public long YdistanceFromCentre = 0;
    }

}



[CustomEditor(typeof(InfinadeckConnection))]
public class PhasespaceInterfaceEditor : Editor
{
    private InfinadeckConnection script;
    private void OnEnable() { script = target as InfinadeckConnection; }

    public float magnitude;

    public override void OnInspectorGUI()
    {

        if (GUILayout.Button("Reset origin")) { script.reset_origin(); }
        if (GUILayout.Button("Connect")) { script.m_EnableConnection = true; }
        if (GUILayout.Button("Disconnect")) { script.m_EnableConnection = false; }
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        {
            if (GUILayout.Button("Left")) { script.setSpeed(0, magnitude); }
            if (GUILayout.Button("Up")) { script.setSpeed(magnitude, 0); }
            if (GUILayout.Button("Down")) { script.setSpeed(-magnitude, 0); }
            if (GUILayout.Button("Right")) { script.setSpeed(0, -magnitude); }
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Stop")) { script.setSpeed(0, 0); }
        GUI.backgroundColor = Color.white;
        var centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.UpperCenter;
        GUILayout.Space(4);
        GUILayout.Label("Magnitude", centeredStyle);
        magnitude = GUILayout.HorizontalSlider(magnitude, 0f, 1.1f);
        GUILayout.Space(10);
        //ySpeed = 
        /*
        GUI.backgroundColor = Color.green;
        GUI.enabled = !PhaseSpace.is_connected;
        if (GUILayout.Button("Connect")) { script.connect(); }
        GUI.enabled = !PhaseSpace.is_pooling && PhaseSpace.is_connected;
        if (GUILayout.Button("Start Pooling Data")) { script.start_pooling(); }
        GUI.backgroundColor = Color.red;
        GUI.enabled = PhaseSpace.is_pooling;
        if (GUILayout.Button("Stop Pooling Data")) { script.stop_pooling(); }
        GUI.enabled = PhaseSpace.is_connected && !PhaseSpace.is_pooling;
        if (GUILayout.Button("Disconnect")) { script.disconnect(); }
        GUI.enabled = true;
        if (GUILayout.Button("Force Disconnect")) { script.disconnect(); }
        GUILayout.Space(8);
        GUI.backgroundColor = Color.white;

        GUI.enabled = PhaseSpace.is_pooling;
        GUILayout.Label("Calibrate Markers Referential");
        selected_tracker_id = EditorGUILayout.Popup("LED use for calibration", selected_tracker_id, script.led_labels != null ? script.led_labels : null_list);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        {
            if (GUILayout.Button("Origin")) { script.calibrate_markers_origin((uint)selected_tracker_id); }
            if (GUILayout.Button("Right")) { script.calibrate_markers_right((uint)selected_tracker_id); }
            if (GUILayout.Button("Front")) { script.calibrate_markers_front((uint)selected_tracker_id); }
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Calibrate")) { script.calibrate_markers(); }
        GUI.backgroundColor = Color.white;
        GUILayout.Space(8);
        */

        base.OnInspectorGUI();
    }
}