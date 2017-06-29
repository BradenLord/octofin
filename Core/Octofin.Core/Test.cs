using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Octofin.Core.Items;
using Octofin.Core.Items.Subtypes;
using Octofin.Core.Mechanics.Combat;
using Octofin.Core.Utility;
using Octofin.Core.Utility.Cache;

namespace Octofin.Core
{
    class Test
    {
        public static void Main(string[] args)
        {
            Log.info("Starting log.");

            Weapon weapon = DataCache.getData<Weapon>("testwep");

            DataCache.getData<Weapon>("bill");
            DataCache.getData<Weapon>("clinton");
            DataCache.getData<Enhancement>("sux");
            DataCache.getData<Enhancement>("man");

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create("./test.wep");

            formatter.Serialize(file, weapon);
            file.Close();

            file = File.Open("./test.wep", FileMode.Open);

            weapon = (Weapon) formatter.Deserialize(file);
            file.Close();

            Log.warn("I'm warnin ya, I'm unstable!");
            Log.info(weapon.slot + ": " + weapon.label);

            Log.info("Type: " + weapon.quality + " " + weapon.material + " " + weapon.subtype);

            Damage damage = weapon.primaryAttack.damages[0];

            try
            {
                throw new Exception("Ha, caught one!");
            }
            catch (Exception e)
            {
                Log.error("This ain't good!", e);
            }

            Log.info("Damage: " + damage.minimumDamage + "-" + damage.maximumDamage + " " + damage.damageType);
        }
    }
}
