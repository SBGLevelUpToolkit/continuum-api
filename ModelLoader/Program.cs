using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLoader
{
    
    class Program
    {
        private const string DIMENSIONS_FILE = "Dimensions.txt";
        private const string CAPABILITIES_FILE = "Capabilities.txt";
        private const string LEVELS_FILE = "Levels.txt";
        private const int EXPECTED_FIELD_COUNT = 5;

        static void Main(string[] args)
        {
            if (!CheckForLevelsFile())
            {
                Console.Write("{0} was not found.", LEVELS_FILE);
                return;
            }

            LoadLevels();


            if (!CheckForDimensionsFile())
            {
                Console.Write("{0} was not found.", DIMENSIONS_FILE);
                return;
            }

            LoadDimensions();

            if (!CheckForCapabiltiesFile())
            {
                Console.Write("{0} was not found.", CAPABILITIES_FILE);
                return;
            }

            LoadCapabilities();

            Console.WriteLine("Press a key to continue.");
            Console.ReadKey();
        }

        private static bool CheckForLevelsFile()
        {
            return System.IO.File.Exists(LEVELS_FILE);
        }

        private static void LoadLevels()
        {
            Continuum.Data.DimensionRepo repo = new Continuum.Data.DimensionRepo();

            Console.WriteLine("Creating Capabilies...");

            var levels = repo.CapabilityLevels();

            var reader = GetReader(LEVELS_FILE);

            while (!reader.EndOfStream)
            {
                string level = reader.ReadLine().Trim();
                if (!levels.Any(i => i.DisplayName == level))
                {
                    Console.WriteLine(string.Format("{0}", level));
                    repo.CreateLevel(level);
                }
            }

            repo.SaveChanges();

            Console.WriteLine("...complete.");
        }


        private static void LoadCapabilities()
        {
            Continuum.Data.DimensionRepo dimensionRepo = new Continuum.Data.DimensionRepo();
          
            Console.WriteLine("Creating Capabilies...");

            int linecount = 0;

            var reader = GetReader(CAPABILITIES_FILE);
           
            while (!reader.EndOfStream)
            {
                bool skipLine = false;
                linecount++;

                string importLine = reader.ReadLine();
                string[] fields = importLine.Split(';');
                if (fields.Length == EXPECTED_FIELD_COUNT)
                {
                    var row = ExtractFields(fields);
                    if (row.IsValid())
                    {
                        var dimension = dimensionRepo.GetDimensionByName(row.Dimension);
                        if (dimension != null)
                        {
                            var level = dimensionRepo.CapabilityLevels().Where(i => i.DisplayName == row.Level).FirstOrDefault();
                            if (level != null)
                            {

                                Console.WriteLine(string.Format("{0}\t{1}", row.Dimension, row.Description));

                                Continuum.Data.Capability capability = new Continuum.Data.Capability()
                                {
                                    Active = false,
                                    Description = row.Description,
                                    Dimension = dimension,
                                    Level = level
                                };

                                dimension.Capabilities.Add(capability);
                            }
                            else
                            {
                                skipLine = true;
                            }
                        }
                        else
                        {
                            skipLine = true;
                        }
                    }
                    else
                    {
                        skipLine = true;
                    }
                }
                else
                {
                    skipLine = true;
                }

                if(skipLine)
                {
                    Console.WriteLine(String.Format("WARNING: Skipping line {0} due to incorrect line format or data issue.", linecount));
                    Console.WriteLine("WARNING: " + importLine);
                }
            }

            dimensionRepo.SaveChanges();

            Console.WriteLine("...complete.");

        }

        private static ImportRow ExtractFields(string[] fields)
        {
            ImportRow row = new ImportRow();
            row.Dimension = fields[0].Trim();
            row.Level = fields[1].Trim();
            row.TempId = fields[2].Trim();
            row.Description = fields[3].Trim();
            row.Predecessors = fields[4].Split(',');
            return row;
        }

        private static bool CheckForCapabiltiesFile()
        {
            return System.IO.File.Exists(CAPABILITIES_FILE);
        }

        private static void LoadDimensions()
        {
            Continuum.Data.DimensionRepo repo = new Continuum.Data.DimensionRepo();

            Console.WriteLine("Creating dimensions....");

            var reader = GetReader(DIMENSIONS_FILE);
            while (!reader.EndOfStream)
            {
                string dimensionName = reader.ReadLine().Trim();
                if (!String.IsNullOrEmpty(dimensionName))
                {
                    if (!repo.DimensionExists(dimensionName))
                    {
                        Console.WriteLine(String.Format("Creating {0}", dimensionName));
                        var dimension = new Continuum.Data.Dimension() { Name = dimensionName, Active = false };
                        repo.Create(dimension);
                    }   
                }
            }

            repo.SaveChanges();

            Console.WriteLine("...complete.");
        }

        private static bool CheckForDimensionsFile()
        {
            return System.IO.File.Exists(DIMENSIONS_FILE);
        }

        private static System.IO.StreamReader GetReader(string DIMENSIONS_FILE)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(DIMENSIONS_FILE);
            return reader;
        }
    }
}
