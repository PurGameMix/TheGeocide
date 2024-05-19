using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Data.GameEvent.Definition
{
    public enum DialogGameTriggerType
    {
        DialogEnd
    }

    public static class DialogGameTriggerTypeExtensions
    {

        private static Dictionary<DialogGameTriggerType, GameEventType> _map = new Dictionary<DialogGameTriggerType, GameEventType>()
        {
            {DialogGameTriggerType.DialogEnd,GameEventType.DialogEnd }
        };
        public static GameEventType GetGameEventType(this DialogGameTriggerType dgtt)
        {

            if (_map.ContainsKey(dgtt))
            {
                return _map[dgtt];
            }

            throw new NotImplementedException();
        }
    }
}
