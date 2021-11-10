using UnityEngine;

public class EnemyColourManager
{
    public Color GetVisualColour(EnemyColour colour)
    {
        switch (colour)
        {
           case EnemyColour.RED:
               return Color.red;
           case EnemyColour.GREEN:
               return Color.green;
           case EnemyColour.BLUE:
               return Color.blue;
           default:
               return Color.white;
        }
    }
    
}