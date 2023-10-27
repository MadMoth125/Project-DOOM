using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDOOM.Weapons.V2
{
	public abstract class ConditionalTrigger
    {
	    public delegate void ShouldFireDelegate();
	    
    	public abstract void ShouldFire(ShouldFireDelegate fireDelegate, bool[] conditions);
    	
    	protected bool CheckConditionals( bool[] conditions)
    	{
    		if (conditions == null || conditions.Length == 0) return true;
    
    		foreach (bool condition in conditions)
    		{
    			// result &= condition; old implementation
    			// if any condition is false, early return false
    			if (!condition) return false;
    		}
    			
    		// if loop completes, all conditions must be true
    		return true;
    	}
    }

    public class ManualTrigger : ConditionalTrigger
    {
    	public override void ShouldFire(ShouldFireDelegate fireDelegate, bool[] conditions)
    	{
    		if (CheckConditionals(conditions) && Input.GetMouseButtonDown(0))
    		{
    			Debug.Log("ManualTriggerType_SO");
			    fireDelegate();
    		}
    	}
    }

    public class AutomaticTrigger : ConditionalTrigger
    {
    	public override void ShouldFire(ShouldFireDelegate fireDelegate, bool[] conditions)
    	{
    		if (CheckConditionals(conditions) && Input.GetMouseButton(0))
    		{
    			Debug.Log("AutomaticTriggerType_SO");
			    fireDelegate();
    		}
    	}
    }
}