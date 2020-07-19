using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonistSkills : MonoBehaviour {

	[SerializeField] public SkillSet skillSet;
	[SerializeField] public List<Trait> traits;

	[SerializeField] int id;

	/// <summary>Unique colonist ID</summary>
	public int ID {
		get { return id; }
	}

	// For debugging only
	public void LoadSkills (int id) { skillSet.LoadSkills (id); }
	public void SaveSkills (int id) { skillSet.SaveSkills (id); }
	public void GenerateNewValues (int id) { skillSet.GenerateSkillValues (id, traits); }

	void Start () {
		InitializeSkillSet ();
	}

	// Loads skill values from storage. Generates new values and saves them if none exist.
	public void InitializeSkillSet () {

		// Instantiate SkillSet.
		if (!skillSet.name.Contains (id.ToString())) {
			skillSet = Instantiate (skillSet);
			// Instantiate every Skill in the SkillSet.
			foreach (Skill s in skillSet.Skills.ToArray ()) {

				Skill newSkill = skillSet.Skills[skillSet.Skills.IndexOf (s)] = Instantiate (s);
				newSkill.name = newSkill.name.Replace ("(Clone) ", id.ToString()); // Replace "(Clone)" with the colonist ID on each obj.
			}
			skillSet.name = skillSet.name.Replace ("(Clone) ", id.ToString());
		}

		// Generate random values for each skill if no saved data for the Colonist exists (mapped by the ID).
		// Otherwise, if there is data, load it.
		if (!System.IO.File.Exists (Application.persistentDataPath + id)) {
			skillSet.GenerateSkillValues (id, traits);
			return;
		}
		skillSet.LoadSkills (id);
	}
}
