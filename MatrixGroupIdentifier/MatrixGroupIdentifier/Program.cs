using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;

namespace MatrixGroupIdentifier
{
    public struct elementLocation
    {
        public int xCoordinate;
        public int yCoordinate;

        public elementLocation(int p1, int p2)
        {
            xCoordinate = p1;
            yCoordinate = p2;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string inputText = System.IO.File.ReadAllText(@"Input.txt");
            inputText = inputText.Replace("\r", "");
            int[,] inputMatrix = GetIntegerArray(inputText);
            //int[,] inputMatrix = new int[,] {{1,1,0,0,1},
            //                                 {1,0,1,1,0},
            //                                 {1,1,0,1,0}};

            var stopWatch = new Stopwatch();

            stopWatch.Start();

            #region Code Section
            List<elementLocation> allElements = new List<elementLocation>();

            int widthOfArray = inputMatrix.GetLength(0);
            int heightOfArray = inputMatrix.GetLength(1);

            for (int i = 0;i<widthOfArray ; i++ )   // Width of Array
            {
                for(int j=0;j<heightOfArray; j++)    // Height of Array
                {
                    if(inputMatrix[i,j].Equals(1))
                    {
                        elementLocation tempElement = new elementLocation();
                        tempElement.xCoordinate = i;
                        tempElement.yCoordinate = j;

                        allElements.Add(tempElement);
                    }
                }
            }

            List<List<elementLocation>> groupsList = new List<List<elementLocation>>();

            List<int> test = new List<int>(new int[] {1,2,3});
            int e =1;
            while(e != 0)
            {
                List<elementLocation> tempGroupList = new List<elementLocation>();
                if(allElements.Count != 0)
                {
                    tempGroupList.Add(allElements[0]);
                }
                int tempLength = 0;
                int a = 1;
                while(a !=0)
                {
                    int tempGroupLength = tempGroupList.Count;
                    List<elementLocation> swapList = new List<elementLocation>(tempGroupList);
                    if(tempLength < tempGroupLength)
                    {
                        foreach (var tempEle in swapList)
                        {
                            var adjCells = GetAdjecentLocations(tempEle, heightOfArray, widthOfArray);
                            foreach(var adj in adjCells)
                            {
                                if (allElements.Contains(adj))
                                {
                                    if (!tempGroupList.Contains(adj))
                                    {
                                        tempGroupList.Add(adj);
                                    }
                                }
                            }
                        }
                        tempLength = tempGroupLength;
                    }
                    else
                    {
                        a = 0;
                    }
                }

                #region Alternate Code
                //foreach(elementLocation l in allElements)
                //{
                //    var adjCells = GetAdjecentLocations(l, heightOfArray, widthOfArray);
                //    if (tempGroupList.Contains(l))
                //    {
                //        foreach (elementLocation adj in adjCells)
                //        {
                //            if (allElements.Contains(adj))
                //            {
                //                if (!tempGroupList.Contains(adj))
                //                {
                //                    tempGroupList.Add(adj);
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        foreach (elementLocation adj in adjCells)
                //        {
                //            if (tempGroupList.Contains(adj))
                //            {
                //                tempGroupList.Add(l);
                //                foreach (elementLocation lAdj in adjCells)
                //                {
                //                    if(allElements.Contains(lAdj))
                //                    {
                //                        if (!tempGroupList.Contains(lAdj))
                //                            {
                //                                tempGroupList.Add(lAdj);
                //                            }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion Alternate Code
                foreach (elementLocation delLoc in tempGroupList)
                {
                    if(allElements.Contains(delLoc))
                    {
                        allElements.Remove(delLoc);
                    }
                }
                groupsList.Add(tempGroupList);
                if (allElements.Count == 0)
                {
                    e = 0;
                }
            }
            #endregion Code Section

            stopWatch.Stop();
            Console.WriteLine("Number of Groups: "+groupsList.Count);
            Console.WriteLine("Groups Are: \nX Y");
            foreach(var l in groupsList)
            {
                Console.WriteLine("======");
                foreach(var subList in l)
                {
                    Console.WriteLine(subList.xCoordinate + " " + subList.yCoordinate);
                    
                }
            }
            Console.Write("\n\n Time Taken:  "+ stopWatch.Elapsed.TotalSeconds+" sec");
            Console.ReadLine();
        }

       

        public static List<elementLocation> GetAdjecentLocations(elementLocation location, int height, int width)
        {
            List<elementLocation> allAdjecentCells = new List<elementLocation>();

            if(location.xCoordinate >= 0 && location.xCoordinate< width-1)
            {
                elementLocation tempRight = new elementLocation(location.xCoordinate+1,location.yCoordinate);
                allAdjecentCells.Add(tempRight);
            }
            if (location.xCoordinate > 0 && location.xCoordinate <= width - 1)
            {
                elementLocation tempLeft = new elementLocation(location.xCoordinate - 1, location.yCoordinate);
                allAdjecentCells.Add(tempLeft);
            }
            if (location.yCoordinate >= 0 && location.yCoordinate < height - 1)
            {
                elementLocation tempBottom = new elementLocation(location.xCoordinate, location.yCoordinate+1);
                allAdjecentCells.Add(tempBottom);
            }
            if (location.yCoordinate > 0 && location.yCoordinate <= height - 1)
            {
                elementLocation tempTop = new elementLocation(location.xCoordinate, location.yCoordinate - 1);
                allAdjecentCells.Add(tempTop);
            }

            return allAdjecentCells;
        }

        public static int[,] GetIntegerArray(string inputFile)
        {


            string[] inputSplit = inputFile.Split('\n');

            int[,] integerInput = new int[inputSplit.Count(), inputSplit[0].Length];

            int i = 0;
            foreach (string split in inputSplit)
            {
                char[] charSplit = split.ToCharArray();
                int j = 0;
                foreach (char c in charSplit)
                {
                    integerInput[i, j] = (int)Char.GetNumericValue(c);
                    j++;
                }
                i++;
            }

            return integerInput;
        }
    }
}
