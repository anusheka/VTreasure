using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{

    [SerializeField] GameObject eventPanelUserInRange;
    [SerializeField] GameObject eventPanelUserNotInRange;
    bool isUIPanelActive;

    int tempEvent;

    [SerializeField] private EventManager eventManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayEventPanel(int eventID) {
        tempEvent = eventID;

        if (isUIPanelActive == false) {
            eventPanelUserInRange.SetActive(true);
     //       TMP_Text myText = textObjForEvent.GetComponent<TMP_Text>();
      
               
            isUIPanelActive = true;
        }
        
    }

    public void onJoinButtonClick() {
        eventManager.ActivateEvent(tempEvent);
    }

    public void HideStartEventPanel()
    {
     
        eventPanelUserInRange.SetActive(false);
    }

    public void DisplayUserNotInRangePanel()
    {
        if (isUIPanelActive == false)
        {
            eventPanelUserNotInRange.SetActive(true);
            isUIPanelActive = true;
        }
        
    }


    public void HideUserNotInRangePanel()
    {
        eventPanelUserNotInRange.SetActive(false);
    }

    public void CloseButtonClick() {
        Debug.Log("\n\n\n");
        Debug.Log("close button clicked");
        HideStartEventPanel();
        HideUserNotInRangePanel();
        isUIPanelActive = false;
    }

}
