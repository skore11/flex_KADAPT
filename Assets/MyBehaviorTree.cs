﻿using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using UnityEngine.UI;

public class MyBehaviorTree : MonoBehaviour
{
    public Transform wander1;
    public Transform wander2;
    public Transform wander3;
    public bool meltSelection;
    public GameObject participant;
    public GameObject participant2;

    public int iteration1;
    public int iteration2;

    public float gravityVal;

    private BehaviorAgent behaviorAgent;
    private BehaviorAgent behaviorAgent2;
    // Use this for initialization

    public Text debugText;

    public UIController uIController;
    //public enum iter
    //{
    //    rigid = 10,
    //    loose = 15
    //};
    private BehaviorUpdater behaviorUpdater;

    void Start()
    {
        behaviorUpdater = FindObjectOfType<BehaviorUpdater>();

        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();

        //behaviorAgent2 = new BehaviorAgent(this.Trial());
        //BehaviorManager.Instance.Register(behaviorAgent2);
        //behaviorAgent2.StartBehavior();
    }

    // Update is called once per frame
    void Update()
    {
        String DebugOutput = "";
        DebugOutput += Node.PrintTree(behaviorAgent.treeRoot);
        foreach (Node n in behaviorAgent.treeRoot.Trace())
        {
            String hCode = "" + n.GetHashCode();
            DebugOutput = DebugOutput.Replace(hCode, "<b>" + hCode + "</b>");
        }
        if (debugText != null)
        {
            debugText.text = DebugOutput;
            Debug.Log(DebugOutput);
        }
        //try to turn of behavior updater here and restart when needed!
        if (uIController.turnOffAnim)
        {
            behaviorUpdater.enabled = false;
        }
    }

    protected Node ST_ApproachAndWait(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    protected Node ST_Melt(bool x)
    {
        return new Selector(participant2.GetComponent<FlexController>().Node_Melt(x));
    }

    
    //protected Node ST_Iter(iter iter)
    //{

    //    foreach (int item in iter.GetValues(typeof(iter)))
    //    {
    //        print(item);
    //        return new Sequence(participant2.GetComponent<FlexController>().Node_Iter(item)); 
    //    }
    //    return new Selector();
    //}

    protected Node ST_Iter(int iter)
    {
        FlexController flexController = participant2.GetComponent<FlexController>();
        return new Selector(new LeafInvoke(
            () => flexController.flexParams = iter
        ));
    }

    protected Node ST_Jiggle(bool x)
    {
        return new Selector(participant2.GetComponent<FlexController>().Node_Jiggle(x));
    }

    protected Node ST_Gravity(float y_gravity)
    {
        FlexController flexController = participant2.GetComponent<FlexController>();
        return new Selector(new LeafInvoke(
            () => flexController.gravityY = y_gravity
            ));
    }

    //protected Node ST_pauseTree()
    //{
        
       

    //}

    //protected Node ST_deform()
    //{
    //    FlexController flexController = participant2.GetComponent<FlexController>();
    //    return new Selector(new LeafInvoke(
    //        () => flexController.getBehavior.assign 
    //        )
    //        )
    //}

    protected Node BuildTreeRoot()
	{
        //iter value = new iter();
        Node roaming = new DecoratorLoop(
                        //new Selector(this.pauseAnim,
                        new SequenceShuffle(
                            //this.ST_Melt(meltSelection),
                            
                            this.ST_Iter(/*value = */this.iteration1),
                            this.ST_Iter(this.iteration2),
                        //this.ST_Jiggle(true),
                        //this.ST_Jiggle(false),
                        this.ST_ApproachAndWait(this.wander1),
                        this.ST_ApproachAndWait(this.wander2),
                        this.ST_ApproachAndWait(this.wander3)));//);
        return roaming;

    }

    protected Node Trial()
    {
        Node gravity = new DecoratorLoop(
                        new Sequence(
                            this.ST_Gravity(gravityVal),
                            this.ST_Gravity(-9.8f), new LeafWait(500)
                            )
                            );
        return gravity;
            
    }
}
