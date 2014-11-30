using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimpleKmean
{
    class Program
    {
        static int loopCount = 1;
        static int recursiveFlag = 0;
        static void Main(string[] args)
        {
            int[,] nodes;
            int[,] classes;
            //
            // user input 
            //
            System.Console.Write("how many nodes do you want? :");
            int nodeNumber = Convert.ToInt32(System.Console.ReadLine());
            System.Console.Write("how many classes do you want? :");
            int classNumber = Convert.ToInt32(System.Console.ReadLine());
            //
            // dynamic create 2D array
            //
            nodes = new int[nodeNumber,3]; // index -> x = 0 , y = 1 , classFlag = 3
            classes = new int[nodeNumber,2]; // classnumber coordinate , clustering coordinates 
            //
            // get random coordinates
            //
            for (int i = 0; i < nodeNumber; i++)
            {
                Random random = new Random((int)DateTime.Now.Millisecond);
                // get x 
                nodes[i, 0] = random.Next(0, 10);
                Thread.Sleep(20);
                // get y
                nodes[i, 1] = random.Next(0, 10);
                System.Console.WriteLine("({0},{1})",nodes[i, 0],nodes[i, 1]);
            }
            System.Console.WriteLine("-------------------");
            // 
            // get random class
            //
            for (int i = 0; i < classNumber; i++)
            {
                Random random = new Random((int)DateTime.Now.Millisecond);
                // get x 
                
                classes[i, 0] = random.Next(0, 10);
                Thread.Sleep(20);
                // get y
                classes[i, 1] = random.Next(0, 10);
                System.Console.WriteLine("class[{0}]--({1},{2})",i, classes[i, 0], classes[i, 1]);
            }
            System.Console.WriteLine("-------------------");


            //showResult(nodes,nodeNumber);
            kMean(nodeNumber, classNumber, nodes, classes);
            System.Console.WriteLine("=================================");
            System.Console.WriteLine("finish! recusive times : {0}",loopCount);
            showResult(nodes, nodeNumber, classes, classNumber);
            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey();
        }
        //
        // use the euclidean distane to calculate two coordinates.
        //
        static double distance(int x1, int y1, int x2 , int y2)
        {
            return Math.Sqrt( Math.Pow((x1-x2),2) + Math.Pow((y1-y2),2));
        }
        //
        // procedure that show the group
        //
        static void showResult(int[,] nodes,int nodeNumber,int[,] classes,int classNumber)
        {
            for (int i = 0; i < classNumber; i++)
            {
                System.Console.WriteLine("class[{0}]--({1},{2})",i, classes[i, 0], classes[i, 1]);
            }
            System.Console.WriteLine("-------------------");
            for (int i = 0; i < nodeNumber; i++)
            {
                System.Console.WriteLine("({0},{1}) --> class[{2}]", nodes[i, 0], nodes[i, 1], nodes[i, 2]);
            }          
        }
        //
        // K-mean Algorithm
        //
        static void kMean(int nodeNumber , int classNumber, int[,] nodes, int[,] classes)
        {
            //
            // calculate the distance between the class and the coordinate . 
            //
            for (int i = 0; i < nodeNumber; i++)
            {
                double min = 100000;
                for (int j = 0; j < classNumber; j++)
                {
                    double mindDistance = distance(nodes[i, 0], nodes[i, 1], classes[j, 0], classes[j, 1]);
                    if (mindDistance < min)
                    {
                        min = mindDistance;
                        nodes[i, 2] = j; // flag to set the group that the coordinate belong to. 
                    }
                }
            }

            showResult(nodes, nodeNumber,classes,classNumber);

            //
            // calculate the new center class
            //
            
            int[,] tempClasses = new int[nodeNumber, 3];
            for (int j = 0; j < classNumber; j++)
            {
                int[] tempCoordinate = new int[2];
                for (int i = 0; i < nodeNumber; i++)
                {
                    if (nodes[i, 2] == j)
                    {
                        // new class
                        tempCoordinate[0] += nodes[i, 0];
                        tempCoordinate[1] += nodes[i, 1];
                        tempClasses[j, 2]++;
                    }
                }
                if (tempClasses[j, 2] == 0)
                    tempClasses[j, 2] = 1;
                tempClasses[j, 0] = tempCoordinate[0] / tempClasses[j, 2];
                tempClasses[j, 1] = tempCoordinate[1] / tempClasses[j, 2];
                System.Console.WriteLine("count = {3},class[{0}] :new cor ({1},{2})", j, tempClasses[j, 0], tempClasses[j, 1], tempClasses[j, 2]);
                
            }
            //System.Console.ReadKey();
            int k = 0;
            for ( k = 0; k < classNumber; k++ )
            {
                if ((tempClasses[k, 0] != classes[k, 0]) || (tempClasses[k, 1] != classes[k, 1]))
                {
                    recursiveFlag = 1;
                    break;
                }
            }
            if (k >= classNumber)
                recursiveFlag = 0;
            if (recursiveFlag == 1)
            {
                for (int j = 0; j < classNumber; j++)
                {
                    classes[j, 0] = tempClasses[j, 0];
                    classes[j, 1] = tempClasses[j, 1];
                }
                System.Console.ReadKey();
                // recursive
                kMean(nodeNumber, classNumber, nodes, classes);
                loopCount++;
            }
            if (recursiveFlag == 0)
            {
                //System.Console.WriteLine("=================================");
                //System.Console.WriteLine("finish! recusive times : {0}",loopCount);
                showResult(nodes, nodeNumber, classes, classNumber);
            }
        }
    }
}
