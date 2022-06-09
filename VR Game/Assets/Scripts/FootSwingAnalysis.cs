using Delahaye;
using Delahaye.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FootSwingAnalysis : MonoBehaviour
{
    public SteamVRInterface steamVRInterface;
    public double MINIMUM_STEP_HEIGHT = 0.06d;
    public double INITIAL_HEIGHT = 0;
    public double MINIMUM_HORIZONTAL_STEP = 10.0d;
    [Range(100, 900)]
    public double ACCELERATION_COEFFICIENT = 500;
    public double max_accel_x = 0;
    public double max_accel_z = 0;
    public double max_height;

    public double magnitude_threshold = 0.0005;
    private bool apply_acceleration_detection = false;
    private bool calibrated;

    int tracker_left_foot_id;
    int tracker_right_foot_id;

    TrackerData tracker_left_foot;
    TrackerData tracker_right_foot;

    public Vector3[] offsetArray;
    public int offsetPtr;
    public int offsetSize = 2;

    public uint window_size = 20; // number of updates to detect acceleration

    private uint ptr = 0;
    Vector3[] left_tracker_data;
    Vector3[] right_tracker_data;

    DateTime[] time_stamps;



    void stopRecording()
    {
        print("max accel x = " + max_accel_x + " max accel z = " + max_accel_z + " max height = " + max_height);
    }

    void startRecording()
    {
        max_accel_x = 0;
        max_accel_z = 0;
        max_height = 0;
    }

    public Vector3 OffsetVector()
    {
        if (apply_acceleration_detection) return offsetArray.Aggregate((v1,v2) => v1+v2)/offsetArray.Length;
        return Vector3.zero;

    }

    // Start is called before the first frame update
    void Start()
    {
        left_tracker_data = new Vector3[window_size];
        right_tracker_data = new Vector3[window_size];

        time_stamps = new DateTime[window_size];

        offsetArray = new Vector3[offsetSize];
        offsetPtr = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (ptr == window_size)
        {
            Check_accelerations();
            ptr = 0;
            //calculate_second_derivatives(filter_raw_values(values));
            //check vertical value of foot trackers && acceleration limit for 2nd derivatives

            //si un trackeur a une valeur de position verticale très differente entre debut et fin du pas, alors on check les valeurs horizontales
            //si un trackeur a une valeur d'acceleration horizontale très differente entre debut et fin du pas, alors on set la user position d'une maniere correspondante
        }
        else
        {
            if (calibrated)
            {
                left_tracker_data[ptr] = tracker_left_foot.position.as_unity_vector3();
                right_tracker_data[ptr] = tracker_right_foot.position.as_unity_vector3();
                time_stamps[ptr] = DateTime.Now;
                ptr++;
            }

        }
    }

    void Check_accelerations()
    {
        //attention, on doit encore traiter les accelerations due aux rotations sur-place qui ne devraient pas faire bouger le user


        List<Vector3> list = Enumerable.ToList(left_tracker_data);
        List<double> leftXValues = list.ConvertAll(new Converter<Vector3, double>(VectorToX));
        List<double> leftYValues = list.ConvertAll(new Converter<Vector3, double>(VectorToY));
        List<double> leftZValues = list.ConvertAll(new Converter<Vector3, double>(VectorToZ));
        list = Enumerable.ToList(right_tracker_data);
        List<double> rightXValues = list.ConvertAll(new Converter<Vector3, double>(VectorToX));
        List<double> rightYValues = list.ConvertAll(new Converter<Vector3, double>(VectorToY));
        List<double> rightZValues = list.ConvertAll(new Converter<Vector3, double>(VectorToZ));


        double[] filteredPositionsX = filter_raw_values(leftXValues.ToArray());
        double[] filteredPositionsZ = filter_raw_values(leftZValues.ToArray());
        double[] filteredPositionsYLeft = filter_raw_values(leftYValues.ToArray());
        double[] filteredPositionsYRight = filter_raw_values(rightYValues.ToArray());

        /*
        for(int i = 0; i < filteredPositionsX.Length-1; ++i)
        {
            
            print("forloop : position " + i + " = " + leftXValues[i]);
            print("forloop : filtered_" + i + " = " + filteredPositionsX[i]);
            
            print("forloop : time " + i + " = " + (time_stamps[i+1] - time_stamps[i]).TotalMilliseconds);
        }*/




        double[] accelerationsLeftX = calculate_second_derivatives(filteredPositionsX);
        double[] accelerationsLeftZ = calculate_second_derivatives(filteredPositionsZ);

        double[] accelerationsRightX = calculate_second_derivatives(filter_raw_values(rightXValues.ToArray()));
        double[] accelerationsRightZ = calculate_second_derivatives(filter_raw_values(rightZValues.ToArray()));

        Vector3 offset = new Vector3();
        double maxAcceleration = 0d;
        /*
        print("left x = " + accelerationsLeftX.Max());
        print("left z = " + accelerationsLeftZ.Max());
        print("right x = " + accelerationsRightX.Max());
        print("right z = " + accelerationsRightZ.Max());
        
        print("filter x = " + filteredPositionsX.Max());
        print("filter z = " + filteredPositionsZ.Max());
        */

        //Si au dessus de minimum height
        // trouve la magnitude de l'accel
        // Si superieur à limite alors : 
        // trouve les accelerations correpspondantes x, y 
        // modifier la position du user proportionnellement 

        double[] accelerationsMagnitudeLeft = new double[accelerationsLeftX.Length];
        double[] accelerationsMagnitudeRight = new double[accelerationsRightX.Length];

        double maxMagnitudeLeft = 0;
        double maxMagnitudeRight = 0;
        int maxMagnitudeIndexLeft = -1;
        int maxMagnitudeIndexRight = -1;

        for (int i = 0; i < accelerationsRightX.Length; ++i)
        {
            double magnitudeLeft = Math.Sqrt(accelerationsLeftX[i] * accelerationsLeftX[i] + accelerationsLeftZ[i] * accelerationsLeftZ[i]);
            double magnitudeRight = Math.Sqrt(accelerationsRightX[i] * accelerationsRightX[i] + accelerationsRightZ[i] * accelerationsRightZ[i]);

            if (magnitudeLeft > maxMagnitudeLeft)
            {
                maxMagnitudeLeft = magnitudeLeft;
                maxMagnitudeIndexLeft = i;
            }

            if (magnitudeRight > maxMagnitudeRight)
            {
                maxMagnitudeRight = magnitudeRight;
                maxMagnitudeIndexRight = i;
            }

            accelerationsMagnitudeLeft[i] = magnitudeLeft;
            accelerationsMagnitudeRight[i] = magnitudeRight;
        }


        if (maxMagnitudeLeft > magnitude_threshold)
        {
            print("if-statement 1");
            if (filteredPositionsYLeft.Max() > MINIMUM_STEP_HEIGHT)
            {
                print("if-statement 2");
                offset.x += (float)(accelerationsLeftX[maxMagnitudeIndexLeft] * ACCELERATION_COEFFICIENT * -1);
                offset.z += (float)(accelerationsLeftZ[maxMagnitudeIndexLeft] * ACCELERATION_COEFFICIENT);
            }
        }
        if (maxMagnitudeRight > magnitude_threshold)
        {
            print("if-statement 3");
            if (filteredPositionsYRight.Max() > MINIMUM_STEP_HEIGHT)
            {
                print("if-statement 4");
                offset.x += (float)(accelerationsRightX[maxMagnitudeIndexRight] * ACCELERATION_COEFFICIENT * -1);
                offset.z += (float)(accelerationsRightZ[maxMagnitudeIndexRight] * ACCELERATION_COEFFICIENT);
            }
        }

        print("Adjusted x offset = " + offset.x + ", Adjusted z offset = " + offset.z + ", current magnitude = " + maxMagnitudeLeft);

        //print("acceleration on max Magnitude : x = " + accelerationsRightX[maxMagnitudeIndexRight] 
        //   + " z = " + accelerationsRightZ[maxMagnitudeIndexRight] + " maxes : x = " + accelerationsRightX.Max() + " z = " + accelerationsRightZ.Max());

        offsetArray[offsetPtr] = offset;
        offsetPtr = (offsetPtr+1) % offsetSize;
        //offsetVector = offset;


        if (Math.Abs(accelerationsLeftX.Max()) > max_accel_x)
        {
            max_accel_x = accelerationsLeftX.Max();
            //print("new max acceleration : " + max_accel_x);
        }

        if (Math.Abs(accelerationsLeftZ.Max()) > max_accel_z)
        {
            max_accel_z = accelerationsLeftZ.Max();
            //print("new max acceleration : " + max_accel_z);
        }

        if (Math.Abs(rightYValues.Max()) > Math.Abs(max_height))
        {
            max_height = rightYValues.Max();
            //print("new max acceleration : " + max_height);
        }
    }


    public static double VectorToX(Vector3 v)
    {
        return v.x;
    }

    public static double VectorToY(Vector3 v)
    {
        return v.y;
    }

    public static double VectorToZ(Vector3 v)
    {
        return v.z;
    }

    public void BindTracker()
    {
        SetOfTrackers trackers = steamVRInterface.get_trackers_positions();
        string label = steamVRInterface.get_tracker_id((uint)tracker_left_foot_id);
        tracker_left_foot = trackers.at(label);
        label = steamVRInterface.get_tracker_id((uint)tracker_right_foot_id);
        tracker_right_foot = trackers.at(label);
        calibrated = true;

    }

    double[] calculate_second_derivatives(double[] values)
    {

        double[] derivatives = new double[values.Length - 1];
        double[] second_derivatives = new double[values.Length - 2];
        double[] time_differences = new double[window_size];
        double delta_time = 0f;

        for (int i = 0; i < values.Length - 1; ++i)
        {
            delta_time = (double)(time_stamps[i + 1] - time_stamps[i]).TotalMilliseconds;
            derivatives[i] = (values[i + 1] - values[i]) / delta_time;
            //print("postion "+ i + " delta_time = " + delta_time + " i + 1 : " + values[i + 1] + " i : " + values[i] + " | derivative = " + derivatives[i]);
        }

        for (int i = 0; i < values.Length - 2; ++i)
        {
            delta_time = (double)(time_stamps[i + 1] - time_stamps[i]).TotalMilliseconds;
            second_derivatives[i] = (derivatives[i + 1] - derivatives[i]) / delta_time;
        }

        return second_derivatives;
    }

    double[] filter_raw_values(double[] values)
    {
        double[] filtered_values = Butterworth(values, (time_stamps[5] - time_stamps[4]).TotalSeconds, 5d);
        return filtered_values;
    }

    //--------------------------------------------------------------------------
    // This function returns the data filtered. Converted to C# 2 July 2014.
    // Original source written in VBA for Microsoft Excel, 2000 by Sam Van
    // Wassenbergh (University of Antwerp), 6 june 2007.
    //--------------------------------------------------------------------------
    public static double[] Butterworth(double[] indata, double deltaTimeinsec, double CutOff)
    {
        if (indata == null) return null;
        if (CutOff == 0) return indata;

        double Samplingrate = 1 / deltaTimeinsec;
        long dF2 = indata.Length - 1;        // The data range is set with dF2
        double[] Dat2 = new double[dF2 + 4]; // Array with 4 extra points front and back
        double[] data = indata; // Ptr., changes passed data

        // Copy indata to Dat2
        for (long r = 0; r < dF2; r++)
        {
            Dat2[2 + r] = indata[r];
        }
        Dat2[1] = Dat2[0] = indata[0];
        Dat2[dF2 + 3] = Dat2[dF2 + 2] = indata[dF2];

        const double pi = 3.14159265358979;
        double wc = Math.Tan(CutOff * pi / Samplingrate);
        if (wc == 0) wc = 0.000001; //correction for null values of wc causing infinite accelerations
        double k1 = 1.414213562 * wc; // Sqrt(2) * wc
        double k2 = wc * wc;
        double a = k2 / (1 + k1 + k2);
        double b = 2 * a;
        double c = a;
        double k3 = b / k2;
        double d = -2 * a + k3;
        double e = 1 - (2 * a) - k3;

        //print("tan = " + wc);

        // RECURSIVE TRIGGERS - ENABLE filter is performed (first, last points constant)
        double[] DatYt = new double[dF2 + 4];
        DatYt[1] = DatYt[0] = indata[0];
        for (long s = 2; s < dF2 + 2; s++)
        {
            DatYt[s] = a * Dat2[s] + b * Dat2[s - 1] + c * Dat2[s - 2]
                       + d * DatYt[s - 1] + e * DatYt[s - 2];
        }
        DatYt[dF2 + 3] = DatYt[dF2 + 2] = DatYt[dF2 + 1];

        // FORWARD filter
        double[] DatZt = new double[dF2 + 2];
        DatZt[dF2] = DatYt[dF2 + 2];
        DatZt[dF2 + 1] = DatYt[dF2 + 3];
        for (long t = -dF2 + 1; t <= 0; t++)
        {
            DatZt[-t] = a * DatYt[-t + 2] + b * DatYt[-t + 3] + c * DatYt[-t + 4]
                        + d * DatZt[-t + 1] + e * DatZt[-t + 2];
        }

        // Calculated points copied for return
        for (long p = 0; p < dF2; p++)
        {
            data[p] = DatZt[p];
        }

        return data;
    }

    void Calibrate_foot()
    {

        MINIMUM_STEP_HEIGHT = tracker_left_foot.position.y + INITIAL_HEIGHT;
        max_height = tracker_left_foot.position.y; //maybe substract MINIMUM_STEP_HEIGHT
    }



    [CustomEditor(typeof(FootSwingAnalysis))]
    public class FottSwingEditor : Editor
    {
        private FootSwingAnalysis script;
        private void OnEnable() { script = target as FootSwingAnalysis; }

        static string[] default_labels = new string[0];


        public override void OnInspectorGUI()
        {
            if (script.steamVRInterface != null) script.tracker_left_foot_id = EditorGUILayout.Popup("Left foot", script.tracker_left_foot_id, script.steamVRInterface.get_tracker_labels() != null ? script.steamVRInterface.get_tracker_labels() : default_labels);

            if (script.steamVRInterface != null) script.tracker_right_foot_id = EditorGUILayout.Popup("Right foot", script.tracker_right_foot_id, script.steamVRInterface.get_tracker_labels() != null ? script.steamVRInterface.get_tracker_labels() : default_labels);

            if (GUILayout.Button("Set trackers"))
            {
                script.BindTracker();
            }

            if (GUILayout.Button("Calibrate foot height"))
            {
                script.Calibrate_foot();
            }

            GUI.backgroundColor = script.apply_acceleration_detection ? Color.red : Color.green;

            if (GUILayout.Button("Toggle acceleration detection"))
            {
                script.apply_acceleration_detection = !script.apply_acceleration_detection;
            }

            GUI.backgroundColor = Color.white;

            //if (GUILayout.Button("Start recording data")) { script.startRecording(); }
            //if (GUILayout.Button("Stop recording data")) { script.stopRecording(); }

            base.OnInspectorGUI();
        }

    }
}