using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace UltimateUtil
{
	/// <summary>
	/// [WIP] Various utilities involving threading.
	/// </summary>
	public static class ThreadUtil
	{
		/// <summary>
		/// Runs code on a separate thread after a delay.
		/// </summary>
		/// <param name="delay">Amount of time to delay the code</param>
		/// <param name="todo">Code to run</param>
		public static Thread RunDelayed(TimeSpan delay, Action todo)
		{
			Thread thread = new Thread(() => 
			{
				Thread.Sleep(delay);
				todo();
			});
			thread.Name = "Delayed Action Thread";
			thread.IsBackground = true;

			thread.Start();

			return thread;
		}
		/// <summary>
		/// Runs code on a separate thread after a delay in miliseconds.
		/// </summary>
		/// <param name="milisecondsDelay">Amount of time to delay the code by, in miliseconds</param>
		/// <param name="todo">Code to run</param>
		public static Thread RunDelayed(double milisecondsDelay, Action todo)
		{
			return RunDelayed(TimeSpan.FromMilliseconds(milisecondsDelay), todo);
		}
	}
}
