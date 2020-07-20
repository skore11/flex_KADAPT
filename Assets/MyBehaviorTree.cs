using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree : MonoBehaviour
{
	public Transform wander1;
	public Transform wander2;
	public Transform wander3;
    public bool meltSelection;
	public GameObject participant;
    public GameObject participant2;

	private BehaviorAgent behaviorAgent;
    // Use this for initialization

    public enum iter
    {
        rigid = 1,
        loose = 15
    };

	void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	protected Node ST_ApproachAndWait(Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}

    protected Node ST_Melt(bool x)
    {
        return new Selector(participant2.GetComponent<FlexController>().Node_Melt(x));
    }

    protected Node ST_Iter(iter iter)
    {
        
        foreach (int item in iter.GetValues(typeof(iter)))
        {
            return new Selector(participant2.GetComponent<FlexController>().Node_Iter(item));
            
        }
        return new Selector();
    }

	protected Node BuildTreeRoot()
	{
        iter value =  new iter();
        Node roaming = new DecoratorLoop(
                        new SequenceShuffle(
                            //this.ST_Melt(meltSelection),
                            this.ST_Iter(/*value = */value),
						this.ST_ApproachAndWait(this.wander1),
						this.ST_ApproachAndWait(this.wander2),
						this.ST_ApproachAndWait(this.wander3)));
		return roaming;
	}
}
