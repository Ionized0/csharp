using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChanceToDraw
{
    class Program
    {
        static void Main(string[] args)
        {
            double numCards = double.Parse(args[0]);
            double turnNum = (double.Parse(args[1]) == 0) ? 1 : double.Parse(args[1]);
            double numCardsMulliganed = (args[2] == "y") ?  3 : 4; // First : Second
            double deckSize = 30;
            double numCopiesWanted = 2; // Describes the higher amount wanted. Amount wanted: 1 - numCopiesWanted
            double chanceToDraw = 0;
            double chanceBeforeMulligan;
            double chanceMulligan;
            double chanceByTurn;

            Console.WriteLine("Number of copies in deck: {0}", numCards);
            Console.WriteLine("Number of turns to get 1 or more copies: {0}", turnNum);
            Console.WriteLine("Number of cards mulliganed: {0}\n", numCardsMulliganed);

            chanceBeforeMulligan = getHypergeometricDistribution(deckSize, numCards, numCardsMulliganed, numCopiesWanted);
            Console.WriteLine("Chance by initial draw: {0:f2}%", chanceBeforeMulligan * 100);
            chanceToDraw += (1 - chanceBeforeMulligan);

            deckSize = 30 - numCardsMulliganed;

            chanceMulligan = getHypergeometricDistribution(deckSize, numCards, numCardsMulliganed, numCopiesWanted);
            Console.WriteLine("Chance by mulliganning {1} cards: {0:f2}%", chanceMulligan * 100, numCardsMulliganed);
            chanceToDraw *= (1 - chanceMulligan);

            chanceByTurn = getHypergeometricDistribution(deckSize, numCards, turnNum, numCopiesWanted);
            Console.WriteLine("Chance to draw by turn {1}: {0:f2}%", chanceByTurn * 100, turnNum);

            chanceToDraw *= (1 - chanceByTurn);
            chanceToDraw = 100 * (1 - chanceToDraw);    // Convert to percentage

            Console.WriteLine("Overall chance to draw by turn {1} and after mulliganing: {0:f2}%", chanceToDraw, turnNum);
        }

        static double getCombination(double numItems, double numSuccessItems)
        {
            double combinationValue = calcPreCombinationValue(numItems) / (calcPreCombinationValue(numSuccessItems) * calcPreCombinationValue(numItems - numSuccessItems));
            return combinationValue;
        }

        static double calcPreCombinationValue(double input)
        {
            double result = 1;
            for (double i = 0; i < input; i++)
            {
                double workingValue = i + 1;
                result *= workingValue;
            }
            return (double)result;
        }

        static double getHypergeometricDistribution(double numPopulation, double numPopulationSuccesses, double numSample, double numSampleSuccessesHighest)
        {
            double firstExp;
            double secondExp;
            double thirdExp;
            double hyperGeometricDistribution;
            double cumulativeHyperGeometricDistribution = 0;
            for (double numSampleSuccesses = 1; numSampleSuccesses <= numSampleSuccessesHighest; numSampleSuccesses++)
            {
                firstExp = getCombination(numPopulationSuccesses, numSampleSuccesses);
                secondExp = getCombination(numPopulation - numPopulationSuccesses, numSample - numSampleSuccesses);
                thirdExp = getCombination(numPopulation, numSample);
                hyperGeometricDistribution = firstExp * secondExp / thirdExp;
                cumulativeHyperGeometricDistribution += hyperGeometricDistribution;
            }
            return cumulativeHyperGeometricDistribution;
        }
    }
}
