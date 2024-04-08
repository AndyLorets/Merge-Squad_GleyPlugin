using UnityEditor;
using UnityEngine;

public class SnakeFormation : FormationComponent
{
    internal override void PlaceAdditionalPointsInFormation(float spaceBetween, int additionalPointsCount)
    {
        for (int i = 1; i <= additionalPointsCount; i++)
        {
            Vector3 newPos = new Vector3(0, 0, spaceBetween * -i);
            GameObject go = new();
            go.transform.SetParent(transform);
            go.transform.localPosition = newPos;

            FormationPoints.Add(go.transform);
        }
    }
}