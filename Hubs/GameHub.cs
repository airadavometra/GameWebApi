using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManchkinWebApi.Hubs
{
	public class GameHub : Hub
	{
		readonly Random rnd = new Random();
		public async Task SendMessage(string message)
		{
			await Clients.All.SendAsync("ReceiveMessage", message, "ALLO FROM SERVER");
		}

		public async Task SendRandomNumber()
		{
			int num = rnd.Next(1, 13);

			await Clients.All.SendAsync("ReceiveRandomNumber", num);
		}
	}
}