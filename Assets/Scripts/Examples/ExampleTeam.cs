using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExampleTeam
{
    public static List<GameObject> Team1List = new List<GameObject>();
    public static List<GameObject> Team2List = new List<GameObject>();
    public static List<GameObject> AllTeamsList = new List<GameObject>();

    [SerializeField]
    public TeamType teamType;
    public ExampleTeam(TeamType teamType, GameObject trackedTeamObject){
        this.teamType = teamType;

        if(teamType == TeamType.Team1){
            Team1List.Add(trackedTeamObject);
        }else{
            Team2List.Add(trackedTeamObject);
        }
        AllTeamsList.Add(trackedTeamObject);

    }

    public ExampleTeam(TeamType teamType){
        this.teamType = teamType;

    }

    public void RemoveFromTeam(GameObject trackedTeamObject){

        if(teamType == TeamType.Team1){
            Team1List.Remove(trackedTeamObject);
        }else{
            Team2List.Remove(trackedTeamObject);
        }
        AllTeamsList.Remove(trackedTeamObject);

    }
}

[Serializable]
public enum TeamType
{
    Team1,
    Team2
}