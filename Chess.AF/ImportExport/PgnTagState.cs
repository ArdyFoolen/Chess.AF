using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF.ImportExport
{
    public abstract class PgnTagState
    {
        protected IPgnTagStateContext Context;
        protected Dictionary<StatesEnum, PgnTagState> States;
        protected Dictionary<string, string> EventTags;

        public static PgnTagState CreateInitialState(IPgnTagStateContext context, Dictionary<string, string> eventTags)
        {
            var states = new Dictionary<StatesEnum, PgnTagState>();

            states.Add(StatesEnum.Event, new PgnTagEventState(context, states, eventTags));
            states.Add(StatesEnum.Site, new PgnTagSiteState(context, states, eventTags));
            states.Add(StatesEnum.Date, new PgnTagDateState(context, states, eventTags));
            states.Add(StatesEnum.Round, new PgnTagRoundState(context, states, eventTags));
            states.Add(StatesEnum.White, new PgnTagWhiteState(context, states, eventTags));
            states.Add(StatesEnum.Black, new PgnTagBlackState(context, states, eventTags));
            states.Add(StatesEnum.Result, new PgnTagResultState(context, states, eventTags));
            states.Add(StatesEnum.Optional, new PgnTagOptionalState(context, states, eventTags));

            return states[StatesEnum.Event];
        }

        private PgnTagState(IPgnTagStateContext context, Dictionary<StatesEnum, PgnTagState> states, Dictionary<string, string> eventTags)
        {
            this.Context = context;
            this.States = states;
            this.EventTags = eventTags;
        }

        public virtual Validation<KeyValuePair<string, string>> TryAddTagPair(string tagPair)
            => createKeyValuePair(tagPair).Bind(TryAddTagPair);

        public virtual Validation<KeyValuePair<string, string>> TryAddTagPair(KeyValuePair<string, string> kv)
        {
            if (!IsValidTag(kv.Key.ToLowerInvariant()))
                return Error($"Key {kv.Key} not valid for state {this.GetType().Name}");
            EventTags.Add(kv.Key.ToLowerInvariant(), kv.Value);
            ChangeContextState(Context);
            return kv;
        }

        private Validation<KeyValuePair<string, string>> createKeyValuePair(string tagPair)
        {
            string[] keyValueTagPair = tagPair.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            if (keyValueTagPair.Length != 1)
                return Error($"Not valid Tag Pair: {tagPair}");

            int index = keyValueTagPair[0].IndexOf(' ');
            string tag = keyValueTagPair[0].Substring(0, index).Trim();
            string value = removeQuotes(keyValueTagPair[0].Substring(index));

            return new KeyValuePair<string, string>(tag, value);
        }

        private string removeQuotes(string value)
            => value.Substring(0, value.Length - 1).Trim().Substring(1);

        protected abstract bool IsValidTag(string tag);
        protected abstract void ChangeContextState(IPgnTagStateContext context);

        private class PgnTagEventState : PgnTagState
        {
            public PgnTagEventState(IPgnTagStateContext context, Dictionary<StatesEnum, PgnTagState> states, Dictionary<string, string> eventTags) : base(context, states, eventTags) { }

            protected override bool IsValidTag(string tag)
                => !tag.Equals("event") ? false : true;

            protected override void ChangeContextState(IPgnTagStateContext context)
                => context.State = States[StatesEnum.Site];
        }
        private class PgnTagSiteState : PgnTagState
        {
            public PgnTagSiteState(IPgnTagStateContext context, Dictionary<StatesEnum, PgnTagState> states, Dictionary<string, string> eventTags) : base(context, states, eventTags) { }

            protected override bool IsValidTag(string tag)
                => !tag.Equals("site") ? false : true;

            protected override void ChangeContextState(IPgnTagStateContext context)
                => context.State = States[StatesEnum.Date];
        }
        private class PgnTagDateState : PgnTagState
        {
            public PgnTagDateState(IPgnTagStateContext context, Dictionary<StatesEnum, PgnTagState> states, Dictionary<string, string> eventTags) : base(context, states, eventTags) { }

            protected override bool IsValidTag(string tag)
                => !tag.Equals("date") ? false : true;

            protected override void ChangeContextState(IPgnTagStateContext context)
                => context.State = States[StatesEnum.Round];
        }
        private class PgnTagRoundState : PgnTagState
        {
            public PgnTagRoundState(IPgnTagStateContext context, Dictionary<StatesEnum, PgnTagState> states, Dictionary<string, string> eventTags) : base(context, states, eventTags) { }

            protected override bool IsValidTag(string tag)
                => !tag.Equals("round") ? false : true;

            protected override void ChangeContextState(IPgnTagStateContext context)
                => context.State = States[StatesEnum.White];
        }
        private class PgnTagWhiteState : PgnTagState
        {
            public PgnTagWhiteState(IPgnTagStateContext context, Dictionary<StatesEnum, PgnTagState> states, Dictionary<string, string> eventTags) : base(context, states, eventTags) { }

            protected override bool IsValidTag(string tag)
                => !tag.Equals("white") ? false : true;

            protected override void ChangeContextState(IPgnTagStateContext context)
                => context.State = States[StatesEnum.Black];
        }
        private class PgnTagBlackState : PgnTagState
        {
            public PgnTagBlackState(IPgnTagStateContext context, Dictionary<StatesEnum, PgnTagState> states, Dictionary<string, string> eventTags) : base(context, states, eventTags) { }

            protected override bool IsValidTag(string tag)
                => !tag.Equals("black") ? false : true;

            protected override void ChangeContextState(IPgnTagStateContext context)
                => context.State = States[StatesEnum.Result];
        }
        private class PgnTagResultState : PgnTagState
        {
            public PgnTagResultState(IPgnTagStateContext context, Dictionary<StatesEnum, PgnTagState> states, Dictionary<string, string> eventTags) : base(context, states, eventTags) { }

            protected override bool IsValidTag(string tag)
                => !tag.Equals("result") ? false : true;

            protected override void ChangeContextState(IPgnTagStateContext context)
                => context.State = States[StatesEnum.Optional];
        }
        private class PgnTagOptionalState : PgnTagState
        {
            public PgnTagOptionalState(IPgnTagStateContext context, Dictionary<StatesEnum, PgnTagState> states, Dictionary<string, string> eventTags) : base(context, states, eventTags) { }

            protected override bool IsValidTag(string tag)
                => true;

            protected override void ChangeContextState(IPgnTagStateContext context)
            { }
        }
    }
}
