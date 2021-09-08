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
                Enimal.Chick,
                Enimal.Goat,
                Enimal.Chick,
                Enimal.Goat,
                Enimal.Chick,
                Enimal.Pigg,
                Enimal.Chick,
                Enimal.Fox,
                Enimal.Chick,
                Enimal.Goat,
                Enimal.Chick,
                Enimal.Cow
            };

            dice2 = new []
            {                  
                Enimal.Chick,
                Enimal.Horse,
                Enimal.Chick,
                Enimal.Pigg,
                Enimal.Chick,
                Enimal.Goat,
                Enimal.Chick,
                Enimal.Pigg,
                Enimal.Chick,
                Enimal.Wolf,      
                Enimal.Chick,
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
            Chick,
            Goat,
            Pigg,
            Cow,
            Horse,
            Fox,
            Wolf
        }
    }
}