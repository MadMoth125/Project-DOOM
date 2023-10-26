using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDOOM.Weapons.V2
{
	public abstract class TriggerMethod_SO : ScriptableObject
    {
    	public abstract void ShouldFire();
    	
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
    
    [CreateAssetMenu(fileName = "Single Fire Method", menuName = "Weapons/Single Fire Method", order = 1)]
    public class SingleTriggerMethod_SO : TriggerMethod_SO
    {
    	public override void ShouldFire()
    	{
    		if (Input.GetMouseButtonDown(0))
    		{
    			Debug.Log("ManualTriggerType_SO");
    		}
    	}
    	
    	public void ShouldFire(bool[] conditions)
    	{
    		if (CheckConditionals(conditions) && Input.GetMouseButtonDown(0))
    		{
    			Debug.Log("ManualTriggerType_SO");
    		}
    	}
    }
    
    [CreateAssetMenu(fileName = "Auto Fire Method", menuName = "Weapons/Auto Fire Method", order = 1)]
    public class AutomaticTriggerMethod_SO : TriggerMethod_SO
    {
    	public override void ShouldFire()
    	{
    		if (Input.GetMouseButton(0))
    		{
    			Debug.Log("AutomaticTriggerType_SO");
    		}
    	}
    	
    	public void ShouldFire(bool[] conditions)
    	{
    		if (CheckConditionals(conditions) && Input.GetMouseButton(0))
    		{
    			Debug.Log("AutomaticTriggerType_SO");
    		}
    	}
    }
}