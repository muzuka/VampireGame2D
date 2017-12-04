using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attackable.
/// Use if object can be attacked and destroyed
/// </summary>
[RequireComponent(typeof(DebugComponent))]
public class Attackable : MonoBehaviour {

	// maximum health of object
	public float maxHealth { get; set; }

	// current health of object
	public float health { get; set; }

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
		if (health <= 0) 
		{
			if (GetComponent<DebugComponent>().debug)
				Debug.Log(gameObject.name + " ran out of health.");

            EventManager.TriggerEvent("LoseGame");
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Increases health by amount.
	/// </summary>
	/// <param name="repairAmount">Repair amount.</param>
	public void repair (float repairAmount)
	{
		if (health + repairAmount >= maxHealth)
			health = maxHealth;
		else
			health += repairAmount;
	}

	/// <summary>
	/// Sets the health.
	/// For object initialization only.
	/// </summary>
	/// <param name="health">Health.</param>
	public void setHealth (float health) 
	{
		this.health = health;
		maxHealth = health;
	}

	/// <summary>
	/// Returns the current health of object.
	/// </summary>
	/// <returns>The health.</returns>
	public float getHealth () 
	{
		return health;
	}

	/// <summary>
	/// Sets the health for slider object.
	/// </summary>
	/// <param name="slider">Slider script.</param>
	public void setHealth (Slider slider) 
	{
		slider.value = health;
		slider.minValue = 0;
		slider.maxValue = maxHealth;
	}

	/// <summary>
	/// Decreases health by amount.
	/// </summary>
	/// <param name="damage">Damage amount.</param>
	public void attacked (float damage) 
	{
		health = health - damage;
	}
}
