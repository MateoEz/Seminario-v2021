﻿using System;
using System.Collections.Generic;
using DefaultNamespace;

namespace AI.Core.GOAP.Core
{
    public class ReGoapState<T, W> : IObservable
    {
        // can change to object
        private Dictionary<T, W> values;
        private readonly Dictionary<T, W> bufferA;
        private readonly Dictionary<T, W> bufferB;
        private List<IObserver> _observers;

        public static int DefaultSize = 20;

        private ReGoapState()
        {
            bufferA = new Dictionary<T, W>(DefaultSize);
            bufferB = new Dictionary<T, W>(DefaultSize);
            values = bufferA;
        }

        private void Init(ReGoapState<T, W> old)
        {
            if(_observers == null) _observers = new List<IObserver>();
            values.Clear();
            if (old != null)
            {
                lock (old.values)
                {
                    foreach (var pair in old.values)
                    {
                        values[pair.Key] = pair.Value;
                    }
                }
            }
        }

        public static ReGoapState<T, W> operator +(ReGoapState<T, W> a, ReGoapState<T, W> b)
        {
            ReGoapState<T, W> result;
            lock (a.values)
            {
                result = Instantiate(a);
            }
            lock (b.values)
            {
                foreach (var pair in b.values)
                    result.values[pair.Key] = pair.Value;
                return result;
            }
        }

        public void AddFromState(ReGoapState<T, W> b)
        {
            lock (values) lock (b.values)
            {
                foreach (var pair in b.values)
                    values[pair.Key] = pair.Value;
            }
        }

        public int Count
        {
            get { return values.Count; }
        }
        public bool HasAny(ReGoapState<T, W> other)
        {
            lock (values) lock (other.values)
            {
                foreach (var pair in other.values)
                {
                    W thisValue;
                    values.TryGetValue(pair.Key, out thisValue);
                    if (Equals(thisValue, pair.Value))
                        return true;
                }
                return false;
            }
        }
        public bool HasAnyConflict(ReGoapState<T, W> other) // used only in backward for now
        {
            lock (values) lock (other.values)
                {
                    foreach (var pair in other.values)
                    {
                        var otherValue = pair.Value;

                        // not here, ignore this check
                        W thisValue;
                        if (!values.TryGetValue(pair.Key, out thisValue))
                            continue;
                        if (!Equals(otherValue, thisValue))
                            return true;
                    }
                    return false;
                }
        }

        // this method is more relaxed than the other, also accepts conflits that are fixed by "changes"
        public bool HasAnyConflict(ReGoapState<T, W> changes, ReGoapState<T, W> other)
        {
            lock (values) lock (other.values)
                {
                    foreach (var pair in other.values)
                    {
                        var otherValue = pair.Value;

                        // not here, ignore this check
                        W thisValue;
                        if (!values.TryGetValue(pair.Key, out thisValue))
                            continue;
                        W effectValue;
                        changes.values.TryGetValue(pair.Key, out effectValue);
                        if (!Equals(otherValue, thisValue) && !Equals(effectValue, thisValue))
                            return true;
                    }
                    return false;
                }
        }

        public int MissingDifference(ReGoapState<T, W> other, int stopAt = int.MaxValue)
        {
            lock (values)
            {
                var count = 0;
                foreach (var pair in values)
                {
                    W otherValue;
                    other.values.TryGetValue(pair.Key, out otherValue);
                    if (!Equals(pair.Value, otherValue))
                    {
                        count++;
                        if (count >= stopAt)
                            break;
                    }
                }
                return count;
            }
        }

        // write differences in "difference"
        public int MissingDifference(ReGoapState<T, W> other, ref ReGoapState<T, W> difference, int stopAt = int.MaxValue, Func<KeyValuePair<T, W>, W, bool> predicate = null, bool test = false)
        {
            lock (values)
            {
                var count = 0;
                foreach (var pair in values)
                {
                    W otherValue;
                    other.values.TryGetValue(pair.Key, out otherValue);
                    if (!Equals(pair.Value, otherValue) && (predicate == null || predicate(pair, otherValue)))
                    {
                        count++;
                        if (difference != null)
                            difference.values[pair.Key] = pair.Value;
                        if (count >= stopAt)
                            break;
                    }
                }
                return count;
            }
        }

        // keep only missing differences in values
        public int ReplaceWithMissingDifference(ReGoapState<T, W> other, int stopAt = int.MaxValue, Func<KeyValuePair<T, W>, W, bool> predicate = null, bool test = false)
        {
            lock (values)
            {
                var count = 0;
                var buffer = values;
                values = values == bufferA ? bufferB : bufferA;
                values.Clear();
                foreach (var pair in buffer)
                {
                    W otherValue;
                    other.values.TryGetValue(pair.Key, out otherValue);
                    if (!Equals(pair.Value, otherValue) && (predicate == null || predicate(pair, otherValue)))
                    {
                        count++;
                        values[pair.Key] = pair.Value;
                        if (count >= stopAt)
                            break;
                    }
                }
                return count;
            }
        }

        public ReGoapState<T, W> Clone()
        {
            return Instantiate(this);
        }


        #region StateFactory
        private static Stack<ReGoapState<T, W>> cachedStates;

        public static void Warmup(int count)
        {
            cachedStates = new Stack<ReGoapState<T, W>>(count);
            for (int i = 0; i < count; i++)
            {
                cachedStates.Push(new ReGoapState<T, W>());
            }
        }

        public void Recycle()
        {
            lock (cachedStates)
            {
                cachedStates.Push(this);
            }
        }

        public static ReGoapState<T, W> Instantiate(ReGoapState<T, W> old = null)
        {
            ReGoapState<T, W> state;
            if (cachedStates == null)
            {
                cachedStates = new Stack<ReGoapState<T, W>>();
            }
            lock (cachedStates)
            {
                state = cachedStates.Count > 0 ? cachedStates.Pop() : new ReGoapState<T, W>();
            }
            state.Init(old);
            return state;
        }
        #endregion

        public override string ToString()
        {
            lock (values)
            {
                var result = "";
                foreach (var pair in values)
                    result += string.Format("'{0}': {1}, ", pair.Key, pair.Value);
                return result;
            }
        }

        public W Get(T key)
        {
            lock (values)
            {
                if (!values.ContainsKey(key))
                    return default(W);
                return values[key];
            }
        }

        public void Set(T key, W value)
        {
            lock (values)
            {
                values[key] = value;
            }
            Notify();
        }

        public void Remove(T key)
        {
            lock (values)
            {
                values.Remove(key);
            }
        }

        public Dictionary<T, W> GetValues()
        {
            lock (values)
                return values;
        }

        public bool HasKey(T key)
        {
            lock (values)
                return values.ContainsKey(key);
        }

        public void Clear()
        {
            lock (values)
                values.Clear();
        }

        public void RegisterObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.OnNotify();
            }
        }
    }
}