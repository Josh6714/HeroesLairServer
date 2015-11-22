using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

public class EconomyManager : MonoBehaviour {

    //Expense List Input
    public InputField metalPlateInput;
    public InputField plasticWallInput;
    public InputField flashlightInput;
    public InputField spaceSuitInput;
    public InputField turretInput;
    public InputField laserGunInput;
    public InputField forceGunInput;
    public InputField oxygenTankInput;
    public InputField batteryPackInput;
    public InputField gravitySwitchInput;
    public InputField gravityWheelInput;
    public InputField entranceInput;
    public InputField exitInput;
    public InputField debrisInput;

    //Income List Input
    public InputField levelCompleteInput;
    public InputField improveStandingInput;

    //Economy
    public int metalPlateValue;
    public int plasticWallValue;
    public int flashlightValue;
    public int spaceSuitValue;
    public int turretValue;
    public int laserGunValue;
    public int forceGunValue;
    public int oxygenTankValue;
    public int batteryPackValue;
    public int gravitySwitchValue;
    public int gravityWheelValue;
    public int entranceValue;
    public int exitValue;
    public int debrisValue;
    public int levelCompleteValue;
    public int improveStandingValue;

	void Start ()
    {
        loadEcon();
	}

    private void loadEcon()
    {
        Debug.Log("Loading Economy");

        //Economy list
        List<int> list = new List<int>();

        /*
         * Loads Economy File. Creates list with values
        */ 
        if (File.Exists("ServerData/Economy/Values.txt"))
        { //If save file exists
            var reader = new StreamReader(File.OpenRead("ServerData/Economy/Values.txt"));

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                int input = Int32.Parse(line);
                list.Add(input);
            }
        }
        else
        {
            Debug.Log("ERROR LOADING ECONOMY LIST FILE FROM loadEcon()");
            return;
        }

        /*
         * Places Economy list into values
        */
        metalPlateValue = list[0];
        plasticWallValue = list[1];
        flashlightValue = list[2];
        spaceSuitValue = list[3];
        turretValue = list[4];
        laserGunValue = list[5];
        forceGunValue = list[6];
        oxygenTankValue = list[7];
        batteryPackValue = list[8];
        gravitySwitchValue = list[9];
        gravityWheelValue = list[10];
        entranceValue = list[11];
        exitValue = list[12];
        debrisValue = list[13];
        levelCompleteValue = list[14];
        improveStandingValue = list[15];

        /*
         * Places Economy values into inputs
        */
        metalPlateInput.text = metalPlateValue.ToString();
        plasticWallInput.text = plasticWallValue.ToString();
        flashlightInput.text = flashlightValue.ToString();
        spaceSuitInput.text = spaceSuitValue.ToString();
        turretInput.text = turretValue.ToString();
        laserGunInput.text = laserGunValue.ToString();
        forceGunInput.text = forceGunValue.ToString();
        oxygenTankInput.text = oxygenTankValue.ToString();
        batteryPackInput.text = batteryPackValue.ToString();
        gravitySwitchInput.text = gravitySwitchValue.ToString();
        gravityWheelInput.text = gravityWheelValue.ToString();
        entranceInput.text = entranceValue.ToString();
        exitInput.text = exitValue.ToString();
        debrisInput.text = debrisValue.ToString();
        levelCompleteInput.text = levelCompleteValue.ToString();
        improveStandingInput.text = improveStandingValue.ToString();

        Debug.Log("Economy Setup");
    }

    public void saveEcon()
    {
        /*
         * Saves Economy File.
        */ 
        string[] lines = new string[16];

        lines[0] = metalPlateInput.text;
        lines[1] = plasticWallInput.text;
        lines[2] = flashlightInput.text;
        lines[3] = spaceSuitInput.text;
        lines[4] = turretInput.text;
        lines[5] = laserGunInput.text;
        lines[6] = forceGunInput.text;
        lines[7] = oxygenTankInput.text;
        lines[8] = batteryPackInput.text;
        lines[9] = gravitySwitchInput.text;
        lines[10] = gravityWheelInput.text;
        lines[11] = entranceInput.text;
        lines[12] = exitInput.text;
        lines[13] = debrisInput.text;
        lines[14] = levelCompleteInput.text;
        lines[15] = improveStandingInput.text;

        System.IO.File.WriteAllLines("ServerData/Economy/Values.txt", lines);
        Debug.Log("Saved Econ: ServerData/Economy/Values.txt");

        /*
         *  Loads new values
        */
        loadEcon();

        /*
         * Displays main menu
        */ 
        GetComponent<serverMenu>().showServerMenu();
    }

    private void defaultEcon()
    {

    }
}
