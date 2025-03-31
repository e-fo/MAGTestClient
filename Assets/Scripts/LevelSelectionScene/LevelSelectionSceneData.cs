using UnityEngine;

public class LevelSelectionSceneData : MonoBehaviour
{
    [SerializeField] LevelSelectionPanel _levelSelectionPanel;
    public LevelSelectionPanel LevelSelectionPanel => _levelSelectionPanel;
}
