using PChecker;
using PChecker.Actors;
using PChecker.Actors.Events;
using PChecker.Runtime;
using PChecker.Specifications;
using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Plang.CSharpRuntime;
using Plang.CSharpRuntime.Values;
using Plang.CSharpRuntime.Exceptions;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 162, 219, 414, 1998
namespace PImplementation
{
}
namespace PImplementation
{
    internal partial class eRequestFork : PEvent
    {
        public eRequestFork() : base() {}
        public eRequestFork (PMachineValue payload): base(payload){ }
        public override IPrtValue Clone() { return new eRequestFork();}
    }
}
namespace PImplementation
{
    internal partial class eGrantFork : PEvent
    {
        public eGrantFork() : base() {}
        public eGrantFork (IPrtValue payload): base(payload){ }
        public override IPrtValue Clone() { return new eGrantFork();}
    }
}
namespace PImplementation
{
    internal partial class eReleaseFork : PEvent
    {
        public eReleaseFork() : base() {}
        public eReleaseFork (PMachineValue payload): base(payload){ }
        public override IPrtValue Clone() { return new eReleaseFork();}
    }
}
namespace PImplementation
{
    internal partial class eStartEating : PEvent
    {
        public eStartEating() : base() {}
        public eStartEating (PMachineValue payload): base(payload){ }
        public override IPrtValue Clone() { return new eStartEating();}
    }
}
namespace PImplementation
{
    internal partial class eStopEating : PEvent
    {
        public eStopEating() : base() {}
        public eStopEating (PMachineValue payload): base(payload){ }
        public override IPrtValue Clone() { return new eStopEating();}
    }
}
namespace PImplementation
{
    internal partial class Fork : PMachine
    {
        private PMachineValue owner = null;
        public class ConstructorEvent : PEvent{public ConstructorEvent(IPrtValue val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPrtValue value) { return new ConstructorEvent((IPrtValue)value); }
        public Fork() {
            this.sends.Add(nameof(eGrantFork));
            this.sends.Add(nameof(eReleaseFork));
            this.sends.Add(nameof(eRequestFork));
            this.sends.Add(nameof(eStartEating));
            this.sends.Add(nameof(eStopEating));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(eGrantFork));
            this.receives.Add(nameof(eReleaseFork));
            this.receives.Add(nameof(eRequestFork));
            this.receives.Add(nameof(eStartEating));
            this.receives.Add(nameof(eStopEating));
            this.receives.Add(nameof(PHalt));
        }
        
        public void Anon(Event currentMachine_dequeuedEvent)
        {
            Fork currentMachine = this;
            PMachineValue philosopher = (PMachineValue)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PMachineValue TMP_tmp0 = null;
            PEvent TMP_tmp1 = null;
            owner = (PMachineValue)(((PMachineValue)((IPrtValue)philosopher)?.Clone()));
            TMP_tmp0 = (PMachineValue)(((PMachineValue)((IPrtValue)owner)?.Clone()));
            TMP_tmp1 = (PEvent)(new eGrantFork(null));
            currentMachine.TrySendEvent(TMP_tmp0, (Event)TMP_tmp1);
            currentMachine.TryGotoState<Taken>();
            return;
        }
        public void Anon_1(Event currentMachine_dequeuedEvent)
        {
            Fork currentMachine = this;
            PMachineValue philosopher_1 = (PMachineValue)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PrtBool TMP_tmp0_1 = ((PrtBool)false);
            TMP_tmp0_1 = (PrtBool)((PrtValues.SafeEquals(philosopher_1,owner)));
            if (TMP_tmp0_1)
            {
                currentMachine.TryGotoState<Available>();
                return;
            }
        }
        [Start]
        [OnEntry(nameof(InitializeParametersFunction))]
        [OnEventGotoState(typeof(ConstructorEvent), typeof(Available))]
        class __InitState__ : State { }
        
        [OnEventDoAction(typeof(eRequestFork), nameof(Anon))]
        class Available : State
        {
        }
        [OnEventDoAction(typeof(eReleaseFork), nameof(Anon_1))]
        [DeferEvents(typeof(eRequestFork))]
        class Taken : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class Philosopher_Deadlock : PMachine
    {
        private PrtInt id = ((PrtInt)0);
        private PMachineValue leftFork = null;
        private PMachineValue rightFork = null;
        private PrtInt forksHeld = ((PrtInt)0);
        public class ConstructorEvent : PEvent{public ConstructorEvent(PrtNamedTuple val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPrtValue value) { return new ConstructorEvent((PrtNamedTuple)value); }
        public Philosopher_Deadlock() {
            this.sends.Add(nameof(eGrantFork));
            this.sends.Add(nameof(eReleaseFork));
            this.sends.Add(nameof(eRequestFork));
            this.sends.Add(nameof(eStartEating));
            this.sends.Add(nameof(eStopEating));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(eGrantFork));
            this.receives.Add(nameof(eReleaseFork));
            this.receives.Add(nameof(eRequestFork));
            this.receives.Add(nameof(eStartEating));
            this.receives.Add(nameof(eStopEating));
            this.receives.Add(nameof(PHalt));
        }
        
        public void Anon_2(Event currentMachine_dequeuedEvent)
        {
            Philosopher_Deadlock currentMachine = this;
            PrtNamedTuple payload = (PrtNamedTuple)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PrtInt TMP_tmp0_2 = ((PrtInt)0);
            PrtInt TMP_tmp1_1 = ((PrtInt)0);
            PMachineValue TMP_tmp2 = null;
            PMachineValue TMP_tmp3 = null;
            PMachineValue TMP_tmp4 = null;
            PMachineValue TMP_tmp5 = null;
            TMP_tmp0_2 = (PrtInt)(((PrtNamedTuple)payload)["id"]);
            TMP_tmp1_1 = (PrtInt)(((PrtInt)((IPrtValue)TMP_tmp0_2)?.Clone()));
            id = TMP_tmp1_1;
            TMP_tmp2 = (PMachineValue)(((PrtNamedTuple)payload)["left"]);
            TMP_tmp3 = (PMachineValue)(((PMachineValue)((IPrtValue)TMP_tmp2)?.Clone()));
            leftFork = TMP_tmp3;
            TMP_tmp4 = (PMachineValue)(((PrtNamedTuple)payload)["right"]);
            TMP_tmp5 = (PMachineValue)(((PMachineValue)((IPrtValue)TMP_tmp4)?.Clone()));
            rightFork = TMP_tmp5;
            forksHeld = (PrtInt)(((PrtInt)(0)));
            currentMachine.TryGotoState<Thinking>();
            return;
        }
        public void Anon_3(Event currentMachine_dequeuedEvent)
        {
            Philosopher_Deadlock currentMachine = this;
            currentMachine.TryGotoState<Hungry>();
            return;
        }
        public void Anon_4(Event currentMachine_dequeuedEvent)
        {
            Philosopher_Deadlock currentMachine = this;
            PMachineValue TMP_tmp0_3 = null;
            PEvent TMP_tmp1_2 = null;
            PMachineValue TMP_tmp2_1 = null;
            TMP_tmp0_3 = (PMachineValue)(((PMachineValue)((IPrtValue)leftFork)?.Clone()));
            TMP_tmp1_2 = (PEvent)(new eRequestFork(null));
            TMP_tmp2_1 = (PMachineValue)(currentMachine.self);
            currentMachine.TrySendEvent(TMP_tmp0_3, (Event)TMP_tmp1_2, TMP_tmp2_1);
        }
        public void Anon_5(Event currentMachine_dequeuedEvent)
        {
            Philosopher_Deadlock currentMachine = this;
            PrtInt TMP_tmp0_4 = ((PrtInt)0);
            PrtBool TMP_tmp1_3 = ((PrtBool)false);
            PMachineValue TMP_tmp2_2 = null;
            PEvent TMP_tmp3_1 = null;
            PMachineValue TMP_tmp4_1 = null;
            PrtBool TMP_tmp5_1 = ((PrtBool)false);
            TMP_tmp0_4 = (PrtInt)((forksHeld) + (((PrtInt)(1))));
            forksHeld = TMP_tmp0_4;
            TMP_tmp1_3 = (PrtBool)((PrtValues.SafeEquals(forksHeld,((PrtInt)(1)))));
            if (TMP_tmp1_3)
            {
                TMP_tmp2_2 = (PMachineValue)(((PMachineValue)((IPrtValue)rightFork)?.Clone()));
                TMP_tmp3_1 = (PEvent)(new eRequestFork(null));
                TMP_tmp4_1 = (PMachineValue)(currentMachine.self);
                currentMachine.TrySendEvent(TMP_tmp2_2, (Event)TMP_tmp3_1, TMP_tmp4_1);
            }
            else
            {
                TMP_tmp5_1 = (PrtBool)((PrtValues.SafeEquals(forksHeld,((PrtInt)(2)))));
                if (TMP_tmp5_1)
                {
                    currentMachine.TryGotoState<Eating>();
                    return;
                }
            }
        }
        public void Anon_6(Event currentMachine_dequeuedEvent)
        {
            Philosopher_Deadlock currentMachine = this;
            PMachineValue TMP_tmp0_5 = null;
            PEvent TMP_tmp1_4 = null;
            PMachineValue TMP_tmp2_3 = null;
            PMachineValue TMP_tmp3_2 = null;
            PEvent TMP_tmp4_2 = null;
            PMachineValue TMP_tmp5_2 = null;
            currentMachine.Announce((Event)new eStartEating(null), currentMachine.self);
            TMP_tmp0_5 = (PMachineValue)(((PMachineValue)((IPrtValue)leftFork)?.Clone()));
            TMP_tmp1_4 = (PEvent)(new eReleaseFork(null));
            TMP_tmp2_3 = (PMachineValue)(currentMachine.self);
            currentMachine.TrySendEvent(TMP_tmp0_5, (Event)TMP_tmp1_4, TMP_tmp2_3);
            TMP_tmp3_2 = (PMachineValue)(((PMachineValue)((IPrtValue)rightFork)?.Clone()));
            TMP_tmp4_2 = (PEvent)(new eReleaseFork(null));
            TMP_tmp5_2 = (PMachineValue)(currentMachine.self);
            currentMachine.TrySendEvent(TMP_tmp3_2, (Event)TMP_tmp4_2, TMP_tmp5_2);
            currentMachine.Announce((Event)new eStopEating(null), currentMachine.self);
            forksHeld = (PrtInt)(((PrtInt)(0)));
            currentMachine.TryGotoState<Thinking>();
            return;
        }
        [Start]
        [OnEntry(nameof(InitializeParametersFunction))]
        [OnEventGotoState(typeof(ConstructorEvent), typeof(Init))]
        class __InitState__ : State { }
        
        [OnEntry(nameof(Anon_2))]
        class Init : State
        {
        }
        [OnEntry(nameof(Anon_3))]
        class Thinking : State
        {
        }
        [OnEntry(nameof(Anon_4))]
        [OnEventDoAction(typeof(eGrantFork), nameof(Anon_5))]
        class Hungry : State
        {
        }
        [OnEntry(nameof(Anon_6))]
        class Eating : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class Philosopher_DeadlockFree : PMachine
    {
        private PrtInt id_1 = ((PrtInt)0);
        private PMachineValue leftFork_1 = null;
        private PMachineValue rightFork_1 = null;
        private PrtInt forksHeld_1 = ((PrtInt)0);
        public class ConstructorEvent : PEvent{public ConstructorEvent(PrtNamedTuple val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPrtValue value) { return new ConstructorEvent((PrtNamedTuple)value); }
        public Philosopher_DeadlockFree() {
            this.sends.Add(nameof(eGrantFork));
            this.sends.Add(nameof(eReleaseFork));
            this.sends.Add(nameof(eRequestFork));
            this.sends.Add(nameof(eStartEating));
            this.sends.Add(nameof(eStopEating));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(eGrantFork));
            this.receives.Add(nameof(eReleaseFork));
            this.receives.Add(nameof(eRequestFork));
            this.receives.Add(nameof(eStartEating));
            this.receives.Add(nameof(eStopEating));
            this.receives.Add(nameof(PHalt));
        }
        
        public void Anon_7(Event currentMachine_dequeuedEvent)
        {
            Philosopher_DeadlockFree currentMachine = this;
            PrtNamedTuple payload_1 = (PrtNamedTuple)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PrtInt TMP_tmp0_6 = ((PrtInt)0);
            PrtInt TMP_tmp1_5 = ((PrtInt)0);
            PMachineValue TMP_tmp2_4 = null;
            PMachineValue TMP_tmp3_3 = null;
            PMachineValue TMP_tmp4_3 = null;
            PMachineValue TMP_tmp5_3 = null;
            TMP_tmp0_6 = (PrtInt)(((PrtNamedTuple)payload_1)["id"]);
            TMP_tmp1_5 = (PrtInt)(((PrtInt)((IPrtValue)TMP_tmp0_6)?.Clone()));
            id_1 = TMP_tmp1_5;
            TMP_tmp2_4 = (PMachineValue)(((PrtNamedTuple)payload_1)["left"]);
            TMP_tmp3_3 = (PMachineValue)(((PMachineValue)((IPrtValue)TMP_tmp2_4)?.Clone()));
            leftFork_1 = TMP_tmp3_3;
            TMP_tmp4_3 = (PMachineValue)(((PrtNamedTuple)payload_1)["right"]);
            TMP_tmp5_3 = (PMachineValue)(((PMachineValue)((IPrtValue)TMP_tmp4_3)?.Clone()));
            rightFork_1 = TMP_tmp5_3;
            forksHeld_1 = (PrtInt)(((PrtInt)(0)));
            currentMachine.TryGotoState<Thinking>();
            return;
        }
        public void Anon_8(Event currentMachine_dequeuedEvent)
        {
            Philosopher_DeadlockFree currentMachine = this;
            currentMachine.TryGotoState<Hungry>();
            return;
        }
        public void Anon_9(Event currentMachine_dequeuedEvent)
        {
            Philosopher_DeadlockFree currentMachine = this;
            PrtBool TMP_tmp0_7 = ((PrtBool)false);
            PMachineValue TMP_tmp1_6 = null;
            PEvent TMP_tmp2_5 = null;
            PMachineValue TMP_tmp3_4 = null;
            PMachineValue TMP_tmp4_4 = null;
            PEvent TMP_tmp5_4 = null;
            PMachineValue TMP_tmp6 = null;
            TMP_tmp0_7 = (PrtBool)((PrtValues.SafeEquals(id_1,((PrtInt)(4)))));
            if (TMP_tmp0_7)
            {
                TMP_tmp1_6 = (PMachineValue)(((PMachineValue)((IPrtValue)rightFork_1)?.Clone()));
                TMP_tmp2_5 = (PEvent)(new eRequestFork(null));
                TMP_tmp3_4 = (PMachineValue)(currentMachine.self);
                currentMachine.TrySendEvent(TMP_tmp1_6, (Event)TMP_tmp2_5, TMP_tmp3_4);
            }
            else
            {
                TMP_tmp4_4 = (PMachineValue)(((PMachineValue)((IPrtValue)leftFork_1)?.Clone()));
                TMP_tmp5_4 = (PEvent)(new eRequestFork(null));
                TMP_tmp6 = (PMachineValue)(currentMachine.self);
                currentMachine.TrySendEvent(TMP_tmp4_4, (Event)TMP_tmp5_4, TMP_tmp6);
            }
        }
        public void Anon_10(Event currentMachine_dequeuedEvent)
        {
            Philosopher_DeadlockFree currentMachine = this;
            PrtInt TMP_tmp0_8 = ((PrtInt)0);
            PrtBool TMP_tmp1_7 = ((PrtBool)false);
            PrtBool TMP_tmp2_6 = ((PrtBool)false);
            PMachineValue TMP_tmp3_5 = null;
            PEvent TMP_tmp4_5 = null;
            PMachineValue TMP_tmp5_5 = null;
            PMachineValue TMP_tmp6_1 = null;
            PEvent TMP_tmp7 = null;
            PMachineValue TMP_tmp8 = null;
            PrtBool TMP_tmp9 = ((PrtBool)false);
            TMP_tmp0_8 = (PrtInt)((forksHeld_1) + (((PrtInt)(1))));
            forksHeld_1 = TMP_tmp0_8;
            TMP_tmp1_7 = (PrtBool)((PrtValues.SafeEquals(forksHeld_1,((PrtInt)(1)))));
            if (TMP_tmp1_7)
            {
                TMP_tmp2_6 = (PrtBool)((PrtValues.SafeEquals(id_1,((PrtInt)(4)))));
                if (TMP_tmp2_6)
                {
                    TMP_tmp3_5 = (PMachineValue)(((PMachineValue)((IPrtValue)leftFork_1)?.Clone()));
                    TMP_tmp4_5 = (PEvent)(new eRequestFork(null));
                    TMP_tmp5_5 = (PMachineValue)(currentMachine.self);
                    currentMachine.TrySendEvent(TMP_tmp3_5, (Event)TMP_tmp4_5, TMP_tmp5_5);
                }
                else
                {
                    TMP_tmp6_1 = (PMachineValue)(((PMachineValue)((IPrtValue)rightFork_1)?.Clone()));
                    TMP_tmp7 = (PEvent)(new eRequestFork(null));
                    TMP_tmp8 = (PMachineValue)(currentMachine.self);
                    currentMachine.TrySendEvent(TMP_tmp6_1, (Event)TMP_tmp7, TMP_tmp8);
                }
            }
            else
            {
                TMP_tmp9 = (PrtBool)((PrtValues.SafeEquals(forksHeld_1,((PrtInt)(2)))));
                if (TMP_tmp9)
                {
                    currentMachine.TryGotoState<Eating>();
                    return;
                }
            }
        }
        public void Anon_11(Event currentMachine_dequeuedEvent)
        {
            Philosopher_DeadlockFree currentMachine = this;
            PMachineValue TMP_tmp0_9 = null;
            PEvent TMP_tmp1_8 = null;
            PMachineValue TMP_tmp2_7 = null;
            PMachineValue TMP_tmp3_6 = null;
            PEvent TMP_tmp4_6 = null;
            PMachineValue TMP_tmp5_6 = null;
            currentMachine.Announce((Event)new eStartEating(null), currentMachine.self);
            TMP_tmp0_9 = (PMachineValue)(((PMachineValue)((IPrtValue)leftFork_1)?.Clone()));
            TMP_tmp1_8 = (PEvent)(new eReleaseFork(null));
            TMP_tmp2_7 = (PMachineValue)(currentMachine.self);
            currentMachine.TrySendEvent(TMP_tmp0_9, (Event)TMP_tmp1_8, TMP_tmp2_7);
            TMP_tmp3_6 = (PMachineValue)(((PMachineValue)((IPrtValue)rightFork_1)?.Clone()));
            TMP_tmp4_6 = (PEvent)(new eReleaseFork(null));
            TMP_tmp5_6 = (PMachineValue)(currentMachine.self);
            currentMachine.TrySendEvent(TMP_tmp3_6, (Event)TMP_tmp4_6, TMP_tmp5_6);
            currentMachine.Announce((Event)new eStopEating(null), currentMachine.self);
            forksHeld_1 = (PrtInt)(((PrtInt)(0)));
            currentMachine.TryGotoState<Thinking>();
            return;
        }
        [Start]
        [OnEntry(nameof(InitializeParametersFunction))]
        [OnEventGotoState(typeof(ConstructorEvent), typeof(Init))]
        class __InitState__ : State { }
        
        [OnEntry(nameof(Anon_7))]
        class Init : State
        {
        }
        [OnEntry(nameof(Anon_8))]
        class Thinking : State
        {
        }
        [OnEntry(nameof(Anon_9))]
        [OnEventDoAction(typeof(eGrantFork), nameof(Anon_10))]
        class Hungry : State
        {
        }
        [OnEntry(nameof(Anon_11))]
        class Eating : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class Monitor : PMonitor
    {
        private PrtInt forksInUse = ((PrtInt)0);
        private PrtSet philosophersEating = new PrtSet();
        static Monitor() {
            observes.Add(nameof(eGrantFork));
            observes.Add(nameof(eReleaseFork));
            observes.Add(nameof(eStartEating));
            observes.Add(nameof(eStopEating));
        }
        
        public void CheckDeadlockCondition()
        {
            Monitor currentMachine = this;
            PrtInt TMP_tmp0_10 = ((PrtInt)0);
            PrtBool TMP_tmp1_9 = ((PrtBool)false);
            PrtBool TMP_tmp2_8 = ((PrtBool)false);
            PrtBool TMP_tmp3_7 = ((PrtBool)false);
            PrtString TMP_tmp4_7 = ((PrtString)"");
            PrtString TMP_tmp5_7 = ((PrtString)"");
            PrtString TMP_tmp6_2 = ((PrtString)"");
            TMP_tmp0_10 = (PrtInt)(((PrtInt)(philosophersEating).Count));
            TMP_tmp1_9 = (PrtBool)((PrtValues.SafeEquals(TMP_tmp0_10,((PrtInt)(0)))));
            TMP_tmp3_7 = (PrtBool)(((PrtBool)((IPrtValue)TMP_tmp1_9)?.Clone()));
            if (TMP_tmp3_7)
            {
                TMP_tmp2_8 = (PrtBool)((PrtValues.SafeEquals(forksInUse,((PrtInt)(5)))));
                TMP_tmp3_7 = (PrtBool)(((PrtBool)((IPrtValue)TMP_tmp2_8)?.Clone()));
            }
            if (TMP_tmp3_7)
            {
                TMP_tmp4_7 = (PrtString)(((PrtString) String.Format("PSpec\\Monitor.p:8:13")));
                TMP_tmp5_7 = (PrtString)(((PrtString) String.Format("Deadlock detected: All forks held but no one eating")));
                TMP_tmp6_2 = (PrtString)(((PrtString) String.Format("{0} {1}",TMP_tmp4_7,TMP_tmp5_7)));
                currentMachine.TryAssert(((PrtBool)false),"Assertion Failed: " + TMP_tmp6_2);
                throw new PUnreachableCodeException();
            }
        }
        public void Anon_12(Event currentMachine_dequeuedEvent)
        {
            Monitor currentMachine = this;
            PrtSet TMP_tmp0_11 = new PrtSet();
            forksInUse = (PrtInt)(((PrtInt)(0)));
            TMP_tmp0_11 = (PrtSet)(new PrtSet());
            philosophersEating = TMP_tmp0_11;
            CheckDeadlockCondition();
        }
        public void Anon_13(Event currentMachine_dequeuedEvent)
        {
            Monitor currentMachine = this;
            PrtInt TMP_tmp0_12 = ((PrtInt)0);
            TMP_tmp0_12 = (PrtInt)((forksInUse) + (((PrtInt)(1))));
            forksInUse = TMP_tmp0_12;
            CheckDeadlockCondition();
        }
        public void Anon_14(Event currentMachine_dequeuedEvent)
        {
            Monitor currentMachine = this;
            PrtInt TMP_tmp0_13 = ((PrtInt)0);
            TMP_tmp0_13 = (PrtInt)((forksInUse) - (((PrtInt)(1))));
            forksInUse = TMP_tmp0_13;
            CheckDeadlockCondition();
        }
        public void Anon_15(Event currentMachine_dequeuedEvent)
        {
            Monitor currentMachine = this;
            PMachineValue philosopher_2 = (PMachineValue)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PMachineValue TMP_tmp0_14 = null;
            TMP_tmp0_14 = (PMachineValue)(((PMachineValue)((IPrtValue)philosopher_2)?.Clone()));
            ((PrtSet)philosophersEating).Add(TMP_tmp0_14);
            CheckDeadlockCondition();
        }
        public void Anon_16(Event currentMachine_dequeuedEvent)
        {
            Monitor currentMachine = this;
            PMachineValue philosopher_3 = (PMachineValue)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PrtBool TMP_tmp0_15 = ((PrtBool)false);
            TMP_tmp0_15 = (PrtBool)(((PrtBool)(((PrtSet)philosophersEating).Contains(philosopher_3))));
            if (TMP_tmp0_15)
            {
                ((PrtSet)philosophersEating).Remove(philosopher_3);
            }
            CheckDeadlockCondition();
        }
        [Start]
        [OnEntry(nameof(Anon_12))]
        [OnEventDoAction(typeof(eGrantFork), nameof(Anon_13))]
        [OnEventDoAction(typeof(eReleaseFork), nameof(Anon_14))]
        [OnEventDoAction(typeof(eStartEating), nameof(Anon_15))]
        [OnEventDoAction(typeof(eStopEating), nameof(Anon_16))]
        class Init : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class Setup_Deadlock : PMachine
    {
        private PrtSeq forks = new PrtSeq();
        private PrtSeq philosophers = new PrtSeq();
        private PrtInt i = ((PrtInt)0);
        public class ConstructorEvent : PEvent{public ConstructorEvent(IPrtValue val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPrtValue value) { return new ConstructorEvent((IPrtValue)value); }
        public Setup_Deadlock() {
            this.sends.Add(nameof(eGrantFork));
            this.sends.Add(nameof(eReleaseFork));
            this.sends.Add(nameof(eRequestFork));
            this.sends.Add(nameof(eStartEating));
            this.sends.Add(nameof(eStopEating));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(eGrantFork));
            this.receives.Add(nameof(eReleaseFork));
            this.receives.Add(nameof(eRequestFork));
            this.receives.Add(nameof(eStartEating));
            this.receives.Add(nameof(eStopEating));
            this.receives.Add(nameof(PHalt));
            this.creates.Add(nameof(I_Fork));
            this.creates.Add(nameof(I_Philosopher_Deadlock));
        }
        
        public void Anon_17(Event currentMachine_dequeuedEvent)
        {
            Setup_Deadlock currentMachine = this;
            PrtSeq TMP_tmp0_16 = new PrtSeq();
            PrtBool TMP_tmp1_10 = ((PrtBool)false);
            PrtBool TMP_tmp2_9 = ((PrtBool)false);
            PMachineValue TMP_tmp3_8 = null;
            PrtInt TMP_tmp4_8 = ((PrtInt)0);
            PrtSeq TMP_tmp5_8 = new PrtSeq();
            PrtBool TMP_tmp6_3 = ((PrtBool)false);
            PrtBool TMP_tmp7_1 = ((PrtBool)false);
            PrtInt TMP_tmp8_1 = ((PrtInt)0);
            PMachineValue TMP_tmp9_1 = null;
            PrtInt TMP_tmp10 = ((PrtInt)0);
            PrtInt TMP_tmp11 = ((PrtInt)0);
            PMachineValue TMP_tmp12 = null;
            PrtNamedTuple TMP_tmp13 = (new PrtNamedTuple(new string[]{"id","left","right"},((PrtInt)0), null, null));
            PMachineValue TMP_tmp14 = null;
            PrtInt TMP_tmp15 = ((PrtInt)0);
            i = (PrtInt)(((PrtInt)(0)));
            TMP_tmp0_16 = (PrtSeq)(new PrtSeq());
            forks = TMP_tmp0_16;
            while (((PrtBool)true))
            {
                TMP_tmp1_10 = (PrtBool)((i) < (((PrtInt)(5))));
                TMP_tmp2_9 = (PrtBool)(((PrtBool)((IPrtValue)TMP_tmp1_10)?.Clone()));
                if (TMP_tmp2_9)
                {
                }
                else
                {
                    break;
                }
                TMP_tmp3_8 = (PMachineValue)(currentMachine.CreateInterface<I_Fork>( currentMachine));
                ((PrtSeq)forks)[i] = TMP_tmp3_8;
                TMP_tmp4_8 = (PrtInt)((i) + (((PrtInt)(1))));
                i = TMP_tmp4_8;
            }
            i = (PrtInt)(((PrtInt)(0)));
            TMP_tmp5_8 = (PrtSeq)(new PrtSeq());
            philosophers = TMP_tmp5_8;
            while (((PrtBool)true))
            {
                TMP_tmp6_3 = (PrtBool)((i) < (((PrtInt)(5))));
                TMP_tmp7_1 = (PrtBool)(((PrtBool)((IPrtValue)TMP_tmp6_3)?.Clone()));
                if (TMP_tmp7_1)
                {
                }
                else
                {
                    break;
                }
                TMP_tmp8_1 = (PrtInt)(((PrtInt)((IPrtValue)i)?.Clone()));
                TMP_tmp9_1 = (PMachineValue)(((PrtSeq)forks)[i]);
                TMP_tmp10 = (PrtInt)((i) + (((PrtInt)(1))));
                TMP_tmp11 = (PrtInt)((TMP_tmp10) % (((PrtInt)(5))));
                TMP_tmp12 = (PMachineValue)(((PrtSeq)forks)[TMP_tmp11]);
                TMP_tmp13 = (PrtNamedTuple)((new PrtNamedTuple(new string[]{"id","left","right"}, TMP_tmp8_1, TMP_tmp9_1, TMP_tmp12)));
                TMP_tmp14 = (PMachineValue)(currentMachine.CreateInterface<I_Philosopher_Deadlock>( currentMachine, TMP_tmp13));
                ((PrtSeq)philosophers)[i] = TMP_tmp14;
                TMP_tmp15 = (PrtInt)((i) + (((PrtInt)(1))));
                i = TMP_tmp15;
            }
        }
        [Start]
        [OnEntry(nameof(InitializeParametersFunction))]
        [OnEventGotoState(typeof(ConstructorEvent), typeof(Init))]
        class __InitState__ : State { }
        
        [OnEntry(nameof(Anon_17))]
        class Init : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class Setup_DeadlockFree : PMachine
    {
        private PrtSeq forks_1 = new PrtSeq();
        private PrtSeq philosophers_1 = new PrtSeq();
        private PrtInt i_1 = ((PrtInt)0);
        public class ConstructorEvent : PEvent{public ConstructorEvent(IPrtValue val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPrtValue value) { return new ConstructorEvent((IPrtValue)value); }
        public Setup_DeadlockFree() {
            this.sends.Add(nameof(eGrantFork));
            this.sends.Add(nameof(eReleaseFork));
            this.sends.Add(nameof(eRequestFork));
            this.sends.Add(nameof(eStartEating));
            this.sends.Add(nameof(eStopEating));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(eGrantFork));
            this.receives.Add(nameof(eReleaseFork));
            this.receives.Add(nameof(eRequestFork));
            this.receives.Add(nameof(eStartEating));
            this.receives.Add(nameof(eStopEating));
            this.receives.Add(nameof(PHalt));
            this.creates.Add(nameof(I_Fork));
            this.creates.Add(nameof(I_Philosopher_DeadlockFree));
        }
        
        public void Anon_18(Event currentMachine_dequeuedEvent)
        {
            Setup_DeadlockFree currentMachine = this;
            PrtSeq TMP_tmp0_17 = new PrtSeq();
            PrtBool TMP_tmp1_11 = ((PrtBool)false);
            PrtBool TMP_tmp2_10 = ((PrtBool)false);
            PMachineValue TMP_tmp3_9 = null;
            PrtInt TMP_tmp4_9 = ((PrtInt)0);
            PrtSeq TMP_tmp5_9 = new PrtSeq();
            PrtBool TMP_tmp6_4 = ((PrtBool)false);
            PrtBool TMP_tmp7_2 = ((PrtBool)false);
            PrtInt TMP_tmp8_2 = ((PrtInt)0);
            PMachineValue TMP_tmp9_2 = null;
            PrtInt TMP_tmp10_1 = ((PrtInt)0);
            PrtInt TMP_tmp11_1 = ((PrtInt)0);
            PMachineValue TMP_tmp12_1 = null;
            PrtNamedTuple TMP_tmp13_1 = (new PrtNamedTuple(new string[]{"id","left","right"},((PrtInt)0), null, null));
            PMachineValue TMP_tmp14_1 = null;
            PrtInt TMP_tmp15_1 = ((PrtInt)0);
            i_1 = (PrtInt)(((PrtInt)(0)));
            TMP_tmp0_17 = (PrtSeq)(new PrtSeq());
            forks_1 = TMP_tmp0_17;
            while (((PrtBool)true))
            {
                TMP_tmp1_11 = (PrtBool)((i_1) < (((PrtInt)(5))));
                TMP_tmp2_10 = (PrtBool)(((PrtBool)((IPrtValue)TMP_tmp1_11)?.Clone()));
                if (TMP_tmp2_10)
                {
                }
                else
                {
                    break;
                }
                TMP_tmp3_9 = (PMachineValue)(currentMachine.CreateInterface<I_Fork>( currentMachine));
                ((PrtSeq)forks_1)[i_1] = TMP_tmp3_9;
                TMP_tmp4_9 = (PrtInt)((i_1) + (((PrtInt)(1))));
                i_1 = TMP_tmp4_9;
            }
            i_1 = (PrtInt)(((PrtInt)(0)));
            TMP_tmp5_9 = (PrtSeq)(new PrtSeq());
            philosophers_1 = TMP_tmp5_9;
            while (((PrtBool)true))
            {
                TMP_tmp6_4 = (PrtBool)((i_1) < (((PrtInt)(5))));
                TMP_tmp7_2 = (PrtBool)(((PrtBool)((IPrtValue)TMP_tmp6_4)?.Clone()));
                if (TMP_tmp7_2)
                {
                }
                else
                {
                    break;
                }
                TMP_tmp8_2 = (PrtInt)(((PrtInt)((IPrtValue)i_1)?.Clone()));
                TMP_tmp9_2 = (PMachineValue)(((PrtSeq)forks_1)[i_1]);
                TMP_tmp10_1 = (PrtInt)((i_1) + (((PrtInt)(1))));
                TMP_tmp11_1 = (PrtInt)((TMP_tmp10_1) % (((PrtInt)(5))));
                TMP_tmp12_1 = (PMachineValue)(((PrtSeq)forks_1)[TMP_tmp11_1]);
                TMP_tmp13_1 = (PrtNamedTuple)((new PrtNamedTuple(new string[]{"id","left","right"}, TMP_tmp8_2, TMP_tmp9_2, TMP_tmp12_1)));
                TMP_tmp14_1 = (PMachineValue)(currentMachine.CreateInterface<I_Philosopher_DeadlockFree>( currentMachine, TMP_tmp13_1));
                ((PrtSeq)philosophers_1)[i_1] = TMP_tmp14_1;
                TMP_tmp15_1 = (PrtInt)((i_1) + (((PrtInt)(1))));
                i_1 = TMP_tmp15_1;
            }
        }
        [Start]
        [OnEntry(nameof(InitializeParametersFunction))]
        [OnEventGotoState(typeof(ConstructorEvent), typeof(Init))]
        class __InitState__ : State { }
        
        [OnEntry(nameof(Anon_18))]
        class Init : State
        {
        }
    }
}
namespace PImplementation
{
    public class tcDeadlockVariant {
        public static void InitializeLinkMap() {
            PModule.linkMap.Clear();
            PModule.linkMap[nameof(I_Philosopher_Deadlock)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_Fork)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_Setup_Deadlock)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_Setup_Deadlock)].Add(nameof(I_Fork), nameof(I_Fork));
            PModule.linkMap[nameof(I_Setup_Deadlock)].Add(nameof(I_Philosopher_Deadlock), nameof(I_Philosopher_Deadlock));
        }
        
        public static void InitializeInterfaceDefMap() {
            PModule.interfaceDefinitionMap.Clear();
            PModule.interfaceDefinitionMap.Add(nameof(I_Philosopher_Deadlock), typeof(Philosopher_Deadlock));
            PModule.interfaceDefinitionMap.Add(nameof(I_Fork), typeof(Fork));
            PModule.interfaceDefinitionMap.Add(nameof(I_Setup_Deadlock), typeof(Setup_Deadlock));
        }
        
        public static void InitializeMonitorObserves() {
            PModule.monitorObserves.Clear();
            PModule.monitorObserves[nameof(Monitor)] = new List<string>();
            PModule.monitorObserves[nameof(Monitor)].Add(nameof(eGrantFork));
            PModule.monitorObserves[nameof(Monitor)].Add(nameof(eReleaseFork));
            PModule.monitorObserves[nameof(Monitor)].Add(nameof(eStartEating));
            PModule.monitorObserves[nameof(Monitor)].Add(nameof(eStopEating));
        }
        
        public static void InitializeMonitorMap(IActorRuntime runtime) {
            PModule.monitorMap.Clear();
            PModule.monitorMap[nameof(I_Philosopher_Deadlock)] = new List<Type>();
            PModule.monitorMap[nameof(I_Philosopher_Deadlock)].Add(typeof(Monitor));
            PModule.monitorMap[nameof(I_Fork)] = new List<Type>();
            PModule.monitorMap[nameof(I_Fork)].Add(typeof(Monitor));
            PModule.monitorMap[nameof(I_Setup_Deadlock)] = new List<Type>();
            PModule.monitorMap[nameof(I_Setup_Deadlock)].Add(typeof(Monitor));
            runtime.RegisterMonitor<Monitor>();
        }
        
        
        [PChecker.SystematicTesting.Test]
        public static void Execute(IActorRuntime runtime) {
            runtime.RegisterLog(new PLogFormatter());
            runtime.RegisterLog(new PJsonFormatter());
            PModule.runtime = runtime;
            PHelper.InitializeInterfaces();
            PHelper.InitializeEnums();
            InitializeLinkMap();
            InitializeInterfaceDefMap();
            InitializeMonitorMap(runtime);
            InitializeMonitorObserves();
            runtime.CreateActor(typeof(_GodMachine), new _GodMachine.Config(typeof(Setup_Deadlock)));
        }
    }
}
namespace PImplementation
{
    public class tcDeadlockFreeVariant {
        public static void InitializeLinkMap() {
            PModule.linkMap.Clear();
            PModule.linkMap[nameof(I_Philosopher_DeadlockFree)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_Fork)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_Setup_DeadlockFree)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_Setup_DeadlockFree)].Add(nameof(I_Fork), nameof(I_Fork));
            PModule.linkMap[nameof(I_Setup_DeadlockFree)].Add(nameof(I_Philosopher_DeadlockFree), nameof(I_Philosopher_DeadlockFree));
        }
        
        public static void InitializeInterfaceDefMap() {
            PModule.interfaceDefinitionMap.Clear();
            PModule.interfaceDefinitionMap.Add(nameof(I_Philosopher_DeadlockFree), typeof(Philosopher_DeadlockFree));
            PModule.interfaceDefinitionMap.Add(nameof(I_Fork), typeof(Fork));
            PModule.interfaceDefinitionMap.Add(nameof(I_Setup_DeadlockFree), typeof(Setup_DeadlockFree));
        }
        
        public static void InitializeMonitorObserves() {
            PModule.monitorObserves.Clear();
            PModule.monitorObserves[nameof(Monitor)] = new List<string>();
            PModule.monitorObserves[nameof(Monitor)].Add(nameof(eGrantFork));
            PModule.monitorObserves[nameof(Monitor)].Add(nameof(eReleaseFork));
            PModule.monitorObserves[nameof(Monitor)].Add(nameof(eStartEating));
            PModule.monitorObserves[nameof(Monitor)].Add(nameof(eStopEating));
        }
        
        public static void InitializeMonitorMap(IActorRuntime runtime) {
            PModule.monitorMap.Clear();
            PModule.monitorMap[nameof(I_Philosopher_DeadlockFree)] = new List<Type>();
            PModule.monitorMap[nameof(I_Philosopher_DeadlockFree)].Add(typeof(Monitor));
            PModule.monitorMap[nameof(I_Fork)] = new List<Type>();
            PModule.monitorMap[nameof(I_Fork)].Add(typeof(Monitor));
            PModule.monitorMap[nameof(I_Setup_DeadlockFree)] = new List<Type>();
            PModule.monitorMap[nameof(I_Setup_DeadlockFree)].Add(typeof(Monitor));
            runtime.RegisterMonitor<Monitor>();
        }
        
        
        [PChecker.SystematicTesting.Test]
        public static void Execute(IActorRuntime runtime) {
            runtime.RegisterLog(new PLogFormatter());
            runtime.RegisterLog(new PJsonFormatter());
            PModule.runtime = runtime;
            PHelper.InitializeInterfaces();
            PHelper.InitializeEnums();
            InitializeLinkMap();
            InitializeInterfaceDefMap();
            InitializeMonitorMap(runtime);
            InitializeMonitorObserves();
            runtime.CreateActor(typeof(_GodMachine), new _GodMachine.Config(typeof(Setup_DeadlockFree)));
        }
    }
}
// TODO: NamedModule DeadlockPhilosopher
// TODO: NamedModule DeadlockFreePhilosopher
// TODO: NamedModule Fork_1
namespace PImplementation
{
    public class I_Fork : PMachineValue {
        public I_Fork (ActorId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public class I_Philosopher_Deadlock : PMachineValue {
        public I_Philosopher_Deadlock (ActorId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public class I_Philosopher_DeadlockFree : PMachineValue {
        public I_Philosopher_DeadlockFree (ActorId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public class I_Setup_Deadlock : PMachineValue {
        public I_Setup_Deadlock (ActorId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public class I_Setup_DeadlockFree : PMachineValue {
        public I_Setup_DeadlockFree (ActorId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public partial class PHelper {
        public static void InitializeInterfaces() {
            PInterfaces.Clear();
            PInterfaces.AddInterface(nameof(I_Fork), nameof(eGrantFork), nameof(eReleaseFork), nameof(eRequestFork), nameof(eStartEating), nameof(eStopEating), nameof(PHalt));
            PInterfaces.AddInterface(nameof(I_Philosopher_Deadlock), nameof(eGrantFork), nameof(eReleaseFork), nameof(eRequestFork), nameof(eStartEating), nameof(eStopEating), nameof(PHalt));
            PInterfaces.AddInterface(nameof(I_Philosopher_DeadlockFree), nameof(eGrantFork), nameof(eReleaseFork), nameof(eRequestFork), nameof(eStartEating), nameof(eStopEating), nameof(PHalt));
            PInterfaces.AddInterface(nameof(I_Setup_Deadlock), nameof(eGrantFork), nameof(eReleaseFork), nameof(eRequestFork), nameof(eStartEating), nameof(eStopEating), nameof(PHalt));
            PInterfaces.AddInterface(nameof(I_Setup_DeadlockFree), nameof(eGrantFork), nameof(eReleaseFork), nameof(eRequestFork), nameof(eStartEating), nameof(eStopEating), nameof(PHalt));
        }
    }
    
}
namespace PImplementation
{
    public partial class PHelper {
        public static void InitializeEnums() {
            PrtEnum.Clear();
        }
    }
    
}
#pragma warning restore 162, 219, 414
