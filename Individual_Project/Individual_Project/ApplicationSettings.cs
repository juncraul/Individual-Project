using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual_Project
{
    public static class ApplicationSettings
    {
        public static Random random = new Random();
        public const float MUTATION_RATE = 0.007f;
        public const float CROSSOVER_RATE = 0.7f;
        public const int POPULATION_SIZE = 100;
        public const int CHROMO_LENGTH = 40;
        public const int MAX_ALLOWABLE_GENERATIONS = 10000;
    }
}
