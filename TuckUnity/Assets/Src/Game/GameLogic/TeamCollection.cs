using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TeamCollection 
{
    public const int kTeamCount = 2;

    public List<List<PlayerState>> teams { get; private set; }
    


    public static TeamCollection Create(List<PlayerState> playerStates)
    {
        TeamCollection teamCollection = new TeamCollection();
        teamCollection.teams = new List<List<PlayerState>>(kTeamCount);
        teamCollection.teams.Add(new List<PlayerState>());
        teamCollection.teams.Add(new List<PlayerState>());

        foreach(PlayerState player in playerStates)
        {
            teamCollection.teams[player.teamIndex].Add(player);
        }
        return teamCollection;
    }

    public List<PlayerState> GetTeam(int teamIndex)
    {
        return teams[teamIndex];
    }

    public PlayerState GetPartner(int teamIndex, int playerIndex)
    {
        foreach(PlayerState player in teams[teamIndex])
        {
            if(player.index != playerIndex)
            {
                return player;
            }
        }
        return null;
    }

}
