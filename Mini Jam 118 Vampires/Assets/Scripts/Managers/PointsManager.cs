using UnityEngine;
using TMPro;

public class PointsManager : MonoBehaviour
{
    int actualPoints;
    [SerializeField]
    private TextMeshProUGUI pointTextHud;
    public static PointsManager Instance {private set; get;}

    void Awake()
    {
        Instance = this;
    }

    public void AddPoints(int points)
    {
        actualPoints += points;
        pointTextHud.text = actualPoints.ToString();
    }
    
}
