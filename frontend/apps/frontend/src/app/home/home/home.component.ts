import {Component, OnInit} from "@angular/core";
import {
  IDTokenPayload,
  MainframeIdTokenService
} from "mainframe-connector";
import {Plugin} from "../../shared/classes/plugin";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"],
})
export class HomeComponent implements OnInit {

  plugins: Plugin[] = [];

  constructor(private service: MainframeIdTokenService) {
  }

  jwtPayload: IDTokenPayload | undefined;
  showDashboard: boolean = true;

  // getNewPlugin = (name: string, image: string) => new Plugin( name, [], null, null, null, null, `../../../assets/images/${image}`);
  //
  // plugins: Plugin[] = [
  // this.getNewPlugin('BeReal Anfängerkurs', 'nodla.jpeg'),
  // this.getNewPlugin('Kirtag in Lambrechten', 'wiesn.jpg'),
  // this.getNewPlugin('Beamer hi mochn', 'heimooo.jpg'),
  // this.getNewPlugin('Zugbremse reparieren', 'BeReal.jpeg'),
  // this.getNewPlugin('Roboter-Quartett mit Sperrer', 'loata.jpeg'),
  // this.getNewPlugin('Schuhkressenbauer', 'schuhkressenbauer.jpg'),
  // this.getNewPlugin('Minecraft Modpacks', 'simon.jpeg'),
  // this.getNewPlugin('MIchael?', 'miche2.JPG'),
  // this.getNewPlugin('Chayacheck', 'groans.JPG'),
  // this.getNewPlugin('periodischer Chill', 'chille.jpg'),
  // this.getNewPlugin('ORF interview', 'meeting.jpg')
  // ];

  ngOnInit(): void {
    this.service.getJwt().then(jwt=>{
      this.jwtPayload = this.service.decodeJwt(jwt);
    });
  }
}
