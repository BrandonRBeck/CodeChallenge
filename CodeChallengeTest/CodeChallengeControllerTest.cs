using CodeChallenge.Controllers;
using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace CodeChallengeTest;

public class CodeChallengeControllerTest
{
    private readonly ITestOutputHelper _output;

    public CodeChallengeControllerTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [InlineData("test")]
    public async void TestControllerEndpointSuccess(string input)
    {
        var codeChallengeService = new CodeChallengeService();
        var controller = new CodeChallengeController(codeChallengeService);
        var result = await controller.ReverseString(input);
        var resultObject = GetObjectResultContent<ChallengeResponseObject>(result);
        Assert.NotNull(resultObject.Response);
        Assert.Equal("tset", resultObject.Response);
    }

    [Theory]
    [InlineData("test")]
    public async void TestControllerEndpointFailure(string input)
    {
        var codeChallengeService = new CodeChallengeService();
        var controller = new CodeChallengeController(codeChallengeService);
        var result = await controller.ReverseString(input);
        var resultObject = GetObjectResultContent<ChallengeResponseObject>(result);
        Assert.NotNull(resultObject.Response);
        Assert.NotEqual("test stuff", resultObject.Response);
    }

    private static T GetObjectResultContent<T>(ActionResult<T> result)
    {
        return (T)((ObjectResult)result.Result).Value;
    }

    public enum Option
    {
        BUY,
        SELL,
        NOTHING
    }

    [Fact]
    public void TestInline()
    {
        var max = 10;
        SomeCode(0,1,1,max);
    }

    public void SomeCode(int first, int second, int counter, int max)
    {
        if (counter > max)
            return;
        _output.WriteLine($"{first} ");
        SomeCode(second, first + second, counter+1, max);

    }
    public void LootCode()
    {
        var potion = new Loot()
        {
            id = 1,
            val = 30,
            width = 1,
            height = 1,
            name = "Potion of Potionentiality"

        };
        var jewel = new Loot()
        {
            id = 2,
            val = 150,
            width = 3,
            height = 1,
            name = "Jeweled Dog Draught Excluder"
        };
        var shield = new Loot()
        {
            id = 3,
            val = 300,
            width = 2,
            height = 2,
            name = "Spartan Shield"
        };
        var sword = new Loot()
        {
            id = 4,
            val = 120,
            width = 1,
            height = 3,
            name = "Palindromic Sword o’Drows"
        };
        var armor = new Loot()
        {
            id = 5,
            val = 540,
            width = 2,
            height = 3,
            name = "Unobsidian Armor"
        };
        var wardrobe = new Loot()
        {
            id = 6,
            val = 1337,
            width = 20,
            height = 10,
            name = "Wardrobe of Infinite Lions"
        };
        var lootTable = new List<Loot>
        {
            potion,
            jewel,
            shield,
            sword,
            armor,
            wardrobe
        };

        lootTable = lootTable.OrderByDescending(x => x.GetValPerSpace()).ToList();

        while (true)
        {
            int space = InventorySpaceLeft();
            var lootItemToFill = getLootAvailableForGivenSpace(lootTable, space);
            if(lootItemToFill != null)
            {
                FillInventory(lootItemToFill);
            }

            if(space == 0)
            {
                break;
            }
        }

        var builder = new StringBuilder();
        for(int i = 0; i< inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                builder.Append(inventory[i,j]);
            }
            builder.AppendLine();
        }
        _output.WriteLine(builder.ToString());
    }

    public int[,] inventory = new int[4, 5] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };

    public class Loot
    {
        public double val { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string? name { get; set; }
        public int id { get; set; }
        public double GetValPerSpace()
        {
            return val / width * height;
        }
    }

    private void FillInventory(Loot item, int startHeight = 0, int startWidth = 0)
    {
        if(startHeight >= inventory.GetLength(1) || startWidth >= inventory.GetLength(0))
        {
            return;
        }
        if(allSlotsAvailable(item, startHeight, startWidth))
        {
            for (int i = 0; i < item.width -1; i++)
            {
                for (int j = 0; j < item.height -1; j++)
                {
                    inventory[startWidth + i, startHeight + j] = item.id;
                }
            }
            return;
        }
        else
        {
            startWidth++;
            if (startWidth > inventory.GetLength(0))
            {
                startHeight++;
                startWidth = 0;
            }
            if (startHeight > inventory.GetLength(1))
            {
                return;
            }
            FillInventory(item, startHeight, startWidth);
        }
    }

    private bool allSlotsAvailable(Loot item, int startHeight, int startWidth)
    {
        try
        {
            for (int i = 0; i < item.width -1; i++)
            {
                for (int j = 0; j < item.height -1; j++)
                {
                    if (inventory[startWidth + i, startHeight + j] != 0)
                    {
                        return false;
                    }
                }
            }
        }
        catch(IndexOutOfRangeException)
        {
            return false;
        }
        return true;
    }

    private Loot getLootAvailableForGivenSpace(List<Loot> lootTable, int space)
    {
        foreach(var item in lootTable)
        {
            if(item.height * item.width < space)
            {
                return item;
            }
        }
        return null;
    }

    public int InventorySpaceLeft()
    {
        int space = 0;
        for(int i = 0; i < inventory.GetLength(0); i++)
        {
            for(int j = 0; j < inventory.GetLength(1); j++)
            {
                if (inventory[i,j] == 0)
                {
                    space++;
                }
            }
        }
        return space;
    }

    public static string[] ReverseArray(string[] array)
    {
        int i = 0;
        int j = array.Length - 1;

        while (i <= j)
        {
            string tempLeft = array[i];
            array[i] = array[j];
            array[j] = tempLeft;
            i++;
            j--;
        }
        return array;

    }
}