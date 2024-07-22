using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CBuildingManager : MonoBehaviour
{
    [SerializeField] private int Team = 0;
    [SerializeField] private int StartingArmy = 0;
    [SerializeField] private string BuildingType = "";
    [SerializeField] private float ProductionSpeedMultiplier = 1f;
    [SerializeField] private float SendSpeedMultiplier = 1f;
    [SerializeField] private GameObject[] BuildingLevels;
    [SerializeField] private GameObject[] ConnectedBuildings;
    [SerializeField] private GameObject[] Splines;
    [SerializeField] private GameObject[] RouteIndicators;
    [SerializeField] private Material[] TeamMaterials;
    [SerializeField] private GameObject ArmyValueText;
    [SerializeField] private int CurrentArmy = 0;
    [SerializeField] private GameObject ArmyPrefab;
    [SerializeField] private bool[] ActivatedRoutes;
    private bool IsArmyProductionStopped = false;
    private bool IsArmyMovementStopped = false;
    private int CurrentBuildingLevel = 0;
    private float ArmyProductionSpeed = 1.1f;
    private float ArmySendSpeed = 1f;
    [SerializeField] private GameObject GameManager;
    void Start()
    {
        CurrentArmy = StartingArmy;
        ArmyValueText.GetComponent<TextMeshProUGUI>().text = CurrentArmy + "";
        foreach (var buildingLevel in BuildingLevels)
        {
            buildingLevel.GetComponent<Renderer>().material = TeamMaterials[Team];
            if (buildingLevel.name == "Tower lvl3" || buildingLevel.name == "Tower lvl4")
            {
                buildingLevel.transform.GetChild(0).GetComponent<Renderer>().material = TeamMaterials[Team];
            }

        }
        if (Team != 0)
        {
            StartCoroutine(TriggerArmyProduction());
        }
        else
        {
            IsArmyProductionStopped = true;
        }

        ArmyProductionSpeed /= ProductionSpeedMultiplier;
        ArmySendSpeed /= SendSpeedMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    IEnumerator TriggerArmyProduction()
    {
        while (!IsArmyProductionStopped)
        {
            yield return new WaitForSeconds(ArmyProductionSpeed);
            ProduceArmy();
            UpdatePowerValueText();
        }
    }
    IEnumerator TriggerArmyMovement(int move_towards_index_obj)
    {
        while (!IsArmyMovementStopped && ActivatedRoutes[move_towards_index_obj])
        {
            yield return new WaitForSeconds(ArmySendSpeed);
            if(CurrentArmy < 2)
            {
                IsArmyMovementStopped = true;
                
            }
            else
            {
                InstantiateMoveArmy(move_towards_index_obj);
                UpdatePowerValueText();
            }
            
        }
        if(IsArmyMovementStopped)
        {
            ActivateArmyMovement(move_towards_index_obj);
        }
    }

    void ProduceArmy()
    {
        CurrentArmy += 1;
        UpgradeBuilding();

    }
    void DecreaseArmy()
    {
        CurrentArmy -= 1;
    }
    void UpdatePowerValueText()
    {
        ArmyValueText.GetComponent<TextMeshProUGUI>().text = CurrentArmy + "";
    }

    void InstantiateMoveArmy(int move_towards_index_obj)
    {
        CurrentArmy -= 1;
        GameObject army_unit = Instantiate(ArmyPrefab, transform.position, transform.rotation);
        army_unit.GetComponent<CArmyUnitManager>().SetMaterials(TeamMaterials[Team]);
        army_unit.GetComponent<CArmyUnitManager>().InitializeArmyUnit(Splines[move_towards_index_obj], gameObject ,Team);
    }

    public void OnArmyEnter(int team)
    {
        if (team == Team)
        {
            ProduceArmy();
            ForwardArmy();
        }
        else
        {
            DecreaseArmy();
        }

        if (CurrentArmy < 0)
        {
            UpdateTeamType(team);
        }
        UpdatePowerValueText();
    }
    void UpdateTeamType(int team)
    {
        if (Team == 0 && team != 0)
        {
            IsArmyProductionStopped = false;
            StartCoroutine(TriggerArmyProduction());
        }
        Team = team;
        foreach (var buildingLevel in BuildingLevels)
        {
            buildingLevel.GetComponent<Renderer>().material = TeamMaterials[Team];
            if(buildingLevel.name == "Tower lvl3"|| buildingLevel.name == "Tower lvl4")
            {
                buildingLevel.transform.GetChild(0).GetComponent<Renderer>().material = TeamMaterials[Team];
            }
            CurrentArmy = 0;

        }
        if(Team == 1)
        {
            GameManager.GetComponent<CGameManager>().PlayBuildingCaptured();

        }
        else
        {
            GameManager.GetComponent<CGameManager>().PlayBuildingLost();
        }
        GameManager.GetComponent<CGameManager>().CheckIfAllBuildingsSameTeam();

    }
    void UpgradeBuilding()
    {
        
        if (CurrentArmy >= 10 & CurrentBuildingLevel == 0)
        {
            BuildingLevels[CurrentBuildingLevel].SetActive(false);
            CurrentBuildingLevel += 1;
            BuildingLevels[CurrentBuildingLevel].SetActive(true);
            ArmyProductionSpeed /= 1.2f;
            ArmySendSpeed /= 1.2f;
            GameManager.GetComponent<CGameManager>().PlayBuildingLevelUp();
        }
        else if (CurrentArmy >= 30 & CurrentBuildingLevel == 1)
        {
            BuildingLevels[CurrentBuildingLevel].SetActive(false);
            CurrentBuildingLevel += 1;
            BuildingLevels[CurrentBuildingLevel].SetActive(true);
            ArmyProductionSpeed /= 1.2f;
            ArmySendSpeed /= 1.2f;
            GameManager.GetComponent<CGameManager>().PlayBuildingLevelUp();
        }
        else if (CurrentArmy >= 60 & CurrentBuildingLevel == 2)
        {
            BuildingLevels[CurrentBuildingLevel].SetActive(false);
            CurrentBuildingLevel += 1;
            BuildingLevels[CurrentBuildingLevel].SetActive(true);
            ArmyProductionSpeed /= 1.2f;
            ArmySendSpeed /= 1.2f;
            GameManager.GetComponent<CGameManager>().PlayBuildingLevelUp();
        }
    }

    public int CheckIfBuildingConnected(GameObject obj)
    {
        int index = 0;
        Debug.Log("Other obj: "+obj.name);
        foreach (var building in ConnectedBuildings)
        {
            Debug.Log("Other obj: " + building.name);
            if (building.name == obj.name)
            {
                return index;
            }
            index++;
        }
        return -1;
    }
    public void ActivateArmyMovement(int index)
    {
        if (index != -1)
        {
            if (!ActivatedRoutes[index])
            {
                ActivatedRoutes[index] = true;
                IsArmyMovementStopped = false;
                RouteIndicators[index].SetActive(true);
                StartCoroutine(TriggerArmyMovement(index));
            }
            else
            {
                RouteIndicators[index].SetActive(false);
                ActivatedRoutes[index] = false;
            }
            
        }
        
    }
    public int GetTeam()
    {
        return Team;
    }
    public GameObject[] GetConnectedBuildings()
    {
        return ConnectedBuildings;
    }
    public int GetCurrentArmy()
    {
        return CurrentArmy;
    }
    public bool[] GetActivatedRoutes()
    {
        return ActivatedRoutes;
    }
    void ForwardArmy()
    {
        int index = 0;
        foreach (bool check in ActivatedRoutes)
        {
            if (check)
            {
                InstantiateMoveArmy(index);
                UpdatePowerValueText();
            }
            index++;
        }
    }
}
