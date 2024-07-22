using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CGameManager : MonoBehaviour
{
    [SerializeField] int SceneID = 0;
    [SerializeField] GameObject[] Buildings;


    public void CheckIfAllBuildingsSameTeam()
    {
        int teamid = -1;
        foreach (GameObject building in Buildings)
        {
            if(teamid == -1)
            {
                teamid = building.GetComponent<CBuildingManager>().GetTeam();
            }
            else
            {
                if(teamid != building.GetComponent<CBuildingManager>().GetTeam())
                {
                    return;
                }

            }
        }
        //if all buildings same team
        if(teamid == 1)
        {
            LoadNextScene();
        }
        else
        {
            ReloadScene();
        }
        
    }
    public void LoadNextScene()
    {
        if(SceneID == 0)
        {
            CLoader.Load(CLoader.Scene.Level2);
        }
        else if (SceneID == 1)
        {
            CLoader.Load(CLoader.Scene.Level3);
        }
        else if (SceneID == 2)
        {
            CLoader.Load(CLoader.Scene.Level4);
        }
        else if (SceneID == 3)
        {
            CLoader.Load(CLoader.Scene.Level5);
        }
        else if (SceneID == 4)
        {
            CLoader.Load(CLoader.Scene.Level1);
        }


    }
    public void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void PlayBuildingCaptured()
    {
        transform.GetChild(1).GetComponent<AudioSource>().Play();
    }
    public void PlayBuildingLost()
    {
        transform.GetChild(0).GetComponent<AudioSource>().Play();
    }
    public void PlayBuildingLevelUp()
    {
        transform.GetChild(2).GetComponent<AudioSource>().Play();
    }
}
