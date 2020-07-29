using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu (fileName = "SkillSet", menuName = "ProjectColoni/Objects/AI/Skills/New SkillSet", order = 4)]
public class SkillSet : ScriptableObject {

    [SerializeField] List<Skill> skills;
    public List<Skill> Skills {
        get { return skills; }
    }

    /// <summary>Generate new levels for skills and apply modifications from traits, then saves.</summary>
    public void GenerateSkillValues (int id, List<Trait> traits) {

        foreach (Skill s in skills) s.Level = Random.Range (1, 20); // Generate random skill values.
        ApplyTraitMods (traits); // Apply skill modifications from traits.
        SaveSkills (id); // Store skills for loading.
    }

    /// <summary>Store skill values in a file at the 'persistent data path'/colonistID.dat</summary>
    public void SaveSkills (int id) {

        FileStream file = File.Create (Application.persistentDataPath + "/" + id + ".dat");

        SkillData data = new SkillData ();
        foreach (Skill s in Skills) {

            data.skillValues.Add (new SkillData.Values {
                level = s.Level,
                passionLevel = s.PassionLevel,
                disabled = s.Disabled
            });
        }

        BinaryFormatter bf = new BinaryFormatter ();
        bf.Serialize (file, data);
        file.Close ();
    }

    /// <summary>Loads skill values from storage and applies them to the Skills on the colonist with ID of id.</summary>
    public void LoadSkills (int id) {

        string path = Application.persistentDataPath + id;
        if (!File.Exists (path)) return;
        FileStream file = File.Open (path, FileMode.Open);

        BinaryFormatter bf = new BinaryFormatter ();
        SkillData data = (SkillData) bf.Deserialize (file);

        file.Close ();


        foreach (Skill s in Skills) {

            SkillData.Values values = data.skillValues[Skills.IndexOf (s)];

            s.Level = values.level;
            s.PassionLevel = values.passionLevel;
            s.Disabled = values.disabled;
        }
    }

    /// <summary>Applies skill value modifications defined in the colonist's traits, to their skills.</summary>
    public void ApplyTraitMods (List<Trait> traits) {

        foreach (Trait t in traits) {
            foreach (Trait.SkillMod skillMod in t.SkillModifiers) {

                Skill moddedSkill = skills.Find ((Skill s) => { return s.name.Contains (skillMod.skillName); });
                moddedSkill.Level += skillMod.levelMod;
                moddedSkill.Disabled = skillMod.disabled;
                skills[skills.IndexOf (moddedSkill)] = moddedSkill;
            }
        }
    }

 
}

[System.Serializable]
class SkillData {

    [System.Serializable]
    public struct Values {

        public int level;
        public int passionLevel;
        public bool disabled;
    }

    public List<Values> skillValues;

    public SkillData () {
        skillValues = new List<Values> (12);
    }
}