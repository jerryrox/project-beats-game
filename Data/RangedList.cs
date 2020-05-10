using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.Data
{
    /// <summary>
    /// List wrapper which comes with low and high index ranging feature.
    /// For the sake of being loop-friendly, the low and high indexes range between 0 to list count, inclusive.
    /// </summary>
    public class RangedList<T> : IList<T> {

        private List<T> rawList;
        private int lowIndex;
        private int highIndex;


        public T this[int index]
        {
            get => rawList[index];
            set => rawList[index] = value;
        }

        public int Count => rawList.Count;

        /// <summary>
        /// The current low range index.
        /// </summary>
        public int LowIndex
        {
            get => lowIndex;
            set => lowIndex = ValidateLowIndex(value);
        }

        /// <summary>
        /// The current high range index.
        /// </summary>
        public int HighIndex
        {
            get => highIndex;
            set => highIndex = ValidateHighIndex(value);
        }

        public bool IsReadOnly => false;


        public RangedList() : this(0) { }

        public RangedList(int capacity)
        {
            rawList = new List<T>(capacity);
        }

        public void Add(T item) => rawList.Add(item);

        public void Clear()
        {
            rawList.Clear();
            RefreshRangeIndexes();
        }

        public bool Contains(T item) => rawList.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => rawList.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            if (rawList.Remove(item))
                RefreshRangeIndexes();
            return false;
        }

        public int IndexOf(T item) => rawList.IndexOf(item);

        public void Insert(int index, T item) => rawList.Insert(index, item);

        public void RemoveAt(int index)
        {
            rawList.RemoveAt(index);
            RefreshRangeIndexes();
        }

        public IEnumerator<T> GetEnumerator() => rawList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Resets low/high range indexes.
        /// </summary>
        public void ResetIndex()
        {
            LowIndex = 0;
            HighIndex = 0;
        }

        /// <summary>
        /// Advances low index by 1 to specified direction.
        /// Returns whether advancement has been made.
        /// </summary>
        public bool AdvanceLowIndex(bool upward) => AdvanceIndex(upward, true, ref lowIndex);

        /// <summary>
        /// Advances high index by 1 to specified direction.
        /// Returns whether advancement has been made.
        /// </summary>
        public bool AdvanceHighIndex(bool upward) => AdvanceIndex(upward, false, ref highIndex);

        /// <summary>
        /// Refreshes low and high range index values.
        /// </summary>
        private void RefreshRangeIndexes()
        {
            LowIndex = lowIndex;
            HighIndex = highIndex;
        }

        /// <summary>
        /// Internally handles low/high index advancement.
        /// Returns whether advancement has been made.
        /// </summary>
        private bool AdvanceIndex(bool upward, bool isLowIndex, ref int index)
        {
            int dir = upward ? 1 : -1;
            int desiredIndex = index + dir;
            int newIndex = isLowIndex ? ValidateLowIndex(desiredIndex) : ValidateHighIndex(desiredIndex);
            index = newIndex;
            return newIndex == desiredIndex;
        }

        /// <summary>
        /// Returns the validated value of the index for use as low range index.
        /// </summary>
        private int ValidateLowIndex(int index)
        {
            if(ValidateIndex(ref index))
                return index;
            else if(index > highIndex)
                return highIndex;
            return index;
        }

        /// <summary>
        /// Returns the validated value of the index for use as high range index.
        /// </summary>
        private int ValidateHighIndex(int index)
        {
            if (ValidateIndex(ref index))
                return index;
            else if(index < lowIndex)
                return lowIndex;
            return index;
        }

        /// <summary>
        /// Validates the specified index using common criteria.
        /// </summary>
        private bool ValidateIndex(ref int index)
        {
            if (rawList.Count == 0 || index < 0)
            {
                index = 0;
                return true;
            }
            if (index > rawList.Count)
            {
                index = rawList.Count;
                return true;
            }
            return false;
        }
    }
}