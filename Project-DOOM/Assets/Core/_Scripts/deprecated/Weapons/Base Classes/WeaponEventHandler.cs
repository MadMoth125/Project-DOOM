using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDOOM.deprecated
{
	public class WeaponEventHandler : MonoBehaviour
    {
    	protected readonly int firePropertyStringHash = Animator.StringToHash("Fire");
    	
    	public event Action OnFireBeginFrame; // should be called when fire anim starts
    	public event Action OnFireEndFrame; // should be called when fire anim ends
    	public event Action OnFireFrame; // should be called when projectile would be fired
    	
    	public bool CanFire { get; protected set; } = true;
    	public bool IsFiring { get; protected set; } = false;
    	
    	public Animator AnimatorComponent { get; protected set; }
    	public Gun GunReference { get; set; }
    	
    	#region Unity Functions
    
    	protected virtual void Awake()
    	{
    		AnimatorComponent = GetComponent<Animator>();
    	}
    
    	protected virtual void OnEnable()
    	{
    		OnFireBeginFrame += HandleFireState;
    		OnFireEndFrame += HandleFireState;
    		
    		GunReference.OnFireActivated += EventPlayerFired;
    	}
    	
    	protected virtual void OnDisable()
    	{
    		OnFireBeginFrame -= HandleFireState;
    		OnFireEndFrame -= HandleFireState;
    		
    		GunReference.OnFireActivated -= EventPlayerFired;
    	}
    	
    	#endregion
    
    	/// <summary>
    	/// Called from the Animator when the fire anim starts
    	/// </summary>
    	public virtual void EventFireStart()
    	{
    		OnFireBeginFrame?.Invoke();
    		Debug.Log($"{this} EventFireStart()");
    	}
    	
    	/// <summary>
    	/// Called from the Animator when the fire anim ends
    	/// </summary>
    	public virtual void EventFireEnd()
    	{
    		OnFireEndFrame?.Invoke();
    		Debug.Log($"{this} EventFireEnd()");
    	}
    	
    	/// <summary>
    	/// Called from the Animator when the fire anim would fire a projectile
    	/// </summary>
    	public virtual void EventFire()
    	{
    		OnFireFrame?.Invoke();
    		Debug.Log($"{this} EventFire()");
    	}
    
    	/// <summary>
    	/// Called at the start AND end of the fire animation.
    	/// </summary>
    	protected virtual void HandleFireState()
    	{
    		
    	}
    	
    	/// <summary>
    	/// Called when the player wants to fire.
    	/// </summary>
    	protected virtual void EventPlayerFired()
    	{
    		Debug.Log($"{this} EventPlayerFired()");
    	}
    }
}