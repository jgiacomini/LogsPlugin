using System;
using System.Text;
namespace Plugin.Logs.Extension
{
	/// <summary>
	/// Add some method to exception
	/// </summary>
	public static class ExceptionExtension
	{
		/// <summary>
		/// Creates the exception string.
		/// </summary>
		/// <param name="e">The e.</param>
		/// <param name="message">The message.</param>
		/// <returns>return exception string formatted</returns>
		public static string CreateExceptionString(this Exception e, string message = null)
		{
			var sb = new StringBuilder(2000);
			if (!string.IsNullOrEmpty(message))
			{
				sb.AppendLine(message);
			}

			CreateExceptionString(sb, e, string.Empty);

			return sb.ToString();
		}

		/// <summary>
		/// Creates the exception string.
		/// </summary>
		/// <param name="sb">The sb.</param>
		/// <param name="e">The e.</param>
		/// <param name="indent">The indent.</param>
		private static void CreateExceptionString(StringBuilder sb, Exception e, string indent)
		{
			if (indent == null)
			{
				indent = string.Empty;
			}
			else if (indent.Length > 0)
			{
				sb.AppendFormat("{0}Inner ", indent);
			}

			sb.AppendFormat("Exception type: {1}", indent, e.GetType().FullName);
			sb.AppendLine();
			sb.AppendLine();

			sb.AppendFormat("{0}Message: {1}", indent, e.Message);
			if (e.Source != null)
			{
				sb.AppendLine();
				sb.AppendLine();
				sb.AppendFormat("{0}Source: {1}", indent, e.Source);
			}

			if (e.StackTrace != null)
			{
				sb.AppendLine();
				sb.AppendLine();
				sb.AppendFormat("{0}Stacktrace: {1}", indent, e.StackTrace);
			}

			if (e.InnerException != null)
			{
				CreateExceptionString(sb, e.InnerException, indent + "  ");
			}
		}

	}
}
