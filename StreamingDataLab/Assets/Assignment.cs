/*
This RPG data streaming assignment was created by Fernando Restituto.
Pixel RPG characters created by Sean Browning.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;


#region Assignment Instructions

/*  Hello!  Welcome to your first lab :)

Wax on, wax off.

    The development of saving and loading systems shares much in common with that of networked gameplay development.  
    Both involve developing around data which is packaged and passed into (or gotten from) a stream.  
    Thus, prior to attacking the problems of development for networked games, you will strengthen your abilities to develop solutions using the easier to work with HD saving/loading frameworks.

    Try to understand not just the framework tools, but also, 
    seek to familiarize yourself with how we are able to break data down, pass it into a stream and then rebuild it from another stream.


Lab Part 1

    Begin by exploring the UI elements that you are presented with upon hitting play.
    You can roll a new party, view party stats and hit a save and load button, both of which do nothing.
    You are challenged to create the functions that will save and load the party data which is being displayed on screen for you.

    Below, a SavePartyButtonPressed and a LoadPartyButtonPressed function are provided for you.
    Both are being called by the internal systems when the respective button is hit.
    You must code the save/load functionality.
    Access to Party Character data is provided via demo usage in the save and load functions.

    The PartyCharacter class members are defined as follows.  */

public partial class PartyCharacter
{
    public int classID;

    public int health;
    public int mana;

    public int strength;
    public int agility;
    public int wisdom;

    public LinkedList<int> equipment;

}


/*
    Access to the on screen party data can be achieved via …..

    Once you have loaded party data from the HD, you can have it loaded on screen via …...

    These are the stream reader/writer that I want you to use.
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamreader

    Alright, that’s all you need to get started on the first part of this assignment, here are your functions, good luck and journey well!
*/


#endregion


#region Assignment Part 1

static public class AssignmentPart1
{

    const int PartyCharacterSaveDataSignifier = 0;
    const int PartyCharacterEquipmentSaveDataSignifier = 1;

    static public void SavePartyButtonPressed()
    {

        StreamWriter sw = new StreamWriter(Application.dataPath + Path.DirectorySeparatorChar + "Party.txt");

        foreach (PartyCharacter pc in GameContent.partyCharacters)
        {
            //Debug.Log("PC class id == " + pc.classID);

            sw.WriteLine(PartyCharacterSaveDataSignifier + "," + pc.classID + "," + pc.health
            + "," + pc.mana
            + "," + pc.strength
            + "," + pc.agility
            + "," + pc.wisdom);

            foreach (int equip in pc.equipment)
            {
                sw.WriteLine(PartyCharacterEquipmentSaveDataSignifier + "," + equip);
            }

        }

        sw.Close();

    }

    static public void LoadPartyButtonPressed()
    {
        GameContent.partyCharacters.Clear();

        StreamReader sr = new StreamReader(Application.dataPath + Path.DirectorySeparatorChar + "Party.txt");

        string line;
        while ((line = sr.ReadLine()) != null)
        {
            Debug.Log(line);

            string[] csv = line.Split(',');

            int signifier = int.Parse(csv[0]);

            if (signifier == PartyCharacterSaveDataSignifier)
            {
                PartyCharacter pc = new PartyCharacter(int.Parse(csv[1]), int.Parse(csv[2]), int.Parse(csv[3]), int.Parse(csv[4]), int.Parse(csv[5]), int.Parse(csv[6]));
                GameContent.partyCharacters.AddLast(pc);
            }
            else if (signifier == PartyCharacterEquipmentSaveDataSignifier)
            {
                GameContent.partyCharacters.Last.Value.equipment.AddLast(int.Parse(csv[1]));
            }

        }

        GameContent.RefreshUI();

    }

}


#endregion


#region Assignment Part 2

//  Before Proceeding!
//  To inform the internal systems that you are proceeding onto the second part of this assignment,
//  change the below value of AssignmentConfiguration.PartOfAssignmentInDevelopment from 1 to 2.
//  This will enable the needed UI/function calls for your to proceed with your assignment.
static public class AssignmentConfiguration
{
    public const int PartOfAssignmentThatIsInDevelopment = 2;
}

/*

In this part of the assignment you are challenged to expand on the functionality that you have already created.  
    You are being challenged to save, load and manage multiple parties.
    You are being challenged to identify each party via a string name (a member of the Party class).

To aid you in this challenge, the UI has been altered.  

    The load button has been replaced with a drop down list.  
    When this load party drop down list is changed, LoadPartyDropDownChanged(string selectedName) will be called.  
    When this drop down is created, it will be populated with the return value of GetListOfPartyNames().

    GameStart() is called when the program starts.

    For quality of life, a new SavePartyButtonPressed() has been provided to you below.

    An new/delete button has been added, you will also find below NewPartyButtonPressed() and DeletePartyButtonPressed()

Again, you are being challenged to develop the ability to save and load multiple parties.
    This challenge is different from the previous.
    In the above challenge, what you had to develop was much more directly named.
    With this challenge however, there is a much more predicate process required.
    Let me ask you,
        What do you need to program to produce the saving, loading and management of multiple parties?
        What are the variables that you will need to declare?
        What are the things that you will need to do?  
    So much of development is just breaking problems down into smaller parts.
    Take the time to name each part of what you will create and then, do it.

Good luck, journey well.

*/


static public class AssignmentPart2
{


    const int PartyCharacterSaveDataSignifier = 0;
    const int PartyCharacterEquipmentSaveDataSignifier = 1;


    const int LastUsedIndexSignifier = 1;
    const int IndexAndNameSignifier = 2;


    static int lastIndexUsed;
    static List<string> partyNames;
    static LinkedList<NameAndIndex> nameAndIndices;

    const string IndexFilePath = "indices.txt";


    static public void GameStart()
    {

        Debug.Log("Game Start");

        nameAndIndices = new LinkedList<NameAndIndex>();

        if (File.Exists(Application.dataPath + Path.DirectorySeparatorChar + IndexFilePath))
        {
            StreamReader sr = new StreamReader(Application.dataPath + Path.DirectorySeparatorChar + IndexFilePath);

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Debug.Log(line);
                string[] csv = line.Split(',');
                int signifier = int.Parse(csv[0]);

                if (signifier == LastUsedIndexSignifier)
                {
                    lastIndexUsed = int.Parse(csv[1]);
                }
                else if (signifier == IndexAndNameSignifier)
                {
                    nameAndIndices.AddLast(new NameAndIndex(int.Parse(csv[1]), csv[2]));
                }
            }
        }

        partyNames = new List<string>();


        foreach(NameAndIndex nameAndIndex in nameAndIndices)
        {
            partyNames.Add(nameAndIndex.name);
        }

        GameContent.RefreshUI();

    }

    static public List<string> GetListOfPartyNames()
    {
        return partyNames;

    }

    static public void LoadPartyDropDownChanged(string selectedName)
    {

        GameContent.partyCharacters.Clear();

        int indexToLoad = -1;

        foreach(NameAndIndex nameAndIndex in nameAndIndices)
        {
            if(nameAndIndex.name == selectedName)
                indexToLoad = nameAndIndex.index;
        }

        if(indexToLoad != -1)
            ReadPartyDataFromFile(indexToLoad);

    }

    static private void ReadPartyDataFromFile(int indexToLoad)
    {
        StreamReader sr = new StreamReader(Application.dataPath + Path.DirectorySeparatorChar + indexToLoad + ".txt");

        LinkedList<string> partyData = new LinkedList<string>();
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            partyData.AddLast(line);
        }
        sr.Close();

        LoadParty(partyData);

    }

    static public void SavePartyButtonPressed()
    {
        string newPartyName = GameContent.GetPartyNameFromInput();
        bool isUniqueName = true;

        foreach (NameAndIndex nameAndIndex in nameAndIndices)
        {
            if (nameAndIndex.name == newPartyName)
            {
                SaveParty(Application.dataPath + Path.DirectorySeparatorChar + nameAndIndex.index + ".txt");
                isUniqueName = false;
            }
        }

        if (isUniqueName)
        {
            lastIndexUsed++;
            SaveParty(Application.dataPath + Path.DirectorySeparatorChar + lastIndexUsed + ".txt");
            nameAndIndices.AddLast(new NameAndIndex(lastIndexUsed, newPartyName));
            partyNames.Add(newPartyName);
        }


        GameContent.RefreshUI();
        Debug.Log("saving");


        SaveIndexManagementFile();
    }

    static public void NewPartyButtonPressed()
    {
        Debug.Log("create new");

        SaveIndexManagementFile();

    }

    static public void DeletePartyButtonPressed()
    {
        bool isDeleting = false;
        NameAndIndex indexToDelete = null;
        foreach (NameAndIndex nameAndIndex in nameAndIndices)
        {
            if (nameAndIndex.name == GameContent.GetPartyNameFromInput())
            {
                StreamWriter sw =  new StreamWriter(Application.dataPath + Path.DirectorySeparatorChar + nameAndIndex.index + ".txt");
                sw.Dispose();
                
                indexToDelete = nameAndIndex;
                isDeleting = true;
            }
        }

        if(isDeleting)
        { 
            nameAndIndices.Remove(indexToDelete);
            GameContent.RefreshUI();
            Debug.Log("deleting");
            SaveIndexManagementFile();
            partyNames.Remove(indexToDelete.name);
        }
        else
        {
            Debug.Log("could not find file");
        }
    }

    static public void SaveIndexManagementFile()
    {

        StreamWriter sw = new StreamWriter(Application.dataPath + Path.DirectorySeparatorChar + IndexFilePath);

        sw.WriteLine(LastUsedIndexSignifier + "," + lastIndexUsed);
        //Debug.Log("1," + lastIndexUsed);

        foreach (NameAndIndex nameAndIndex in nameAndIndices)
        {
            sw.WriteLine(IndexAndNameSignifier + "," + nameAndIndex.index + "," + nameAndIndex.name);
        }

        sw.Close();

    }

    static public void SaveParty(string fileName)
    {
        LinkedList<string> partyData = CreatePartySaveDataStrings();

        StreamWriter sw = new StreamWriter(fileName);

        foreach (string line in partyData)
        {
            sw.WriteLine(line);
        }

        sw.Close();
    }

    static public void SendCurrentPartyToSharingServer(NetworkClient networkClient)
    {
        LinkedList<string> data = CreatePartySaveDataStrings();

        networkClient.SendMessageToHost(ClientToServerSignifiers.PartyTransferDataStart + "");

        foreach(string line in data)
        {
            string msg = ClientToServerSignifiers.PartyTransferData + "," + line;
            networkClient.SendMessageToHost(msg);
        }
        networkClient.SendMessageToHost(ClientToServerSignifiers.PartyTransferDataEnd + "");
    }

    static private LinkedList<string> CreatePartySaveDataStrings()
    {
        LinkedList<string> data = new LinkedList<string>();

        foreach (PartyCharacter pc in GameContent.partyCharacters)
        {
   
            data.AddLast(PartyCharacterSaveDataSignifier + "," + pc.classID + "," + pc.health
            + "," + pc.mana
            + "," + pc.strength
            + "," + pc.agility
            + "," + pc.wisdom);

            foreach (int equip in pc.equipment)
            {
                data.AddLast(PartyCharacterEquipmentSaveDataSignifier + "," + equip);
            }
        }

        return data;
    }

    static public void LoadParty(LinkedList<string> data)
    {
        GameContent.partyCharacters.Clear();

        foreach(string line in data)
        { 
            string[] csv = line.Split(',');

            int signifier = int.Parse(csv[0]);

            if (signifier == PartyCharacterSaveDataSignifier)
            {
                PartyCharacter pc = new PartyCharacter(int.Parse(csv[1]), int.Parse(csv[2]), int.Parse(csv[3]), int.Parse(csv[4]), int.Parse(csv[5]), int.Parse(csv[6]));
                GameContent.partyCharacters.AddLast(pc);
            }
            else if (signifier == PartyCharacterEquipmentSaveDataSignifier)
            {
                GameContent.partyCharacters.Last.Value.equipment.AddLast(int.Parse(csv[1]));
            }
        }

        GameContent.RefreshUI();
    }

}


public class NameAndIndex
{
    public string name;
    public int index;

    public NameAndIndex(int Index, string Name)
    {
        name = Name;
        index = Index;
    }

}


#endregion



//identify all variables used!
// Idea was to have additional file
// With name,index
//we should make a system to save incrementing number
//bug - duplicate party saving
//save the actual party data into the file that is named after the index of the party
//populate the load party list with the party names that have been saved
//load via index



//detele
//"new" adds the text inputted to the dropdown















//




//(done) Get the abilit to save a line of text working
//
//!!!Figure out how we want to format the data that we save????
//
//write the character values to the .txt
//
//
//Do something something party size, how are we support party size???
//
//equipment?
//
//
//
//Load the data
//
//
//reload the ui with new content



//Clear all save data function