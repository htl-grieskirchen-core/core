import {Component, OnInit} from "@angular/core";
import {
  IDTokenPayload,
  MainframeIdTokenService
} from "../../../../../../libs/mainframe-connector/src/lib/mainframe-id-token.service";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"],
})
export class HomeComponent implements OnInit {
  constructor(private service: MainframeIdTokenService) {
  }

  jwtPayload: IDTokenPayload | undefined;

  ngOnInit(): void {
    this.jwtPayload = this.service.decodeJwt(this.service.getJwt());
  }
}
