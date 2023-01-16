# OmegaSodukoSolver #Ori-Boteach

My version of the Omega_Soduko_Solver works with two options of solving algorithms -> backtracking algorithm and dancing links algorithm.

The user is presented with a main menu where he can decide what he wishes to do (add puzzle from console / from file or quit):

![image](https://user-images.githubusercontent.com/117098140/212703142-e269210d-0375-4ab5-bc4c-da6db83b19c7.png)
![image](https://user-images.githubusercontent.com/117098140/212703492-4a3a7149-b655-4cd9-a1be-2ec8c0e1a300.png)

Then, the program recieves a string representation of soduko puzzle that needs to be solved (both if it's from a file or from the console)
and checks it's validation (StartAndValidation function).
It converts the inputted string to a matrix -> 2D array (ConvertToBoard function).

And according to the user's choise the different algorithms are being activated:

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
2. Transforming the matrix into a mesh, a spares "matrix" that contains Nodes -> each HeaderNode contains the Nodes below it and all headers are gathered in an array.
3. Activating the Search method on the mesh -> this method covers (unlinks) and uncovers (re-links) different nodes in the mesh until a solution is found (or not found)
4. At last, the algorithm recievs a list of nodes that represent the final solution and converts it back into a readable sudoku string

At last, if there is no solution -> a message is printed (SodukoResult function).
      else -> the solved soduko puzzle is being printed in a string format (SodukoResult, PrintBoard, ConvertBackToString functions).

For example:

![image](https://user-images.githubusercontent.com/117098140/212704114-cc7246a2-f16a-4b0f-9cbc-ba5a18a5ad45.png)
![image](https://user-images.githubusercontent.com/117098140/212704482-82403329-78e5-4d79-9d7b-98e6f9bed44b.png)
      
This whole process is being done in a loop (Main function) that repeats itself as long as the user wants.
**Each calculation time is being timed and printed to the screen with the solved puzzle at string and board formats (StartAndValidation function).
