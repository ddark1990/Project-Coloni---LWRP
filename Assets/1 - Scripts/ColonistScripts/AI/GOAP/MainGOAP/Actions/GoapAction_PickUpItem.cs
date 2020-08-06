using UnityEngine;

namespace ProjectColoni
{
    public class GoapAction_PickUpItem : GoapAction
    {
        public float extraWaitTime = 0.5f;
    
        private float _startTime;
        private bool _completed;
        private Item _targetItem;
        private static readonly int pickUpItem = Animator.StringToHash("PickUpItem");

        public GoapAction_PickUpItem()
        {
            addPrecondition("forcePickUp", true);
            addEffect("pickUpItem", true);
        }

        public override void reset()
        {
            _startTime = 0f;
            _completed = false;
        }

        public override bool isDone()
        {
            return _completed;
        }

        public override bool requiresInRange()
        {
            return true;
        }

        public override bool checkProceduralPrecondition(AiController controller)
        {
            return true;
        }
    
        public override bool perform(AiController controller)
        {
            if (_startTime == 0)
            {
                _targetItem = target.GetComponent<Item>();

                //Debug.Log("Starting to pick up item.");
            
                _startTime = Time.time;
                controller.animator.SetTrigger(pickUpItem);
            }

            if ((Time.time - _startTime > controller.animator.GetCurrentAnimatorStateInfo(1).length + extraWaitTime)) //wait til animation is over
            {
                //Debug.Log("Picked up item.");
        
                //controller.inventory.AddItemToInventory(_targetItem);
            
                //controller.pickUpAvailable = false; //reset 
                _completed = true;
            }
       
            return true;
        }
    }
}
