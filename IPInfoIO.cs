using System.Net;
using System.Text;
using System.Web.Script.Serialization;

/// <summary>
/// Wrapper to contect and retrieve info about a given IP.
/// </summary>
public class IPInfoIO {
	#region Properties

	/// <summary>
	/// IP.
	/// </summary>
	public string ip { get; set; }

	/// <summary>
	/// Hostname.
	/// </summary>
	public string hostname { get; set; }

	/// <summary>
	/// City.
	/// </summary>
	public string city { get; set; }

	/// <summary>
	/// Region (eg.: county).
	/// </summary>
	public string region { get; set; }

	/// <summary>
	/// Country.
	/// </summary>
	public string country { get; set; }

	/// <summary>
	/// Geo location.
	/// </summary>
	public string loc { get; set; }

	/// <summary>
	/// IP owner organization.
	/// </summary>
	public string org { get; set; }

	/// <summary>
	/// Postcode/zip-code.
	/// </summary>
	public string postal { get; set; }

	/// <summary>
	/// Longitude.
	/// </summary>
	public decimal lng { get; set; }

	/// <summary>
	/// Latitude.
	/// </summary>
	public decimal lat { get; set; }

	#endregion
	#region Helper Methods

	/// <summary>
	/// Contact ipinfo.io and retrieve info about given IP.
	/// </summary>
	/// <param name="ip">IP to get info for.</param>
	/// <returns>Class with info.</returns>
	public static IPInfoIO Get(string ip) {
		try {
			var webClient = new WebClient {
				Encoding = Encoding.UTF8
			};

			var json = webClient.DownloadString(
				string.Format(
					"http://ipinfo.io/{0}/json",
					ip));

			var data = new JavaScriptSerializer().Deserialize<IPInfoIO>(json);

			if (data.loc.IndexOf(',') <= -1)
				return data;

			decimal lng;
			decimal lat;

			if (decimal.TryParse(data.loc.Substring(0, data.loc.IndexOf(',')), out lng))
				data.lng = lng;

			if (decimal.TryParse(data.loc.Substring(data.loc.IndexOf(',') + 1), out lat))
				data.lat = lat;

			return data;
		}
		catch {
			return null;
		}
	}

	#endregion
}