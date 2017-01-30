using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace EntryPoint
{
	public class Sorting
	{
		public static void MergeSortByHouse(Vector2[] B, int p, int r, Vector2 house)
		{
			if (p < r)
			{
				int q = (p + r) / 2;

				MergeSortByHouse(B, p, q, house);
				MergeSortByHouse(B, q + 1, r, house);

			}

		}

		public static void MergeByHouse(Vector2[] B, int p, int q, int r, Vector2 house)
		{
			int n1 = q - p + 1;
			int n2 = r - q;

			var L = new Vector2[n1];
			var R = new Vector2[n2];

			for (int x = 0; x < n1; x++)
			{
				L[x] = B[p + x];
			}

			for (int y = 0; y < n2; y++)
			{
				R[y] = B[q + y + 1];
			}

			int i = 0;
			int j = 0;
			int k = p;

			while (i < n1 && j < n2)
			{
				if (Distance(L[i], house) < Distance(R[j], house))
				{
					B[k] = L[i];
					i++;
				}
				else {
					B[k] = R[j];
					j++;
				}
				k++;
			}

			while (i < n1)
			{
				B[k] = L[i];
				i++;
				k++;
			}

			while (j < n2)
			{
				B[k] = R[j];
				j++;
				k++;
			}
		}


		public static float Distance(Vector2 a, Vector2 b)
		{
			return (float)Math.Sqrt(Math.Pow(a.X - b.X,2) + Math.Pow(a.Y - b.Y,2));
		}

		public static void MergeSort(Vector2[] points, int p, int r, int depth)
		{
			if (p < r)
			{
				int q = (p + r) / 2;
				MergeSort(points, p, q, depth);
				MergeSort(points, q + 1, r, depth);
				Merge(points, p, q, r, depth);
			}
		}

		public static void Merge(Vector2[] points, int p, int q, int r, int depth)
		{
			int n1 = q - p + 1;
			int n2 = r - q;

			Vector2[] L = new Vector2[n1];
			Vector2[] R = new Vector2[n2];

			for (int i = 0; i < n1; i++)
				L[i] = points[p + i];

			for (int j = 0; j < n2; j++)
				R[j] = points[j + q + 1];

			int ii = 0;
			int jj = 0;
			int k = p;

			while (ii < n1 && jj < n2)
			{
				if (depth % 2 == 0)
				{
					if (L[ii].X < R[jj].X)
					{
						points[k] = L[ii];
						ii++;
					}
					else
					{
						points[k] = R[jj];
						jj++;
					}
				}
				else {
					if (L[ii].Y < R[jj].Y)
					{
						points[k] = L[ii];
						ii++;
					}
					else
					{
						points[k] = R[jj];
						jj++;
					}
				}
				k++;
			}

			while (ii < n1)
			{
				points[k] = L[ii];
				ii++;
				k++;
			}
			while (jj < n2)
			{
				points[k] = R[jj];
				jj++;
				k++;
			}
		}

		public static void TreeWalker(ITree<Vector2> T)
		{
			if (!T.IsEmpty)
			{
				//L.Add(T.Value);
				Console.WriteLine(T.Value);
				TreeWalker(T.Left);
				TreeWalker(T.Right);
			}
		}


		public static void TreeRange(ITree<Vector2> T, Tuple<Vector2, float> hd, int depth, List<Vector2> L)
		{
			if (!T.IsEmpty)
			{
				var Xmin = hd.Item1.X - hd.Item2;
				var Xmax = hd.Item1.X + hd.Item2;

				var Ymin = hd.Item1.Y - hd.Item2;
				var Ymax = hd.Item1.Y + hd.Item2;

				if (T.Value.X > Xmin && T.Value.X < Xmax && T.Value.Y > Ymin && T.Value.Y < Ymax)
				{
					if(Distance(T.Value, hd.Item1) <= hd.Item2)
						L.Add(T.Value);
				}

				bool axis = depth % 2 == 0;

				if (T.Value.X >= Xmin && T.Value.X <= Xmax && axis ||
					T.Value.Y >= Ymin && T.Value.Y <= Ymax && !axis)
				{
					TreeRange(T.Left, hd, depth + 1, L);
					TreeRange(T.Right, hd, depth + 1, L);
				}
				else if (T.Value.X < Xmin && axis ||
						 T.Value.Y < Ymin && !axis)
				{
					TreeRange(T.Right, hd, depth + 1, L);
				}
				else if (T.Value.X > Xmax && axis ||
						 T.Value.Y > Ymax && !axis)
				{
					TreeRange(T.Left, hd, depth + 1, L);
				}
			}
		}

		public static ITree<Vector2> BuildKdTree(Vector2[] P, int depth)
		{
			Sorting.MergeSort(P, 0, P.Length - 1, depth);
			int v = P.Length / 2;
			
			Vector2[] L = P.Take(P.Length / 2).ToArray();
			Vector2[] R = P.Skip(P.Length / 2 + 1).ToArray();

			if (P.Length == 1)
			{
				return new Node<Vector2>(new Empty<Vector2>(), P[0], new Empty<Vector2>());
			}
			ITree<Vector2> root;
			if (P.Length == 0)
				root = new Empty<Vector2>();
			else {
				root = new Node<Vector2>(new Empty<Vector2>(), P[v], new Empty<Vector2>());
				root.Left = BuildKdTree(L, depth + 1);
				root.Right = BuildKdTree(R, depth + 1);
			}

			return root;
		}

		public static ITree<Vector2> BuildKdTree2(Vector2[] P, int p, int r, int depth)
		{
			Sorting.MergeSort(P, p, r, depth);
			int median = P.Length / 2;

			ITree<Vector2> root;
			if (r - p == 1)
			{
				return new Node<Vector2>(new Empty<Vector2>(), P[p], new Empty<Vector2>());
			}
			if (r < p)
			{
				root = new Empty<Vector2>();
			}
			else {
				root = new Node<Vector2>(new Empty<Vector2>(), P[median], new Empty<Vector2>());
				root.Left = BuildKdTree2(P, p, median - 1, depth - 1);
				root.Right = BuildKdTree2(P, median + 1, r, depth - 1);
			}
		
			return root;
		}
	}


	public interface ITree<T>
	{
		bool IsEmpty { get; }
		ITree<T> Left { get; set; }
		ITree<T> Right { get; set; }
		T Value { get; }
	}

	public class Empty<T> : ITree<T>
	{
		public bool IsEmpty
		{
			get
			{
				return true;
			}
		}

		public ITree<T> Left
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public ITree<T> Right
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public T Value
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		ITree<T> ITree<T>.Left
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		ITree<T> ITree<T>.Right
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}
	}

	public class Node<T> : ITree<T>
	{
		public bool IsEmpty
		{
			get
			{
				return false;
			}
		}

		public ITree<T> Left
		{
			get;set;
		}

		public ITree<T> Right
		{
			get;set;
		}

		public T Value
		{
			get;set;
		}

		public Node(ITree<T> l, T v, ITree<T> r)
		{
			this.Left = l;
			this.Value = v;
			this.Right = r;
		}
	}

}