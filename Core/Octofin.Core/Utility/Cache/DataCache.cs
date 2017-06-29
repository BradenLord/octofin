using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Octofin.Core.Items;
using Octofin.Core.Items.Subtypes;
using Octofin.Core.Mechanics.Combat;


namespace Octofin.Core.Utility.Cache
{
    public sealed class DataCache
    {
        static DataCache()
        {
            cache = new Dictionary<Type, Dictionary<string, MemoryStream>>();
            typeDirectories = new SortedDictionary<Type, string>(new TypeComparer());
            typeExtensions = new Dictionary<Type, string>();

            string baseLocation = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar;

            //Cache type initialization
            cache.Add(typeof(Weapon), new Dictionary<string, MemoryStream>());
            cache.Add(typeof(Enhancement), new Dictionary<string, MemoryStream>());

            typeDirectories.Add(typeof(Weapon), baseLocation + "Items" + Path.DirectorySeparatorChar + "Weapons" + Path.DirectorySeparatorChar);
            typeDirectories.Add(typeof(Enhancement), baseLocation + "Items" + Path.DirectorySeparatorChar + "Enhancements" + Path.DirectorySeparatorChar);

            typeExtensions.Add(typeof(Weapon), ".wep");
            typeExtensions.Add(typeof(Enhancement), ".enh");

            // Determine if game is in edit mode
            editLock = baseLocation + "edit.lock";
            EditMode = File.Exists(editLock);

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(destroyCache);

            // GAME BUILDING ONLY
            enableEditMode();

            // Test code (delet this)
            Weapon weapon = new Weapon("testwep");

            weapon.label = "'The Elvan Frosty'";
            weapon.primaryAttack = new Attack();
            weapon.primaryAttack.damages = new Damage[] { new Damage(DamageType.Ice, 23, 68) };

            weapon.material = Material.Mithril;
            weapon.subtype = WeaponSubtype.Axe;

            weapon.quality = Quality.Damaged;

            if (verboseLogs)
            {
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, weapon);

                Log.info("Static block");
                Log.info(stream.Position.ToString());
                Log.info(stream.Length.ToString());
                Log.info(BitConverter.ToString(stream.ToArray()));

                stream.Seek(0, SeekOrigin.Begin);
                Weapon ds = (Weapon)formatter.Deserialize(stream);
            }

            //setData(new Weapon("bill"));
            //setData(new Weapon("clinton"));
            //setData(new Enhancement("sux"));
            //setData(new Enhancement("man"));

            //setData(weapon);

            //exportCache(new CacheCounter());
            //importCache(new CacheCounter());
        }

        // Test code (delet this)
        public static readonly string RootLocation = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar;
        public static readonly string ItemLocation = RootLocation + "Items" + Path.DirectorySeparatorChar;

        public static readonly string WeaponExtension = ".wep";
        public static readonly string SecondaryExtension = ".sec";
        public static readonly string HeadgearExtension = ".hdg";
        public static readonly string UpperBodyExtension = ".ubd";
        public static readonly string LowerBodyExtension = ".lbd";
        public static readonly string FootgearExtension = ".fgr";
        public static readonly string ArmgearExtension = ".agr";
        public static readonly string EnhancementExtension = ".enh";
        public static readonly string ConsumableExtension = ".csm";
        //

        private static readonly Dictionary<Type, Dictionary<string, MemoryStream>> cache;
        private static readonly SortedDictionary<Type, string> typeDirectories;
        private static readonly Dictionary<Type, string> typeExtensions;
        private static readonly BinaryFormatter formatter = new BinaryFormatter();
        private static readonly string editLock;

        public static readonly bool EditMode;

        //Temporary ALPHA debug until serialization is established
        private static readonly bool verboseLogs = false;

        private DataCache() { }

        /// <summary>
        /// Writes edit lock file.  Requires restart to take effect.
        /// </summary>
        public static void enableEditMode()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(editLock));
            FileStream file = File.Create(editLock);
            file.Close();
        }

        public static T getData<T>(string name) where T : BinaryData
        {
            Type type = typeof(T);

            if(cache.ContainsKey(type))
            {
                if(cache[type].ContainsKey(name))
                {
                    MemoryStream stream = cache[type][name];

                    if (verboseLogs)
                    {
                        Log.info("Get data");
                        Log.info(stream.Position.ToString());
                        Log.info(stream.Length.ToString());
                        Log.info(BitConverter.ToString(stream.ToArray()));
                    }

                    stream.Seek(0, SeekOrigin.Begin);
                    T data = (T) formatter.Deserialize(stream);

                    if (Log.Debugging)
                    {
                        Log.info("Loaded '" + name + "' of type " + type + " from the cache.");
                    }

                    return data;
                }
                else
                {
                    throw new DataException("Could not find a entry with type " + type + " and name " + name + " in the cache.");
                }
            }
            else
            {
                throw new DataException("Attempted to load an object of unkown type " + type + " from the cache.");
            }
        }

        public static void setData<T>(T data) where T : BinaryData
        {
            if (EditMode)
            {
                Type type = typeof(T);

                if (cache.ContainsKey(type))
                {
                    if (cache[type].ContainsKey(data.name))
                    {
                        cache[type][data.name].Close();
                    }

                    MemoryStream stream = new MemoryStream();
                    formatter.Serialize(stream, data);

                    if (verboseLogs)
                    {
                        Log.info("Set data");
                        Log.info(stream.Position.ToString());
                        Log.info(stream.Length.ToString());
                        Log.info(BitConverter.ToString(stream.ToArray()));
                    }

                    cache[type][data.name] = stream;

                    if (Log.Debugging)
                    {
                        Log.info("Saved '" + data.name + "' of type " + type + " to the cache.");
                    }
                }
                else
                {
                    throw new DataException("Attempted to save an object of unkown type " + type + " to the cache.");
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot save data unless the game is in edit mode.");
            }
        }

        public static void importCache(CacheCounter counter)
        {
            SortedDictionary<Type, string[]> files = new SortedDictionary<Type, string[]>(new TypeComparer());

            counter.currentOperation = "Discovering data files...";
            counter.totalObjects = typeDirectories.Count;
            counter.consumedObjects = 0;
            counter.operationsFinished = false;

            int totalFiles = 0;

            Log.info("Starting import of data cache.");

            if (Log.Debugging)
            {
                Log.info("Searching datafile directories.");
            }

            foreach(Type type in typeDirectories.Keys)
            {
                string directory = typeDirectories[type];
                string extension = typeExtensions[type];
                string[] fileNames = Directory.GetFiles(directory, "*" + extension);

                Array.Sort(fileNames);
                files.Add(type, fileNames);

                counter.consumedObjects++;
                totalFiles += fileNames.Length;

                if(Log.Debugging)
                {
                    Log.info("Found " + fileNames.Length + " files of type " + type + " at " + directory);
                }

                System.Threading.Thread.Sleep(1000);
            }

            counter.totalObjects = totalFiles;
            counter.consumedObjects = 0;

            if(Log.Debugging)
            {
                Log.info("Reading in file streams.");
            }

            //Make this multithreaded if speed becomes an issue (KISS for now)
            using (MD5 md5 = MD5.Create())
            {
                byte[][] rawChecksum = new byte[totalFiles][];

                foreach (Type type in files.Keys)
                {
                    counter.currentOperation = "Loading " + type.Name + " data...";
                    string[] fileNames = files[type];

                    foreach (string file in fileNames)
                    {
                        string name = Path.GetFileNameWithoutExtension(file);
                        importData(file, type, name);

                        //if (!EditMode) //Diasable check for debugging purposes
                        {
                            rawChecksum[counter.consumedObjects] = md5.ComputeHash(cache[type][name]);
                        }

                        counter.consumedObjects++;

                        if(Log.Debugging)
                        {
                            Log.info("Imported " + name + " of type " + type);
                        }
                    }
                }

                //if (!EditMode) // Disable check for debugging purposes
                {
                    byte[] rawFilesum = rawChecksum.SelectMany(x => x).ToArray();
                    byte[] byteChecksum = md5.TransformFinalBlock(rawFilesum, 0, rawFilesum.Length);
                    string checksum = BitConverter.ToString(byteChecksum).Replace("-", "");

                    if (!EditMode) // Enable/Disable check for debugging purposes
                    {
                        if (!checksum.Equals(Game.versionChecksum))
                        {
                            throw new DataException("Checksum mismatch.  Data files have been corrupted.");
                        }
                    }

                    if(Log.Debugging)
                    {
                        Log.info(checksum);
                    }

                    Log.info("Data cache import checksum validated.");
                }
            }

            counter.currentOperation = "Loading complete!";
            counter.operationsFinished = true;
            Log.info("Data cache import complete.");
        }

        public static void exportCache(CacheCounter counter)
        {
            if(!EditMode)
            {
                throw new InvalidOperationException("Cannot export the cache unless the game is in edit mode");
            }

            int fileCount = 0;

            foreach(Type type in cache.Keys)
            {
                fileCount += cache[type].Count;
            }

            counter.totalObjects = fileCount;
            counter.consumedObjects = 0;
            counter.operationsFinished = false;

            Log.info("Starting data cache export.");

            foreach(Type type in cache.Keys)
            {
                counter.currentOperation = "Saving " + type.Name + " data...";

                foreach(string name in cache[type].Keys)
                {
                    string path = typeDirectories[type] + name + typeExtensions[type];
                    exportData(path, type, name);

                    if(Log.Debugging)
                    {
                        Log.info("Exported " + name + " of type" + type);
                    }

                    counter.consumedObjects++;
                }
            }

            counter.currentOperation = "Saving complete!";
            counter.operationsFinished = true;
            Log.info("Data cache export complete.");
        }

        private static void exportData(string path, Type type, string name)
        {
            if(path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                throw new FormatException("Specified " + type.Name + " has an invalid name '" + name + "'");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (FileStream file = File.Create(path))
            {
                MemoryStream stream = cache[type][name];

                if (verboseLogs)
                {
                    Log.info("Export data");
                    Log.info(stream.Position.ToString());
                    Log.info(stream.Length.ToString());
                    Log.info(BitConverter.ToString(stream.ToArray()));
                }

                stream.WriteTo(file);

                System.Threading.Thread.Sleep(1000);
            }
        }

        private static void importData(string path, Type type, string name)
        {
            using (FileStream file = File.Open(path, FileMode.Open))
            {
                byte[] content = new byte[file.Length];
                file.Read(content, 0, (int) file.Length);

                MemoryStream stream = new MemoryStream(content);

                if(verboseLogs)
                {
                    Log.info("Import data");
                    Log.info(stream.Position.ToString());
                    Log.info(stream.Length.ToString());
                    Log.info(BitConverter.ToString(stream.ToArray()));
                }

                cache[type][name] = stream;

                System.Threading.Thread.Sleep(1000);
            }
        }

        public static void destroyCache(object sender, EventArgs args)
        {
            foreach (Type firstKey in cache.Keys)
            {
                foreach (string secondKey in cache[firstKey].Keys)
                {
                    cache[firstKey][secondKey].Close();
                }
            }

            cache.Clear();
        }
    }

    // Used to sort dictionaries by type name (actual ordering is not important as long as they are ordered)
    class TypeComparer : IComparer<Type>
    {
        public int Compare(Type x, Type y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
