using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UltimateUtil.Test
{
	[TestClass]
	public class CollectionUtil_Test
	{
		[TestMethod, TestCategory("collections")]
		public void ToReadableString()
		{
			List<int> list = new List<int>(new int[] { 1, 2, 3, 4 });

			Assert.AreEqual("{ 1;2;3;4 }", list.ToReadableString(";"));
		}

		[TestMethod, TestCategory("collections")]
		public void ToReadableStringConverted()
		{
			List<int> list = new List<int>(new int[] { 1, 2, 3, 4 });

			string res = list.ToReadableString((n) => "#" + n.ToString());
			Assert.AreEqual("{ #1, #2, #3, #4 }", res);
		}

		[TestMethod, TestCategory("collections")]
		public void IsEmpty()
		{
			List<int> list = new List<int>();
			Assert.IsTrue(list.IsEmpty());

			list.Add(5);
			Assert.IsFalse(list.IsEmpty());
		}

		[TestMethod, TestCategory("collections")]
		public void IsNullOrEmpty()
		{
			List<int> list = null;
			Assert.IsTrue(list.IsNullOrEmpty());

			list = new List<int>();
			Assert.IsTrue(list.IsNullOrEmpty());

			list.Add(50);
			Assert.IsFalse(list.IsNullOrEmpty());
		}

		[TestMethod, TestCategory("collections")]
		public void AddRange()
		{
			List<int> stuff = new List<int>();
			stuff.AddRange(5, 6, 7, 8);

			Assert.IsTrue(stuff.Contains(5));
			Assert.IsTrue(stuff.Contains(6));
			Assert.IsTrue(stuff.Contains(7));
			Assert.IsTrue(stuff.Contains(8));
		}

		[TestMethod, TestCategory("collections")]
		public void InsertIfMissing()
		{
			List<int> stuff = new List<int>();

			bool res = stuff.AddIfMissing(5);
			Assert.IsTrue(res);
			Assert.AreEqual(1, stuff.Count((n) => n == 5));

			res = stuff.AddIfMissing(5);
			Assert.IsFalse(res);
			Assert.AreEqual(1, stuff.Count((n) => n == 5));
		}

		[TestMethod, TestCategory("collections")]
		public void IndexOf()
		{
			List<int> stuff = new List<int>();
			stuff.AddRange(5, 6, 7, 8);

			int index = stuff.IndexOf((n) => n > 6);
			Assert.AreEqual(2, index);

			index = stuff.IndexOf((n) => n > 100);
			Assert.AreEqual(-1, index);
		}

		[TestMethod, TestCategory("collections")]
		public void LastIndexOf()
		{
			List<int> stuff = new List<int>();
			stuff.AddRange(5, 6, 7, 8);

			int index = stuff.LastIndexOf((n) => n < 7);
			Assert.AreEqual(1, index);

			index = stuff.LastIndexOf((n) => n > 100);
			Assert.AreEqual(-1, index);
		}

		[TestMethod, TestCategory("collections")]
		public void Put()
		{
			Dictionary<string, int> dict = new Dictionary<string, int>();

			dict.Put("1", 1);
			Assert.IsTrue(dict.ContainsKey("1"));
			Assert.AreEqual(1, dict["1"]);

			dict.Put("1", 5);
			Assert.AreEqual(5, dict["1"]);
		}

		[TestMethod, TestCategory("collections")]
		public void GetOrDefault()
		{
			Dictionary<int, string> dict = new Dictionary<int, string>();
			dict.Add(1, "one");

			Assert.AreEqual("one", dict.GetOrDefault(1));
			Assert.IsNull(dict.GetOrDefault(4));

			dict.Add(2, "two");
			dict.Add(3, "three");
			dict.Add(4, "four");
			Assert.AreEqual("three", dict.GetOrDefault((k, v) => !v.Contains('o')));
		}

		[TestMethod, TestCategory("collections")]
		public void GetIgnoreCase()
		{
			Dictionary<string, int> dict = new Dictionary<string, int>();
			dict.Add("ONE", 1);
			dict.Add("TWO", 2);
			dict.Add("one", 11);

			Assert.AreEqual(1, dict.GetIgnoreCase("oNe"));
		}

		[TestMethod, TestCategory("collections")]
		public void ContainsKeyIgnoreCase()
		{
			Dictionary<string, int> dict = new Dictionary<string, int>();
			dict.Add("ONE", 1);
			dict.Add("TWO", 2);
			dict.Add("THREE", 3);

			Assert.IsTrue(dict.ContainsKeyIgnoreCase("one"));
		}

		[TestMethod, TestCategory("collections")]
		public void ContainsValueIgnoreCase()
		{
			Dictionary<int, string> dict = new Dictionary<int, string>();
			dict.Add(1, "ONE");
			dict.Add(2, "TWO");
			dict.Add(3, "THREE");

			Assert.IsTrue(dict.ContainsValueIgnoreCase("two"));
		}

		[TestMethod, TestCategory("collections")]
		public void ConvertAll()
		{
			IList<string> list = new List<string>();
			list.AddRange("1", "2", "3", "4");

			IEnumerable<int> res = list.ConvertAll((s) => int.Parse(s));
			Assert.IsTrue(res.Contains(1));
			Assert.IsTrue(res.Contains(2));
			Assert.IsTrue(res.Contains(3));
			Assert.IsTrue(res.Contains(4));
		}

		[TestMethod, TestCategory("collections")]
		public void AnonToDict()
		{
			object anon = new { width = 30, length = 45 };
			Dictionary<string, object> dict = anon.ToDictionary();

			Assert.IsTrue(dict.ContainsKey("width"));
			Assert.AreEqual(30, (int)dict["width"]);
			Assert.IsTrue(dict.ContainsKey("length"));
			Assert.AreEqual(45, (int)dict["length"]);
		}

		[TestMethod, TestCategory("collections")]
		public void EmptyIfNull()
		{
			IEnumerable<string> ienum = null;

			ienum = ienum.EmptyIfNull();
			Assert.IsNotNull(ienum);
			Assert.AreEqual(0, ienum.Count());
		}
	}
}
