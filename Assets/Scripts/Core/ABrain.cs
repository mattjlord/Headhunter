using UnityEngine;

public abstract class ABrain : MonoBehaviour
{
    [SerializeField] protected AIOrganism organism;

    private void Start()
    {
        InitDefaultTasks();
    }

    public AIOrganism Organism { get { return organism; } }

    public void RespondToStimulus(Stimulus stimulus)
    {
        if (organism.Memory.IsStimulusActive(stimulus) || organism.LocationKnowledge.IsLocationBlocked(stimulus.Location) || stimulus.ProducerOrganism == organism)
            return;
        
        StimulusInterpretation interpretation = AcceptAndInterpret(stimulus);
        StimulusResponseType responseType = interpretation.EvaluateResponseType();
        BehaviorTask task = GenerateStimulusResponseTask(stimulus, responseType);
        task.Priority = interpretation.EvaluatePriority();

        if (task.Priority > 0)
        {
            //Debug.Log(organism.OrganismType + " " + LanguageUtils.GetSenseVerb(stimulus.SenseType) + " " + stimulus.GetDescription() + ", response type: " + responseType + " (priority " + task.Priority + ")");
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

    public abstract StimulusInterpretation AcceptAndInterpret(Stimulus stimulus);

    public abstract void AcceptAndInteract(Stimulus stimulus, StimulusResponseType type);

    public StimulusResponseTask GenerateStimulusResponseTask(Stimulus stimulus, StimulusResponseType responseType)
    {
        return new StimulusResponseTask(organism, stimulus, responseType, this);
    }

    // Actions
    public void Eat(FoodOrWaterObject obj)
    {
        ActionManagement actionManagement = organism.ActionManagement;

        if (actionManagement.IsReadyForQueue())
        {
            OrganismAction action = new OrganismAction();
            action.Duration = 1.0f; // Placeholder, until an eating animation is added
            action.TriggeredAction = () => obj.ConsumeThis(organism);
            actionManagement.QueueAction(action);
        }
    }
}
