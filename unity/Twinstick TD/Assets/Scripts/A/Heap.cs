using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Optimisation of a pathfindin algorithm with a Heap
/// </summary>
/// <typeparam name="T"></typeparam>
public class Heap<T> where T : IHeapItem<T> {
	
	T[] items; // items, in our case the nodes
	int currentItemCount; // the length of the heap array
	
    /// <summary>
    /// The constructor Heap
    /// </summary>
    /// <param name="maxHeapSize"></param>
	public Heap(int maxHeapSize) {
		items = new T[maxHeapSize];
	}


	/// <summary>
    /// Adding an Item to the heapArray
    /// </summary>
    /// <param name="item"></param>
	public void Add(T item) {
		item.HeapIndex = currentItemCount;
		items[currentItemCount] = item;
		SortUp(item);
		currentItemCount++;
	}

    /// <summary>
    /// Remove the first value of the Heaparray for later evaluation
    /// </summary>
    /// <returns></returns>
	public T RemoveFirst() {
		T firstItem = items[0];
		currentItemCount--;
		items[0] = items[currentItemCount];
		items[0].HeapIndex = 0;
		SortDown(items[0]);
		return firstItem;
	}

    /// <summary>
    /// Item T is evaluated and is sorted up till it is in the right place in the heap
    /// </summary>
    /// <param name="item"></param>
	public void UpdateItem(T item) {
		SortUp(item);
	}

    /// <summary>
    /// returning the currentItemCount
    /// </summary>
    /// <returns name ="currentItemCount"> </returns>
	public int Count {
		get {
			return currentItemCount;
		}
	}

    /// <summary>
    /// checks whether item is in the Heap array and in the right place
    /// </summary>
    /// <param name="item"></param>
    /// <returns name = "Equals(items[item.HeapIndex], item)"></returns>
	public bool Contains(T item) {
		return Equals(items[item.HeapIndex], item);
	}

    /// <summary>
    /// Sorting down an item in the Heap till it reaches the right place
    /// </summary>
    /// <param name="item"></param>
	void SortDown(T item) {
		while (true) {
			int childIndexLeft = item.HeapIndex * 2 + 1;
			int childIndexRight = item.HeapIndex * 2 + 2;
			int swapIndex = 0;

			if (childIndexLeft < currentItemCount) {
				swapIndex = childIndexLeft;

				if (childIndexRight < currentItemCount) {
					if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
						swapIndex = childIndexRight;
					}
				}

				if (item.CompareTo(items[swapIndex]) < 0) {
					Swap (item,items[swapIndex]);
				}
				else {
					return;
				}

			}
			else {
				return;
			}

		}
	}
	
    /// <summary>
    /// Sortin up an item till it reahes the right place
    /// </summary>
    /// <param name="item"></param>
	void SortUp(T item) {
		int parentIndex = (item.HeapIndex-1)/2;
		
		while (true) {
			T parentItem = items[parentIndex];
			if (item.CompareTo(parentItem) > 0) {
				Swap (item,parentItem);
			}
			else {
				break;
			}

			parentIndex = (item.HeapIndex-1)/2;
		}
	}
	
    /// <summary>
    /// Swap two items in the Heaparray
    /// </summary>
    /// <param name="itemA"></param>
    /// <param name="itemB"></param>
	void Swap(T itemA, T itemB) {
		items[itemA.HeapIndex] = itemB;
		items[itemB.HeapIndex] = itemA;
		int itemAIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = itemAIndex;
	}
}

/// <summary>
/// The interface of an Heapitem
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IHeapItem<T> : IComparable<T> {
	int HeapIndex {
		get;
		set;
	}
}
