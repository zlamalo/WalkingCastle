using Godot;

public class InformationLogService
{
    public void DisplayInformation(string information, Color? informationColor = null)
    {
        GlobalNodes.Instance.InformationLogUi?.AddLog(information, informationColor);
    }
}