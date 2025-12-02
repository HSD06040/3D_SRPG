using UnityEngine;

namespace SRPG.Command
{
    public struct MoveUnitCommand : ICommand
    {
        public BaseUnit Unit { get; }
        public Tile TargetTile { get; }

        public MoveUnitCommand(BaseUnit unit, Tile targetTile)
        {
            Unit = unit;
            TargetTile = targetTile;
        }

        public void Execute()
        {
            
        }

        public void Undo()
        {
            
        }
    }
}