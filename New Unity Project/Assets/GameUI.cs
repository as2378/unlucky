using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/**
 * This class encapsulates the behaviour of the game’s user interface.
 * The class shows/hides the allocation input fields and handles the execution of the allocation button clicks.
 */ 
public class GameUI : MonoBehaviour
{
    private GameObject gangMembersLeftLabel;
    private GameObject gangMemberInputField;
    private GameObject allocateButton;

	/**
     * Awake():
     * Initializes the game UI.
     * 
     * If Start() is used instead, the order in which all the scripts
     * are run is undefined in Unity, thus we can't guarantee that other
     * scripts will be able to use the methods in this class.
     * */
    void Awake()
    {
        // Initialize allocation UI elements
        gangMembersLeftLabel = GameObject.Find("UICanvas/GangMembersLeftLabel");
        gangMemberInputField = GameObject.Find("UICanvas/GangMemberInputField");
        allocateButton = GameObject.Find("UICanvas/AllocateButton");
        allocateButton.GetComponent<Button>().onClick.AddListener(onAllocateButtonClicked);

        showGangMembersLeftLabel(false);
        showAllocationUIForm(false);
    }

	/**
     * showGangMembersLeftLabel():
     * Makes a block visible that represents the number of gang members
     * left for allocation
     */
    public void showGangMembersLeftLabel(bool active)
    {
        gangMembersLeftLabel.SetActive(active);

        // Whenever the label is shown, we update the number of gang members
        if (active)
        {
            updateGangMembersLeftLabel();
        }
    }

	/**
     * updateGangMembersLeftLabel():
     * Used for updating the label.
     */
    public void updateGangMembersLeftLabel()
    {
        gangMembersLeftLabel.GetComponentInChildren<Text>().text = "Gang members left: " + GameClass.CurrentPlayer.GangMembersLeft;
    }

	/**
     * showAllocationUIForm():
     * Makes both the input field and allocation button (in)visible
     */
    public void showAllocationUIForm(bool active)
    {
        gangMemberInputField.SetActive(active);
        allocateButton.SetActive(active);
    }

	/**
     * onAllocationButtonClicked()
     * Used as an event listener for the allocation button.
     */
    public void onAllocateButtonClicked()
    {
        Button button = allocateButton.GetComponent<Button>();

        // To prevent double-clicking
        if (button.interactable)
        {
            button.interactable = false;

            InputField input = gangMemberInputField.GetComponent<InputField>();
            int value;

            // Using Int32.TryParse as the value of input.text can be "" (invalid)
            if(Int32.TryParse(input.text, out value))
            {
                if (value < 0 || value > GameClass.CurrentPlayer.GangMembersLeft)
                {
                    ColorBlock colorBlock = input.colors;
                    colorBlock.normalColor = Color.red;
                    input.colors = colorBlock;
                }
                else
                {
                    // Allocate gang members
                    GameClass.CurrentPlayer.GangMembersLeft -= value;
                    GameObject selectedSector = GameObject.Find("Map").GetComponent<MapClass>().getSelectedSector();
                    Sector sectorClass = selectedSector.GetComponent<Sector>();

					sectorClass.addUnits (value);

                    // Reset the allocation button to the default color
                    ColorBlock colorBlock = input.colors;
                    colorBlock.normalColor = Color.white;
                    input.colors = colorBlock;

                    updateGangMembersLeftLabel();

                    // If the player has allocated all of his members, go to the next player
                    if (GameClass.CurrentPlayer.GangMembersLeft == 0)
                    {
                        showGangMembersLeftLabel(false);
                        GameClass.CurrentPlayer.Allocated = true;
                        GameClass.changeTurn();
                    }

                    showAllocationUIForm(false);

                    // Deselect the sector that a number of gang members have just been allocated to
                    GameObject.Find("Map").GetComponent<MapClass>().deselectAll();
                }

                // Reset the placeholder of the input field
                input.text = "";
            }
            button.interactable = true;
        }
    }
}
