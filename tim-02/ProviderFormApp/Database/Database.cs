using System;
using System.Data;
using System.Data.Common;

namespace ProviderFormApp.Database;
public class Database
{
	private static Database instance;
	

	public static Database getDatabase()
	{
		if(instance == null)
			instance = new Database();

		return instance;
	}

	// TO - BE implemented
	private Database()
	{
		
	}
	
}
