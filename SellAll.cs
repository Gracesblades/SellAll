using AOSharp.Common.GameData;
using AOSharp.Common.GameData.UI;
using AOSharp.Core;
using AOSharp.Core.Inventory;
using AOSharp.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SellAllPlugin
{
    public class SellAll : AOPluginEntry
    {
        public static bool Toggle = false;
        private static readonly List<string> VendorNames = new List<string>
        {
            "OT Specialist Commerce",
            "Specialist Commerce",
            "Clan Specialist Commerce"
        };
        private bool _lastHadVendor = false;

        public override void Run()
        {
            // Chat info at startup
            Chat.WriteLine("==== SellAll ====");
            Chat.WriteLine("Type /sellall to toggle auto-selling of all items in bags named 'loot'.");
            Chat.WriteLine("This will automatically open loot bags, move items to your main inventory, and sell them to the nearest vendor.");
            Chat.WriteLine("I will NOT sell your Containers themselves!");
            Chat.WriteLine("Make sure you are near a Specialist Commerce Terminal.");
            Chat.WriteLine("====================");
            Chat.WriteLine("     ");
            Chat.WriteLine("===============================================");
            Chat.WriteLine("!!! WARNING: CLEAR YOUR INVENTORY OF ALL ITEMS");
            Chat.WriteLine("!!! YOU DON'T WANT SOLD!!");
            Chat.WriteLine("===============================================");

            Game.OnUpdate += OnUpdate;

            Chat.RegisterCommand("sellall", (string command, string[] param, ChatWindow chatWindow) =>
            {
                Toggle = !Toggle;
                Chat.WriteLine($"SellAll is now {(Toggle ? "ENABLED" : "DISABLED")}");
            });
        }

        private void OnUpdate(object s, float deltaTime)
        {
            if (!Toggle)
                return;

            // Find the nearest vendor by your provided list
            SimpleItem vendor = null;
            foreach (string vname in VendorNames)
            {
                if (DynelManager.Find(vname, out vendor))
                    break;
            }

            if (vendor == null)
            {
                if (_lastHadVendor)
                {
                    Chat.WriteLine("[SellAll] No Trader shop found nearby.");
                    _lastHadVendor = false;
                }
                return;
            }
            _lastHadVendor = true;

            // Step 1: Ensure all "loot" bags are open (by using them)
            foreach (Item bag in Inventory.Items
                .Where(c => c != null &&
                            c.UniqueIdentity.Type == IdentityType.Container &&
                            string.Equals(c.Name, "loot", StringComparison.OrdinalIgnoreCase)))
            {
                // If bag is not already open, open it (Inventory.Backpacks = open containers)
                if (!Inventory.Backpacks.Any(b => b.Identity == bag.UniqueIdentity))
                {
                    bag.Use();
                }
            }

            // Step 2: Move all items from open "loot" bags to main inventory (if space)
            foreach (Container lootBag in Inventory.Backpacks
                .Where(b => b != null &&
                            string.Equals(b.Name, "loot", StringComparison.OrdinalIgnoreCase) &&
                            b.Items.Count() > 0))
            {
                foreach (Item item in lootBag.Items.ToList())
                {
                    if (item == null)
                        continue;
                    // If the item is a container, skip it
                    if (item.UniqueIdentity.Type == IdentityType.Container)
                        continue;
                    if (Inventory.NumFreeSlots < 1)
                    {
                        Chat.WriteLine("[SellAll] Main inventory is full, can't move more items.");
                        break;
                    }
                    item.MoveToInventory();
                }
            }

            // Step 3: Sell all non-container items in main inventory
            foreach (Item sellItem in Inventory.Items
                .Where(c => c != null &&
                            c.Slot.Type == IdentityType.Inventory &&
                            c.UniqueIdentity.Type != IdentityType.Container))
            {
                vendor.Use(); // Open vendor shop
                Trade.AddItem(DynelManager.LocalPlayer.Identity, sellItem.Slot);
                Trade.Accept(Identity.None);
            }
        }
    }
}
