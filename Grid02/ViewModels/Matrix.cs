using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Grid02.ViewModels;
/**
* The School Navigator
* This is the Matrix class
* This class implements Djikstra's short path algorithm along with breadth-search algorithm to find the most optimal solution.
* Source: https://www.codeproject.com/Articles/9040/Maze-Solver-shortest-path-finder
*
* @author  Vaishnavi Sinha
* @version 1.0
* @Date   05-01-2019 
*/

namespace Grid02.ViewModels
{
    /**
     * This class creates an empty grid with 20 rows ad 25 columns - which will be used to fill in the information of each floor from the database 
     */
    public class Matrix
    {
        //coordinate of each location in grid set as x,y in range x: 0 - 19 and y: 0 - 24
        public int[, ] grid = new int[20,25];
        //stores the name of the location in grid
        public String[,] label = new String[20, 25];
        //stores path value (0 for available, 1 for blocked) for location
        public int[,] path = new int[20, 25]; 
    }


}

/*
 * @param iAltered This is the first paramter to addNum method
 * @param jAltered  This is the second parameter to addNum method
 * @return void This returns nothing 
 */
public delegate void GridAlteredHandler(int iAltered, int jAltered);

/*
 * This is the main class
 */
public class ShortPath
{

    int[,] n_Grid;
    int n_Rows;
    int n_Cols;
    int Path = 7;
    bool diagonalWay = false;
    private Matrix matrix;


    public event GridAlteredHandler OnGridAltered;

    /**
    * This method gets the length of the row and column of the grid and stores it in a 2D array
    * @param iGrid This is the paramter (2D array) to ShortPath method
    */
    public ShortPath(int[,] iGrid)
    {
        n_Grid = iGrid;
        n_Rows = iGrid.GetLength(0);
        n_Cols = iGrid.GetLength(1);
    }

    /**
    * This method sets the length of the row and column of the grid
    * @param Rows This is the first paramter to ShortPath method
    * @param Cols  This is the second parameter to ShortPath method
    */
    public ShortPath(int Rows, int Cols)
    {
        n_Grid = new int[Rows, Cols];
        n_Rows = Rows;
        n_Cols = Cols;
    }

    /**
    * This method assigns parameter to member variable
    * @param matrix This is the parameter for ShortPath method
    */
    public ShortPath(Matrix matrix)
    {
        this.matrix = matrix;
    }

    /**
    * This method gets Rows
    * @return n_Rows
    */
    public int Rows
    {
        get { return n_Rows; }
    }

    /**
     * This method gets Cols
     * @return n_Cols
     */
    public int Cols
    {
        get { return n_Cols; }
    }

    /**
    * This method gets GetGrid
    * @return n_Grid
    */
    public int[,] GetGrid
    {
        get { return n_Grid; }
    }

    /**
    * This method gets PathVal and sets it to the value 
    * @return Path 
    */
    public int PathVal
    {
        get { return Path; }
        set
        {
            if (value == 0)
                throw new Exception("Invalid path value specified");
            else
                Path = value;
        }
    }

    /**
    * This method gets AllowDiagonals and sets diagnolWay to value
    * @return diagonalWay
    */
    public bool AllowDiagonals
    {
        get { return diagonalWay; }
        set { diagonalWay = value; }
    }

    /**
    * This method  gets n_Grid and sets it to value
    * @param nRow This is the first parameter to this method which will get row number
    * @param nCol This is the second parameter to this method which will get column number
    * @return nGrid[nRow, nCol]
    */
    public int this[int nRow, int nCol]
    {
        get { return n_Grid[nRow, nCol]; }
        set
        {
            n_Grid[nRow, nCol] = value;
            if (this.OnGridAltered != null)  // trigger event
                this.OnGridAltered(nRow, nCol);
        }
    }

    /**
    * This method to get the node value
    * @param iGrid This parameter passes the row and column as a 2D array
    * @param NodeNum This parameter passes the value at node
    * @return iGrid
    */
    private int GetNodeVal(int[,] iGrid, int NodeNum)
    {
        int Cols = iGrid.GetLength(1);
        return iGrid[NodeNum / Cols, NodeNum - NodeNum / Cols * Cols];
    }

    /**
    * This method is updates the value at the node 
    * @param iGrid This parameter passes the row and column as a 2D array
    * @param NodeNum This parameter passes the value at node
    * @param NewVal This parameter cotains the new value of node
    */
    private void ChangeNodeVal(int[,] iGrid, int NodeNum, int NewVal)
    {
        int Cols = iGrid.GetLength(1);
        iGrid[
            NodeNum / Cols, NodeNum - NodeNum / Cols * Cols] = NewVal;
    }

    /**
    * This method Searches available routes from start location (x,y) and end location (x,y)
    * @param FromY This parameter is used to calculate start node
    * @param FromX This parameter is used to calculate start node
    * @param ToY This parameter is used to calculate end node
    * @param ToX This parameter is used to calculate end node
    * @return Search
    */
    public int[,] FindPath(int FromY, int FromX, int ToY, int ToX)
    {
        int StartNode = FromY * this.Cols + FromX;
        int EndNode = ToY * this.Cols + ToX;
        return (Search(StartNode, EndNode));
    }

    private enum Status { Ready, Waiting, Processed }

    /**
    * This method does the breadth-first analysis to the matrix to find the shortest available path to user
    * @param Start This parameter is the start location entered by user
    * @param Stop  This parameter is the start location entered by user
    * @return null This is returned if no available path is found (or)
    *         iGridSolved This is the final solution returned if path is found
    */
    private int[,] Search(int Start, int Stop)
    {
        const int empty = 0;

        int Rows = n_Rows;
        int Cols = n_Cols;
        int Maximum = Rows * Cols;
        int[] Queue = new int[Maximum];
        int[] Origin = new int[Maximum];
        int Front = 0, Rear = 0;


        if (GetNodeVal(n_Grid, Start) != empty || GetNodeVal(n_Grid, Stop) != empty)
        {
            return null;
        }

        int[,] iGridStatus = new int[Rows, Cols];

        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Cols; j++)
                iGridStatus[i, j] = (int)Status.Ready;


        Queue[Rear] = Start;
        Origin[Rear] = -1;
        Rear++;
        int Current, Left, Right, Top, Down;
        while (Front != Rear) // while Queue is not empty	
        {
            if (Queue[Front] == Stop)   // pahg is solved
                break;

            Current = Queue[Front];

            Left = Current - 1;
            if (Left >= 0 && Left / Cols == Current / Cols)  //if left node exists
                if (GetNodeVal(n_Grid, Left) == empty)   //if path available for left node
                    if (GetNodeVal(iGridStatus, Left) == (int)Status.Ready) //if left node is ready
                    {
                        Queue[Rear] = Left; //add to Queue 
                        Origin[Rear] = Current;
                        ChangeNodeVal(iGridStatus, Left, (int)Status.Waiting); //status is changed to waiting
                        Rear++;
                    }

            Right = Current + 1;
            if (Right < Maximum && Right / Cols == Current / Cols)  //if right node exists
                if (GetNodeVal(n_Grid, Right) == empty)  //if path available right node is open
                    if (GetNodeVal(iGridStatus, Right) == (int)Status.Ready)  //if right node is ready
                    {
                        Queue[Rear] = Right; //add to Queue
                        Origin[Rear] = Current;
                        ChangeNodeVal(iGridStatus, Right, (int)Status.Waiting); //status changed to waiting
                        Rear++;
                    }

            Top = Current - Cols;
            if (Top >= 0)  //if top node exists
                if (GetNodeVal(n_Grid, Top) == empty)  //if path available for top node
                    if (GetNodeVal(iGridStatus, Top) == (int)Status.Ready)  //if top node is ready
                    {
                        Queue[Rear] = Top; //add to Q
                        Origin[Rear] = Current;
                        ChangeNodeVal(iGridStatus, Top, (int)Status.Waiting); //status changed to to waiting
                        Rear++;
                    }

            Down = Current + Cols;
            if (Down < Maximum)   //if bottom node exists
                if (GetNodeVal(n_Grid, Down) == empty)   //if path available for bottom node
                    if (GetNodeVal(iGridStatus, Down) == (int)Status.Ready) //if bottom node is ready
                    {
                        Queue[Rear] = Down; //add to Queue
                        Origin[Rear] = Current;
                        ChangeNodeVal(iGridStatus, Down, (int)Status.Waiting); //status changed to waiting
                        Rear++;
                    }

            //status of current node that has to be processed is changed
            ChangeNodeVal(iGridStatus, Current, (int)Status.Processed);
            Front++;

        }

        //create an array to store solution
        int[,] iGridSolved = new int[Rows, Cols];
        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Cols; j++)
                iGridSolved[i, j] = n_Grid[i, j];

        //make a path in the Solved Maze
        Current = Stop;
        ChangeNodeVal(iGridSolved, Current, Path);
        for (int i = Front; i >= 0; i--)
        {
            if (Queue[i] == Current)
            {
                Current = Origin[i];
                if (Current == -1)   // path is found
                    return (iGridSolved);
                ChangeNodeVal(iGridSolved, Current, Path);
            }
        }

        return null;
    }
}







