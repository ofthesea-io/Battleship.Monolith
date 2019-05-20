import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";
import { HttpClientModule } from "@angular/common/http";
import { AppComponent } from "./app.component";
import { GamingGridComponent } from "./gaming-grid/gaming-grid.component";
import { ScoreCardComponent } from "./score-card/score-card.component";
import { Configuration } from "./config/configuration";
import { BattleshipService } from "./services/battleship.service";
import { PlayerFormComponent } from "./player-form/player-form.component";

@NgModule({
  declarations: [GamingGridComponent, ScoreCardComponent, AppComponent, PlayerFormComponent],
  imports: [BrowserModule, FormsModule, HttpModule, HttpClientModule],
  providers: [Configuration, BattleshipService],
  bootstrap: [AppComponent]
})
export class AppModule {}
