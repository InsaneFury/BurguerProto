using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuAnimations : MonoBehaviour
{
    [Header("Principal Menu")]
    public Animator menuTab;
    public Animator room;
    public Animator settings;
    public Animator modes;

    bool settingsOpened = false;
    bool menuTabOpened = false;
    bool modesOpened = false;
    bool roomOpened = true;

    [Header("Modes Menu")]
    public Animator classic;
    public Animator frenetic;
    public Animator story;

    bool classicOpened = false;
    bool freneticOpened = false;
    bool storyOpened = false;

    private void Start()
    {
        menuTab.SetBool("close", true);
        settings.SetBool("close", true);
        modes.SetBool("close", true);
        room.SetBool("close", false);
    }

    #region PrincipalMenu
    public void OpenRoom()
    {
        room.SetBool("close", roomOpened);
        roomOpened = !roomOpened;
    }

    public void OpenMenuTab()
    {
        menuTab.SetBool("close", menuTabOpened);
        menuTabOpened = !menuTabOpened;
    }

    public void OpenSettings()
    {
        settings.SetBool("close", settingsOpened);
        settingsOpened = !settingsOpened;
    }

    public void OpenModes()
    {
        modes.SetBool("close", modesOpened);
        modesOpened = !modesOpened;
    }
    #endregion

    #region ModesMenu
    public void EnableClassic()
    {
        classicOpened = !classicOpened;
        classic.SetBool("open", classicOpened); 
    }
    public void EnableFrenetic()
    {
        freneticOpened = !freneticOpened;
        frenetic.SetBool("open", freneticOpened);
    }
    public void EnableStory()
    {
        storyOpened = !storyOpened;
        story.SetBool("open", storyOpened);
    }
    #endregion

}
