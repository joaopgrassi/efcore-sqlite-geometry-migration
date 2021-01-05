using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Linq;

namespace App
{
	class Program
	{
		static void Main(string[] args)
		{
			using var db = new AppContext();

			db.Database.Migrate();

			var resource = new Resource
			{
				Id = Guid.NewGuid(),
				Location = new Point(1, 1)
				{
					SRID = 4326
				}
			};

			db.Resources.Add(resource);
			db.SaveChanges();

			var r = db.Resources.Single(r => r.Id == resource.Id);

			Console.ReadLine();
		}
	}
}
