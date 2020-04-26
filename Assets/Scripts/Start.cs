using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : MonoBehaviour
{
    public Player_Script player;


    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindObjectOfType<Player_Script>();
        if(player!=null)
        {
            player.start_pos = this.gameObject;
        }
    }
}
