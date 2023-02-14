using System;
using System.Collections.Generic;

[Serializable]
public struct SquadMision
{
    public List<MisionType> misionTypes;
    public int membersAssigned;
    public int priority;
    public int minimunMembersToMision;
}