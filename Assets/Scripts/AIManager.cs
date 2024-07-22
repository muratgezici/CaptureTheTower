using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Linq;

public class AIManager : MonoBehaviour
{
    private GameObject[] ConnectedBuildings;
    private int[] TeamIndexes;
    private bool[] ActivatedRoutes;
    private int CurrentArmy = 0;
    private int Team;
    CBuildingManager BuildingManager;
    List<int> ConnectedTeams;

    //AI bazý adýmlar uygular.
    //1. boþa git
    //2. düþmana git
    //3. dosta git
    //4. canýn 5 in altýna düþerse göndermeyi býrak
    //5. canýn 10 un üstüne çýkýnca göndermeye baþla
    //her 3 snde yeniden hesapla
    private void Start()
    {
        BuildingManager = gameObject.GetComponent<CBuildingManager>();
        ConnectedBuildings = BuildingManager.GetConnectedBuildings();
        CurrentArmy = BuildingManager.GetCurrentArmy();
        ActivatedRoutes = BuildingManager.GetActivatedRoutes();
        Team = BuildingManager.GetTeam();
        ConnectedTeams = new List<int>();
        StartCoroutine(CheckConnectedBuildingStatus());

    }
    IEnumerator CheckConnectedBuildingStatus()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            Team = BuildingManager.GetTeam();
            ActivatedRoutes = BuildingManager.GetActivatedRoutes();
            if (Team == 0 || Team == 1)
            {
                continue;
            }
            if (ActivatedRoutes.Contains(true))
            {

            
                bool IsAllConnectedTeamSame = true;
                for (int a = 0; a < ConnectedBuildings.Length; a++)
                {
                    if(ConnectedBuildings[a].GetComponent<CBuildingManager>().GetTeam() != ConnectedTeams[a])
                    {
                       IsAllConnectedTeamSame = false;break;
                    }
                }
              if (IsAllConnectedTeamSame)
               {
                  continue;
               }
            }


            int i = 0;
            bool IsAnyActive = false;
            foreach (bool active in ActivatedRoutes)
            {
                
                if (active)
                {
                    BuildingManager.ActivateArmyMovement(i);
                    IsAnyActive = true;
                }
                i++;
            }
            CurrentArmy = BuildingManager.GetCurrentArmy();
            if (CurrentArmy < 5)
            {
                continue;
            }
            else if (!IsAnyActive && CurrentArmy < 10)
            {
                continue;
            }
            List<int> team_ids = new List<int>();
            foreach (GameObject connected_building in ConnectedBuildings)
            {
                
                int team = connected_building.GetComponent<CBuildingManager>().GetTeam();
                team_ids.Add(team);

            }
            if (team_ids.Contains(0))
            {
                //Do empty with index
                int index = 0;
                foreach(int id in team_ids)
                {
                    if(id == 0)
                    {
                        //Connect route with index
                        Debug.Log(index);
                        BuildingManager.ActivateArmyMovement(index);
                        
                        break;
                    }
                    index++;
                }
            }
            else{
                int index = 0;
                bool IsAnotherFound = false;
                foreach (int id in team_ids)
                {
                    if (id != Team)
                    {
                        //Connect route with index
                        BuildingManager.ActivateArmyMovement(index);
                        IsAnotherFound = true;
                        break;
     
                    }
                        
                    index++;
                    
                        
                }
                index = 0;
                foreach (int id in team_ids)
                {
                    if (id == Team && !IsAnotherFound)
                    {
                        //Connect route with index
                        BuildingManager.ActivateArmyMovement(index);
                        break;
                    }
                    index++;
                }
            }
        

            ConnectedTeams.Clear();
            foreach (GameObject go in ConnectedBuildings)
            {
                ConnectedTeams.Add(go.GetComponent<CBuildingManager>().GetTeam());
            }

        }
        
    }
}
