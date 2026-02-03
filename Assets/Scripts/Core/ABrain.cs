using UnityEngine;

public abstract class ABrain : MonoBehaviour
{
    [SerializeField] protected AIOrganism organism;

    private void Start()
    {
        InitDefaultTasks();
    }

    public void RespondToStimulus(AStimulus stimulus)
    {
        StimulusInterpretation interpretation = AcceptStimulus(stimulus);
        StimulusResponseType responseType = interpretation.EvaluateResponseType();
        BehaviorTask task = GenerateStimulusResponseTask(stimulus, responseType);
        if (task.Priority > 0)
            organism.TaskManagement.AddTask(task);
    }

    protected virtual void InitDefaultTasks()
    {
        organism.TaskManagement.ClearTasks();
        organism.TaskManagement.AddTask(new VitalTask(organism, VitalType.Hunger));
        organism.TaskManagement.AddTask(new VitalTask(organism, VitalType.Thirst));
        organism.TaskManagement.AddTask(new VitalTask(organism, VitalType.Exhaustion));
        organism.TaskManagement.AddTask(new VitalTask(organism, VitalType.Heat));
        organism.TaskManagement.AddTask(new VitalTask(organism, VitalType.Injury));
    }

    public abstract StimulusInterpretation AcceptStimulus(AStimulus stimulus);

    public abstract  StimulusResponseTask GenerateStimulusResponseTask(AStimulus stimulus, StimulusResponseType responseType);
}
