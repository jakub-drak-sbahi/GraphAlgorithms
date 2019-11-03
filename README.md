# GraphAlgorithms
Solutions of several problems using graph algorithms.

### FindBestCitiesPair  
##### FindBestCitiesPair()  
Function finds two cities from group of k (k<=n) nominated cities that time of travel between them is the shortest time of all couples.  
Input:  
+ Graph times - graph which vertices represent cities and edge weights represent time of travel between cities  
+ double[] passThroughCityTimes - times of travels through the cities  
+ int[] nominatedCities - numbers of nominated cities  
+ bool buildBypass - possibility of building one ring road  

Output:  
+ int c1 - number of first city  
+ int c2 - number of second city  
+ int? bypass - number of city encircled by ring road  
+ double time - time of travel between c1 and c2  
+ Edge[] path - path between c1 and c2  

### RoutePlanner
Problem:  
Find bus routes which start and finish is in the same place, and don't contain same road (edge) two times.  
##### FindShortRoutes()  
Addidtional assumption, that any road can contain some vertice only once  
Input:  
+ Graph g - graph representing required connections  

Output:  
+ int[][] - table of routes, or null if it is impossible to solve the problem  

##### FindLongRoutes()  
Every road can contain every vertice many times.  
Input:  
+ Graph g - graph representing required connections  

Output:  
+ int[][] - table of routes, or null if it is impossible to solve the problem  

### DistributionFinder
##### FindBestDistribution()  
Function finds best distribution of children to classes. Sport classes can be opened only if all places are taken. Every child can attend only one class. Children attend only prefered classes. If isSportActivity!=null children attend only sport classes.  
Input:  
+ int[] limits - limits of children in classes  
+ int[][] preferences - children's preferences (indexes of prefered classes)  
+ bool[] isSportActivity - defines which classes are sport classes (can be null)  

Output:  
+ int satisfactionLevel - 1 if all sport classes can be opened, 0 if not, if isSportActivity==null it is number of satisfied children  
+ int[] bestDistribution - distribution (distribution[i] - child attends class "i")  

### ProgramPlanning
##### CalculateTimesLatestPossible()  
Function calculates program execution time. Presence of edge (v, w) means that execution of process v is needed to execute w.  
Input:  
+ Graph taskGraph - graph defining dependencies between procedures  
+ double[] taskTimes - times of processes' executions  

Output:  
+ double _return - time of program execution  
+ out double[] startTimes - the latest possible time of running a process  
+ out int[] criticalPath - path of processes, which extension causes extending time of execution of the program  
