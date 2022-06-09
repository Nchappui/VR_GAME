using Delahaye;
using Delahaye.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TreadmillAlgorithm : MonoBehaviour
{
    public SteamVRInterface steamInterface;
    Camera player;

    [Range(0f, 5f)]
    public float SPEED_GAIN = 1f;

    private bool sendSpeed = false;
    private bool stop = false;
    public float INNERRADIUS = 0.15f;
    public float OUTERRADIUS = 1f;
    public Vector3 tracker_virtual_center_offset = Vector3.zero;
    public Vector3 virtual_centered_tracker_position = Vector3.zero;
    public Vector3 infinadeckCenter = Vector3.zero;
    

    // --- Initialization ---
    float k1 = 0.1f;
    float k2 = 0.6f;
    float k3 = 0.9f;
    float beta = 0.5f;
    float kref = 0.1f;
    float iterationDelay = 0.05f;


    Vector2 xref = new Vector2();
    DateTime startTime;

    Vector2 prevObsState = new Vector2();
    Vector2 prevUsrPos = new Vector2();
    Vector2 prevUsrAbsSpeed = new Vector2();
    Vector2 prevTreadmillSpeed = new Vector2();
    Vector2 prevWalkSpeedEstimate = new Vector2();
    DateTime prevTime;
    public int back_tracker_id = 0;
    public int head_tracker_id = 0;
    private bool calibrated;
    UnityEngine.Quaternion tracker_invert_rotation_at_calibration;

    SetOfTrackers trackers;
    TrackerData back_tracker_source;
    TrackerData head_tracker_source;
    Vector3 new_center_position;

    // Start is called before the first frame update
    void Start()
    {
        steamInterface = transform.parent.gameObject.GetComponent<SteamVRInterface>();
        player = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        startTime = DateTime.Now;
        prevTime = startTime;
        FindInfinadeckCenter();
        Infinadeck.InfinadeckInitError e = Infinadeck.InfinadeckInitError.InfinadeckInitError_None;
        Infinadeck.Infinadeck.InitConnection(ref e);
        if (e != Infinadeck.InfinadeckInitError.InfinadeckInitError_None) print(e);
        Infinadeck.Infinadeck.StartTreadmillManualControl();
        InvokeRepeating(nameof(RunAlgorithm), 1f, iterationDelay); // Peut etre que 0.01 est trop court ?
    }

    public void BindTracker()
    {
        trackers = steamInterface.get_trackers_positions();
        string label = steamInterface.get_tracker_id((uint)back_tracker_id);
        back_tracker_source = trackers.at(label);
        label = steamInterface.get_tracker_id((uint)head_tracker_id);
        head_tracker_source = trackers.at(label);
    }

    // Update is called once per frame
    void Update()
    {


        if (tracker_invert_rotation_at_calibration == null) return;
        if (calibrated)
        {
            new_center_position = back_tracker_source.position.as_unity_vector3() + (back_tracker_source.rotation.as_unity_quaternion() * tracker_invert_rotation_at_calibration) * tracker_virtual_center_offset;

            Debug.DrawLine(virtual_centered_tracker_position, new_center_position, Color.green);
            Debug.DrawLine(back_tracker_source.position.as_unity_vector3(), new_center_position, Color.red);
        }
        
        //print("Connection state = " + Infinadeck.Infinadeck.CheckConnection());
        //print("Speeds = " + Infinadeck.Infinadeck.GetFloorSpeeds().v0 +""+Infinadeck.Infinadeck.GetFloorSpeeds().v1);
    }

    private void FindInfinadeckCenter()
    {
        Vector3 frontReferential = (steamInterface.frontPosition - steamInterface.originPosition);
        Vector3 rightReferential = (steamInterface.rightPosition - steamInterface.originPosition);
        infinadeckCenter = steamInterface.transform.position + (-frontReferential + rightReferential) / 2;
        infinadeckCenter.y = 0;
    }

    public Vector2 InfinadeckDifferenceToCenter()
    {
        //print(tracker_invert_rotation_at_calibration);
        if (tracker_invert_rotation_at_calibration == null) return new Vector2(0,0);

        var tracker_displacement_from_center = new_center_position - virtual_centered_tracker_position;

        print("Tracker distance from center: " + (tracker_displacement_from_center).ToString() + " new center = " + new_center_position + " virtual_pos = " + virtual_centered_tracker_position);

        return new Vector2(-tracker_displacement_from_center.x, -tracker_displacement_from_center.z);

    }

    public void CalibrateTracker()
    {
        BindTracker();
        //new calibration
        virtual_centered_tracker_position = (back_tracker_source.position + head_tracker_source.position).as_unity_vector3() / 2;
        tracker_virtual_center_offset = virtual_centered_tracker_position - back_tracker_source.position.as_unity_vector3();
        tracker_invert_rotation_at_calibration = back_tracker_source.rotation.invert.as_unity_quaternion();

        if (!calibrated)
        {
            Infinadeck.Infinadeck.StartTreadmillManualControl();

            InvokeRepeating(nameof(RunAlgorithm), 1f, iterationDelay); // Peut etre que 0.01 est trop court ?
        }
        


        //old calibration
        /*
        tracker_origin = back_tracker_source.position.as_unity_vector3();
        tracker_origin = (back_tracker_source.position + head_tracker_source.position).as_unity_vector3() / 2;
        back_to_centre_offset = (head_tracker_source.position - back_tracker_source.position).as_unity_vector3() / 2;
        */

    }

    public void RunAlgorithm()
    {
        if (back_tracker_source == null) return;
        //Thread.Sleep(1000 * iterationDelay); //c# sleep in ms, python sleep in s


        //The main loop of the algorithm which updates the different values to keep the user centered
        // --- Iteration ---

        DateTime currentTime = DateTime.Now;
        float deltaTime = (float)(currentTime - prevTime).TotalMilliseconds;
        float[] speeds = new float[] { (float) Infinadeck.Infinadeck.GetFloorSpeeds().v0, (float)Infinadeck.Infinadeck.GetFloorSpeeds().v1 };//infinadeck.getFilteredSpeeds();
        Vector2 treadmillSpeed = new Vector2(speeds[0], speeds[1]);//	GetTreadmillSpeed(orderedTreadmillSpeed, prevTreadmillSpeed);
        //print("Recieved speeds : " + treadmillSpeed);

        Vector2 usrPos = InfinadeckDifferenceToCenter();//Get_mouse_relative_pos_from_center_inv_y() * 2; //max pos in both axis is 2 
                                                        //Vector2 obsState = Calculate_new_obs_state_simple(beta, deltaTime, prevObsState, prevTreadmillSpeed, prevUsrPos);

        //Corrections using observations : 
        usrPos *= -1.5f / 1.1f;
        float temp = usrPos.y;
        usrPos.y = -1 * usrPos.x;
        usrPos.x = 1 * temp;

        Vector3 footOffset = GetComponentInParent<FootSwingAnalysis>().OffsetVector();
        usrPos.x += footOffset.z;
        usrPos.y += footOffset.x;

        Vector2 obsState = Calculate_new_obs_state(prevObsState, deltaTime, beta, prevTreadmillSpeed, treadmillSpeed, prevUsrPos);
        Vector2 walkSpeedEstimate = new Vector2(beta * (usrPos.x - obsState.x), beta * (usrPos.y - obsState.y));
        //walkSpeedEstimate += getWalkSpeedVariations(currentTime - startTime, prevWalkSpeedEstimate)

        Vector2 usrAbsSpeed = Calculate_new_abs_speed(deltaTime, prevWalkSpeedEstimate, prevTreadmillSpeed, treadmillSpeed);
        //Vector2 usrAbsSpeed = Calculate_new_abs_speed_simple(treadmillSpeed, walkSpeedEstimate);
        Vector2 usrAbsAcc = Calculate_new_abs_acc(deltaTime, prevUsrAbsSpeed, usrAbsSpeed);

        float newTargetSpeedX = -(k1 * (usrAbsAcc.x) + (k2 - 1) * usrAbsSpeed.x + k3 * (usrPos.x - xref.x) + walkSpeedEstimate.x);
        float newTargetSpeedY = -(k1 * (usrAbsAcc.y) + (k2 - 1) * usrAbsSpeed.y + k3 * (usrPos.y - xref.y) + walkSpeedEstimate.y);


        //orderedTreadmillSpeed = [newTargetSpeedX, newTargetSpeedY];
        // =====> SUPPOSE WE GIVE ORDER TO THE TREADMILL HERE <======
        print("Speed sent : oldxSpeed = " + newTargetSpeedX + ", oldySpeed = " + newTargetSpeedY + " | userPos = " + usrPos);
        if (newTargetSpeedX > OUTERRADIUS) newTargetSpeedX = 1f;
        if (newTargetSpeedY > OUTERRADIUS) newTargetSpeedY = 1f;
        if (new Vector2(newTargetSpeedX, newTargetSpeedY).magnitude < INNERRADIUS)
        {
            newTargetSpeedX = 0f;
            newTargetSpeedY = 0f;
        }
        if (sendSpeed)
        {
            if (stop) Infinadeck.Infinadeck.SetManualSpeeds(0, 0);//infinadeck.setSpeed(0, 0);
            else
            {
                newTargetSpeedX = newTargetSpeedX * SPEED_GAIN > 0.9d ? 0.9f : newTargetSpeedX * SPEED_GAIN;
                newTargetSpeedY = newTargetSpeedY * SPEED_GAIN > 0.9d ? 0.9f : newTargetSpeedY * SPEED_GAIN;
                Infinadeck.Infinadeck.SetManualSpeeds(newTargetSpeedX, newTargetSpeedY);
                print("Speed sent : newxSpeed = " + newTargetSpeedX + ", newySpeed = " + newTargetSpeedY + " | userPos = " + usrPos);

            }



        }


        //float x_ref = calculate_new_ref_pos(kref, walkSpeedEstimate);

        // Update the values for next iteration
        prevObsState = obsState;
        prevUsrPos = usrPos;
        prevUsrAbsSpeed = usrAbsSpeed;
        prevTreadmillSpeed = treadmillSpeed;
        prevWalkSpeedEstimate = walkSpeedEstimate;
        prevTime = currentTime;

        // wait since treadmill hardware can't respond as fast as software
        //Thread.Sleep(1000 * iterationDelay);


    }

    public Vector2 Calculate_new_obs_state(Vector2 prevState, float deltaTime, float beta, Vector2 prevTreadmillSpeed, Vector2 treadmillSpeed, Vector2 prevUsrPos)
    {
        //Use Runge-Kutta to obtain new internal state
        Vector2 x_t = beta * (-prevState + prevUsrPos);
        Vector2 interpolatedTrSpeed = (treadmillSpeed + prevTreadmillSpeed) / 2;

        Vector2 a_t = deltaTime * (x_t - prevTreadmillSpeed);
        Vector2 b_t = deltaTime * (x_t + 1 / 2 * a_t - interpolatedTrSpeed);
        Vector2 c_t = deltaTime * (x_t + 1 / 2 * b_t - interpolatedTrSpeed);
        Vector2 d_t = deltaTime * (x_t + c_t - treadmillSpeed);

        Vector2 new_state = x_t + 1 / 6 * (a_t + 2 * b_t + 2 * c_t + d_t);
        return new_state;
    }

    public Vector2 Calculate_new_obs_state_simple(float beta, float deltaTime, Vector2 prevObsState, Vector2 prevTreadmillSpeed, Vector2 prevUsrPos)
    {
        return prevObsState + new Vector2(deltaTime * beta * (-prevObsState - prevTreadmillSpeed + prevUsrPos).x, deltaTime * beta * (-prevObsState - prevTreadmillSpeed + prevUsrPos).y);
    }
    public Vector2 Calculate_new_abs_speed(float deltaTime, Vector2 prevWalkSpeed, Vector2 prevTreadmillSpeed, Vector2 treadmillSpeed)
    {
        //Use Runge-Kutta to obtain new internal state
        Vector2 x_t = prevWalkSpeed;
        Vector2 interpolatedTrSpeed = (treadmillSpeed + prevTreadmillSpeed) / 2;

        Vector2 a_t = deltaTime * (x_t - prevTreadmillSpeed);
        Vector2 b_t = deltaTime * (x_t + 1 / 2 * a_t - interpolatedTrSpeed);
        Vector2 c_t = deltaTime * (x_t + 1 / 2 * b_t - interpolatedTrSpeed);
        Vector2 d_t = deltaTime * (x_t + c_t - treadmillSpeed);

        Vector2 new_abs_speed = x_t + 1 / 6 * (a_t + 2 * b_t + 2 * c_t + d_t);
        return new_abs_speed;
    }
    public Vector2 Calculate_new_abs_speed_simple(Vector2 treadmillSpeed, Vector2 walkSpeedEstimate)
    {
        return walkSpeedEstimate - treadmillSpeed;
    }

    public Vector2 Calculate_new_abs_acc(float deltaTime, Vector2 prevUsrAbsSpeed, Vector2 usrAbsSpeed)
    {
        Vector2 usrAbsAcc = (usrAbsSpeed - prevUsrAbsSpeed) / (deltaTime);
        return usrAbsAcc;
    }
    /*
	public float Calculate_new_ref_pos(float kref, float walkSpeed)
	{
		return kref * Math.Atan(walkSpeed);
	}
	*/

    [CustomEditor(typeof(TreadmillAlgorithm))]
    public class PhasespaceInterfaceEditor : Editor
    {

        private TreadmillAlgorithm script;
        private void OnEnable() { script = target as TreadmillAlgorithm; }

        static string[] default_labels = new string[0];

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Tracker used");
            if (script.steamInterface != null) script.head_tracker_id = EditorGUILayout.Popup("Headset", script.head_tracker_id, script.steamInterface.get_tracker_labels() != null ? script.steamInterface.get_tracker_labels() : default_labels);

            if (script.steamInterface != null) script.back_tracker_id = EditorGUILayout.Popup("Tracker use", script.back_tracker_id, script.steamInterface.get_tracker_labels() != null ? script.steamInterface.get_tracker_labels() : default_labels);

            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Configurate tracker"))
            {
                script.CalibrateTracker(); //do tracker configuration here
                script.calibrated = true;
            }

            GUI.backgroundColor = Color.white;

            GUI.enabled = script.calibrated;


            if (GUILayout.Button("Send speeds")) { script.sendSpeed = true; }
            if (GUILayout.Button("Stop sending")) { script.sendSpeed = false; }

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Stop")) { script.stop = true; }
            if (GUILayout.Button("Reset stop")) { script.stop = false; }

            GUI.backgroundColor = Color.white;


            base.OnInspectorGUI();
        }
    }
}
