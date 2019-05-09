import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpResponse } from "@angular/common/http";
import { Observable } from "rxjs";
import { Subject } from "rxjs/Subject";
import { Player } from "../player";
import "rxjs/add/operator/map";
import { Coordinate } from "../coordinate";
import { Configuration } from "../config/configuration";

@Injectable({
  providedIn: "root"
})
export class BattleshipService {
  private config: Configuration;

  public gamingMessage = new Subject<String>();
  public gamingGridSubject = new Subject<any>();
  public gamingGridNewGameSubject = new Subject<any>();
  public playerSubject = new Subject<Player>();

  private host: string = "http://localhost";
  private battleshipUrl: string = this.host + "/api/Battleship/";

  constructor(private httpClient: HttpClient) {
    this.config = new Configuration();
  }

  private authHeader(): HttpHeaders {
    let authHeaders = new HttpHeaders({
      "Content-Type": "application/json",
      Authorization: localStorage.getItem("authToken")
    });
    return authHeaders;
  }

  startGame(player: Player): Observable<HttpResponse<any>> {
    let startGameUrl: string = this.battleshipUrl + "StartGame";
    return this.httpClient.post<any>(startGameUrl, player, {
      headers: this.authHeader(),
      observe: "response"
    });
  }

  buildGamingGrid() {
    this.getGamingGrid();
  }

  getGamingGrid() {
    let getGamingGridUrl: string = this.battleshipUrl + "GetGamingGrid";
    return this.httpClient
      .get<any>(getGamingGridUrl, {
        headers: this.authHeader()
      })
      .subscribe(
        data => this.gamingGridSubject.next(data),
        error => console.log(error)
      );
  }

  sendUserCommand(xAxis: number, yAxis: number): Observable<HttpResponse<any>> {
    if (xAxis <= 0 && xAxis <= 0) {
      this.gamingGridSubject.next(this.config.incorrect);
    } else {
      let coordinate: Coordinate = { x: xAxis, y: yAxis };
      let userInputUrl: string = this.battleshipUrl + "UserInput";
      return this.httpClient.post<any>(userInputUrl, coordinate, {
        headers: this.authHeader(),
        observe: "response"
      });
    }
  }
}
