
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    // Hide these variables from Unity editor.
    [HideInInspector]
    public bool playerPlaced = false,start_placed,end_placed;
    [HideInInspector]
    public bool saveLoadMenuOpen = false;

    public Animator itemUIAnimation;
    public Animator optionUIAnimation;
    public Animator saveUIAnimation;
    public Animator loadUIAnimation;
    public SpriteRenderer mouseObject;
    public Selection user;
    public Mesh playerMarker;
    public Slider rotSlider;
    public GameObject rotUI;
    public InputField levelName;
    public InputField levelNameLoad;
    public Text levelMessage;
    public Text coins;
    public Text Message;

    public Animator messageAnim;

    private bool itemPositionIn = true;
    private bool optionPositionIn = true;
    private bool saveLoadPositionIn = false;
    private Editor level;


    // Start is called before the first frame update
    void Start()
    {
        rotSlider.onValueChanged.AddListener(delegate { RotationValueChange(); }); 
        CreateEditor(); 
        start_placed = false;
        end_placed = false;
    }

    Editor CreateEditor()
    {
        level = new Editor();
        level.Objects = new List<Editor_data.Info>(); 
        return level;
    }

    //Rotating an object and saving the info
    void RotationValueChange()
    {
        user.rotObject.transform.localEulerAngles = new Vector3(0, 0, rotSlider.value); 
        user.rotObject.GetComponent<Editor_data>().info.rotz = user.rotObject.transform.rotation; 
    }


    public void SlideItemMenu()
    {
        if (itemPositionIn == false)
        {
            itemUIAnimation.SetTrigger("ItemMenuIn"); 
            itemPositionIn = true; 
        }
        else
        {
            itemUIAnimation.SetTrigger("ItemMenuOut"); 
            itemPositionIn = false; 
        }
    }

    public void SlideOptionMenu()
    {
        if (optionPositionIn == false)
        {
            optionUIAnimation.SetTrigger("OptionMenuIn"); 
            optionPositionIn = true; 
        }
        else
        {
            optionUIAnimation.SetTrigger("OptionMenuOut"); 
            optionPositionIn = false; 
        }
    }

    public void ChooseSave()
    {
        if (saveLoadPositionIn == false)
        {
            saveUIAnimation.SetTrigger("SaveLoadIn"); 
            saveLoadPositionIn = true; 
            saveLoadMenuOpen = true; 
        }
        else
        {
            saveUIAnimation.SetTrigger("SaveLoadOut"); 
            saveLoadPositionIn = false; 
            saveLoadMenuOpen = false; 
        }
    }

    public void ChooseLoad()
    {
        if (saveLoadPositionIn == false)
        {
            loadUIAnimation.SetTrigger("SaveLoadIn");
            saveLoadPositionIn = true; 
            saveLoadMenuOpen = true; 
        }
        else
        {
            loadUIAnimation.SetTrigger("SaveLoadOut"); 
            saveLoadPositionIn = false;
            saveLoadMenuOpen = false; 
        }
    }


    public void ChoosePlatform()
    {
        user.itemOption = Selection.Items.Platform; 
        GameObject platform = user.Platforms;
        mouseObject.sprite = platform.GetComponent<SpriteRenderer>().sprite;
        
    }

    public void ChooseCoins()
    {
        user.itemOption = Selection.Items.Coins; 
        GameObject coins = user.Coins;
        mouseObject.sprite = coins.GetComponent<SpriteRenderer>().sprite;
    }

    public void ChooseStart()
    {
        user.itemOption = Selection.Items.Start_pos; 
        GameObject start = user.Start_pos; 
        mouseObject.sprite = start.GetComponent<SpriteRenderer>().sprite;

    }

    public void ChoosePlayer()
    {
        user.itemOption = Selection.Items.Player;
        GameObject player = user.Player;
        mouseObject.sprite =player.GetComponent<SpriteRenderer>().sprite; 
    }
    public void ChoosePlayerEnd()
    {
        user.itemOption = Selection.Items.End_pos;
        GameObject end = user.End_pos;
        mouseObject.sprite=end.GetComponent<SpriteRenderer>().sprite;
    }



    public void ChooseCreate()
    {
        user.Level_option = Selection.Level_Options.Create; 
        user.mr.enabled = true;
        Message.text = "Select objects \n Click to create objects\n W,A,S,D to move the Camera";
        rotUI.SetActive(false); 
    }
    public void ChooseMove()
    {
        user.Level_option = Selection.Level_Options.Move;
        user.mr.enabled = false;
        Message.text = " Click to select objects\n W,A,S,D to move the Objects";
        rotUI.SetActive(false);
    }

    public void ChooseRotate()
    {
        user.Level_option = Selection.Level_Options.Rotate; 
        user.mr.enabled = false;
        Message.text = " Click to select objects\n Use Slider to rotate objects";
        rotUI.SetActive(true); 
    }

    public void ChooseDestroy()
    {
        user.Level_option = Selection.Level_Options.Destroy; 
        user.mr.enabled = false;
        Message.text = " Click to destroy objects";
        rotUI.SetActive(false); 
    }



    // Saving a level
    public void SaveLevel()
    {
       
        Editor_data[] foundObjects = FindObjectsOfType<Editor_data>();
        foreach (Editor_data obj in foundObjects)
            level.Objects.Add(obj.info); 

        string json = JsonUtility.ToJson(level); 
        string folder = Application.dataPath + "/LevelData/"; 
        string levelFile = "";

       
        if (levelName.text == "")
            levelFile = "new_level.json";
        else
            levelFile = levelName.text + ".json";

       
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string path = Path.Combine(folder, levelFile); 
      
        if (File.Exists(path))
            File.Delete(path);

     
        File.WriteAllText(path, json);

      
        saveUIAnimation.SetTrigger("SaveLoadOut");
        saveLoadPositionIn = false;
        saveLoadMenuOpen = false;
        levelName.text = ""; 
        levelName.DeactivateInputField(); 

      
        levelMessage.text = levelFile + " saved to LevelData folder.";
        messageAnim.Play("MessageFade", 0, 0);
    }


    // Loading a level
    public void LoadLevel()
    {
        string folder = Application.dataPath + "/LevelData/";
        string levelFile = "";

    
        if (levelNameLoad.text == "")
            levelFile = "new_level.json";
        else
            levelFile = levelNameLoad.text + ".json";

        string path = Path.Combine(folder, levelFile); 

        if (File.Exists(path)) 
        {
            
            Editor_data[] foundObjects = FindObjectsOfType<Editor_data>();
            foreach (Editor_data obj in foundObjects)
                Destroy(obj.gameObject);

            playerPlaced = false; 

            string json = File.ReadAllText(path); 
            level = JsonUtility.FromJson<Editor>(json); 
            CreateFromFile(); 
        }
        else 
        {
            loadUIAnimation.SetTrigger("SaveLoadOut"); 
            saveLoadPositionIn = false; 
            saveLoadMenuOpen = false; 
            levelMessage.text = levelFile + " could not be found!"; 
            messageAnim.Play("MessageFade", 0, 0);
            levelNameLoad.DeactivateInputField(); 
        }
    }


    void CreateFromFile()
    {
        GameObject newObj; 

        for (int i = 0; i < level.Objects.Count; i++)
        {
                
                newObj = Instantiate(level.Objects[i].object_type,transform.position,Quaternion.identity);
           
            newObj.transform.position = level.Objects[i].location ; 
                newObj.transform.rotation = level.Objects[i].rotz; 
                newObj.layer = 9; 

                Editor_data eo = newObj.AddComponent<Editor_data>();
                eo.info.location = newObj.transform.position;
                eo.info.rotz = newObj.transform.rotation;
                eo.info.object_type = level.Objects[i].object_type;
            
        }

      
        levelNameLoad.text = "";
        levelNameLoad.DeactivateInputField(); 

        loadUIAnimation.SetTrigger("SaveLoadOut"); 
        saveLoadPositionIn = false; 
        saveLoadMenuOpen = false; 

        
        levelMessage.text = "Level loading...done.";
        messageAnim.Play("MessageFade", 0, 0);
    }
    public void GameOver()
    {
        levelMessage.text = "Restarting";
        messageAnim.Play("MessageFade", 0, 0);
    }
}


