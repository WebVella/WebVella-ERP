namespace WebVella.Erp.Eql
{
	public class EqlError
	{
		public string Message { get; set; }
		public int? Line { get; set; }
		public int? Column { get; set; }
	}
}
