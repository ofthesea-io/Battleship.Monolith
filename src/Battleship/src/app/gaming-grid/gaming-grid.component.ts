import { Component, OnInit, Input } from "@angular/core";
import { BattleshipService } from "../services/battleship.service";
import { PlayerStatistics } from "../playerstatistics";
import { Configuration } from "../config/configuration";
import { Coordinate } from "../coordinate";

@Component({
  selector: "battleship-gaming-root",
  templateUrl: "./gaming-grid.component.html",
  styleUrls: ["./gaming-grid.component.css"]
})
export class GamingGridComponent implements OnInit {
  private config: Configuration;

  constructor(private battleShipService: BattleshipService) {
    this.config = new Configuration();
  }

  private currentCoridnates: Array<Coordinate> = [];
  private xAxis: Array<string> = [];
  private yAxis: Array<Number> = [];

  x: number;
  y: number;

  numberOfShips: number = 0;
  total: number = 0;
  miss: number = 0;
  hit: number = 0;
  sunk: number = 0;
  completed: boolean = false;
  message: string = "";

  ngOnInit() {
    this.battleShipService.gamingGridSubject.subscribe(data => {
      this.xAxis = data["x"];
      this.yAxis = data["y"];
    });

    this.battleShipService.gamingGridNewGameSubject.subscribe(data => {
      this.xAxis = [];
      this.yAxis = [];
      this.total = 0;
      this.miss = 0;
      this.hit = 0;
      this.sunk = 0;
      this.message = "";
    });

    this.battleShipService.gamingMessage.subscribe(data => {
      this.message = data.toString();
    });

    this.battleShipService.playerSubject.subscribe(data => {
      this.numberOfShips = data.numberOfShips;
    });
  }

  isCoordinateHandled(x: number, y: number): boolean {
    let result = false;
    if (this.currentCoridnates.filter(q => q.x == x && q.y == y).length == 0) {
      let coordinate: Coordinate = { x: x, y: y };
      this.currentCoridnates.push(coordinate);
    } else {
      result = true;
    }

    return result;
  }

  checkCoordinate(event) {
    var target = event.target || event.srcElement || event.currentTarget;
    if (target != null) {
      this.x = target.getAttribute("data-x").charCodeAt(0);
      this.y = parseInt(target.getAttribute("data-y"));
      if (!this.isCoordinateHandled(this.x, this.y)) {
        if (this.sunk == this.numberOfShips) {
          this.message = this.config.completed;
        } else {
          this.battleShipService
            .sendUserCommand(this.x, this.y)
            .subscribe(result => {
              if (result.ok) {
                let playerStatistics = result.body as PlayerStatistics;
                this.miss = playerStatistics.miss;
                this.hit = playerStatistics.hit;
                this.sunk = playerStatistics.sunk;
                if (playerStatistics.status) {
                  this.message = this.config.hit;
                  target.className =
                    "col-1 bg-danger text-white border text-center";
                } else {
                  this.message = this.config.miss;
                  target.className =
                    "col-1 bg-warning text-white border text-center";
                }
                this.total++;
              }
            });
        }
      } else {
        this.message = this.config.tried;
      }
    }
  }
}
