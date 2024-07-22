using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject ChoosenIndicatorObject;
    private bool IsFirstBuildingSelected = false;
    private GameObject FirstBuilding;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse is down");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "Building")
                {
                    Debug.Log("It's working!");
                    transform.GetChild(0).GetComponent<AudioSource>().Play();
                    if (!IsFirstBuildingSelected && hitInfo.transform.gameObject.GetComponent<CBuildingManager>().GetTeam() == 1)
                    {
                        FirstBuilding = hitInfo.transform.gameObject;
                        IsFirstBuildingSelected = true;
                        ChoosenIndicatorObject.SetActive(true);
                        ChoosenIndicatorObject.transform.position = hitInfo.transform.position;
                    }
                    else if(IsFirstBuildingSelected)
                    {
                        if (hitInfo.transform.gameObject != FirstBuilding)
                        {
                            int index = FirstBuilding.GetComponent<CBuildingManager>().CheckIfBuildingConnected(hitInfo.transform.gameObject);
                            Debug.Log("index: "+index);
                            FirstBuilding.GetComponent<CBuildingManager>().ActivateArmyMovement(index);
                        }
                            ChoosenIndicatorObject.SetActive(false);
                            FirstBuilding = null;
                            IsFirstBuildingSelected = false;
                        
                    }
                    else
                    {
                        ChoosenIndicatorObject.SetActive(false);
                        FirstBuilding = null;
                        IsFirstBuildingSelected = false;
                    }
                    
                }
                else
                {
                    transform.GetChild(1).GetComponent<AudioSource>().Play();
                    ChoosenIndicatorObject.SetActive(false);
                    FirstBuilding = null;
                    IsFirstBuildingSelected = false;
                    Debug.Log("nopz");
                }
            }
            else
            {
                transform.GetChild(1).GetComponent<AudioSource>().Play();
                ChoosenIndicatorObject.SetActive(false);
                FirstBuilding = null;
                IsFirstBuildingSelected = false;
                Debug.Log("No hit");
            }
        }
    }
}
