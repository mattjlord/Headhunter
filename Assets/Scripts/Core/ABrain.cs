using UnityEngine;

public abstract class ABrain : MonoBehaviour
{
    [SerializeField] protected AIOrganism organism;

    private void Start()
    {
        InitDefaultTasks();
    }

    public AIOrganism Organism { get { return organism; } }

    public void RespondToStimulus(AStimulus stimulus)
    {
        if (organism.Memory.IsStimulusActive(stimulus) || organism.LocationKnowledge.IsLocationBlocked(stimulus.Location))
            return;
        
        StimulusInterpretation interpretation = AcceptStimulus(stimulus);
        StimulusResponseType responseType = interpretation.EvaluateResponseType();
        BehaviorTask task = GenerateStimulusResponseTask(stimulus, responseType);
        task.Priority = interpretation.EvaluatePriority();

        if (task.Priority > 0)
        {
            organism.Memory.AddStimulus(stimulus);
            organism.TaskManagement.AddTask(task);
        }
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

    public StimulusResponseTask GenerateStimulusResponseTask(AStimulus stimulus, StimulusResponseType responseType)
    {
        return new StimulusResponseTask(organism, stimulus, responseType);
    }
}
