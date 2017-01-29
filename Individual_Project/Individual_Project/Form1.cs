using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Individual_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            AllocConsole();

            Console.WriteLine(Helper.Convert4BitStringToInt("0000"));
            Console.WriteLine(Helper.Convert4BitStringToInt("0001"));
            Console.WriteLine(Helper.Convert4BitStringToInt("0010"));
            Console.WriteLine(Helper.Convert4BitStringToInt("0100"));
            Console.WriteLine(Helper.Convert4BitStringToInt("1000"));
            Console.WriteLine(Helper.Convert4BitStringToInt("1111"));

            for (int i = 0; i < 10; i++)
            {
                string test = Helper.GenerateXbitString(36);
                Console.WriteLine(Helper.PrintExpression(Helper.DecodeString(test)));
                Console.WriteLine(Helper.CalculateExpresssion(Helper.DecodeString(test)));
            }

            RunTheAlgorithm();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();


        private void RunTheAlgorithm()
        {
            //just loop endlessly until user gets bored :0)
            while (true)
            {
                //storage for our population of chromosomes.
                chromo_typ[] Population = new chromo_typ[ApplicationSettings.POPULATION_SIZE];

                //get a target number from the user. (no error checking)
                float Target;
                Console.WriteLine("Input a target number: ");
                Target = float.Parse(Console.ReadLine());

                //first create a random population, all with zero fitness.
                for (int i = 0; i < ApplicationSettings.POPULATION_SIZE; i++)
                {
                    Population[i] = new chromo_typ(Helper.GenerateXbitString(ApplicationSettings.CHROMO_LENGTH), 0);
                }

                int GenerationsRequiredToFindASolution = 0;

                //we will set this flag if a solution has been found
                bool bFound = false;
                float maxFitness = 0;
                int maxFitnessIndex = 0;

                //enter the main GA loop
                while (!bFound)
                {
                    //this is used during roulette wheel sampling
                    float TotalFitness = 0.0f;

                    // test and update the fitness of every chromosome in the 
                    // population
                    for (int i = 0; i < ApplicationSettings.POPULATION_SIZE; i++)
                    {
                        Population[i].fitness = Helper.AssignFitness(Population[i].bits, Target);

                        TotalFitness += Population[i].fitness;

                        if(maxFitness < Population[i].fitness)
                        {
                            maxFitness = Population[i].fitness;
                            maxFitnessIndex = i;
                        }
                    }

                    if(GenerationsRequiredToFindASolution % 100 == 0)
                    {
                        Console.WriteLine(Population[maxFitnessIndex].fitness);
                    }

                    // check to see if we have found any solutions (fitness will be 999)
                    for (int i = 0; i < ApplicationSettings.POPULATION_SIZE; i++)
                    {
                        if (Population[i].fitness == 100000.0f)
                        {
                            Console.WriteLine("Solution found in " + GenerationsRequiredToFindASolution + " generations!");

                            Console.WriteLine(Helper.PrintExpression(Population[i].bits));

                            bFound = true;

                            break;
                        }
                    }

                    // create a new population by selecting two parents at a time and creating offspring
                    // by applying crossover and mutation. Do this until the desired number of offspring
                    // have been created. 

                    //define some temporary storage for the new population we are about to create
                    chromo_typ[] temp = new chromo_typ[ApplicationSettings.POPULATION_SIZE];

                    int cPop = 0;

                    //loop until we have created POP_SIZE new chromosomes
                    while (cPop < ApplicationSettings.POPULATION_SIZE)
                    {
                        // we are going to create the new population by grabbing members of the old population
                        // two at a time via roulette wheel selection.
                        string offspring1 = Helper.Roulette(TotalFitness, Population);
                        string offspring2 = Helper.Roulette(TotalFitness, Population);

                        //add crossover dependent on the crossover rate
                        Helper.Crossover(ref offspring1, ref offspring2);

                        //now mutate dependent on the mutation rate
                        Helper.Mutate(offspring1);
                        Helper.Mutate(offspring2);

                        //add these offspring to the new population. (assigning zero as their
                        //fitness scores)
                        temp[cPop++] = new chromo_typ(offspring1, 0.0f);
                        temp[cPop++] = new chromo_typ(offspring2, 0.0f);

                    }//end loop

                    //copy temp population into main population array
                    for (int i = 0; i < ApplicationSettings.POPULATION_SIZE; i++)
                    {
                        Population[i] = temp[i];
                    }

                    ++GenerationsRequiredToFindASolution;

                    // exit app if no solution found within the maximum allowable number
                    // of generations
                    if (GenerationsRequiredToFindASolution > ApplicationSettings.MAX_ALLOWABLE_GENERATIONS)
                    {
                        Console.WriteLine("No solutions found this run!");

                        bFound = true;
                    }

                }

            }//end while
        }
    }
}

