using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Trait", menuName = "ProjectColoni/Objects/AI/Skills/New Trait", order = 4)]
public class Trait : ScriptableObject {

	[Header ("Player visible")]
	[SerializeField] [TextArea] private string description;

	[Header ("Modifiers")]
	[SerializeField] private int moodModifier;
	[SerializeField] private float workSpeedModifier;
	[SerializeField] private float movementSpeedModifier;
	[SerializeField] private float learnSpeedModifier;

	[System.Serializable]
	public struct SkillMod {
		public string skillName; // The skill name to affect the following values.
		public int levelMod; // The number to add to the level. Negative #s will decrease.
		public bool disabled; // Set to true to disable the colonists ability to work with this skill.
	}

	[Header ("Skill Modifiers/Restrictions")]
	[SerializeField] List<SkillMod> skillModifiers;

	/** Getters/Setters */
	/// <summary>Description of the trait.</summary>
	public string Description { get { return description; } }

	// For the following fields, use negative values for speed/effect reduction.
	/// <summary>Permanent mood effect this trait gives a person. 0 for no change.</summary>
	public int MoodModifier { get { return moodModifier; } }
	/// <summary>Workspeed multiplier. 1 for no change.</summary>
	public float WorkSpeedModifier { get { return workSpeedModifier; } }
	/// <summary>Movement speed per unit addition. 0 for no change.</summary>
	public float MovementSpeedModifier { get { return movementSpeedModifier; } }
	/// <summary>Learn speed addition.</summary>
	public float LearnSpeedModifier { get { return learnSpeedModifier; } }

	/// <summary>Skills this trait changes.</summary>
	public List<SkillMod> SkillModifiers { get { return skillModifiers; } }
}