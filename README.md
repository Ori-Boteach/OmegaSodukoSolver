# OmegaSodukoSolver #Ori-Boteach

My version of the Omega_Soduko_Solver works by using the backtracking algorithm.
First, the program recieves a string representation of soduko puzzle that needs to be solved and checks it's validation (StartAndValidation function)
Then, it converts the inputted string to a matrix -> 2D array (ConvertToBoard function).
AfterWards, in the Calculation modul, the soduko is being solved (SolveSudoku function).

My algorithm works like so:
1. Start at the first empty cell.
2. Try filling the cell with a number from 1 to 9.
3. Check if the number is safe to place in the cell (using CanBePlaced function).
   *if the number is safe, move on to the next empty cell and repeat the process (recursive calling).
   *if the number is not safe or if there are no more empty cells and a solution has not been found, "backtrack" to the previous cell and try a different number.
5.If a solution is found, return true and the modified soduko borad. If no solution is found after trying all possible numbers, return that no solution exists.

Then, if there is no solution -> a message is printed (SodukoResult function).
      else -> the solved soduko puzzle is being printed in a string format (SodukoResult, PrintBoard, ConvertBackToString functions).
      
This whole process is being done in a loop (Main function) that repeats itself as long as the user wants.
**Each calculation time is being timed and printed to the screen with the solved puzzle (StartAndValidation function).
