using System;
using Serilog;

namespace Farm
{
    public class FarmDice
    {
        private readonly Enimal[] dice1;
        private readonly Enimal[] dice2;
        private readonly Random random;

        public FarmDice()
        {
            dice1 = new []
            {            
                Enimal.Hen,
                Enimal.Goat,
                Enimal.Hen,
                Enimal.Goat,
                Enimal.Hen,
                Enimal.Pig,
                Enimal.Hen,
                Enimal.Fox,
                Enimal.Hen,
                Enimal.Goat,
                Enimal.Hen,
                Enimal.Cow
            };

            dice2 = new []
            {                  
                Enimal.Hen,
                Enimal.Horse,
                Enimal.Hen,
                Enimal.Pig,
                Enimal.Hen,
                Enimal.Goat,
                Enimal.Hen,
                Enimal.Pig,
                Enimal.Hen,
                Enimal.Wolf,      
                Enimal.Hen,
                Enimal.Goat
            };

            random = new Random();
        }

        public (Enimal, Enimal) Roll()
        {
            int diceIndex1 = random.Next(dice1.Length - 1);
            int diceIndex2 = random.Next(dice2.Length - 1);
            Log.Information($"FarmDice.Roll()\ndiceIndex1: {diceIndex1}\ndiceIndex2: {diceIndex2}");
            return (dice1[diceIndex1], dice2[diceIndex2]);
        }

        public enum Enimal
        {
            Hen,
            Goat,
            Pig,
            Cow,
            Horse,
            Fox,
            Wolf
        }
    }
}