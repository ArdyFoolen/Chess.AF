﻿using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF
{
    internal class ResignCommand : Command
    {
        public ResignCommand(Option<Position> position) : base(position) { }

        public override void Execute()
            => Position = Position.Bind(p => p.Resign())
            .Match(None: () => Position,
                    Some: s => s);
    }
}
