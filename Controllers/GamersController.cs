using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;

namespace ManchkinWebApi.Controllers
{
	public class GamersController
	{
		private List<Gamer> gamers = new List<Gamer>(0);
		private static CloudBlobClient _blobClient;
		private const string _blobContainerName = "gamerscontainer";
		private static CloudBlobContainer _blobContainer;
		private readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=gamewebapistorage;AccountKey=KofDXqFjouAQ10HEb+fXz0pYi5cwdkMczo/KAV/nJutWzyuszT/S4KBfeHfmc5mitaIPZhED82IxsT0QX/fwsA==;EndpointSuffix=core.windows.net";

		public List<Gamer> AddNewGamer(string gamerName)
		{
			try
			{
				CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

				_blobClient = storageAccount.CreateCloudBlobClient();
				_blobContainer = _blobClient.GetContainerReference(_blobContainerName);

				CloudBlockBlob blob = _blobContainer.GetBlockBlobReference("players.txt");

				string text = blob.DownloadTextAsync().Result;

				var lines = text.Split("\r\n");
				foreach (var line in lines)
				{
					if (!string.IsNullOrEmpty(line))
					{
						var t = line.Split(", ");
						gamers.Add(new Gamer
						{
							Id = t[0],
							Name = t[1]
						});
					}
					
				}

				var newGamerId = gamers.Count;
				if (newGamerId >= 6)
				{
					return null;
				}
				gamers.Add(new Gamer
				{
					Id = newGamerId.ToString(),
					Name = gamerName
				});

				var newFileContent = ConvertGamersToString();

				blob.UploadText(newFileContent);

				return gamers;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		public List<Gamer> GetAllGamers()
		{
			try
			{
				CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

				_blobClient = storageAccount.CreateCloudBlobClient();
				_blobContainer = _blobClient.GetContainerReference(_blobContainerName);

				CloudBlockBlob blob = _blobContainer.GetBlockBlobReference("players.txt");

				string text = blob.DownloadTextAsync().Result;

				var lines = text.Split("\r\n");
				foreach (var line in lines)
				{
					if (!string.IsNullOrEmpty(line))
					{
						var t = line.Split(", ");
						gamers.Add(new Gamer
						{
							Id = t[0],
							Name = t[1]
						});
					}
				}
				return gamers;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		private string ConvertGamersToString()
		{
			string result = "";
			foreach (var gamer in gamers)
			{
				string gamerInfo = gamer.Id + ", " + gamer.Name;
				result = result + gamerInfo + "\r\n";
			}
			return result;
		}
	}
}
