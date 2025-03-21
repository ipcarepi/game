﻿using UnityEngine;

namespace CoverShooter.AI
{
    [Folder("Arithmetic")]
    public class Inc : BaseExpression
    {
        [ValueType(ValueType.Float)]
        public Value Value;

        public override string GetText(Brain brain)
        {
            return "Inc(" + Value.GetText(brain) + ")";
        }

        public override Value Evaluate(int id, State state)
        {
            return new Value(state.Dereference(ref Value).Float + 1);
        }

        public override ValueType GetReturnType(Brain brain)
        {
            return ValueType.Float;
        }
    }
}