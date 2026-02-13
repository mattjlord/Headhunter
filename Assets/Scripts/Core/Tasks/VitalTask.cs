using UnityEngine;

public class VitalTask : BehaviorTask
{
    private VitalType _vital;

    public VitalTask(AIOrganism organism, VitalType vital) : base(organism)
    {
        IsEssential = true;
        _vital = vital;
    }

    public override void UpdatePriority()
    {
        float rawValue = Organism.Vitals.GetVital(_vital).Value;
        float biasedValue = Organism.HerdManagement.GetHerdBiasedVitalValue(rawValue, _vital);
        Priority = biasedValue;
    }

    public override void Start()
    {
        IsFrozen = true;
    }

    public override void Stop()
    {
        IsFrozen = false;
    }

    public override void Update()
    {
        switch (_vital)
        {
            case VitalType.Hunger:
                LookForFood();
                return;
            case VitalType.Thirst:
                LookForWater();
                return;
            default:
                LookForShelter();
                return;
        }
    }

    private void LookForFood()
    {
        ALocation? closestFood = Organism.LocationKnowledge.GetClosestFood(Organism.Position);
        if (closestFood == null)
            return;
        if (!closestFood.LocationReachedByOrganism(Organism))
        {
            description = "Moving to location";
            Organism.Navigation.MoveTowards(Organism, closestFood.GetClosestPoint(Organism.Position), false);
        }
        else
        {
            description = "Location reached, wandering around";
            Organism.Navigation.WanderAround(Organism, closestFood, false);
        }
    }

    private void LookForWater()
    {
        ALocation? closestWater = Organism.LocationKnowledge.GetClosestWater(Organism.Position);
        if (closestWater == null)
            return;
        if (!closestWater.LocationReachedByOrganism(Organism))
        {
            description = "Moving to location";
            Organism.Navigation.MoveTowards(Organism, closestWater.GetClosestPoint(Organism.Position), false);
        }
        else
        {
            description = "Location reached, wandering around";
            Organism.Navigation.WanderAround(Organism, closestWater, false);
        }
    }

    private void LookForShelter()
    {
        ALocation? closestShelter = Organism.LocationKnowledge.GetClosestShelter(Organism.Position);
        if (closestShelter == null)
            return;
        if (!closestShelter.LocationReachedByOrganism(Organism))
        {
            description = "Moving to location";
            Organism.Navigation.MoveTowards(Organism, closestShelter.GetClosestPoint(Organism.Position), false);
        }
        else
        {
            description = "Location reached, wandering around";
            Organism.Navigation.WanderAround(Organism, closestShelter, false);
        }
    }

    public override string GetName()
    {
        switch (_vital)
        {
            case VitalType.Hunger:
                return "Looking for Food";
            case VitalType.Thirst:
                return "Looking for Water";
            default:
                return "Looking for Shelter";
        }
    }
}