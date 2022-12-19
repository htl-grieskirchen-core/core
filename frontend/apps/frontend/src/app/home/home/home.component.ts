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
  constructor(private service: MainframeIdTokenService) {
  }

  jwtPayload: IDTokenPayload | undefined;

  getNewCategory = (name: string, icon: string) => new Category( name, `../../../assets/icons/${icon}`);

  courses: Category[] = [
    this.getNewCategory('Favoriten', 'star.svg'),
    this.getNewCategory('Meine', 'user-search.svg'),
    this.getNewCategory('Bald fällig', 'hourglass-low.svg'),
    this.getNewCategory('Alle', 'border-all.svg')];

  ngOnInit(): void {
    this.service.getJwt().then(jwt=>{
      this.jwtPayload = this.service.decodeJwt(jwt);
    });
  }
}

export class Category {
  constructor(
    public name: string | null,
    public icon: string | null) {
  }
}
