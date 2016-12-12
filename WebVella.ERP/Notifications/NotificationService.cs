using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebVella.ERP.Utilities;
using WebVella.ERP.Utilities.Dynamic;

namespace WebVella.ERP.Notifications
{
	public class NotificationService : INotificationService
	{
		private static string SQL_NOTIFICATION_CHANNEL_NAME = "ERP_NOTIFICATIONS_CHANNNEL";
		private static NpgsqlConnection sqlConnection;
		private static List<Listener> listeners = new List<Listener>();

		public void Initialize(IServiceProvider serviceProvider)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => ! ( a.FullName.ToLowerInvariant().StartsWith("microsoft.") 
					|| a.FullName.ToLowerInvariant().StartsWith("system.") ) );
			foreach (var assembly in assemblies)
			{
				foreach (Type type in assembly.GetTypes())
				{
					try
					{
						var methods = from method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
									  where method.IsDefined(typeof(NotificationHandlerAttribute))
									  select method;

						foreach (MethodInfo method in methods)
						{
							var arrtibutes = Attribute.GetCustomAttributes(method, false).Where(a => a is NotificationHandlerAttribute);
							foreach (NotificationHandlerAttribute attr in arrtibutes)
							{
								Listener listener = new Listener();
								listener.Instance = new DynamicObjectCreater(type).CreateInstance();
								listener.Method = method;
								listener.Channel = (attr.Channel ?? string.Empty).ToLowerInvariant();
								listeners.Add(listener);
							}
						}
					}
					catch (Exception ex)
					{
						throw new Exception("An exception is thrown while register listener for: '" +
							assembly.FullName + ";" + type.Namespace + "." + type.Name + "'", ex);
					}
				}
			}

			ListenForNotifications();
		}

		private void ListenForNotifications()
		{
			sqlConnection = new NpgsqlConnection(Settings.ConnectionString);
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

			sqlConnection.Open();
			sqlConnection.Notification += (o, e) =>
			{
				string json;
				if (Encoding.UTF8.TryParseBase64(e.AdditionalInformation, out json))
				{
					var notification = JsonConvert.DeserializeObject<Notification>(json, settings);
					HandleNotification(notification);
				}
			};

			Task.Run(() =>
			{
				using (NpgsqlCommand command = new NpgsqlCommand($"LISTEN {SQL_NOTIFICATION_CHANNEL_NAME}", sqlConnection))
				{
					command.ExecuteNonQuery();
				}

				while (true)
				{
					sqlConnection.Wait();
				}


			});
		}

		private void HandleNotification(Notification notification)
		{
			List<Listener> listenersToNotify = listeners;
			if (!string.IsNullOrWhiteSpace(notification.Channel))
				listenersToNotify = listeners.Where(l => l.Channel.ToLowerInvariant() == notification.Channel.ToLowerInvariant()).ToList();

			foreach (var listener in listenersToNotify)
				Task.Run(() => { listener.Method.Invoke(listener.Instance, new object[] { notification }); } );
		}

		public void SendNotification(Notification notification)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
			var json = JsonConvert.SerializeObject(notification, settings);
			var encodedText = Encoding.UTF8.ToBase64(json);
			string sql = $"notify {SQL_NOTIFICATION_CHANNEL_NAME}, '{encodedText}';";

			using (NpgsqlConnection con = new NpgsqlConnection(Settings.ConnectionString))
			{
				con.Open();
				using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
				{
					command.Parameters.Add(new NpgsqlParameter("@message", json));
					command.ExecuteNonQuery();
				}
				con.Close();
			}
		}

	}
}
