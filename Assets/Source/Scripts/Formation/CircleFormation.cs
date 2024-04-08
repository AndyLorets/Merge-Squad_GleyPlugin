using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleFormation : FormationComponent
{
    internal override void PlaceAdditionalPointsInFormation(float radius, int additionalPointsCount)
    {
        ClearPoints();

        base.PlaceAdditionalPointsInFormation(radius, additionalPointsCount);
    }
}