namespace WebVella.Erp.Web.Models
{
	public class JwtTokenLoginModel
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}

	public class JwtTokenModel
	{
		public string Token { get; set; }
	}
}
