using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Skill", menuName = "ProjectColoni/Objects/AI/Skills/New Skill", order = 4)]
public class Skill : ScriptableObject {

	[Header ("Instance values")] // Overridden on load.
	[SerializeField] int level; // If -1, skill is disabled via traits
	[SerializeField] int passionLevel; // 0 - no passion, 1 - mild interest, 2 - very passionate
	[SerializeField] bool disabled = false;

	[Header ("Constant values")]
	[SerializeField] float difficultyLearnMult;

	private const float DEFAULT_LEARN_SPEED = 0.33f;
	private float _levelWorkSpeedMultiplier;
    public float xpToNextLevel;

    // Cool shorthand for properties
    public float XpToNextLevel
    {
        get
        {
            return xpToNextLevel;
        }

        set
        {
            xpToNextLevel = value;
        }
    }

    void Awake () {

		_levelWorkSpeedMultiplier = level / 4f - level / 8f;
	}

	/** Getters/Setters */
	/// <summary>Level of mastery for this skill.</summary>
	public int Level {
		get { return level; }
		set {
			if (disabled) {
				level = -1;
				return;
			}
			level = Mathf.Clamp (value, 0, 20); // Keeps level from being negative or greater than 20.
		}
	}
	
	/// <summary>How passionate the colonist is about this skill. Enhances learn rate.</summary>
	public int PassionLevel {
		get { return passionLevel; }
		set { passionLevel = Mathf.Clamp (value, 0, 2); }
	}

	/// <summary>Determines if the person will do this type of world.</summary>
	public bool Disabled {
		get { return disabled; }
		set {
			disabled = value;
			if (disabled) {
				level = -1;
			}
		}
	}

	/// <summary>The constant default learn speed when passionLevel = 0.</summary>
	public float DefaultLearnSpeed { get { return DEFAULT_LEARN_SPEED; } }

	/// <summary>The rate at which learning is hindered based on how difficult the skill is to learn
	/// (i.e. Medicine takes longer than Mining to learn).</summary>
	public float DifficultyLearnMult { get { return difficultyLearnMult; } }
}