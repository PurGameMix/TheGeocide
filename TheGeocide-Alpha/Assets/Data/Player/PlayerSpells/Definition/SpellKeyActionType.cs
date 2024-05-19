using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Data.Player.PlayerSpells.Definition
{
    public enum SpellKeyActionType
    {
        //Key press and holding
        Hold,
        //Cancel a key hold
        Cancel,
        //Released a key hit or a key hold
        Release
    }
}
