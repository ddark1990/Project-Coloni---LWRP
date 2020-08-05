using System;
using UnityEngine;

namespace ProjectColoni
{
    public class UI_SkillPanel : MonoBehaviour
    {
        [SerializeField] private UI_SkillTab[] skillTabs;


        private void OnEnable()
        {
            if (SelectionManager.Instance.currentlySelectedObject == null) return;
            
            PopulateSkills();
        }

        private void PopulateSkills()
        {
            var skillList = SelectionManager.Instance.currentlySelectedObject.GetComponent<ColonistSkills>().skillSet
                .Skills;
            
            for (int i = 0; i < skillTabs.Length; i++)
            {
                for (int j = 0; j < skillList.Count; j++)
                {
                    var skillTab = skillTabs[i];
                    var skill = skillList[i];

                    //skill name
                    var cleanName = skill.name.Replace("(Clone)", "");
                    
                    skillTab.skillName.text = cleanName;
                    
                    //skill bar
                    var levelFloat = skill.Level / 20f;
                    
                    skillTab.skillBar.fillAmount = levelFloat;
                    
                    //skill level
                    skillTab.skillLevel.text = skill.Level.ToString();
                    var skillInt = int.Parse(skillTab.skillLevel.text);
                    
                    if (skillInt <= -1)
                    {
                        skillTab.skillLevel.text = "-";
                    }

                    //passion level
                    var hasPassion = skill.PassionLevel > 0;
                    var color = skillTab.passionIcon.color;
                        
                    color = hasPassion ? new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, 1, Time.deltaTime * 15)) : new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, 0, Time.deltaTime * 15));
                    skillTab.passionIcon.color = color;

                }
            }
        }
    }
}
