using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selection : MonoBehaviour
{

    public enum Level_Options { Create, Rotate,Move, Destroy };
    public enum Items { Platform, Coins, Start_pos, End_pos, Player };

    public Items itemOption = Items.Platform;

    public Level_Options Level_option = Level_Options.Create;

    public SpriteRenderer mr;

    public GameObject rotObject,movObject;

    public bool moving;
    public GameObject Player, Platforms, Coins, Start_pos, End_pos;
    public Manager ms;

    public GameObject selected;
    public GameObject Inventory,Controls;
    public GameObject move_icon;
    public GameObject rot_icon;
    public ParticleSystem explode;

    private Vector3 mousePos;
    private bool colliding;

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<SpriteRenderer>();
        colliding = false;
        Controls.SetActive(false);
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!Inventory.activeSelf)
            {
                Inventory.SetActive(true);
                Controls.SetActive(false);
            }
            else
            {
                Controls.SetActive(true);
                Inventory.SetActive(false);
                mr.sprite = null;
            }
        }

        mousePos = Input.mousePosition;
        mousePos.z = 100;
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = screenPos;


        if (colliding)
            mr.color = Color.red;
        else
            mr.color = Color.green;

        if (itemOption == Items.Player && ms.playerPlaced)
            mr.color = Color.red;
        else if (itemOption == Items.Player && !ms.playerPlaced)
            mr.color = Color.green;

        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (colliding == false && Level_option == Level_Options.Create)
                    CreateObject();
                else if (colliding == true && Level_option == Level_Options.Rotate)
                    SetRotateObject();
                else if (colliding == true && Level_option == Level_Options.Destroy)
                {
                    if (selected.GetComponent<Editor_data>().info.object_type == Player)
                        ms.playerPlaced = false;
                    Instantiate(explode, selected.transform.position, Quaternion.identity);
                    Destroy(selected);
                }
                else if (colliding = true && Level_option == Level_Options.Move)
                {
                    SetMoveObject();
                    moving = true;
                }
            }
        }
        if (Level_option != Level_Options.Move)
        {
            moving = false;
            move_icon.SetActive(false);
        }
        else
        {
            if (movObject != null)
            {
                move_icon.SetActive(true);
                move_icon.transform.position = movObject.transform.position;
                Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
                movObject.transform.Translate(input * 0.5f);

            }
        }
        if (Level_option == Level_Options.Rotate)
        {
            if (rotObject != null)
            {
                rot_icon.SetActive(true);
                rot_icon.transform.position = rotObject.transform.position;
            }
        }
        else
            rot_icon.SetActive(false);

        if (!Inventory.activeSelf)
        {
            Level_option = Level_Options.Rotate;
            rot_icon.SetActive(false);
        }
    }


    void CreateObject()
    {
        GameObject newObj;

        if (itemOption == Items.Platform || itemOption == Items.Coins)
        {
            if (itemOption == Items.Coins)
                newObj = Instantiate(Coins, transform.position, Quaternion.identity);
            else
                newObj = Instantiate(Platforms, transform.position, Quaternion.identity);

            newObj.transform.position = transform.position;
            newObj.layer = 9; 

          
            Editor_data eo = newObj.AddComponent<Editor_data>();
            eo.info.location = newObj.transform.position;
            eo.info.rotz = newObj.transform.rotation;
            eo.selection = this.gameObject;
            if (itemOption == Items.Coins)
                eo.info.object_type = Coins;
            else
                eo.info.object_type = Platforms;
        }
        else if (itemOption == Items.Player) 
        {
            if (!ms.playerPlaced)
            {

                newObj = Instantiate(Player, transform.position, Quaternion.identity);
                newObj.layer = 9;
                ms.playerPlaced = true;


                Editor_data eo = newObj.AddComponent<Editor_data>();
                eo.info.location = newObj.transform.position;
                eo.info.rotz = newObj.transform.rotation;
                eo.info.object_type = Player;
                eo.selection = this.gameObject;
            }
            else
                mr.color = Color.red;
        }
        else if (itemOption == Items.Start_pos)
        {
            if (!ms.start_placed)
            {
                ms.start_placed = true;
                newObj = Instantiate(Start_pos, transform.position, Quaternion.identity);
                newObj.layer = 9;

                Editor_data eo = newObj.AddComponent<Editor_data>();
                eo.info.location = newObj.transform.position;
                eo.info.rotz = newObj.transform.rotation;
                eo.info.object_type = Start_pos;

            }
        }
        else if (itemOption == Items.End_pos)
        {
            if (!ms.end_placed)
            {
                ms.end_placed = true;
                newObj = Instantiate(End_pos, transform.position, Quaternion.identity);
                newObj.layer = 9;

                Editor_data eo = newObj.AddComponent<Editor_data>();
                eo.info.location = newObj.transform.position;
                eo.info.rotz = newObj.transform.rotation;
                eo.info.object_type = End_pos;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!collision.CompareTag("Background"))
        {
            colliding = true;
            selected = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer==9)
        {
            colliding = false;
            selected = null;
        }
    }
    void SetMoveObject()
    {
        movObject = selected;
    }
    void SetRotateObject()
    {
        if (selected.tag != "Player")
        {
            rotObject = selected;
            ms.rotSlider.value = rotObject.transform.rotation.y;
        }
    }
}


