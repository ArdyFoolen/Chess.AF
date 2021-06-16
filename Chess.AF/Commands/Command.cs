﻿using AF.Functional;
using Chess.AF.PositionBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF.Commands
{
    internal abstract class Command
    {
        public Option<IPositionAbstraction> Position { get; protected set; }

        public Command()
        {
            this.Position = None;
        }

        public Command(Option<IPositionAbstraction> position)
        {
            this.Position = position;
        }

        public abstract void Execute();
    }
}