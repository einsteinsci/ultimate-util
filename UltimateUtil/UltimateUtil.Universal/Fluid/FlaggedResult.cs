using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Fluid
{
	/// <summary>
	/// Used for returning a value with the option for additional data, avoiding <c>out</c>
	/// syntax, while maintaining strong typing.
	/// </summary>
	/// <typeparam name="TResult">Resulting type returned</typeparam>
	/// <typeparam name="TFlags">Type of objects attached</typeparam>
	public class FlaggedResult<TResult, TFlags> : ICollection<TFlags>
	{
		/// <summary>
		/// Returned item.
		/// </summary>
		public TResult Result
		{ get; set; }

		/// <summary>
		/// List of "attached" data that may provide context to <see cref="Result"/>.
		/// </summary>
		public List<TFlags> Flags
		{ get; private set; }

		/// <summary>
		/// Returns the number of flags in the <see cref="FlaggedResult{TResult, TFlags}"/>
		/// </summary>
		public int Count => Flags.Count;

		/// <summary>
		/// Returns whether the collection is read-only
		/// </summary>
		public bool IsReadOnly => ((ICollection<TFlags>)Flags).IsReadOnly;

		/// <summary>
		/// Instantiates a new <see cref="FlaggedResult{TResult, TFlags}"/> from a
		/// result value and array of attached flags
		/// </summary>
		/// <param name="res">Result value</param>
		/// <param name="attachedFlags"><see cref="Array"/> of flags to attach</param>
		public FlaggedResult(TResult res, params TFlags[] attachedFlags) : this(res)
		{
			Flags.AddRange(attachedFlags);
		}

		/// <summary>
		/// Instantiates a new <see cref="FlaggedResult{TResult, TFlags}"/> from a
		/// result value and collection of attached flags
		/// </summary>
		/// <param name="res">Result value</param>
		/// <param name="attachedFlags"><see cref="IEnumerable{T}"/> of flags to attach</param>
		public FlaggedResult(TResult res, IEnumerable<TFlags> attachedFlags) : this(res)
		{
			Flags.AddRange(attachedFlags);
		}

		/// <summary>
		/// Instantiates a new <see cref="FlaggedResult{TResult, TFlags}"/> with the
		/// default value for the result with no attached flags
		/// </summary>
		public FlaggedResult() : this(default(TResult))
		{ }

		/// <summary>
		/// Instantiates a new <see cref="FlaggedResult{TResult, TFlags}"/> from a given
		/// result value with no attached flags
		/// </summary>
		/// <param name="res">Result value</param>
		public FlaggedResult(TResult res)
		{
			Result = res;
			Flags = new List<TFlags>();
		}

		/// <summary>
		/// Converts the <see cref="FlaggedResult{TResult, TFlags}"/> to a string
		/// </summary>
		/// <returns>A string containing <see cref="Result"/> and the number of flags attached</returns>
		public override string ToString()
		{
			return "Result: " + Result.ToString() + " (" + Flags.Count.ToString() + " flags)";
		}

		/// <summary>
		/// Adds a flag to the result
		/// </summary>
		/// <param name="item"></param>
		public void Add(TFlags item)
		{
			Flags.Add(item);
		}

		/// <summary>
		/// Removes all flag from the result
		/// </summary>
		public void Clear()
		{
			Flags.Clear();
		}

		/// <summary>
		/// Returns whether a flag is attached to the result
		/// </summary>
		/// <param name="flag">Flag to test for</param>
		/// <returns>Whether <paramref name="flag"/> is attached to the result.</returns>
		public bool Contains(TFlags flag)
		{
			return Flags.Contains(flag);
		}

		/// <summary>
		/// Copies the contents of the attached flags into an array.
		/// </summary>
		/// <param name="array">Array to copy into</param>
		/// <param name="arrayIndex">Array index to start at</param>
		public void CopyTo(TFlags[] array, int arrayIndex)
		{
			Flags.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Removes a flag from the result
		/// </summary>
		/// <param name="flag">Flag to remove</param>
		/// <returns><c>true</c> if removal was successful, <c>false</c> if not</returns>
		public bool Remove(TFlags flag)
		{
			return Flags.Remove(flag);
		}

		/// <summary>
		/// Gets the <see cref="IEnumerator{T}"/> for iteration within a <c>foreach</c>
		/// loop
		/// </summary>
		/// <returns>An <see cref="IEnumerator{T}"/> from <see cref="Flags"/>.</returns>
		public IEnumerator<TFlags> GetEnumerator()
		{
			return Flags.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
