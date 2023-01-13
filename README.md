# OmegaSodukoSolver #Ori-Boteach

My version of the Omega_Soduko_Solver works with two options of solving algorithms -> backtracking algorithm and dancing links algorithm.

First, the program recieves a string representation of soduko puzzle that needs to be solved (wheater it's from a file or from the console)
and checks it's validation (StartAndValidation function).
Then, it converts the inputted string to a matrix -> 2D array (ConvertToBoard function).

According to the user's choise the different algorithms are being activated:

The backtracking algorithm works like so:
1. Start at the first empty cell.
2. Try filling the cell with a number from 1 to 9.
3. Check if the number is safe to place in the cell (using CanBePlaced function).
   *if the number is safe, move on to the next empty cell and repeat the process (recursive calling).
   *if the number is not safe or if there are no more empty cells and a solution has not been found, "backtrack" to the previous cell and try a different number.
5.If a solution is found, return true and the modified soduko borad. If no solution is found after trying all possible numbers, return that no solution exists.

**please note that this algorithm also uses varius optimization techniques: HiddenDoubles, NakedPairs, SimpleElimination and HiddenSingle.

The Dancing Links algorithm (or DLX in short) works like so:
1. Converting the given matrix to a 1's and 0's matrix according to the sudoku constraints -> cell, row, col and cube.
2. Transforming the matrix into a mesh, a spares "matrix" that contains Nodes -> each HeaderNode contains the Nodes below it
3. Activating the Search method on the mesh -> this method covers (unlinks) and uncovers (links) different nodes in the mesh until a solution is found (or not found)
4. At last, the algorithm recievs a list of nodes that represent the final solution and converts it back into a readable sudoku string

Then, if there is no solution -> a message is printed (SodukoResult function).
      else -> the solved soduko puzzle is being printed in a string format (SodukoResult, PrintBoard, ConvertBackToString functions).
      
This whole process is being done in a loop (Main function) that repeats itself as long as the user wants.
**Each calculation time is being timed and printed to the screen with the solved puzzle (StartAndValidation function).
