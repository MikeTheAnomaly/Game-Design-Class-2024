using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CETeam
{
    

    [SerializeField]
    public CETeamType teamType;

    public CETeam(CETeamType teamType){
        this.teamType = teamType;

    }

}

[Serializable]
public enum CETeamType
{
    Team1,
    Team2
}