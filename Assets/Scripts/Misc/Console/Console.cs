using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    InputField consoleInput;
    GunHandler gunHandler;

    public List<ConsoleCommand> commands = new List <ConsoleCommand>();

    //Command Responses
    string helpResponse = 
    "Try the following commands: \n spawn [gun/attachment name] (spawns a gun or attachment in) \n summon [enemy name] (summons an enemy) \n clear (kills all enemies) \n";

    void Start()
    {
        gunHandler = GameObject.FindWithTag("Gun").GetComponent<GunHandler>();
        consoleInput = this.gameObject.GetComponent<InputField>();
    }

    public void ExecuteCommand()
    {
        string command = consoleInput.text;

        string comPrefix = "";
        for (int i = 0; i < command.Length; i++)
        {
            if (command[i].ToString() != " ")
            {
                comPrefix += command[i];
            }
        }

        switch(comPrefix)
        {
            case "spawn": //Spawns guns

            break;

            case "summon": //Spawns enemies

            break;

            case "help": //Help menu
            Respond(helpResponse);
            break;

            case "clear": //Clears all enemies

            break;

            //case ""
        }
    }

    public void Respond(string response)
    {
        
    }

    public void CommitNewGun()
    {
        string desiredGunName = consoleInput.text;
        Gun desiredGun = Resources.Load<Gun>(desiredGunName);
        Debug.Log("Loading " + desiredGun._gunName);
        if (desiredGun != null)
        {
            gunHandler.currentGun = desiredGun;
        }
    }
}

public class ConsoleCommand
{

}