using UnityEngine;

public class AIOrganism : Organism
{
    [SerializeField] private TaskManagement _taskManagement;
    [SerializeField] private HerdManagement _herdManagement;
    [SerializeField] private LocationKnowledge _locationKnowledge;
    [SerializeField] private Navigation _navigation;
    [SerializeField] private Memory _memory;

    protected override void Start()
    {
        base.Start();
        _memory.OrganismType = OrganismType;
    }

    public TaskManagement TaskManagement { get { return _taskManagement; } }
    public HerdManagement HerdManagement { get { return _herdManagement; } }
    public LocationKnowledge LocationKnowledge { get { return _locationKnowledge; } }
    public Navigation Navigation { get { return _navigation; } }
    public Memory Memory { get { return _memory; } }

    // TODO: Remove this later
    public string TaskMsg = "";
    public string ActionMsg = "";
    public string MoveMsg = "";

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        string str = TaskMsg + "\r\n" + ActionMsg + "\r\n" + MoveMsg;
        UnityEditor.Handles.Label(transform.position + (Vector3.up * 10), str);
        #endif
    }
}