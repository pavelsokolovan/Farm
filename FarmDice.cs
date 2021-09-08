using System;

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

        public (Enimal, Enimal) RollDice()
        {
            return (dice1[random.Next(13)], dice2[random.Next(13)]);
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