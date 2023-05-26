using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardPanel : Panel
{
    public override void Init()
    {
        base.Init();

        HighscoreTable.Instance.Init();
    }

}
