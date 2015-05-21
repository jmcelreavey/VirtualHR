using Leap;
using System;

namespace FinalYearProject
{
    public class CustomListener : Listener
    {
        public event Action<Hand> OnHandRegistered;

        public override void OnFrame(Controller controller)
        {
            using (var frame = controller.Frame())
            {
                if (!frame.Hands.IsEmpty && OnHandRegistered != null)
                {
                    Hand hand = frame.Hands[0];

                    OnHandRegistered(hand);
                }
            }
        }
    }
}
