using System;
using System.Collections.Generic;
using System.IO;

namespace Slcsp
{
    public class Program
    {
        public static void Main(string[] args)
        {
			SLCSP slcsp = new SLCSP();
			Zips zips = new Zips();
			Plans plans = new Plans();
			slcsp.readSLCSP("/Users/rohithkumarsingetham/Projects/Slcsp/Slcsp/slcsp.csv");
			zips.readZips("/Users/rohithkumarsingetham/Projects/Slcsp/Slcsp/zips.csv");
			plans.readPlans("/Users/rohithkumarsingetham/Projects/Slcsp/Slcsp/plans.csv");
			try
			{
				StreamWriter writer = new StreamWriter("/Users/rohithkumarsingetham/Documents/OutputSlcsp.csv");

				writer.WriteLine("zipcode,rate\n");

				foreach (SLCSP zip in slcsp.zipcodeList)
				{
					if (!zips.removeFromZips.Contains(zip.zipcode))
					{
						Zips objZips = zips.zipData[zip.zipcode];
						if (plans.hPlans.ContainsKey(objZips.State + objZips.rate))
						{
							Plans objPlans = plans.hPlans[objZips.State + objZips.rate];

							writer.WriteLine(zip.zipcode + "," + objPlans.SecondleastRate + "\n");
						}
						else
						{
							writer.WriteLine(zip.zipcode + ",\n");
						}
					}
					else
					{
						writer.Write(zip.zipcode + ",\n");
					}
				}

				writer.Flush();
				writer.Close();
			}
			catch (IOException ex)
			{
				Console.Write(ex);
			}
		}
    }
	public class Reader
	{
		public StreamReader streamReader = null;
		public string text = null;
		public void getFile(string filePath)
		{
			try
			{
				if (File.Exists(filePath))
				{
					streamReader = new StreamReader(File.OpenRead(filePath));

				}
			}
			catch (IOException ex)
			{
				Console.Write(ex);
			}
		}
	}
	public class SLCSP : Reader
	{
	public String zipcode;
	public Double rate;
	public List<SLCSP> zipcodeList;
	public List<SLCSP> readSLCSP(String filePath)
	{
		zipcodeList = new List<SLCSP>();
		getFile(filePath);
		try
		{
				streamReader.ReadLine();
			while ((text = streamReader.ReadLine()) != null)
			{
				SLCSP elemSLCSP = new SLCSP();
				text = text.Substring(0, text.Length - 1);

				elemSLCSP.zipcode = text;
				zipcodeList.Add(elemSLCSP);
			}
		}
		catch (IOException ex)
		{
				Console.Write(ex);
		}

		return zipcodeList;
	}
}
public class Zips : Reader
{
	public String ZipCode;
public String State;
public String CountyCode;
public String CountyName;
public int rate;
public Dictionary<String, Zips> zipData;
public List<String> removeFromZips;

public Dictionary<String, Zips> readZips(String filePath)
{
	zipData = new Dictionary<String, Zips>();
	removeFromZips = new List<String>();

	getFile(filePath);

	try
	{
				streamReader.ReadLine();
		while ((text = streamReader.ReadLine()) != null)
		{
			Zips mapZips = new Zips();
			String[] array = text.Split(",");

			mapZips.ZipCode = array[0];
			mapZips.State = array[1];
			mapZips.CountyCode = array[2];
			mapZips.CountyName = array[3];
			mapZips.rate = Convert.ToInt32(array[4]);
			if (zipData.ContainsKey(mapZips.ZipCode))
			{
				Zips objZips = zipData[mapZips.ZipCode];
                        if (objZips.State.Equals(mapZips.State) && objZips.rate == mapZips.rate)
                        {
                            zipData[mapZips.ZipCode] = mapZips;
                        }
                        else
                        {
                            removeFromZips.Add(mapZips.ZipCode);
						}
			}
			else
			{
				zipData.Add(mapZips.ZipCode, mapZips);
			}
		}
	}
	catch (IOException ex)
	{
				Console.Write(ex);
	}

	return zipData;
}
}
public class Plans : Reader
{
public String planId;
public String State;
public String metalLevel;
public Double SecondleastRate;
public Double leastRate;
public int rate;
public Dictionary<String, Plans> hPlans = new Dictionary<string, Plans>();

public Dictionary<String, Plans> readPlans(String filePath)
{
	hPlans = new Dictionary<string, Plans>();
	getFile(filePath);

	try
	{
				streamReader.ReadLine();
		while ((text = streamReader.ReadLine()) != null)
		{
			Plans mapPlans = new Plans();
			String[] array = text.Split(",");

			mapPlans.planId = array[0];
			mapPlans.State = array[1];
			mapPlans.metalLevel = array[2];

			mapPlans.leastRate = Convert.ToDouble(array[3]);
			mapPlans.SecondleastRate = Convert.ToDouble(array[3]);
			mapPlans.rate = Convert.ToInt32(array[4]);
			if (mapPlans.metalLevel.Contains("Silver"))
			{
				if (hPlans.ContainsKey(mapPlans.State + mapPlans.rate))
				{
					Plans objPlans = hPlans[mapPlans.State + mapPlans.rate];

							if (mapPlans.leastRate < objPlans.leastRate)
					{
						mapPlans.SecondleastRate = objPlans.leastRate;
						hPlans[mapPlans.State + mapPlans.rate] = mapPlans;
					}
					else if (mapPlans.SecondleastRate < objPlans.SecondleastRate)
					{
						mapPlans.leastRate = objPlans.leastRate;
						hPlans[mapPlans.State + mapPlans.rate] = mapPlans;
					}
				}
				else
				{
					hPlans.Add(mapPlans.State + mapPlans.rate, mapPlans);
				}
			}
		}
	}
	catch (IOException ex)
	{
				Console.Write(ex);
	}

	return hPlans;
}
}

}
