using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Splines;

public class CArmyUnitManager : MonoBehaviour
{
    private GameObject SplineObject;
    private GameObject SpawnerObject;
    private int TeamNumber = 0;
    private string ObjectType = "army";
    public SplineContainer spline;
    public float speed = 1f;
    float distancePercentage = 0f;
    bool IsSplineSet = false;
    [SerializeField] GameObject AudioHitAnotherArmy;
    [SerializeField] GameObject AudioHitBuilding;

    float splineLength;

    public string GetObjectType()
    {
        return ObjectType;
    }
    public void InitializeArmyUnit(GameObject spline_obj, GameObject spawner_obj, int team)
    {
        SpawnerObject = spawner_obj;
        TeamNumber = team;
        SplineObject = spline_obj;
        spline = SplineObject.GetComponent<SplineContainer>();
        splineLength = spline.CalculateLength();
        IsSplineSet = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsSplineSet)
        {
            return;
        }
        distancePercentage += speed * Time.deltaTime / splineLength;

        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;
        if (distancePercentage > 1f)
        {
            distancePercentage = 0f;
        }

        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.05f);
        Vector3 direction = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }
    public void SetMaterials(Material mat)
    {
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material = mat;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject != SpawnerObject)
        {
            if (collision.gameObject.GetComponent<CArmyUnitManager>() != null)
            {
                if(collision.gameObject.GetComponent<CArmyUnitManager>().GetTeamNumber() != TeamNumber)
                {
                    AudioHitBuilding.GetComponent<AudioSource>().Play();
                    Destroy(gameObject);

                }
            }
            else
            {
                AudioHitAnotherArmy.GetComponent<AudioSource>().Play();
                collision.gameObject.GetComponent<CBuildingManager>().OnArmyEnter(TeamNumber);
                Destroy(gameObject);
            }
            
        }
    }
    public int GetTeamNumber()
    {
        return TeamNumber;
    }
    
}
