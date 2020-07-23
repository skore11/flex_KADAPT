using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;
using uFlex;

public class FlexController : FlexProcessor
{
    private MeltFlexProcessor melt = null;

    public int flexParams;

    private bool iterBool;
    // Start is called before the first frame update
    void Awake() { this.Initialize(); }

    protected void Initialize()
    {
        this.melt = this.GetComponent<MeltFlexProcessor>();
        //this.flexParams = this.GetComponent<FlexParameters>();
    }

    public Node Node_Melt(Val<bool> trigger)
    {
        return new LeafInvoke(
            () => this.melt.Melt(trigger.Value)
            );
    }


    public override void PostContainerUpdate(FlexSolver solver, FlexContainer cntr, FlexParameters parameters)
    {
  
            parameters.m_numIterations = this.flexParams;
            //iterBool = false;

       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
