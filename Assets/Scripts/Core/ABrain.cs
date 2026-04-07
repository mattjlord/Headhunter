using UnityEngine;

public abstract class ABrain : MonoBehaviour
{
    [SerializeField] protected AIOrganism organism;
    [SerializeField] private GameObject _carcass;

    private void Start()
    {
        InitDefaultTasks();
        InitDeathEvents();
        organism.OnStimulus += RespondToStimulus;
    }

    public AIOrganism Organism { get { return organism; } }

    public void RespondToStimulus(Stimulus stimulus)
    {
        if (organism.Memory.IsStimulusActive(stimulus) || organism.LocationKnowledge.IsLocationBlocked(stimulus.Location) || stimulus.ProducerOrganism == organism)
            return;

        if (organism.Memory.HasCloserSimilarStimulus(stimulus, organism.Position))
            return;
        
        StimulusInterpretation interpretation = AcceptAndInterpret(stimulus);
        StimulusResponseType responseType = interpretation.EvaluateResponseType();
        if (responseType == StimulusResponseType.Ignore) 
            return;
        BehaviorTask task = GenerateStimulusResponseTask(stimulus, responseType, interpretation);

        if (task.Priority > 0)
        {
            Debug.Log(organism.OrganismType + " " + LanguageUtils.GetSenseVerb(stimulus.SenseType) + " " + stimulus.GetDescription() + ", response type: " + responseType + " (priority " + task.Priority + ")");
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
        organism.TaskManagement.AddTask(new VitalTask(organism, VitalType.Toxicity));
    }

    public void InitDeathEvents()
    {
        InitDeathEvent(VitalType.Hunger);
        InitDeathEvent(VitalType.Thirst);
        InitDeathEvent(VitalType.Exhaustion);
        InitDeathEvent(VitalType.Heat);
        InitDeathEvent(VitalType.Injury);
        InitDeathEvent(VitalType.Toxicity);
    }

    private void InitDeathEvent(VitalType type)
    {
        organism.Vitals.GetVital(type).OnMaxValueReached += () => Die(type);
    }

    public abstract StimulusInterpretation AcceptAndInterpret(Stimulus stimulus);

    public abstract void AcceptAndInteract(Stimulus stimulus, StimulusResponseType type);

    public StimulusResponseTask GenerateStimulusResponseTask(Stimulus stimulus, StimulusResponseType responseType, StimulusInterpretation interpretation)
    {
        return new StimulusResponseTask(organism, stimulus, responseType, this, interpretation);
    }

    // Actions
    public void Eat(FoodOrWaterObject obj)
    {
        ActionManagement actionManagement = organism.ActionManagement;

        if (actionManagement.IsReadyForQueue())
        {
            OrganismAction action = new OrganismAction(organism);
            action.AnimationName = "Eat";
            action.Duration = 2.5f;
            action.TriggerDelay = 1f;
            action.TriggeredAction = () => obj.ConsumeThis(organism);
            actionManagement.QueueAction(action);
        }
    }

    public void Drink(FoodOrWaterObject obj)
    {
        ActionManagement actionManagement = organism.ActionManagement;

        if (actionManagement.IsReadyForQueue())
        {
            OrganismAction action = new OrganismAction(organism);
            action.AnimationName = "Eat";
            action.Duration = 2.5f;
            action.TriggerDelay = 1f;
            action.TriggeredAction = () => obj.ConsumeThis(organism);
            actionManagement.QueueAction(action);
        }
    }

    public abstract void Attack(Organism obj);

    public void Die(VitalType type)
    {
        if (_carcass != null)
            Instantiate(_carcass, VectorUtils.Vec2ToVec3(organism.Position), Quaternion.identity);
        organism.OnOrganismDie();
        Destroy(gameObject);
    }
}
