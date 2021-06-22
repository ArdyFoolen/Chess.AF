using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ImportExport
{
    internal abstract class PgnTagState
    {
        public virtual bool TryAddTagPair(IPgnTagStateContext context, Dictionary<string, string> eventTags, KeyValuePair<string, string> kv)
        {
            if (!IsValidTag(kv.Key.ToLowerInvariant()))
                return false;
            eventTags.Add(kv.Key.ToLowerInvariant(), kv.Value);
            ChangeContextState(context);
            return true;
        }

        protected abstract bool IsValidTag(string tag);
        protected abstract void ChangeContextState(IPgnTagStateContext context);
    }

    internal class PgnTagEventState : PgnTagState
    {
        protected override bool IsValidTag(string tag)
            => !tag.Equals("event") ? false : true;

        protected override void ChangeContextState(IPgnTagStateContext context)
            => context.State = new PgnTagSiteState();
    }
    internal class PgnTagSiteState : PgnTagState
    {
        protected override bool IsValidTag(string tag)
            => !tag.Equals("site") ? false : true;

        protected override void ChangeContextState(IPgnTagStateContext context)
            => context.State = new PgnTagDateState();
    }
    internal class PgnTagDateState : PgnTagState
    {
        protected override bool IsValidTag(string tag)
            => !tag.Equals("date") ? false : true;

        protected override void ChangeContextState(IPgnTagStateContext context)
            => context.State = new PgnTagRoundState();
    }
    internal class PgnTagRoundState : PgnTagState
    {
        protected override bool IsValidTag(string tag)
            => !tag.Equals("round") ? false : true;

        protected override void ChangeContextState(IPgnTagStateContext context)
            => context.State = new PgnTagWhiteState();
    }
    internal class PgnTagWhiteState : PgnTagState
    {
        protected override bool IsValidTag(string tag)
            => !tag.Equals("white") ? false : true;

        protected override void ChangeContextState(IPgnTagStateContext context)
            => context.State = new PgnTagBlackState();
    }
    internal class PgnTagBlackState : PgnTagState
    {
        protected override bool IsValidTag(string tag)
            => !tag.Equals("black") ? false : true;

        protected override void ChangeContextState(IPgnTagStateContext context)
            => context.State = new PgnTagResultState();
    }
    internal class PgnTagResultState : PgnTagState
    {
        protected override bool IsValidTag(string tag)
            => !tag.Equals("result") ? false : true;

        protected override void ChangeContextState(IPgnTagStateContext context)
            => context.State = new PgnTagOptionalState();
    }
    internal class PgnTagOptionalState : PgnTagState
    {
        protected override bool IsValidTag(string tag)
            => true;

        protected override void ChangeContextState(IPgnTagStateContext context)
        { }
    }
}
