using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_script : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject player;
    public Selection user;
    public Vector3 offset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Inventory.activeSelf)
        {
            if (!user.moving)
            {
                Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
                transform.Translate(input * 0.3f);
            }
        }
        else if (player != null)
        {
                transform.position = player.transform.position - offset;
        } 
    }
}
