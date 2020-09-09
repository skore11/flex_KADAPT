using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;
using uFlex;

public class FlexController : FlexProcessor
{
    private MeltFlexProcessor melt = null;

    private JiggleFlexProcessor jiggle = null;

    public GetBehaviors getBehavior;

    public int flexParams;

    private bool iterBool;

    public FlexParameters flex_Params;

    public float gravityY;

    // Start is called before the first frame update
    void Awake() { this.Initialize(); }

    protected void Initialize()
    {
        this.melt = this.GetComponent<MeltFlexProcessor>();
        this.jiggle = this.GetComponent<JiggleFlexProcessor>();
        this.getBehavior = FindObjectOfType<GetBehaviors>();
        //this.flexParams = this.GetComponent<FlexParameters>();
    }

    public Node Node_Melt(Val<bool> trigger)
    {
        return new LeafInvoke(
            () => this.melt.Melt(trigger.Value)
            );
    }

    public Node Node_Jiggle(Val<bool> trigger)
    {
        return new LeafInvoke(
            () => this.jiggle.jiggle = trigger.Value
            );
    }
    public override void PostContainerUpdate(FlexSolver solver, FlexContainer cntr, FlexParameters parameters)
    {
  
            parameters.m_numIterations = this.flexParams;
            parameters.m_gravity.y = this.gravityY;
            //iterBool = false;

       
    }
    // Update is called once per frame
    void Update()
    {
        if (getBehavior != null)
        {

        }
        
    }
}
