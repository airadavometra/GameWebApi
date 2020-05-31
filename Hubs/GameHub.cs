using ManchkinWebApi.Controllers;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManchkinWebApi.Hubs
{
	public class GameHub : Hub
	{
		private GamersController GamersController = new GamersController();
		public async Task OnNewGamerJoin(string gamerName)
		{
			var gamers = GamersController.AddNewGamer(gamerName);
			//var gamers = GamersController.GetAllGamers();
			await Clients.Others.SendAsync("SetNewGamer", gamerName, gamers.Count -1);
			await Clients.Caller.SendAsync("SetAllGamers", gamers.Count - 1, gamers);
		}
	}
	
}