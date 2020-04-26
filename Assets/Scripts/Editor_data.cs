using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Editor_data : MonoBehaviour
{
    public GameObject selection;
    public enum Objects { Platform,Coin,Start,End, Player }; 
    [Serializable] 
    public struct Info
    {
        public GameObject object_type; 
        public Vector3 location; 
        public Quaternion rotz; 

    }
    private void Update()
    {
       // selection = Selection.instance;
    }

    public Info info;
}
