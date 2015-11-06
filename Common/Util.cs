using System.Text;
using System.Security.Cryptography;

namespace Expense.Common
{
	public class Util
	{
		public static string ToSha256(string input)
		{
			SHA256 sha256 = SHA256.Create();
			
			byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
			
			StringBuilder result = new StringBuilder();
			
			foreach(byte b in bytes) {
				result.Append(b.ToString("X2"));
			}
			
			return result.ToString();
		}
	}	
}