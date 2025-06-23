# SellAll Plugin for AOSharp

## Overview

**SellAll** is an AOSharp in-game plugin for Anarchy Online that helps automate the process of selling loot.  
It is designed for **in-game AOSharp** and is **fully chat-based** for maximum compatibility.

---

## Features

- **Toggle auto-selling** with `/sellall`
- **Sells all items in bags named `'loot'`** (case-insensitive)
- **Never sells the containers/bags themselves**
- **Automatically opens loot bags, moves items to main inventory, and sells to the nearest vendor**
- **Warns you if inventory is full or no vendor is nearby**
- **Only uses chat for all info and warnings** (no popups or UI windows, due to AOSharp plugin limitations)

---

## How to Use

1. **Inject plugin.
2. **In-game, you will see this chat info when the plugin loads:**
    ```
    ==== SellAll ====
    Type /sellall to toggle auto-selling of all items in bags named 'loot'.
    This will automatically open loot bags, move items to your main inventory, and sell them to the nearest vendor.
    I will NOT sell your Containers themselves!
    Make sure you are near a Specialist Commerce Terminal.
    ====================

    ===============================================
    !!! WARNING: CLEAR YOUR INVENTORY OF ALL ITEMS
    !!! YOU DON'T WANT SOLD!!
    ===============================================
    ```
3. **Type `/sellall` in chat** to toggle auto-selling on or off.

---

## Important Warnings

- **Clear your inventory of any items you do not want to be sold!**
- This plugin sells every item inside bags named `'loot'` (can be anywhere in name) when enabled, as soon as you are near a recognized vendor.
- The plugin **cannot create pop-up windows** or custom UI because of AOSharp in-game plugin API restrictions. All feedback and warnings are shown in chat.

---

## Vendor Recognition

The plugin searches for these vendor names (case-insensitive):
- OT Specialist Commerce
- Specialist Commerce
- Clan Specialist Commerce

You must be near one of these to sell items.

---

## Support

If you have questions, feature requests, or want to report a bug,  
please contact the plugin author or your AOSharp admin.

---

**Happy looting, and double-check your inventory before you sell!**
