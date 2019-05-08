export class Configuration {
  shipCounter = [
    { Id: 1, option: "One" },
    { Id: 2, option: "Two" },
    { Id: 3, option: "Three" },
    { Id: 4, option: "Four" },
    { Id: 5, option: "Five" },
    { Id: 6, option: "Six" },
    { Id: 7, option: "Seven" },
    { Id: 8, option: "Eight" },
    { Id: 9, option: "Nine" }
  ];

    readonly hit : string = "You've hit a ship!";
    readonly miss: string = "Sorry you missed, please try again...";
    readonly sunk: string = "You've sunk a ship!";
    readonly tried:string = "Coordinate already tried. Please try a different cell.";
    readonly incorrect: string = "Incorrect input, please try again"; 
    readonly completed: string = "Game completed!"; 
}
 