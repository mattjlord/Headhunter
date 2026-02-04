using UnityEngine;

public class AIOrganism : Organism
{
    [SerializeField] private TaskManagement _taskManagement;
    [SerializeField] private HerdManagement _herdManagement;
    [SerializeField] private LocationKnowledge _locationKnowledge;
    [SerializeField] private Navigation _navigation;
    [SerializeField] private Memory _memory;

    public TaskManagement TaskManagement { get { return _taskManagement; } }
    public HerdManagement HerdManagement { get { return _herdManagement; } }
    public LocationKnowledge LocationKnowledge { get { return _locationKnowledge; } }
    public Navigation Navigation { get { return _navigation; } }
    public Memory Memory { get { return _memory; } }
}
  