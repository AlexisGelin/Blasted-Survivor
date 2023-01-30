using BaseTemplate.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoSingleton<ColorManager>
{
    public Color UIBlueGreenLight, UIBlueGreenDark, UIGreenLight, UIGreenDark, UIRedLight, UIRedDark, UIYellowLight, UIYellowDark, UIBlueLight, UIBlueDark, UIPurpleLight, UIPurpleDark;
    public Color GreenLight, GreenDark, RedLight, RedDark, BlueLight, BlueDark, PurpleLight, PurpleDark, GreyLight, GreyDark;
}
