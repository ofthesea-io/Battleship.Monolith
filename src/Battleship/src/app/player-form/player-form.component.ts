import { Component, Input, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { BattleshipService } from "../services/battleship.service";
import { Configuration } from "../config/configuration";
import { Player } from "../player";

@Component({
  selector: "player-form-root",
  templateUrl: "./player-form.component.html",
  styleUrls: ["./player-form.component.css"]
})
export class PlayerFormComponent implements OnInit {

  numberOfShipOptions: Array<any>;
  @Input("player") player: Player;

  public firstName: string = "";
  public lastName: string = "";
  public selectedShipCounter: number;

  constructor(
    private config: Configuration,
    private battleShipService: BattleshipService
  ) {
    this.numberOfShipOptions = config.shipCounter;
  }

  ngOnInit(): void {
    //just select the default number of ships
    this.selectedShipCounter = 4;
  }

  onSubmit(data) {
    let player = data as Player;
    player.numberOfShips = data.shipsCounter;
    this.battleShipService.gamingGridNewGameSubject.next("clear");
    //mimic that we have logged in and the server has created a session for us
    localStorage.setItem('authToken', "Bearer " + btoa(`${player.firstname} ${player.lastname}`));

    this.battleShipService.startGame(player).subscribe(
      response => {
        console.log(response);
        if (response.status == 200) {
          //reset the auth token to the server session
          var session = response.body["token"];
          localStorage.setItem("authToken", session);
          this.battleShipService.buildGamingGrid();
          this.battleShipService.playerSubject.next(player);
        } else {
          localStorage.setItem("authToken", "");
        }
      },
      err => {
        console.log(err);
      }
    );
  }
}
