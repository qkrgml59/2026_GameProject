using UnityEngine;

[CreateAssetMenu(fileName = "DialogChoice", menuName = "Dialog System/DialogChoice")]
public class DialogChoicesSO : ScriptableObject
{
    public string text;
    public int nextId;
}
