using System;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace Framework.Qath
{
	public static class ProbabilityMath
	{
		private static Random _rand = new Random(1);
		
		static int _n2Cached = 0;
		private static double _n2 = 0.0;

		/// <summary>
		/// 浮点数随机
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static float RandomFloat(float min = 0f, float max = 1f)
		{
			return _rand.NextFloat(min, max);
		}
		
		/// <summary>
		/// 正态分布概率函数
		/// </summary>
		/// <param name="mean"></param>
		/// <param name="stddev"></param>
		/// <returns></returns>
		public static double  NormalRandomNumber(double mean, double stddev)
		{
			if (_n2Cached != 0)
			{
				double x, y, r;
				do
				{
					x = 2 * _rand.NextDouble(0, 1) - 1;
					y = 2 * _rand.NextDouble(0, 1) - 1;
					
					r = x * x + y * y;
					
				} 
				while (r == 0.0 || r > 1.0);

				{
					double d = math.sqrt(-2.0 * math.log(r) / r);
					double n1 = x * d;
					_n2 = y * d;
					double result = n1 * stddev + mean;
					_n2Cached = 1;
					return result;
				}
			}
			else
			{
				_n2Cached = 0;
				return _n2 * stddev + mean;
			}
		}


		/// <summary>
		/// 泊松概率函数
		/// </summary>
		/// <param name="lambda"></param>
		/// <returns></returns>
		public static int PoissonRandomNumber(double lambda)
		{
			double L = math.exp(-lambda);
			double p = 1.0;
			int k = 0;
			do
			{
				k++;
				double u = _rand.NextDouble(0, 1);
				p *= u;
			} while (p > L);

			return k - 1;
		}
		
		/// <summary>
		/// 按照指数函数概率分布随机生成一个介于min和max之间的值, 只用了指数函数中0-0.9部分的曲线
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="lambda">值越大累积函数前期越平缓</param>
		/// <returns></returns>
		public static double ExponentialRandom(double min, double max)
		{
			// 生成一个0到1之间的均匀分布随机数
			double uniformRandom = _rand.NextDouble(0, .9f + float.MinValue);
			uniformRandom = math.clamp(uniformRandom, 0, 0.9f);

			// 使用指数分布的逆累积分布函数
			double exponentialRandom = -math.log(1 - uniformRandom);

			var val = min + (max - min * exponentialRandom);
			return val;
		}

		public static double LogarithmicRandom(double min, double max, int logarithmic = 10)
		{
			// 生成一个0到1之间的均匀分布随机数
			double uniformRandom = _rand.NextDouble(0, .9f + float.MinValue);
			uniformRandom = math.clamp(uniformRandom, 0, 0.9f);

			// 使用指数分布的逆累积分布函数
			double exponentialRandom = -Math.Log(1 - uniformRandom, logarithmic);

			var val = min + (max - min * exponentialRandom);
			return val;
		}
	}
}